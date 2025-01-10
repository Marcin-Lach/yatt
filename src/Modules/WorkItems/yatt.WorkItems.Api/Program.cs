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
    () => Results.Ok(workItems.Select(x => x.Value).ToList()));

app.MapGet("/api/workitems/{id:guid}", 
    ([FromRoute] Guid id)
        => workItems.TryGetValue(id, out var workItem) ? 
            Results.Ok((object?)workItem) : 
            Results.NotFound());

app.MapPost("/api/workitems", ([FromBody] WorkItem workItem) =>
{
    if (workItems.TryAdd(workItem.Id, workItem))
    {
        return Results.Created($"/api/workitems/{workItem.Id}", workItem);
    }

    return Results.Conflict();
});

app.MapPut("/api/workitems/{id:guid}", ([FromRoute] Guid id, [FromBody] WorkItem workItem) =>
{
    if(workItems.ContainsKey(id))
    {
        workItems[id] = workItem;
        return Results.NoContent();
    }
    
    return Results.NotFound();
});

app.MapDelete("/api/workitems/{id:guid}", ([FromRoute] Guid id) =>
{
    workItems.Remove(id);
    return Results.NoContent();
});

app.Run();