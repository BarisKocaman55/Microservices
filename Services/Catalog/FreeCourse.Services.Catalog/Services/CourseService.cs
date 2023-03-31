using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettins databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllAsync()
        {
            var courses = new List<Course>();
            courses = await _courseCollection.Find(i => true).ToListAsync();

            if(courses.Any())
            {
                foreach(var course in courses)
                {
                    var category = await _categoryCollection.Find<Category>(i => i.Id == course.CategoryId).FirstOrDefaultAsync();
                    course.Category = category;
                }
            }

            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
            if(course == null)
            {
                return ResponseDto<CourseDto>.Fail("Course not found", 404);
            }

            var category = await _categoryCollection.Find<Category>(i => i.Id == course.CategoryId).FirstOrDefaultAsync();
            course.Category = category;

            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllAsyncByUserId(string userId)
        {
            var courses = new List<Course>();
            courses = await _courseCollection.Find<Course>(i => i.UserId == userId).ToListAsync();
            
            if(courses.Any())
            {
                foreach(var course in courses)
                {
                    var category = await _categoryCollection.Find<Category>(i => i.Id == course.CategoryId).FirstOrDefaultAsync();
                    course.Category = category;
                }
            }

            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);

            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<ResponseDto<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(i => i.Id == courseUpdateDto.Id, updateCourse);

            if(result == null)
            {
                return ResponseDto<NoContent>.Fail("Course not found", 404);
            }

            return ResponseDto<NoContent>.Success(204);
        }

        public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(i => i.Id == id);
            if (result.DeletedCount > 0)
                return ResponseDto<NoContent>.Success(204);

            return ResponseDto<NoContent>.Fail("Course not found", 404);
        }
    }
}
