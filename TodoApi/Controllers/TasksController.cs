using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using System.Collections.Generic;
using System.Linq;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly APIContext _context;

        public TaskController(APIContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public JsonResult GetTasks([FromQuery] bool? isCompleted = null)
        {
            IQueryable<MyTask> query = _context.Tasks;

            // Filter tasks by completion status if provided
            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }

            var tasks = query.ToList();
            return new JsonResult(tasks);
        }

        
        [HttpGet("{id}")]
        public JsonResult GetOneTask(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task == null)
            {
                return new JsonResult(NotFound(new { message = "Task not found" }));
            }

            return new JsonResult(task);
        }

        
        [HttpPost]
        public JsonResult CreateTask([FromBody] MyTask task)
        {
           
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return new JsonResult(BadRequest(new { message = "Task title cannot be empty" }));
            }

            
            task.IsCompleted ??= false;

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return new JsonResult(CreatedAtAction(nameof(GetOneTask), new { id = task.Id }, task));
        }

        
        [HttpPut("{id}")]
        public JsonResult UpdateTask(int id, [FromBody] MyTask task)
        {
            
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return new JsonResult(BadRequest(new { message = "Task title cannot be empty" }));
            }

            var taskInDb = _context.Tasks.Find(id);
            if (taskInDb == null)
            {
                return new JsonResult(NotFound(new { message = "Task not found" }));
            }

            
            taskInDb.Title = task.Title;
            taskInDb.IsCompleted = task.IsCompleted;

            _context.SaveChanges();
            return new JsonResult(NoContent());
        }

        
        [HttpDelete("{id}")]
        public JsonResult DeleteTask(int id)
        {
            var taskInDb = _context.Tasks.Find(id);
            if (taskInDb == null)
            {
                return new JsonResult(NotFound(new { message = "Task not found" }));
            }

            _context.Tasks.Remove(taskInDb);
            _context.SaveChanges();
            return new JsonResult(Ok(new { message = "Task deleted successfully" }));
        }
    }
}
