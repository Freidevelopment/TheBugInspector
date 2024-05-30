using TheBugInspector.Models;

namespace TheBugInspector.Services.Interfaces
{
    public interface ITaskerItemRepository
    {
        Task<IEnumerable<TaskerItem>> GetTaskerItemsAsync(string userId);
        Task<TaskerItem> CreateTaskerItemAsync(TaskerItem item);
        Task DeleteDbTaskerItemAsync(Guid Id, string userId);
        Task<TaskerItem> GetTaskerItemByIdAsync(Guid Id, string userId);
        Task UpdateTaskerItemAsync(Guid Id, TaskerItem item, string userId);
    }
}
