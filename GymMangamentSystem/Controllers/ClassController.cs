using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassRepo _classRepo;

        public ClassController(IClassRepo classRepo)
        {
            _classRepo = classRepo;
        }

        [HttpGet("GetClass")]
        public async Task<IActionResult> GetClass(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _classRepo.GetClass(id);
                if (result == null)
                {
                    return NotFound($"Class with id {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }

        [HttpGet("GetAllClasses")]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                var result = await _classRepo.GetClasses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }

        [Authorize(Roles = "Admin, Receptionist")]
        [HttpPost]
        public async Task<IActionResult> AddClass(ClassDto classDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trainerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TrainerId");
            if (trainerIdClaim == null)
            {
                return BadRequest("TrainerId claim not found in the token");
            }
            classDto.TrainerId = trainerIdClaim.Value;
            var response = await _classRepo.AddClass(classDto);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize(Roles = "Admin, Receptionist")]
        [HttpDelete]
        public async Task<IActionResult> DeleteClass(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _classRepo.DeleteClass(id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize(Roles = "Admin, Receptionist")]
        [HttpPut]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] ClassDto classDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _classRepo.UpdateClass(id, classDto);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
