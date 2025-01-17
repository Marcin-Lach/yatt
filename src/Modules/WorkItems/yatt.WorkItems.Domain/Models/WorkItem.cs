namespace yatt.WorkItems.Api.Models;

public class WorkItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Details { get; init; }

    public bool IsCompleted { get; protected set; } = false;

    public void MarkAsComplete()
    {
        IsCompleted = true;
    }

    public void MarkAsIncomplete()
    {
        IsCompleted = false;
    }
}