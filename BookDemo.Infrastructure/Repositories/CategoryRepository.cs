using System.Linq.Expressions;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using BookDemo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookDemo.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> filter = null)
        {

            List<Category> categories;
            if (filter != null)
            {
                categories = await _context.Categories.Where(filter).ToListAsync(); 
            }
            else
            {
                categories = await _context.Categories.ToListAsync(); 
            }

            return categories;

        }
        public async Task<Category> GetByIdAsync(int id)
        {


            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return null;

            return category;
        }
        public async Task AddAsync(Category category)
        {
            category.Status = Core.Models.EntityStatus.Active;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories
                                 .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

    }
}
