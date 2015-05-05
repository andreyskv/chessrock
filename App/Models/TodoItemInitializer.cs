using System.Data.Entity;

namespace App.Models
{
    public class TodoItemInitializer : DropCreateDatabaseAlways<TodoItemContext>
    {
        protected override void Seed(TodoItemContext context)
        {
            var list = new TodoList { Title = "tasks for tomorrow", UserId = "jcyamacho" };
            context.TodoLists.Add(list);
            list = new TodoList {Title = "tasks for today", UserId = "jcyamacho"};
            context.TodoLists.Add(list);

            context.TodoItems.Add(new TodoItem { Title = "example task 1", TodoList = list});
            context.TodoItems.Add(new TodoItem { Title = "example task 2", TodoList = list });

            context.SaveChanges();
        }
    }
}