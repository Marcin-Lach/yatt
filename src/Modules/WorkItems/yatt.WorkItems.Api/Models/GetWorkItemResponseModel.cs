using System.ComponentModel;

namespace yatt.WorkItems.Api.Models;

public record GetWorkItemResponseModel
{
    [Description("Identifier of the work item")]
    public required Guid Id { get; init; }
    [Description("Brief description of the work item")]
    public required string Title { get; init; }
    [Description("Detailed description of the work item")]
    public string? Details { get; init; }
    [Description("Whether the work item is completed")]
    public bool IsCompleted { get; set; }
}

public static class WorkItemMapping
{
    public static GetWorkItemResponseModel ToResponseModel(this WorkItem workItem)
    {
        return new GetWorkItemResponseModel
        {
            Id = workItem.Id,
            Title = workItem.Title,
            Details = workItem.Details,
            IsCompleted = workItem.IsCompleted,
        };
    }
}