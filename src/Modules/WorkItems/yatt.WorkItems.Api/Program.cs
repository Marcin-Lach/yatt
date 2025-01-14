using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using yatt.WorkItems.Api.Models;
using yatt.WorkItems.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// TODO In order to have a session-wide workItemsCollection, this needs to be registered as Singleton,
//      while IWorkItemRepository is registered as Scoped, to get new instance for every http request
// TODO Replace with a DbContext
builder.Services.AddKeyedSingleton(
    typeof(Dictionary<Guid, WorkItem>), 
    "workItemsCollection",
    (provider, o) => new Dictionary<Guid, WorkItem>());
builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapGet("/", () => { return Results.Redirect("/openapi/v1.json"); })
        .ExcludeFromDescription();
}

var workItemsEndpoints = app
    .MapGroup("/api/workitems")
    .WithTags("WorkItems");

// TODO: maybe use Vertical Slicing to manage the features
workItemsEndpoints
    .MapGet("/",
        async Task<Ok<List<WorkItemRequestModel>>> (IWorkItemRepository repository)
            => TypedResults.Ok(
                (await repository.GetAllAsync())
                .Select(x => x.ToRequestModel())
                .ToList())
    )
    .WithName("getAllWorkItems")
    .WithDescription("Get full list of work items");

workItemsEndpoints
    .MapGet("/{id:guid}",
        async Task<Results<Ok<WorkItemRequestModel>, NotFound>> ([FromRoute] Guid id, IWorkItemRepository repository)
            =>
        {
            var workItem = await repository.GetByIdAsync(id);
            if (workItem is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(workItem.ToRequestModel());
        })
    .WithName("getWorkItemById")
    .WithDescription("Get a single work item or not found");

workItemsEndpoints
    .MapPost("/",
        async Task<Results<Created<WorkItemRequestModel>, Conflict>> ([FromBody] WorkItemRequestModel workItem,
                IWorkItemRepository repository)
            =>
        {
            if (await repository.TryAddAsync(workItem.ToWorkItem()))
            {
                return TypedResults.Created($"/api/workitems/{workItem.Id}", workItem);
            }

            return TypedResults.Conflict();
        })
    .WithName("addWorkItem")
    .WithDescription("Add new work items. If work item already exists, Conflict status code is returned");

workItemsEndpoints
    .MapPut("/{id:guid}",
        async Task<Results<NoContent, NotFound>> ([FromRoute] Guid id, [FromBody] WorkItemRequestModel workItem,
                IWorkItemRepository repository)
            =>
        {
            if (await repository.TryUpdateAsync(workItem.ToWorkItem()))
            {
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        })
    .WithName("updateWorkItem")
    .WithDescription("Update all properties of a work item");

workItemsEndpoints
    .MapDelete("/{id:guid}",
        async Task<NoContent> ([FromRoute] Guid id, IWorkItemRepository repository) =>
        {
            await repository.DeleteAsync(id);
            return TypedResults.NoContent();
        })
    .WithName("removeWorkItem")
    .WithDescription("Remove work item");

app.Run();