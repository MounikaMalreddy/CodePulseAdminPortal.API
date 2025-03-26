using AutoMapper;
using CodePulseAdminPortal.API.Models.Domain;
using CodePulseAdminPortal.API.Models.DTO;
using CodePulseAdminPortal.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository,IMapper mapper)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet("GetAllBlogPosts")]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPostDomain = await blogPostRepository.GetAllBlogPostAsync();
            if (blogPostRepository is null)
                return NotFound();
            return Ok(mapper.Map<List<BlogPostDto>>(blogPostDomain));
        }
        [HttpPost("AddBlogPost")]
        public async Task<IActionResult> AddBlogPost([FromBody] AddBlogPostRequestDto request)
        {
            var blogPostDomain = mapper.Map<BlogPost>(request);
            // Initialize the Categories list if it's null
            blogPostDomain.Categories ??= new List<Category>();
            foreach (var categoryId in request.Categories)
            {
                var existingCategory = await categoryRepository.GetCategoryByIdAsync(categoryId);
                if (existingCategory is not null)
                    blogPostDomain.Categories.Add(existingCategory);
            }
            blogPostDomain = await blogPostRepository.AddBlogPostAsync(blogPostDomain);
            return Ok(mapper.Map<BlogPostDto>(blogPostDomain));
        }
        [HttpGet("{id}/GetBlogPostById")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPostDomain = await blogPostRepository.GetBlogPostByIdAsync(id);
            if (blogPostDomain is null)
                return NotFound();
            return Ok(mapper.Map<BlogPostDto>(blogPostDomain));
        }
        [HttpGet("GetBlogPostByUrlHandle/{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPostDomain = await blogPostRepository.GetBlogPostByUrlHandleAsync(urlHandle);
            if (blogPostDomain is null)
                return null;
            return Ok(mapper.Map<BlogPostDto>(blogPostDomain));
        }

        [HttpPut("{id}/UpdateBlogPostById")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute]Guid id, [FromBody] AddBlogPostRequestDto request)
        {
            var blogPostDomain = mapper.Map<BlogPost>(request);
            blogPostDomain.Id = id;
            blogPostDomain.Categories = new List<Category>();
            foreach (var category in request.Categories)
            {
                var existingCategory = await categoryRepository.GetCategoryByIdAsync(category);
                if (existingCategory is not null)
                    blogPostDomain.Categories.Add(existingCategory);
            }
            blogPostDomain =await blogPostRepository.UpdateBlogPostByIdAsync(blogPostDomain);
            if (blogPostDomain is null)
                return NotFound();
            return Ok(mapper.Map<BlogPostDto>(blogPostDomain));
        }
        [HttpDelete("{id}/DeleteBlogPost")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var blogPostDomain = await blogPostRepository.DeleteBlogPostById(id);
            if (blogPostDomain is null)
                return NotFound();
            return Ok(mapper.Map<BlogPostDto>(blogPostDomain));
        }
    }
}
