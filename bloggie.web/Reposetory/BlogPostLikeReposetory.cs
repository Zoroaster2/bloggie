
using bloggie.web.Data;
using bloggie.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace bloggie.web.Reposetory
{
    public class BlogPostLikeReposetory : IBlogPostLikeReposetory
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeReposetory(BloggieDbContext bloggieDbContext)
        {

            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostLikes.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid BlogPostId)
        {
            return await bloggieDbContext.BlogPostLikes.CountAsync(x => x.BlogPostId == BlogPostId);
        }
    }
}
