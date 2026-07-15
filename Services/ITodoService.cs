using TodoApp.Models;
namespace TodoApp.Services;

public interface ITodoService
{
    Task<List<TodoItem>> GetAllAsync(string userId);
    Task<TodoItem?> GetByIdAsync(int id, string userId);
    Task<TodoItem> AddAsync(TodoItem item, string userId);
    Task<bool> UpdateAsync(TodoItem item, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> ToggleCompleteAsync(int id, string userId);
}