using System.Data.Entity;

namespace App.Models
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, add the following
    // code to the Application_Start method in your Global.asax file.
    // Note: this will destroy and re-create your database with every model change.
    // 
    // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcApplication2.Models.TodoItemContext>());
    public class TodoItemContext : DbContext
    {
        public TodoItemContext()
            : base("name=DefaultConnection")
        {
        }

        static TodoItemContext()
        {
            Database.SetInitializer(new TodoItemInitializer());
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
    }
}