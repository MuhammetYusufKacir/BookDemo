using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>> filter = null);
        Task<Book> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<List<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<List<Book>> GetBooksWithCategoryAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task DeleteAllAsync();
    }
}
