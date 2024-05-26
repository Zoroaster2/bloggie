using bloggie.web.Data;
using bloggie.web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace bloggie.web.Reposetory
{
    public class BlogPostCommentReposetory : IBlogPostCommentReposetory
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentReposetory(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggieDbContext.BlogPostComments.AddAsync(blogPostComment);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogId(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
