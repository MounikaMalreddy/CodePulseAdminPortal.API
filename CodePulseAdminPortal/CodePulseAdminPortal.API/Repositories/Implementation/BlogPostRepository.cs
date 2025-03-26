using CodePulseAdminPortal.API.Data;
using CodePulseAdminPortal.API.Models.Domain;
using CodePulseAdminPortal.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CodePulseAdminPortal.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> AddBlogPostAsync(BlogPost blogPost)
        {
            await dbContext.BlogPost.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteBlogPostById(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPost.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            if (existingBlogPost is null)
                return null;
            dbContext.BlogPost.Remove(existingBlogPost);
            await dbContext.SaveChangesAsync();
            return existingBlogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostAsync()
        {
            return await dbContext.BlogPost.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(Guid id)
        {
            return await dbContext.BlogPost.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.BlogPost.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateBlogPostByIdAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPost.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost is null)
                return null;
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
            existingBlogPost.Categories.Clear();
            existingBlogPost.Categories = blogPost.Categories;
            await dbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}
