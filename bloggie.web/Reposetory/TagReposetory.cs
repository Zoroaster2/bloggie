using bloggie.web.Data;
using bloggie.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace bloggie.web.Reposetory
{
    public class TagReposetory : ITagReposetory
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagReposetory(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)  
        {
            var exsitingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (exsitingTag != null)
            {
                bloggieDbContext.Tags.Remove(exsitingTag);
                await bloggieDbContext.SaveChangesAsync();
                return exsitingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var exsitingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (exsitingTag !=null)
            {
                exsitingTag.Name = tag.Name;
                exsitingTag.DisplayName = tag.DisplayName;

                await bloggieDbContext.SaveChangesAsync();
                return exsitingTag;
            }

            return null;

            
        }
    }
}
