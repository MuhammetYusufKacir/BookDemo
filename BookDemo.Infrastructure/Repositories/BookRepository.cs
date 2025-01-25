using System.Linq.Expressions;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using BookDemo.Infrastructure.Data;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;

namespace BookDemo.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
    

        public BookRepository(AppDbContext context)
        {
            _context = context;
            
    
        }
        public long Save(Book book)
        {


            _context.Books.Add(book);
            _context.SaveChanges();
            return book.Id;
        }

        public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>> filter = null)
        {
            List<Book> books;
            if (filter != null)
            {
                books = await _context.Books.Where(filter).Include(b => b.Category).ToListAsync();
            }
            else
            {
                books = await _context.Books.ToListAsync();
            }

            return books;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
                
            var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return null;

            return book;
          
        }

        public async Task AddAsync(Book book)
        {
            book.Status= Core.Models.EntityStatus.Active;
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync( Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _context.Books.Where(b => b.CategoryId == categoryId).Include(b => b.Category).ToListAsync();
        }

        public async Task<List<Book>> GetBooksWithCategoryAsync()
        {
            return await _context.Books
                .Include(b => b.Category) 
                .Select(b => new Book
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    CategoryId = b.CategoryId,
                    Category = new Category 
                    {
                        Id = b.Category.Id,
                        Name = b.Category.Name
                    }
                })
        .ToListAsync();
        }

        public async Task DeleteAllAsync()
        {
            var books = await _context.Books.ToListAsync();
            _context.Books.RemoveRange(books);
            await _context.SaveChangesAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories
                                 .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<PagedResult<Book>> GetPage(int pageNumber, int pageSize)
        {
            var totalItems = _context.Books.Count();
            var books = _context.Books
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Book>
            {
                Items = books,
                TotalItems = totalItems,
                PageSize = pageSize,
                PageNumber = pageNumber
            };
        }
    }
}
