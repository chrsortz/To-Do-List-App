using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ITodoService _todoService;

    public IndexModel(ITodoService todoService)
    {
        _todoService = todoService;
    }

    private string UserID => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public List<TodoItem> Todos { get; set; } = new();

    [BindProperty]
    public TodoItem NewTodo { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Filter { get; set; } = "all";

    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int CompletedCount { get; set; }

    public async Task OnGetAsync()
    {
        await LoadTodosAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        ModelState.Clear();
        if (string.IsNullOrWhiteSpace(NewTodo.Title))
        {
            ModelState.AddModelError("NewTodo.Title", "Please enter a task title");
            await LoadTodosAsync();
            return Page();
        }

        await _todoService.AddAsync(new TodoItem
        {
            Title = NewTodo.Title.Trim(),
            Notes = NewTodo.Notes,
            Priority = NewTodo.Priority,
            DueDate = NewTodo.DueDate
        }, UserID);

        return RedirectToPage(new { Filter });
    }

    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        await _todoService.ToggleCompleteAsync(id, UserID);
        return RedirectToPage(new { Filter });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _todoService.DeleteAsync(id, UserID);
        return RedirectToPage(new { Filter });
    }

    public async Task<IActionResult> OnPostEditAsync(int id, string title, string? notes, Priority priority, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return RedirectToPage(new { Filter });
        }

        await _todoService.UpdateAsync(new TodoItem
        {
            Id = id,
            Title = title.Trim(),
            Notes = notes,
            Priority = priority,
            DueDate = dueDate
        }, UserID);

        return RedirectToPage(new { Filter });
    }

    private async Task LoadTodosAsync()
    {
        var all = await _todoService.GetAllAsync(UserID);
        TotalCount = all.Count;
        ActiveCount = all.Count(t => !t.IsCompleted);
        CompletedCount = all.Count(t => t.IsCompleted);
        Todos = Filter switch
        {
            "active" => all.Where(t => !t.IsCompleted).ToList(),
            "completed" => all.Where(t => t.IsCompleted).ToList(),
            _ => all
        };
    }
}