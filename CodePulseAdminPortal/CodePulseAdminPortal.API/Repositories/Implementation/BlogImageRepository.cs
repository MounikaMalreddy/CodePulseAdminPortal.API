using CodePulseAdminPortal.API.Data;
using CodePulseAdminPortal.API.Models.Domain;
using CodePulseAdminPortal.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulseAdminPortal.API.Repositories.Implementation
{
    public class BlogImageRepository : IBlogImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public BlogImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetBlogImagesAsync()
        {
            return await dbContext.BlogImage.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1. Upload the Image to API/Images folder
            var localPath= Path.Combine(webHostEnvironment.ContentRootPath, "Images",$"{blogImage.FileName}{ blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //2. update the databse
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            blogImage.Url = urlPath;
            await dbContext.BlogImage.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();
            return blogImage;
        }
    }
}
