using bloggie.web.Models.Domain;

namespace bloggie.web.Reposetory
{
    public interface IBlogPostLikeReposetory
    {
        Task<int> GetTotalLikes(Guid BlogPostId);
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
    }
}
