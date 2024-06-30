using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskMangementSystem.Models;

namespace TaskMangementSystem.Controllers
{
    [Authorize]
    public class TasksController(DatabaseHelper dbHelper, UserManager<IdentityUser> userManager) : Controller
    {
        private readonly DatabaseHelper _dbHelper = dbHelper;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Index(string? status, string? dueDate)
        {
            var userId = _userManager.GetUserId(User);
            List<TaskModel> tasks = await _dbHelper.GetTasks(userId);

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                bool isComplete = Convert.ToBoolean(status == "Complete");
                tasks = tasks.Where(t => t.IsComplete == isComplete).ToList();
            }

            if (!string.IsNullOrEmpty(dueDate))
            {
                tasks = tasks.Where(t => t.DueDate == Convert.ToDateTime(dueDate)).ToList();
            }

            ViewBag.Status = status;
            ViewBag.DueDate = dueDate;


            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = _userManager.GetUserId(User);
                await _dbHelper.CreateTask(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var task = await _dbHelper.GetTaskById(id.Value, userId);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskModel task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                task.UserId = _userManager.GetUserId(User);
                await _dbHelper.UpdateTask(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var task = await _dbHelper.GetTaskById(id.Value, userId);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _dbHelper.DeleteTask(id, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var task = await _dbHelper.GetTaskById(id.Value, userId);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        public async Task<IActionResult> MarkComplete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            await _dbHelper.MarkTaskComplete(id.Value, userId);

            return RedirectToAction(nameof(Index));
        }
    }

}
