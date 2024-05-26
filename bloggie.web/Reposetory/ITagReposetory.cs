using bloggie.web.Models.Domain;

namespace bloggie.web.Reposetory
{
    public interface ITagReposetory
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetAsync (Guid id);
        Task<Tag> AddAsync (Tag tag);
        Task<Tag?> DeleteAsync (Guid id);
        Task<Tag?> UpdateAsync (Tag tag);

    }
}
