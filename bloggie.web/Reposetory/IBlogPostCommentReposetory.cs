using bloggie.web.Models.Domain;

namespace bloggie.web.Reposetory
{
    public interface IBlogPostCommentReposetory
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogId(Guid blogPostId);
    }
}
