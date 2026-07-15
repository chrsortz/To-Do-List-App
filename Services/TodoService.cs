using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
namespace TodoApp.Services;

public class TodoService : ITodoService
{
    private readonly ApplicationDbContext _context;

    public TodoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetAllAsync(string userId)
    {
        return await _context.Todos
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.IsCompleted)
            .ThenByDescending(i => i.Priority)
            .ThenBy(i => i.DueDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id, string userId)
    {
        return await _context.Todos
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
    }

    public async Task<TodoItem> AddAsync(TodoItem item, string userId)
    {
        item.UserId = userId;
<<<<<<< HEAD
        item.CreatedAt = DateTime.UtcNow;
=======
        item.CreatedAt = DateTime.Now;
>>>>>>> teammate/master
        _context.Todos.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateAsync(TodoItem item, string userId)
    {
        var existing = await _context.Todos
            .FirstOrDefaultAsync(i => i.Id == item.Id && i.UserId == userId);
        if (existing is null) return false;

        existing.Title = item.Title;
        existing.Notes = item.Notes;
        existing.Priority = item.Priority;
        existing.DueDate = item.DueDate;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var existing = await _context.Todos
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (existing is null) return false;

        _context.Todos.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleCompleteAsync(int id, string userId)
    {
        var existing = await _context.Todos
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (existing is null) return false;

        existing.IsCompleted = !existing.IsCompleted;
<<<<<<< HEAD
        existing.CompletedAt = existing.IsCompleted ? DateTime.UtcNow : null;
=======
        existing.CompletedAt = existing.IsCompleted ? DateTime.Now : null;
>>>>>>> teammate/master
        await _context.SaveChangesAsync();
        return true;
    }
}