using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Entities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PupilsController : ControllerBase
    {
        private readonly ApplicationDBContext _dBContext;
        public PupilsController(ApplicationDBContext dBContext)
        {
            this._dBContext = dBContext;
        }

        [HttpGet]
        public IActionResult GetPupils()
        {
            var allPupils = _dBContext.Pupils.ToList();
            return Ok(allPupils);
        }


        [HttpGet("student")]
        public IActionResult GetPupil([FromQuery] Guid userId)
        {
            var pupil = _dBContext.Pupils.FirstOrDefault(p => p.UserId == userId.ToString());

            if (pupil == null)
            {
                return NotFound("There isn't any student with this id");
            }
            pupil = new Pupil
            {
                Id = pupil.Id,
                FirstName = pupil.FirstName,
                LastName = pupil.LastName,
                Email = pupil.Email,
                Phone = pupil.Phone,
                UserId = pupil.UserId
            };

            return Ok(pupil);
        }

        [HttpPost]
        public IActionResult AddPupil(AddPupilDto pupil)
        {
            if (pupil == null)
            {
                return BadRequest("Student cannot be null");
            }
            var newPupil = new Pupil
            {
                FirstName = pupil.FirstName,
                LastName = pupil.LastName,
                Email = pupil.Email,
                Phone = pupil.Phone,
            };
            _dBContext.Pupils.Add(newPupil);
            _dBContext.SaveChanges();
            return Ok(newPupil);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePupil(Guid id, UpdatePupilDto pupil)
        {
            var dbPupil = _dBContext.Pupils.Find(id);
            if (dbPupil is null)
            {
                return NotFound("There isn't any pupil with this id");
            }

            dbPupil.FirstName = pupil.FirstName;
            dbPupil.LastName = pupil.LastName;
            dbPupil.Email = pupil.Email;
            dbPupil.Phone = pupil.Phone;

            _dBContext.SaveChanges();

            return Ok(dbPupil);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePupil(Guid id)
        {
            var pupil = _dBContext.Pupils.Find(id);
            if (pupil is null)
            {
                return NotFound("There isn't any pupil with this id");
            }
            _dBContext.Pupils.Remove(pupil);
            _dBContext.SaveChanges();

            return Ok(pupil);
        }
    }
}
