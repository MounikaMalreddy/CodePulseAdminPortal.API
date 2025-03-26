using AutoMapper;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IBlogImageRepository blogImageRepository;
        private readonly IMapper mapper;

        public ImagesController(IBlogImageRepository blogImageRepository, IMapper mapper) 
        {
            this.blogImageRepository = blogImageRepository;
            this.mapper = mapper;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file, string fileName, string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };
                blogImage = await blogImageRepository.Upload(file, blogImage);
                return Ok(mapper.Map<BlogImageDto>(blogImage));
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlogImages()
        {
            var blogImageDomain = await blogImageRepository.GetBlogImagesAsync();
            if (blogImageDomain is null)
                return NotFound();
            return Ok(mapper.Map<List<BlogImageDto>>(blogImageDomain));
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "FileSize cannot be more than 10MB");
            }
        }

    }

}
