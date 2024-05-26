using bloggie.web.Models;
using bloggie.web.Models.ViewModels;
using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace bloggie.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostReposetory blogPostReposetory;
        private readonly ITagReposetory tagReposetory;

        public HomeController(ILogger<HomeController> logger, IBlogPostReposetory blogPostReposetory,
            ITagReposetory tagReposetory
            )
        {
            _logger = logger;
            this.blogPostReposetory = blogPostReposetory;
            this.tagReposetory = tagReposetory;
        }

        public async Task<IActionResult> Index()
        {
            //getting all blogs
            var blogPost = await blogPostReposetory.GetAllasync();
            //getting all tags
            var tags = await tagReposetory.GetAllAsync();
            // to show to model in one view we define third view model wich bind those two views!
            var models = new HomeViewModel
            {
                BlogPosts = blogPost,
                Tags = tags
            };

            return View(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}