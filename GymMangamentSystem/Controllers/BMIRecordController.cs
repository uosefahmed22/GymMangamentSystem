using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMIRecordController : ControllerBase
    {
        private readonly IBMIRecordRepo _bMIRecordRepo;

        public BMIRecordController(IBMIRecordRepo bMIRecordRepo)
        {
            _bMIRecordRepo = bMIRecordRepo;
        }
        [Authorize]
        [HttpGet("GetBMIRecordsForUser")]
        public async Task<IActionResult> GetBMIRecordsForUser()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (UserIdClaim == null)
                {
                    return BadRequest("UserIdClaim claim not found in the token");
                }
                var result = await _bMIRecordRepo.GetBMIRecordsForUser(UserIdClaim.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize]
        [HttpGet("GetBMIRecord")]
        public async Task<IActionResult> GetBMIRecord(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _bMIRecordRepo.GetBMIRecordById(id);
                if (result == null)
                {
                    return NotFound($"BMI record with id {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize]
        [HttpPost("AddBMIRecord")]
        public async Task<IActionResult> AddBMIRecord(BMIRecordDto bmiRecord)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (UserIdClaim == null)
                {
                    return BadRequest("UserIdClaim claim not found in the token");
                }
                bmiRecord.UserId = UserIdClaim.Value;

                var result = await _bMIRecordRepo.AddBMIRecord(bmiRecord);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize]
        [HttpDelete("DeleteBMIRecord")]
        public async Task<IActionResult> DeleteBMIRecord(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _bMIRecordRepo.DeleteBMIRecord(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
    }
}
