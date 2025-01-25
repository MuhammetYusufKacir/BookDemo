using System.Linq.Expressions;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> filter = null);
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);

    }
}
