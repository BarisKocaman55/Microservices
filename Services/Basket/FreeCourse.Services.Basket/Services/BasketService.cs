using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<ResponseDto<BasketDto>> GetBasket(string userId)
        {
            var existsBasket = await _redisService.GetDb().StringGetAsync(userId);
            if (string.IsNullOrEmpty(existsBasket))
            {
                return ResponseDto<BasketDto>.Fail("Basket not found", 404);
            }

            return ResponseDto<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existsBasket), 200);
        }

        public async Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("Could not save or update", 500);
        }

        public async Task<ResponseDto<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("Basket not found", 404);
        }
    }
}
