namespace yatt.WorkItems.Api.Models;

public class WorkItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}