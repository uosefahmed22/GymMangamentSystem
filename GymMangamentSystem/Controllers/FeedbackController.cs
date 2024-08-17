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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepo _feedbackRepo;

        public FeedbackController(IFeedbackRepo feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }
        [HttpGet("getFeedback")]
        public async Task<IActionResult> GetFeedback(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _feedbackRepo.GetFeedbackById(id);
                if (result == null)
                {
                    return NotFound($"Feedback with id {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [HttpGet("getAllFeedbacks")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var result = await _feedbackRepo.GetAllFeedbacks();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackDto feedbackDto)
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
                    return BadRequest("UserId claim not found in the token");
                }
                feedbackDto.UserId = UserIdClaim.Value;
                var result = await _feedbackRepo.CreateFeedback(feedbackDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _feedbackRepo.DeleteFeedback(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackDto feedbackDto)
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
                    return BadRequest("UserId claim not found in the token");
                }
                var result = await _feedbackRepo.UpdateFeedback(id, feedbackDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
    }
}
