using System.Threading.Tasks;
using System.Web.Http;
using App.Common;
using App.Models;
using App.ViewModels;

namespace App.Controllers
{
   // [Authorize]
    public class TodoItemController : MapEntityController<TodoItem, TodoItemViewModel>
    {
        public async Task<IHttpActionResult> Post(TodoItemViewModel model)
        {     
            if (!ModelState.IsValid)
            {                
                return this.BadRequestError(ModelState);
            }

            if (!await CheckAccess(model.TodoListId))
            {             
                return Unauthorized();
            }

            model = await Add(model);            
            return Ok(model);
        }

        public async Task<IHttpActionResult> Put(TodoItemViewModel model)
        {            
            if (!ModelState.IsValid)
            {                
                return this.BadRequestError(ModelState);
            }

           
            if (!await CheckAccess(model.TodoListId))
            {             
                return Unauthorized();
            }

            model = await Update(model);
            return Ok(model);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {            
            var todoItem = await GetAsync(id);
            if (todoItem == null)
            {         
                return NotFound();
            }

            if (!await CheckAccess(todoItem.TodoListId))
            {             
                return Unauthorized();
            }

            await Remove(id);            
            return Ok();
        }

        public TodoItemController(IAsyncRepository repository)
            :base(repository)
        {            
        }

        protected override void Dispose(bool disposing)
        {         
            base.Dispose(disposing);
        }

        private async Task<bool> CheckAccess(int todoListId)
        {
            var todoList = await Repository.GetAsync<TodoList>(todoListId);
            return todoList.UserId == User.Identity.Name;
        } 

    }
}