using CodePulseAdminPortal.API.Models.Domain;

namespace CodePulseAdminPortal.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllBlogPostAsync();
        Task<BlogPost> AddBlogPostAsync(BlogPost blogPost);
        Task<BlogPost?> GetBlogPostByIdAsync(Guid id);
        Task<BlogPost?> UpdateBlogPostByIdAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteBlogPostById(Guid id);
        Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle);
    }
}
