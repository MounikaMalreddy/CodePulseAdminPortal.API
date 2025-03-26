using CodePulseAdminPortal.API.Models.Domain;

namespace CodePulseAdminPortal.API.Repositories.Interface
{
    public interface IBlogImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetBlogImagesAsync();
    }
}
