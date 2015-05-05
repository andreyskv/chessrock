using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    /// <summary>
    /// Todo item entity
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }

        [Required, MaxLength(40)]
        public string Title { get; set; }
        public bool IsDone { get; set; }

        [ForeignKey("TodoList")]
        public int TodoListId { get; set; }
        public virtual TodoList TodoList { get; set; }
    }
}