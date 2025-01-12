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

// TODO: improve OpenApi generated for those endpoints (use TypedResults and fluent api methods to enhance OpenApi spec)
// TODO: maybe use Vertical Slicing to manage the features
app.MapGet("/api/workitems", 
    Ok<List<WorkItem>>() => TypedResults.Ok(workItems.Select(x => x.Value).ToList()));

app.MapGet("/api/workitems/{id:guid}", 
    Results<Ok<WorkItem>, NotFound>([FromRoute] Guid id)
        => workItems.TryGetValue(id, out var workItem) ? 
            TypedResults.Ok(workItem) : 
            TypedResults.NotFound());

app.MapPost("/api/workitems", Results<Created<WorkItem>, Conflict>([FromBody] WorkItem workItem) =>
{
    if (workItems.TryAdd(workItem.Id, workItem))
    {
        return TypedResults.Created($"/api/workitems/{workItem.Id}", workItem);
    }

    return TypedResults.Conflict();
});

app.MapPut("/api/workitems/{id:guid}", Results<NoContent, NotFound> ([FromRoute] Guid id, [FromBody] WorkItem workItem) =>
{
    if(workItems.ContainsKey(id))
    {
        workItems[id] = workItem;
        return TypedResults.NoContent();
    }
    
    return TypedResults.NotFound();
});

app.MapDelete("/api/workitems/{id:guid}", NoContent ([FromRoute] Guid id) =>
{
    workItems.Remove(id);
    return TypedResults.NoContent();
});

app.Run();