namespace yatt.WorkItems.Api.Models;

public record WorkItem
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }
}