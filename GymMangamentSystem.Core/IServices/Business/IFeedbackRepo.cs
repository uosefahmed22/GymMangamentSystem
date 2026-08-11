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
    public interface IFeedbackRepo
    {
        Task<IEnumerable<FeedbackDto>> GetAllFeedbacks(PaginationParameters? pagination = null);
        Task<FeedbackDto?> GetFeedbackById(int id);
        Task<ApiResponse> CreateFeedback(FeedbackDto feedbackDto);
        Task<ApiResponse> UpdateFeedback(int id, FeedbackDto feedbackDto);
        Task<ApiResponse> DeleteFeedback(int id);

    }
}
