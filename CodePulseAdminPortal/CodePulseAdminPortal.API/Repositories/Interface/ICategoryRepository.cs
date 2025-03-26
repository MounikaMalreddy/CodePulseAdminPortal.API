using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(string? filterQuery = null,
            string? sortBy = null, string? sortDirection = null, int? pageNumber=1, int? pageSize = 5);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category?> UpdateCategoryByIdAsync(Category category);
        Task<Category?> DeleteCategory(Guid id);
        Task<int> CategoriesCountAsync();
    }
}
