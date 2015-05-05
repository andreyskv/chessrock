using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class TodoListViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
    }
}