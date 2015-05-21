using System.Threading.Tasks;
using System.Web.Http;
using App.Common;
using App.Models;
using App.ViewModels;
using log4net;

namespace App.Controllers
{
   // [Authorize]
    public class TodoItemController : MapEntityController<TodoItem, TodoItemViewModel>
    {
        public async Task<IHttpActionResult> Post(TodoItemViewModel model)
        {
            Log.DebugFormat("Entering Post(model.Title = {0}), User: {1}", model.Title, User.Identity.Name);

            if (!ModelState.IsValid)
            {
                Log.Debug("Leaving Post(): Bad request error");
                return this.BadRequestError(ModelState);
            }

            if (!await CheckAccess(model.TodoListId))
            {
                Log.Debug("Leaving Post(): Unauthorized");
                return Unauthorized();
            }

            model = await Add(model);

            Log.DebugFormat("Leaving Post(): Id={0}", model.Id);
            return Ok(model);
        }

        public async Task<IHttpActionResult> Put(TodoItemViewModel model)
        {
            Log.DebugFormat("Entering Put(model.id={0})", model.Id);
            
            if (!ModelState.IsValid)
            {
                Log.Debug("Leaving Put(): Bad request");
                return this.BadRequestError(ModelState);
            }

           
            if (!await CheckAccess(model.TodoListId))
            {
                Log.Debug("Leaving Put(): Unauthorized");
                return Unauthorized();
            }

            model = await Update(model);

            Log.Debug("Leaving Put()");

            return Ok(model);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            Log.DebugFormat("Entering Delete(id={0})", id);

            var todoItem = await GetAsync(id);
            if (todoItem == null)
            {
                Log.Debug("Leaving Delete(): Not found");
                return NotFound();
            }

            if (!await CheckAccess(todoItem.TodoListId))
            {
                Log.Debug("Leaving Delete(): Unauthorized");
                return Unauthorized();
            }

            await Remove(id);

            Log.Debug("Leaving Delete()");
            return Ok();
        }

        public TodoItemController(IAsyncRepository repository)
            :base(repository)
        {
            Log.Debug("Entering TodoItemController()");
        }

        protected override void Dispose(bool disposing)
        {
            Log.DebugFormat("Entering Dispose(disposing={0})", disposing);
            base.Dispose(disposing);
        }

        private async Task<bool> CheckAccess(int todoListId)
        {
            var todoList = await Repository.GetAsync<TodoList>(todoListId);
            return todoList.UserId == User.Identity.Name;
        } 

        private static readonly ILog Log = LogManager.GetLogger(typeof(TodoItemController));
    }
}