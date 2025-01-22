using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDemo.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BookDemo.Core.Entities;
    using BookDemo.Core.Models;


    /* public interface IBookService
{
ApiResponse<List<Book>> GetAll();  
ApiResponse<Book> GetOneBook(int id);
ApiResponse<Book> AddBook(Book newBook);
ApiResponse<Book> UpdateOneBook(int id, Book updatedBook);
ApiResponse<Book> DeleteOneBook(int id);
ApiResponse<List<Book>> DeleteAllBooks();
ApiResponse<List<Book>> GetBooksByCategory(int categoryId);
ApiResponse<List<Book>> GetBooksWithCategory();
}*/

    public interface IBookService
    {
        Task<ApiResponse<List<BookDTO>>> GetAll(Expression<Func<Book, bool>> filter = null);
        Task<ApiResponse<BookDTO>> GetOneBook(int id);
        Task<ApiResponse<BookDTO>> AddBook(BookDTO newBookDto);
        Task<ApiResponse<BookDTO>> UpdateOneBook(int id, BookDTO updatedBookDto);
        Task<ApiResponse<BookDTO>> DeleteOneBook(int id);
        Task<ApiResponse<List<BookDTO>>> DeleteAllBooks();
        Task<ApiResponse<List<BookDTO>>> GetBooksByCategory(int categoryId);
        Task<ApiResponse<List<BookDTO>>> GetBooksWithCategory();
  
    }

}
