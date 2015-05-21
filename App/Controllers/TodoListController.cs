using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using App.Common;
using App.Models;
using App.ViewModels;
using AutoMapper.QueryableExtensions;
using log4net;

namespace App.Controllers
{
   // [Authorize]
    [RoutePrefix("api/TodoList")]
    public class TodoListController: MapEntityController<TodoList, TodoListViewModel>
    {
        [Queryable]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            Log.Debug("Entering Get()");

            var todoLists = await Query(td => td.UserId == User.Identity.Name)
                .ToArrayAsync();
            
            Log.DebugFormat("Leaving Get(): Count={0}", todoLists.Length);

            return Ok(todoLists);
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            Log.DebugFormat("Entering Get(id={0})", id);

            var todoList = await GetAsync(id);

            if (todoList == null)
            {
                Log.Debug("Leaving Get(): Not Found");
                return NotFound();
            }

            Log.DebugFormat("Leaving Get(): Id={0}", todoList.Id);

            return Ok(todoList);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(TodoListViewModel model)
        {
            Log.DebugFormat("Entering Post(model.Title = {0})", model.Title);

            if (!ModelState.IsValid)
            {
                Log.Debug("Leaving Post(): Bad request");
                return this.BadRequestError(ModelState);
            }

            var todoList = GetEntityObject(model);
            todoList.UserId = User.Identity.Name;

            Repository.Add(todoList);
            await SaveChangesAsync();
            
            var result = GetDataObject(todoList);

            Log.DebugFormat("Leaving Post(): Id={0}", result.Id);

            return Ok(result);
        }

        [Queryable]
        [HttpGet, Route("{id:int}/Todos")]
        public async Task<IHttpActionResult> Todos(int id)
        {
            Log.DebugFormat("Entering Todos(id={0})", id);

            var todoList = await Repository.GetAsync<TodoList>(id);

            if (todoList == null)
            {
                Log.Debug("Leaving Todos(): Not found");
                return NotFound();
            }

            if (todoList.UserId != User.Identity.Name)
            {
                Log.Debug("Leaving Todos(): Unauthorized");
                return Unauthorized();
            }

            var todos = await Repository.Query<TodoItem>()
                .Where(x => x.TodoListId == id)
                .Project().To<TodoItemViewModel>()
                .ToArrayAsync();

            Log.DebugFormat("Leaving Todos(): Count={0}", todos.Length);

            return Ok(todos);
        }

        [HttpDelete, Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Log.DebugFormat("Entering Delete(id={0})", id);

            var todoList = await Repository.GetAsync<TodoList>(id);
            if (todoList == null)
            {
                Log.Debug("Leaving Delete(): Not found");
                return NotFound();
            }

            if (todoList.UserId != User.Identity.Name)
            {
                Log.Debug("Leaving Delete(): Unauthorized");
                return Unauthorized();
            }

            await Remove(id);

            Log.Debug("Leaving Delete()");
            return Ok();
        }

        public TodoListController(IAsyncRepository repository)
            :base(repository)
        {
            Log.Debug("Entering TodoListController()");
        }

        protected override void Dispose(bool disposing)
        {
            Log.DebugFormat("Entering Dispose(disposing={0})", disposing);
            base.Dispose(disposing);
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(TodoListController));
    }
}