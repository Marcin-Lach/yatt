using yatt.WorkItems.Api.Models;

namespace yatt.WorkItems.Domain.Tests;

public class WorkItemTests
{
    [Test]
    public async Task Given_IncompletedWorkItem_When_MarkingAsComplete_Should_BeComplete()
    {
        var wi = CreateIncompletedWorkItem();

        wi.MarkAsComplete();

        await Assert.That(wi.IsCompleted).IsTrue();
    }

    [Test]
    public async Task Given_CompletedWorkItem_When_MarkingAsComplete_Should_BeComplete()
    {
        var wi = CreateCompletedWorkItem();

        wi.MarkAsComplete();

        await Assert.That(wi.IsCompleted).IsTrue();
    }

    [Test]
    public async Task Given_CompletedWorkItem_When_MarkingAsIncomplete_Should_BeIncomplete()
    {
        var wi = CreateCompletedWorkItem();

        wi.MarkAsIncomplete();

        await Assert.That(wi.IsCompleted).IsFalse();
    }
    
    [Test]
    public async Task Given_IncompletedWorkItem_When_MarkingAsIncomplete_Should_BeIncomplete()
    {
        var wi = CreateIncompletedWorkItem();

        wi.MarkAsIncomplete();

        await Assert.That(wi.IsCompleted).IsFalse();
    }

    private static WorkItem CreateIncompletedWorkItem()
    {
        var wi = new WorkItem()
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Details = "dets"
        };
        return wi;
    }

    private static WorkItem CreateCompletedWorkItem()
    {
        var wi = CreateIncompletedWorkItem();
        wi.MarkAsComplete();
        return wi;
    }
}