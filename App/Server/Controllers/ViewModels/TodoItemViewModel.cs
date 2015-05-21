using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class TodoItemViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public bool IsDone { get; set; }
        
        [Required]
        public int TodoListId { get; set; }
    }
}