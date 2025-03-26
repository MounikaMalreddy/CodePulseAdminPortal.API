using CodePulseAdminPortal.API.Data;
using CodePulseAdminPortal.API.Models.Domain;
using CodePulseAdminPortal.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodePulseAdminPortal.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            await dbContext.Category.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<int> CategoriesCountAsync()
        {
            return await dbContext.Category.CountAsync();
        }

        public async Task<Category?> DeleteCategory(Guid id)
        {
            var existingCategory = await dbContext.Category.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory is null)
                return null;
            dbContext.Category.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string? filterQuery = null,
            string? sortBy=null, string? sortDirection=null, int? pageNumber = 1, int? pageSize = 5)
        {
            var categories = dbContext.Category.AsQueryable();
            if (!string.IsNullOrEmpty(filterQuery))
            {
              categories=  categories.Where(x => x.Name.Contains(filterQuery));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                if(string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;
                    categories = isAsc ? categories.OrderBy(x => x.Name) : categories.OrderByDescending(x => x.Name);
                }
                if (string.Equals(sortBy, "UrlHandle", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;
                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);
                }
            }
            var skipResults = (pageNumber - 1) * pageSize;
            categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 5);
            return await categories.ToListAsync();
            //return await dbContext.Category.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            var existingCategory = await dbContext.Category.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (existingCategory is null)
                return null;
            return existingCategory;
        }

        public async Task<Category?> UpdateCategoryByIdAsync(Category category)
        {
            var existingCategory = await dbContext.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existingCategory is null)
                return null;
            // Update properties with new values
            dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
            await dbContext.SaveChangesAsync();
            return existingCategory;/// Return updated category

        }
    }
}
