using System.ComponentModel;

namespace yatt.WorkItems.Api.Models;

public record UpdateWorkItemRequestModel
{
    [Description("Identifier of the work item")]
    public required Guid Id { get; init; }
    [Description("Brief description of the work item")]
    public required string Title { get; init; }
    [Description("Detailed description of the work item")]
    public string? Details { get; init; }
}

public static class UpdateWorkItemMapping
{
    public static WorkItem ToWorkItem(this UpdateWorkItemRequestModel responseModel)
    {
        return new WorkItem
        {
            Id = responseModel.Id,
            Title = responseModel.Title,
            Details = responseModel.Details,
        };
    }
}