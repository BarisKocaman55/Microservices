using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : BaseController
    {
        [HttpPost]
        public IActionResult RecievePayment()
        {
            return CreateActionResultInstance(ResponseDto<NoContentResult>.Success(200));
        }
    }
}
