using Microsoft.AspNetCore.Mvc;
using MvcDemo.App.TodoItems;
using MvcDemo.WebSite.Infrastructure;
using MvcDemo.WebSite.Models.Tasks;

namespace MvcDemo.WebSite.Controllers
{
    [Transaction]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _taskRepository.List(User.GetUserId());
            var models = items.Select(x => new TaskListItem
            {
                IsCompleted = x.IsCompleted,
                CreatedAtUtc = x.CreatedAtUtc,
                DaysActive = (int) DateTime.UtcNow.Subtract(x.CreatedAtUtc).TotalDays,
                Title = x.Title
            });
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CreateTask());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTask model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var task = new TodoTask(model.Title, model.Description)
            {
                UserId = User.GetUserId()
            };
            await _taskRepository.Create(task);
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.Get(id);
            await _taskRepository.Delete(task);
            return RedirectToAction(nameof(Index));
        }
    }
}
