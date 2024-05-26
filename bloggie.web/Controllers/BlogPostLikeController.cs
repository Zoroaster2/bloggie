using bloggie.web.Models.Domain;
using bloggie.web.Models.ViewModels;
using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bloggie.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeReposetory blogPostLikeReposetory;

        public BlogPostLikeController(IBlogPostLikeReposetory blogPostLikeReposetory)
        {
            this.blogPostLikeReposetory = blogPostLikeReposetory;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId
            };
            await blogPostLikeReposetory.AddLikeForBlog(model);
            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikeForBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await blogPostLikeReposetory.GetTotalLikes(blogPostId);

            return Ok(totalLikes);
        }
    }
}
