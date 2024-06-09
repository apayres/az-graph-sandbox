using GraphSandbox.Web.Models;
using GraphSandbox.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace GraphSandbox.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoService _service;

        public ToDoController(IToDoService service)
        {
            _service = service;
        }

        [AuthorizeForScopes(Scopes = ["tasks.readwrite"])]
        public async Task<IActionResult> Index(string? id)
        {
            var model = await _service.LoadTaskLists(id);
            return View(model);
        }

        [HttpPost]
        [AuthorizeForScopes(Scopes = ["tasks.readwrite"])]
        public async Task<IActionResult> Index(ToDoModel model)
        {
            ModelState.Clear();
            model = await _service.CreateTask(model);
            model.ToDoItem = string.Empty;
            return View(model);
        }

        [HttpPost]
        [AuthorizeForScopes(Scopes = ["tasks.readwrite"])]
        public IActionResult UpdateTaskStatus(string taskListID, string taskID, bool complete)
        {
            _service.UpdateTaskStatus(taskListID, taskID, complete);
            return Ok();
        }
    }
}
