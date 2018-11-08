using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Todo.EntityModelMappers.TodoLists;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemAPIController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;

        public TodoItemAPIController(ApplicationDbContext dbContext, IUserStore<IdentityUser> userStore)
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
        }

        // GET: api/TodoItemAPI
        //[HttpGet]
        //public IEnumerable<TodoItem> GetTodoItems()
        //{
        //    return dbContext.TodoItems;
        //}

        //// GET: api/TodoItemAPI/5
        //[HttpGet("{todoListId}")]
        //public string GetTodoItemsJson([FromRoute] int todoListId)
        //{
        //    var todoList = dbContext.SingleTodoList(todoListId);
        //    var todoListJson = JsonConvert.SerializeObject(TodoListDetailViewmodelFactory.Create(todoList));

        //    return todoListJson;
        //}

       

        // POST: api/TodoItemAPI
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromBody] Models.TodoItems.TodoItemCreateFields todoItem)
        {
            var item = new TodoItem(todoItem.TodoListId, todoItem.ResponsiblePartyId, todoItem.Title, todoItem.Importance);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.TodoItems.Add(item);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = item.TodoItemId }, todoItem);
        }



        private bool TodoItemExists(int id)
        {
            return dbContext.TodoItems.Any(e => e.TodoItemId == id);
        }
    }
}