using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<ResponseDto<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return ResponseDto<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<ResponseDto<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id = @Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
                return ResponseDto<Models.Discount>.Fail("Discount not found", 404);

            return ResponseDto<Models.Discount>.Success(discount, 200);
        }
        public async Task<ResponseDto<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount where code=@Code AND userid=@UserId", new
            {
                Code = code,
                UserId = userId
            });

            var hasDiscount = discounts.FirstOrDefault();

            if (hasDiscount == null)
                return ResponseDto<Models.Discount>.Fail("Discount not found", 404);

            return ResponseDto<Models.Discount>.Success(hasDiscount, 200);
        }

        public async Task<ResponseDto<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid, rate, code) VALUES (@UserId, @Rate, @Code)", discount);
            if (saveStatus > 0)
                return ResponseDto<NoContent>.Success(204);

            return ResponseDto<NoContent>.Fail("An error occured while adding", 500);
        }

        public async Task<ResponseDto<NoContent>> Update(Models.Discount discount)
        {
            var statusResult = await _dbConnection.ExecuteAsync("UPDATE discount SET userid=@UserId, code=@Code, rate=@Rate where id=@Id", new
            {
                UserId = discount.UserId,
                Code = discount.Code,
                Raate = discount.Rate,
                Id = discount.Id
            });

            if (statusResult > 0)
                return ResponseDto<NoContent>.Success(204);

            return ResponseDto<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<ResponseDto<NoContent>> Delete(int id)
        {
            var statusResult = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id=@Id", new { Id = id });
            if (statusResult > 0)
                return ResponseDto<NoContent>.Success(204);

            return ResponseDto<NoContent>.Fail("Discount not found", 404);
        }
    }
}
