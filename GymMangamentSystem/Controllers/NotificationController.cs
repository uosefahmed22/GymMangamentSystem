using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepo _notification;

        public NotificationController(INotificationRepo notification)
        {
            _notification = notification;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var notifications = await _notification.GetNotifications();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] NotificationDto notification)
        {
            try
            {
                var response = await _notification.AddNotification(notification);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin, Receptionist")]
        [HttpDelete]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var response = await _notification.DeleteNotification(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
