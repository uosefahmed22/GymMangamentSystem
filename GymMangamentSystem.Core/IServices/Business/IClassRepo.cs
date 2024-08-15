using GymMangamentSystem.Core.Dtos.Business;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Business
{
    public interface IClassRepo
    {
        //crud operations
        public Task<ApiResponse> AddClass(ClassDto classDto);
        Task<ApiResponse> UpdateClass(int id, ClassDto classDto);
        public Task<ApiResponse> DeleteClass(int id);
        public Task<ClassDto> GetClass(int id);
        public Task<IEnumerable<ClassDto>> GetClasses();
    }
}
