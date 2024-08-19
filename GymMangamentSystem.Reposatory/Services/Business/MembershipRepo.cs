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
    public class MembershipRepo : IMembershipRepo
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public MembershipRepo(AppDBContext context, IMapper mapper, IImageService fileService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = fileService;
        }
        public async Task<ApiResponse> CreateMembership(MembershipDto membership)
        {
            //var exsistingMembership = await _context.Memberships.FirstOrDefaultAsync(x => x.UserId == membership.UserId);
            //if (exsistingMembership != null)
            //{
            //    return new ApiResponse(400, "User already has a membership, please update the existing membership");
            //}
            //try
            //{
            //    if (membership.Image != null)
            //    {
            //        var fileResult = await _imageService.UploadImageAsync(membership.Image);
            //        if (fileResult.Item1 == 1)
            //        {
            //            membership.ImageUrl = fileResult.Item2;
            //        }
            //        else
            //        {
            //            return new ApiResponse(400, fileResult.Item2);
            //        }
            //    }
            //    var mappedMembership = _mapper.Map<Membership>(membership);
            //    _context.Memberships.Add(mappedMembership);
            //    await _context.SaveChangesAsync();
            //    return new ApiResponse(200, "Membership added successfully");
            //}
            //catch (Exception ex)
            //{
            //    return new ApiResponse(500, "Error: " + ex.Message);
            //}
            throw new NotImplementedException();
        }
        public async Task<ApiResponse> DeleteMembership(int id)
        {
            var membership = await _context.Memberships.FirstOrDefaultAsync(x => x.MembershipId == id);
            if (membership == null)
            {
                return new ApiResponse(404, "Membership not found");
            }
            try
            {
                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Membership deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<MembershipDto>> GetAllMemberships()
        {
            try
            {
                var memberships = await _context.Memberships.ToListAsync();
                var mappedMemberships = _mapper.Map<IEnumerable<MembershipDto>>(memberships);
                return mappedMemberships;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<MembershipDto> GetMembershipById(int id)
        {
            var membership = await _context.Memberships.FirstOrDefaultAsync(x => x.MembershipId == id);
            try
            {
                if (membership == null)
                {
                    return null;
                }

                var membershipDto = _mapper.Map<MembershipDto>(membership);
                return membershipDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateMembership(int id ,MembershipDto membership)
        {
            var existingMembership = await _context.Memberships.FirstOrDefaultAsync(x => x.MembershipId == id);
            if (existingMembership == null)
            {
                return new ApiResponse(404, "Membership not found");
            }
            try
            {
                if (membership.Image != null)
                {

                    if (!string.IsNullOrEmpty(existingMembership.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(existingMembership.ImageUrl);
                    }
                    var fileResult = await _imageService.UploadImageAsync(membership.Image);
                    if (fileResult.Item1 == 1)
                    {
                        membership.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                existingMembership.StartDate= membership.StartDate;
                existingMembership.EndDate = membership.EndDate;
                existingMembership.ImageUrl = membership.ImageUrl;
                existingMembership.MembershipType = (Core.Enums.Business.MembershipType)membership.MembershipType;
                existingMembership.Price = membership.Price;
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "Membership updated successfully");
            }
            catch (Exception ex) {
                return new ApiResponse(500, "Error: " + ex.Message);
            }
        }
    }
}
