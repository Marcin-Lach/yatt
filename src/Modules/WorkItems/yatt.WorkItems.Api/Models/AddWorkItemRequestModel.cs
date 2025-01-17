using System.ComponentModel;

namespace yatt.WorkItems.Api.Models;

public record AddWorkItemRequestModel
{
    [Description("Identifier of the work item")]
    public required Guid Id { get; init; }
    [Description("Brief description of the work item")]
    public required string Title { get; init; }
    [Description("Detailed description of the work item")]
    public string? Details { get; init; }
}

public static class AddWorkItemMapping
{
    public static WorkItem ToWorkItem(this AddWorkItemRequestModel responseModel)
    {
        return new WorkItem
        {
            Id = responseModel.Id,
            Title = responseModel.Title,
            Details = responseModel.Details,
        };
    }
}