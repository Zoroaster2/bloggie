using bloggie.web.Data;
using bloggie.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace bloggie.web.Reposetory
{
    //inharite form iBlogPostReposetory
    public class BlogPostReposetory : IBlogPostReposetory
    {
        private readonly BloggieDbContext bloggieDbContext;
        //Adding BloggieDbContext and give it a name for using it in this 
        public BlogPostReposetory(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        // Task Of Adding from BlogPost from DbContext And Save And reTurn the BlogPost
        public async Task<BlogPost> AddAsync(BlogPost blogpost)
        {
            await bloggieDbContext.AddAsync(blogpost);
            await bloggieDbContext.SaveChangesAsync();
            return blogpost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            //we first check what we want to delete is there oe not
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllasync()
        {
            //want to get the whole list to controller
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            //from databse thow blog post we search for id to find 
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandelAsync(string UrlHandel)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == UrlHandel);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            //this can be null 
            if (existingBlog != null)
            {
                //same thing we did in controller but that was for viewModel but this is actuall updating dataBase
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            {
                return null;
            }
        }
    }
}
