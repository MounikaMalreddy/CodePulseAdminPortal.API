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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetAllCategories([FromQuery]string? filterQuery,
             string? sortBy, string? sortDirection, int? pageNumber, int? pageSize)
        {
            var categories = await categoryRepository.GetAllCategoriesAsync(filterQuery, sortBy,sortDirection,pageNumber,pageSize);
            if (categories is null)
                return NotFound();
            //var response = new List<CategoryDto>();
            //foreach (var category in categories)
            //{
            //    response.Add(new CategoryDto
            //    {
            //        Id = category.Id,
            //        Name = category.Name,
            //        UrlHandle = category.UrlHandle
            //    });
            //}
            return Ok(mapper.Map<List<CategoryDto>>(categories));
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequestDto request)
        {
            var CategoryDomain = mapper.Map<Category>(request);
            //    new Category
            //{
            //    Name = request.Name,
            //    UrlHandle = request.UrlHandle
            //};
            CategoryDomain= await categoryRepository.AddCategoryAsync(CategoryDomain);
            //var response = new CategoryDto
            //{
            //    Id = CategoryDomain.Id,
            //    Name = CategoryDomain.Name,
            //    UrlHandle = CategoryDomain.UrlHandle
            //};
            return Ok(mapper.Map<CategoryDto>(CategoryDomain));
        }
        [HttpGet("{id}/GetCategoryById")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var categoryDomain = await categoryRepository.GetCategoryByIdAsync(id);
            if (categoryDomain is null)
                return NotFound();
            //var response = new CategoryDto
            //{
            //    Id = categoryDomain.Id,
            //    Name = categoryDomain.Name,
            //    UrlHandle = categoryDomain.UrlHandle
            //};
            return Ok(mapper.Map<CategoryDto>(categoryDomain));
        }
        [HttpPut("{id}/UpdateCategoryById")]
        public async Task<IActionResult> UpdateCategoryById([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var CategoryDomain = mapper.Map<Category>(request);
            //    new Category
            //{
            //    Id = id,
            //    Name = request.Name,
            //    UrlHandle = request.UrlHandle
            //};
            CategoryDomain.Id = id;
            CategoryDomain = await categoryRepository.UpdateCategoryByIdAsync(CategoryDomain);
            if (CategoryDomain is null)
                return NotFound();
            //var response = new CategoryDto
            //{
            //    Id = CategoryDomain.Id,
            //    Name = CategoryDomain.Name,
            //    UrlHandle = CategoryDomain.UrlHandle
            //};
            return Ok(mapper.Map<CategoryDto>(CategoryDomain));
        }
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.DeleteCategory(id);
            if (existingCategory is null)
                return NotFound();
            //var response = new CategoryDto
            //{
            //    Id = existingCategory.Id,
            //    Name = existingCategory.Name,
            //    UrlHandle = existingCategory.UrlHandle
            //};
            return Ok(mapper.Map<CategoryDto>(existingCategory));
        }
        [HttpGet("Count")]
        public async Task<IActionResult> CategoriesCount()
        {
            var categoryCount = await categoryRepository.CategoriesCountAsync();
            if (categoryCount is 0)
                return NotFound();
            return Ok(categoryCount);
        }
    }
}
