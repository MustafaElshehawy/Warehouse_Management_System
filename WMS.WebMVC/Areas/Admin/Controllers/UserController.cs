using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
