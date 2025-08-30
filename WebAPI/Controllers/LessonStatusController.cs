using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;

namespace WebAPI.Controllers
{
    public class LessonStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
