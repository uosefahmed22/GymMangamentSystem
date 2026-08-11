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
    public interface IMembershipRepo
    {
        Task<IEnumerable<MembershipDto>> GetAllMemberships(PaginationParameters? pagination = null);
        Task<MembershipDto?> GetMembershipById(int id);
        Task<ApiResponse> CreateMembership(MembershipDto membership);
        Task<ApiResponse> UpdateMembership(int id, MembershipDto membership);
        Task<ApiResponse> DeleteMembership(int id);
    }
}
