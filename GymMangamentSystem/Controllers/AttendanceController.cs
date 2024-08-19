using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendaceRepo _attendaceRepo;

        public AttendanceController(IAttendaceRepo attendaceRepo)
        {
            _attendaceRepo = attendaceRepo;
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpGet("getattendances")]
        public async Task<IActionResult> GetAttendances(string userCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var IsExsistUser = await _attendaceRepo.GetAttendancesForUser(userCode);
            if (IsExsistUser == null)
            {
                return NotFound();
            }
            return Ok(IsExsistUser);
        }

        [Authorize(Roles = "Admin, Receptionist")]
        [HttpPost]
        public async Task<IActionResult> AddAttendance(AttendanceDto attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _attendaceRepo.AddAttendance(attendance);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _attendaceRepo.DeleteAttendance(id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}