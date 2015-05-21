using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    /// <summary>
    /// Todo list entity
    /// </summary>
    public class TodoList
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        public virtual List<TodoItem> Todos { get; set; }
    }
}