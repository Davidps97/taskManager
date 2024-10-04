using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tasks
    [HttpGet]
    public IActionResult GetUserTasks()
    {
        var userId = int.Parse(User.Identity.Name);
        var tasks = _context.Tasks.Where(t => t.UserId == userId).ToList();
        return Ok(tasks);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
    {
        taskItem.UserId = int.Parse(User.Identity.Name);
        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync();
        return Ok(taskItem);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null || task.UserId != int.Parse(User.Identity.Name))
        {
            return NotFound(new { message = "Tarea no encontrada o no tienes permisos." });
        }

        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.DueDate = updatedTask.DueDate;
        task.IsCompleted = updatedTask.IsCompleted;

        await _context.SaveChangesAsync();
        return Ok(task);
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null || task.UserId != int.Parse(User.Identity.Name))
        {
            return NotFound(new { message = "Tarea no encontrada o no tienes permisos." });
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Tarea eliminada exitosamente." });
    }
}
