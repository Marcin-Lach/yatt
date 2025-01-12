using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using yatt.WorkItems.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello Work Items!")
    .WithName("Index");

// TODO: replace in-memory collection with proper DbContext/Repository
var workItems = new Dictionary<Guid, WorkItem>();


var workItemsEndpoints = app
    .MapGroup("/api/workitems")
    .WithTags("WorkItems");

// TODO: maybe use Vertical Slicing to manage the features
workItemsEndpoints
    .MapGet("/",
        Ok<List<WorkItem>> () => TypedResults.Ok(workItems.Select(x => x.Value).ToList()))
    .WithName("Get work items")
    .WithDescription("Get full list of work items");

workItemsEndpoints
    .MapGet("/{id:guid}",
        Results<Ok<WorkItem>, NotFound> ([FromRoute] Guid id)
            => workItems.TryGetValue(id, out var workItem) ? TypedResults.Ok(workItem) : TypedResults.NotFound())
    .WithName("Get work item by identifier")
    .WithDescription("Get a single work item or not found");

workItemsEndpoints
    .MapPost("/", Results<Created<WorkItem>, Conflict> ([FromBody] WorkItem workItem) =>
    {
        if (workItems.TryAdd(workItem.Id, workItem))
            return TypedResults.Created($"/api/workitems/{workItem.Id}", workItem);

        return TypedResults.Conflict();
    })
    .WithName("Add work item")
    .WithDescription("Add new work items. If work item already exists, Conflict status code is returned");

workItemsEndpoints
    .MapPut("/{id:guid}", Results<NoContent, NotFound> ([FromRoute] Guid id, [FromBody] WorkItem workItem) =>
    {
        if (workItems.ContainsKey(id))
        {
            workItems[id] = workItem;
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    })
    .WithName("Update work item")
    .WithDescription("Update all properties of a work item");

workItemsEndpoints
    .MapDelete("/{id:guid}", NoContent ([FromRoute] Guid id) =>
    {
        workItems.Remove(id);
        return TypedResults.NoContent();
    })
    .WithName("Remove work item")
    .WithDescription("Remove work item");

app.Run();