using AutoMapper;
using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices;
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
    public class FeedbackRepo : IFeedbackRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public FeedbackRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> CreateFeedback(FeedbackDto feedbackDto)
        {
            try
            {
                var feedbackEntity = _mapper.Map<Feedback>(feedbackDto);
                _context.Add(feedbackEntity);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "Feedback added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            } 
        }
        public async Task<ApiResponse> DeleteFeedback(int id)
        {
            var exsisitingFeedback = await _context.Feedbacks.FindAsync(id);
            if (exsisitingFeedback == null)
            {
                return new ApiResponse(404, "Feedback not found");
            }
            try
            {
                _context.Feedbacks.Remove(exsisitingFeedback);
                _context.Update(exsisitingFeedback);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Feedback deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacks()
        {
            try
            {
                var feedbacks =await _context.Feedbacks.ToListAsync();
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
                return feedbacksDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<FeedbackDto> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return null;
                }

                var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
                return feedbackDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateFeedback(int id, FeedbackDto feedbackDto)
        {
            var exsisitingFeedback = await _context.Feedbacks.FindAsync(id);
            if (exsisitingFeedback == null)
            {
                return new ApiResponse(404, "Feedback not found");
            }
            try
            {
                exsisitingFeedback.Comments = feedbackDto.Comments;
                exsisitingFeedback.Rating = feedbackDto.Rating;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Feedback updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error: " + ex.Message);
            }
        }

    }
}
