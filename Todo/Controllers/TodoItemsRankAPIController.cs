using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Newtonsoft.Json;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsRankAPIController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemsRankAPIController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/TodoItemsRankAPI
        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems(int todoListId)
        {
            return dbContext.TodoItems.Where(i => i.TodoListId == todoListId);
        }

        // GET: api/TodoItemsRankAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoItem = await dbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // PUT: api/TodoItemsRankAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem([FromRoute] int id, [FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todoItem.TodoItemId)
            {
                return BadRequest();
            }

            dbContext.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public class JsonRankData
        {
            public int todoListItemId { get; set; }
            public int newRank { get; set; }
        }

        // POST: api/TodoItemsRankAPI
        [HttpPost]
        public ActionResult SaveTodoItemRanking([FromBody] JsonRankData jsonRankData)
        {
            TodoItem li = dbContext.TodoItems.Where(i => i.TodoItemId == jsonRankData.todoListItemId).First();
            if (!ModelState.IsValid)
            {
                throw new Exception("Model not valid");
            }
            MoveToDoItemRankings(li, jsonRankData.newRank, dbContext);
            // dbContext.SaveChanges();
            //Data.Entities.TodoItem todoItems = new TodoItem();
            //dbContext.TodoItems.Add(todoItem);
            //await dbContext.SaveChangesAsync();
            return Ok();
            //return CreatedAtAction("GetTodoItem", new { id = todoItem.TodoItemId }, todoItem);
        }

        public static void MoveToDoItemRankings(TodoItem li, int newRank, ApplicationDbContext dbContext)
        {
        
        var items = dbContext.TodoItems
                       .Where(i => (i.TodoListId == li.TodoListId) &&
                             (i.Rank > li.Rank) &&
                             (i.Rank != 1000) &&
                              (i.TodoItemId != li.TodoItemId));
            foreach (var item in items)
                item.Rank--;
            dbContext.SaveChanges();


            // move items above new position up
            items = dbContext.TodoItems
                .Where(i => (i.TodoListId == li.TodoListId) &&
                    (i.Rank >= newRank) &&
                    (i.Rank != 1000) &&
                    (i.TodoItemId != li.TodoItemId));

            foreach (var item in items)
                item.Rank++;

            dbContext.SaveChanges();
            // set the position of the item being dragged
            li.Rank = newRank;
            dbContext.SaveChanges();


        }


        private bool TodoItemExists(int id)
        {
            return dbContext.TodoItems.Any(e => e.TodoItemId == id);
        }
    }
}