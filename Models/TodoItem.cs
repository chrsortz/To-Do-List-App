using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public enum Priority
{
    Low,
    Normal,
    High
}

public class TodoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter a task title")]
    [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Notes { get; set; }

    public bool IsCompleted { get; set; }

    public Priority Priority { get; set; } = Priority.Normal;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? CompletedAt { get; set; }

    public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value.Date < DateTime.Today;

    public string? UserId { get; set; }
}
