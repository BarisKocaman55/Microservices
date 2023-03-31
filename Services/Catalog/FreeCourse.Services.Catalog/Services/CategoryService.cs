using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettins databaseSettins)
        {
            var client = new MongoClient(databaseSettins.ConnectionString);
            var database = client.GetDatabase(databaseSettins.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettins.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            return ResponseDto<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<ResponseDto<CategoryDto>> CreateAsync(Category category)
        {
            await _categoryCollection.InsertOneAsync(category);
            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
        {
            var category = _categoryCollection.Find<Category>(i => i.Id == id).FirstOrDefaultAsync();
            if(category == null)
            {
                return ResponseDto<CategoryDto>.Fail("Category not found", 404);
            }

            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}
