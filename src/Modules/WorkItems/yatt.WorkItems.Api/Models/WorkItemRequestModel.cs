using System.ComponentModel;

namespace yatt.WorkItems.Api.Models;

public record WorkItemRequestModel
{
    [Description("Identifier of the work item")]
    public required Guid Id { get; init; }
    [Description("Brief description of the work item")]
    public required string Title { get; init; }
    [Description("Detailed description of the work item")]
    public string? Details { get; init; }
}

public static class WorkItemMapping
{
    public static WorkItemRequestModel ToRequestModel(this WorkItem workItem)
    {
        return new WorkItemRequestModel
        {
            Id = workItem.Id,
            Title = workItem.Title,
            Details = workItem.Details,
        };
    }

    public static WorkItem ToWorkItem(this WorkItemRequestModel requestModel)
    {
        return new WorkItem
        {
            Id = requestModel.Id,
            Title = requestModel.Title,
            Details = requestModel.Details,
        };
    }
}