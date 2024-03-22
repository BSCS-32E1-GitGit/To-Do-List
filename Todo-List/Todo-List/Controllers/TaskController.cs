using Microsoft.AspNetCore.Mvc;
using Todo_List.Models;
using System.Linq;

public class TaskController : Controller
{
    private readonly TaskDbContext _context;

    public TaskController(TaskDbContext context)
    {
        _context = context;
    }

    // Action method to display tasks
    public IActionResult Index()
    {
        var tasks = _context.Tasks.ToList();
        return View(tasks);
    }

    // Action method to create a new task
    [HttpGet] // Add [HttpGet] attribute to specify that this action responds to GET requests
    public IActionResult Create()
    {
        var newTask = new TaskItem
        {
            TaskName = "Name", // Initialize the TaskName property
            TaskDescription = "Description" // Initialize the TaskDescription property
        };
        return View(newTask); // Pass the newTask object to the view
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(task);
    }

    // Action method to delete a task
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // Action method to edit a task
    [HttpGet] // Specify that this action responds to GET requests
    public IActionResult Edit(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound(); // Return 404 Not Found if the task is not found
        }
        return View(task);
    }

    [HttpPost]
    public IActionResult Edit(TaskItem task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(task);
    }

    [HttpPost]
    public IActionResult MarkAsDone(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task != null)
        {
            task.TaskStatus = true;
            if (task.CompletionTime == null) // Check if CompletionTime is null
            {
                task.CompletionTime = DateTime.Now; // Set the completion time to the current date and time
            }
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}