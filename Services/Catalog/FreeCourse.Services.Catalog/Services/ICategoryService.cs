using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<ResponseDto<List<CategoryDto>>> GetAllAsync();
        Task<ResponseDto<CategoryDto>> CreateAsync(Category category);
        Task<ResponseDto<CategoryDto>> GetByIdAsync(string id);
    }
}
