using GraphSandbox.Web.Models;
using GraphSandbox.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace GraphSandbox.Web.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _service;

        public CalendarController(ICalendarService service)
        {
            _service = service;
        }

        [AuthorizeForScopes(Scopes = ["calendars.readwrite"])]
        public async Task<IActionResult> Index(string? id)
        {
            var model = await _service.LoadCalendarEvents(id);
            return View(model);
        }

        [HttpPost]
        [AuthorizeForScopes(Scopes = ["calendars.readwrite"])]
        public async Task<IActionResult> Index(CalendarModel model)
        {
            ModelState.Clear();
            model = await _service.SaveCalendarEvent(model);
            return View(model);
        }

        [HttpPost]
        [AuthorizeForScopes(Scopes = ["calendars.readwrite"])]
        public IActionResult DeleteEvent(string id)
        {
            _service.DeleteCalendarEvent(id);
            return Ok();
        }
    }
}
