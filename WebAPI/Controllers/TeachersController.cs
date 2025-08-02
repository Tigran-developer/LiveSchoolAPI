using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ApplicationDBContext _dBContext;
        public TeachersController(ApplicationDBContext dBContext)
        {
            this._dBContext = dBContext;
        }
        /*[HttpGet]
        public IActionResult GetTeachers()
        {
            var allTeachers = _dBContext.Teachers.ToList();
            return Ok(allTeachers);
        }*/
    }
}
