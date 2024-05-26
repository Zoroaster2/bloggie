using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace bloggie.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageReposetory imageReposetory;

        public ImagesController(IImageReposetory imageReposetory)
        {
            this.imageReposetory = imageReposetory;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            //call a reposetory
            var imageUrl = await imageReposetory.UploadAsync(file);

            if (imageUrl == null)
            {
                return Problem("Something Went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { Link = imageUrl });
        }
    }
}
