using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using Microsoft.AspNetCore.Http;

namespace BookDemo.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService _cacheService;



        public BookService(ICacheService cacheService, IBookRepository bookRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cacheService = cacheService;
        }


        public static class CacheKeyHelper
        {
            public const string CacheKey = "Books_List";
        }
        public async Task<ApiResponse<List<BookDTO>>> GetAll(Expression<Func<Book, bool>> filter = null)
        {
            try
            {
                string cacheKey = CacheKeyHelper.CacheKey;

                var cacheData = await _cacheService.GetAsync<ApiResponse<List<BookDTO>>>(cacheKey);
                if (cacheData != null)
                {
                    return cacheData;
                }

                var books = await _bookRepository.GetAllAsync(b => b.Status == EntityStatus.Active);
                var bookDtos = _mapper.Map<List<BookDTO>>(books);

                var apiResponse = new ApiResponse<List<BookDTO>>(true, bookDtos, "Books retrieved successfully.", 200);

                await _cacheService.SetAsync(cacheKey, apiResponse, TimeSpan.FromMinutes(10));

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookDTO>>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<BookDTO>> GetOneBook(int id)
        {
            try
            {

                string cacheKey = CacheKeyHelper.CacheKey;

                var cachedBooks = await _cacheService.GetAsync<List<BookDTO>>(cacheKey);
                if (cachedBooks != null && cachedBooks.Any())
                {
                    var cachedBook = cachedBooks.FirstOrDefault(x => x.Id == id);
                    if (cachedBook != null)
                    {
                        return new ApiResponse<BookDTO>(true, cachedBook, "Book retrieved from cache.", 200);
                    }
                }
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                {
                    return new ApiResponse<BookDTO>(false, null, "Book not found.", 404);
                }
                var bookDto = _mapper.Map<BookDTO>(book);
                if (cachedBooks != null)
                {
                    cachedBooks.Add(bookDto);
                    await _cacheService.SetAsync(cacheKey, cachedBooks, TimeSpan.FromMinutes(10));
                }
                else
                {
                    await _cacheService.SetAsync(cacheKey, new List<BookDTO> { bookDto }, TimeSpan.FromMinutes(10));
                }
                return new ApiResponse<BookDTO>(true, bookDto, "Book retrieved successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookDTO>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<BookDTO>> AddBook(BookDTO newBookDto)
        {

            try
            {
                var category = await _bookRepository.GetCategoryByIdAsync(newBookDto.CategoryId);
                if (category == null)
                {
                    return new ApiResponse<BookDTO>(false, null, "Category not found.", 404);
                }

                var newBook = _mapper.Map<Book>(newBookDto);
                await _bookRepository.AddAsync(newBook);

                var createdBookDto = _mapper.Map<BookDTO>(newBook);
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);

                return new ApiResponse<BookDTO>(true, createdBookDto, "Book added successfully.", 201);
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookDTO>(false, null, ex.Message, 500);
            }

        }



        public async Task<ApiResponse<BookDTO>> UpdateOneBook(int id, BookDTO updatedBookDto)
        {
            try
            {
                updatedBookDto.Id = id;
                var existingBook = await _bookRepository.GetByIdAsync(id);
                if (existingBook == null)
                    return new ApiResponse<BookDTO>(false, null, "Book not found.", 404);

                _mapper.Map(updatedBookDto, existingBook);
                //existingBook.Name = updatedBookDto.Name;
                //existingBook.Price = updatedBookDto.Price;
                //existingBook.CategoryId = updatedBookDto.CategoryId;
                await _bookRepository.UpdateAsync(existingBook);

                var updatedDto = _mapper.Map<BookDTO>(existingBook);
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);

                return new ApiResponse<BookDTO>(true, updatedDto, "Book updated successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookDTO>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<BookDTO>> DeleteOneBook(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                    return new ApiResponse<BookDTO>(false, null, "Book not found.", 404);

                book.Status = EntityStatus.Deleted;
                await _bookRepository.UpdateAsync(book);

                var deletedBookDto = _mapper.Map<BookDTO>(book);
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);
                return new ApiResponse<BookDTO>(true, deletedBookDto, "Book marked as deleted successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookDTO>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<BookDTO>>> DeleteAllBooks()
        {
            try
            {
                await _bookRepository.DeleteAllAsync();
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);
                return new ApiResponse<List<BookDTO>>(true, null, "All books deleted successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookDTO>>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<BookDTO>>> GetBooksByCategory(int categoryId)
        {
            try
            {
                string cacheKey = CacheKeyHelper.CacheKey;
                var cachedBooks = await _cacheService.GetAsync<List<BookDTO>>(cacheKey);
                if (cachedBooks != null)
                {
                    return new ApiResponse<List<BookDTO>>(true, cachedBooks, "Books retrieved successfully", 200);
                }



                var books = await _bookRepository.GetBooksByCategoryAsync(categoryId);
                if (books.Count == 0)
                    return new ApiResponse<List<BookDTO>>(false, null, "No books found for this category.", 404);

                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                await _cacheService.SetAsync(cacheKey, bookDtos, TimeSpan.FromMinutes(10));
                return new ApiResponse<List<BookDTO>>(true, bookDtos, "Books retrieved successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookDTO>>(false, null, ex.Message, 500);
            }
        }
        public async Task<ApiResponse<List<BookDTO>>> GetBooksWithCategory()
        {
            try
            {
                string cacheKey = CacheKeyHelper.CacheKey;
                var cachedBooks = await _cacheService.GetAsync<List<BookDTO>>(cacheKey);
                if (cachedBooks != null)
                {
                    return new ApiResponse<List<BookDTO>>(true, cachedBooks, "Books retrieved successfully with their categories.", 200);
                }

                var books = await _bookRepository.GetBooksWithCategoryAsync();

                if (books.Count == 0)
                    return new ApiResponse<List<BookDTO>>(false, null, "No books found.", 404);

                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                await _cacheService.SetAsync(cacheKey, bookDtos, TimeSpan.FromMinutes(10));
                return new ApiResponse<List<BookDTO>>(true, bookDtos, "Books retrieved successfully with their categories.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookDTO>>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PagedResult<BookDTO>>> GetPage(int pageNumber, int pageSize)
        {
            try
            {
                var pagedResult = await _bookRepository.GetPage(pageNumber, pageSize);

                var mappedBooks = pagedResult.Items.Select(book => new BookDTO
                {
                    Id = book.Id,
                    Name = book.Name,
                    Price = book.Price,
                    CategoryId = book.CategoryId,
                    ImagePath = book.ImagePath
                }).ToList();

                var result = new PagedResult<BookDTO>
                {
                    Items = mappedBooks,
                    TotalItems = pagedResult.TotalItems,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                };

                return new ApiResponse<PagedResult<BookDTO>>(true, result , "Books retrieved successfully with their categories.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedResult<BookDTO>>(false, null, ex.Message, 500);
            }
        }

    }
}
