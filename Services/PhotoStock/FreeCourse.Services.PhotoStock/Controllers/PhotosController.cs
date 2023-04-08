﻿using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if(photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken);
                }

                // http://www.photostock.api.com/photos/barisKocaman.jpg
                var returnPath = "photos/" + photo.FileName;
                PhotoDto photoDto = new PhotoDto { Url = returnPath };

                return CreateActionResultInstance(ResponseDto<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(ResponseDto<PhotoDto>.Fail("photo is empty", 400));
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if(!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ResponseDto<NoContent>.Fail("photo not found", 404));
            }

            System.IO.File.Delete(path);
            return CreateActionResultInstance(ResponseDto<NoContent>.Success(204));
        }
    }
}
