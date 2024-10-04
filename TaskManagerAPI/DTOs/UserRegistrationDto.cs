using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.DTOs // Cambia YourNamespace
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public List<TaskItemDto> Tasks { get; set; } 
    }

    public class TaskItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
