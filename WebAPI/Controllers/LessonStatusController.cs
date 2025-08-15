using Microsoft.AspNetCore.Mvc;

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
