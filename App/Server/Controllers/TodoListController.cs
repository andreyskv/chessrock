using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using App.Common;
using App.Models;
using App.ViewModels;
using AutoMapper.QueryableExtensions;
using App.Repository;

namespace App.Controllers
{
   // [Authorize]
    [RoutePrefix("api/TodoList")]
    public class TodoListController: MapEntityController<TodoList, TodoListViewModel>
    {        
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            
            var todoLists = await Query(td => td.UserId == User.Identity.Name).ToArrayAsync();            
            return Ok(todoLists);
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {            
            var todoList = await GetAsync(id);
            if (todoList == null)
            {                
                return NotFound();
            }
            return Ok(todoList);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(TodoListViewModel model)
        {            
            if (!ModelState.IsValid)
            {         
                return this.BadRequestError(ModelState);
            }

            var todoList = GetEntityObject(model);
            todoList.UserId = User.Identity.Name;

            Repository.Add(todoList);
            await SaveChangesAsync();
            
            var result = GetDataObject(todoList);            
            return Ok(result);
        }
     
        [HttpGet, Route("{id:int}/Todos")]
        public async Task<IHttpActionResult> Todos(int id)
        {            
            var todoList = await Repository.GetAsync<TodoList>(id);
            if (todoList == null)
            {             
                return NotFound();
            }

            if (todoList.UserId != User.Identity.Name)
            {                
                return Unauthorized();
            }

            var todos = await Repository.Query<TodoItem>().Where(x => x.TodoListId == id).Project().To<TodoItemViewModel>().ToArrayAsync();
            return Ok(todos);
        }

        [HttpDelete, Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {            
            var todoList = await Repository.GetAsync<TodoList>(id);
            if (todoList == null)
            {         
                return NotFound();
            }

            if (todoList.UserId != User.Identity.Name)
            {             
                return Unauthorized();
            }

            await Remove(id);
            return Ok();
        }

        public TodoListController(IAsyncRepository repository)
            :base(repository)
        {            
        }

        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }
        
    }
}