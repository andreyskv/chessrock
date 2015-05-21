using App.Models;
using System.Data.Entity;

namespace App.Repository
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, add the following
    // code to the Application_Start method in your Global.asax file.
    // Note: this will destroy and re-create your database with every model change.
    // 
    // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcApplication2.Models.TodoItemContext>());

    public class AppRepository : EfRepository<AppContext>
    {
    }

    public class AppContext : DbContext
    {
        public AppContext()
            : base("name=DefaultConnection")
        {
        }

        static AppContext()
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
    }

    public class DataInitializer : DropCreateDatabaseAlways<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            var list = new TodoList { Title = "tasks for tomorrow", UserId = "jcyamacho" };
            context.TodoLists.Add(list);
            list = new TodoList { Title = "tasks for today", UserId = "jcyamacho" };
            context.TodoLists.Add(list);

            context.TodoItems.Add(new TodoItem { Title = "example task 1", TodoList = list });
            context.TodoItems.Add(new TodoItem { Title = "example task 2", TodoList = list });

            context.SaveChanges();
        }
    }
}