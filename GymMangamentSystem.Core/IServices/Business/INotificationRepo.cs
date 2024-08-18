using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface INotificationRepo
    {
        Task<IEnumerable<NotificationDto>> GetNotifications();
        Task<ApiResponse> AddNotification(NotificationDto notificationDto);
        Task<ApiResponse> DeleteNotification(int notificationId);
    }
}
