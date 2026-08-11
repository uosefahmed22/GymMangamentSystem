using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface INotificationRepo
    {
        Task<IEnumerable<NotificationDto>> GetNotifications(PaginationParameters? pagination = null);
        Task<ApiResponse> AddNotification(NotificationDto notificationDto);
        Task<ApiResponse> DeleteNotification(int notificationId);
    }
}
