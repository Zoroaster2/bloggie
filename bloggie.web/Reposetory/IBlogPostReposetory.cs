using bloggie.web.Models.Domain;

namespace bloggie.web.Reposetory
{
    public interface IBlogPostReposetory
    {
        Task<IEnumerable<BlogPost>> GetAllasync();
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetByUrlHandelAsync(string UrlHandel);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost> AddAsync(BlogPost blogpost);
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
// 5 methods to preform curl operations
