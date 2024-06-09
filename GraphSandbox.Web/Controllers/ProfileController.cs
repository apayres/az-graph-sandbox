using GraphSandbox.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace GraphSandbox.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [AuthorizeForScopes(Scopes = ["user.read"])]
        public async Task<IActionResult> Index()
        {
            var model = await _profileService.LoadProfile();
            return View(model);
        }
    }
}
