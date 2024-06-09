using Microsoft.AspNetCore.Mvc;

namespace GraphSandbox.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.User;
            
            return View();
        }
    }
}
