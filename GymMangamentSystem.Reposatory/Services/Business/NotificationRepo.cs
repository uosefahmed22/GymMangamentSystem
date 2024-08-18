using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Reposatory.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Business
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public NotificationRepo(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddNotification(NotificationDto notificationDto)
        {
            try
            { 
                var notificationEntity = _mapper.Map<Notification>(notificationDto);
                _context.Add(notificationEntity);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Notification added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteNotification(int notificationId)
        {
            var existingNotification = await _context.Notifications.FindAsync(notificationId);
            if (existingNotification == null || existingNotification.IsDeleted == true)
            {
                return new ApiResponse(404, "Notification not found");
            }
            try
            {
                existingNotification.IsDeleted = true;
                _context.Notifications.Update(existingNotification);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Notification deleted successfully");
            } 
            catch (Exception ex) {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
               
        }
        public async Task<IEnumerable<NotificationDto>> GetNotifications()
        {
            try
            {
                var notifications = await _context.Notifications.Where(x => x.IsDeleted == false).ToListAsync();
                var notificationDtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
                return notificationDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
