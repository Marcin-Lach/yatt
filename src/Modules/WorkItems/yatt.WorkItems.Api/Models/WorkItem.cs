using System.ComponentModel;

namespace yatt.WorkItems.Api.Models;

public record WorkItem
{
    [Description("Identifier of the work item")]
    public required Guid Id { get; init; }
    [Description("Brief description of the work item")]
    public required string Title { get; init; }
    [Description("Detailed description of the work item")]
    public string? Details { get; init; }
}