using bloggie.web.Models.Domain;
using bloggie.web.Models.ViewModels;
using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bloggie.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostReposetory blogPostReposetory;
        private readonly IBlogPostLikeReposetory blogPostLikeReposetory;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentReposetory blogPostCommentReposetory;

        public BlogsController(IBlogPostReposetory blogPostReposetory, IBlogPostLikeReposetory blogPostLikeReposetory,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IBlogPostCommentReposetory blogPostCommentReposetory)
        {
            this.blogPostReposetory = blogPostReposetory;
            this.blogPostLikeReposetory = blogPostLikeReposetory;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentReposetory = blogPostCommentReposetory;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string UrlHandel)
        {
            var blogPost = await blogPostReposetory.GetByUrlHandelAsync(UrlHandel);
            var liked = false;
            var blogDetailesViewModels = new BlogDetailsViewModels();


            if (blogPost != null)
            {
                //in this var from reposetory im getting total likes from blogpost id
                var TotalLikes = await blogPostLikeReposetory.GetTotalLikes(blogPost.Id);

                if(signInManager.IsSignedIn(User))
                {
                    //get likes for this blog for this user
                    var likesForBlog = await blogPostLikeReposetory.GetLikesForBlog(blogPost.Id);
                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likesForUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likesForUser != null;
                    }
                }
                // get comment for blog post
                var blogCommentsDomainModel = await blogPostCommentReposetory.GetCommentsByBlogId(blogPost.Id);
                var blogCommentsForView = new List<BlogComments>();

                foreach (var blogComments in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComments
                    {
                        Description = blogComments.Description,
                        DateAdded = blogComments.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComments.UserId.ToString())).UserName
                    });
                }

                blogDetailesViewModels = new BlogDetailsViewModels
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    ShortDescription = blogPost.ShortDescription,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    UrlHandle = blogPost.UrlHandle,
                    TotalLikes = TotalLikes,
                    Liked = liked
                };
            }

            return View(blogDetailesViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Index (BlogDetailsViewModels blogDetailsViewModels)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModels.Id,
                    Description = blogDetailsViewModels.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await blogPostCommentReposetory.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { UrlHandel = blogDetailsViewModels.UrlHandle });
            }
            return Forbid();
        }
    }
}
