using System;
using System.ComponentModel.DataAnnotations;

public class TaskItem
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; } = false;

    public int UserId { get; set; }
    public User User { get; set; }
}
