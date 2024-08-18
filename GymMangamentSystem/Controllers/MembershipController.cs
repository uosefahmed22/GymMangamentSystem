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
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepo _membershipRepo;

        public MembershipController(IMembershipRepo membershipRepo)
        {
            _membershipRepo = membershipRepo;
        }
        [HttpGet("GetAllMemberships")]
        public async Task<IActionResult> GetAllMemberships()
        {
            try
            {
                var result = await _membershipRepo.GetAllMemberships();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [HttpGet("GetMembershipById")]
        public async Task<IActionResult> GetMembershipById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _membershipRepo.GetMembershipById(id);
                if (result == null)
                {
                    return NotFound($"Membership with id {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpPost]
        public async Task<IActionResult> CreateMembership([FromForm] MembershipDto membership)
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
                membership.UserId = UserIdClaim.Value;
                var result = await _membershipRepo.CreateMembership(membership);
                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpPut]
        public async Task<IActionResult> UpdateMembership(int id ,[FromForm] MembershipDto membership)
            {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _membershipRepo.UpdateMembership(id, membership);
                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _membershipRepo.DeleteMembership(id);
                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }
    }
}
