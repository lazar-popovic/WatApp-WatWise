using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rad.Data;
using rad.Models;

namespace rad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly DataContext _context;
        public TodoController(DataContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetTodos()
        {
            return Ok(await _context.todos.ToArrayAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Todo>>> CreateTodo(Todo todo)
        {
            _context.todos.Add(todo);
            await _context.SaveChangesAsync();

            return Ok(await _context.todos.ToArrayAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Todo>>> UpdateTodo(Todo todoTemp)
        {
            Todo todo = await _context.todos.FindAsync(todoTemp.Id);

            if (todo == null)
                return BadRequest("Wrong Todo!");

            todo.Title = todoTemp.Title;
            todo.Description = todoTemp.Description;
            await _context.SaveChangesAsync();

            return Ok(await _context.todos.ToArrayAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Todo>>> UpdateTodo(int id)
        {
            Todo todoTemp = await _context.todos.FindAsync(id);

            if (todoTemp == null)
                return BadRequest("Wrong Todo!");


            _context.todos.Remove(todoTemp);
            await _context.SaveChangesAsync();
            
            return Ok(await _context.todos.ToArrayAsync());
        }
    }
}
