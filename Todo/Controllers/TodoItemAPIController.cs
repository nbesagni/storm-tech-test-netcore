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

        public TodoItemAPIController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/TodoItemAPI
        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems(int todoListId)
        {
            return dbContext.TodoItems.Where(i => i.TodoListId == todoListId);
        }

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
            int newRank = dbContext.TodoItems.Where(i => i.TodoListId == todoItem.TodoListId).Count() + 1;
            todoItem.Rank = newRank;
            var item = new TodoItem(todoItem.TodoListId, todoItem.ResponsiblePartyId, todoItem.Title, todoItem.Importance, todoItem.Rank);
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