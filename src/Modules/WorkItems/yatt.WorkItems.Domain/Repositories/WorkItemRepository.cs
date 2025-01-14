using Microsoft.Extensions.DependencyInjection;
using yatt.WorkItems.Api.Models;

namespace yatt.WorkItems.Domain.Repositories;

public interface IWorkItemRepository
{
    Task<List<WorkItem>> GetAllAsync();
    Task<WorkItem?> GetByIdAsync(Guid id);
    Task<bool> TryAddAsync(WorkItem workItem);
    Task<bool> TryUpdateAsync(WorkItem workItem);
    Task DeleteAsync(Guid id);
}

public class WorkItemRepository : IWorkItemRepository
{
    // TODO: replace in-memory collection with proper DbContext/Repository
    private readonly Dictionary<Guid, WorkItem> _workItems;

    public WorkItemRepository([FromKeyedServices("workItemsCollection")] Dictionary<Guid, WorkItem> workItems)
    {
        _workItems = workItems;
    }
    
    public Task<List<WorkItem>> GetAllAsync()
    {
        return Task.FromResult(_workItems.Values.ToList());
    }

    public Task<WorkItem?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_workItems.Values.FirstOrDefault(wi => wi.Id == id));
    }

    public Task<bool> TryAddAsync(WorkItem workItem)
    {
        return Task.FromResult(_workItems.TryAdd(workItem.Id, workItem));
    }

    public Task<bool> TryUpdateAsync(WorkItem workItem)
    {
        if (!_workItems.ContainsKey(workItem.Id))
        {
            return Task.FromResult(false);
        }
        
        _workItems[workItem.Id] = workItem;
        return Task.FromResult(true);
    }

    public Task DeleteAsync(Guid id)
    {
        _workItems.Remove(id);
        return Task.CompletedTask;
    }
}