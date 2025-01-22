using AutoMapper;
using Azure;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using BookDemo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookDemoAPI.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/books")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        /* [HttpGet]
         public IActionResult GetAllBooks()
         {
             var books = ApplicationContext.Books;
             return Ok(books);
         }

         [HttpGet("{id:int}")]
         public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
         {
             var book = ApplicationContext
                 .Books
                 .Where(b => b.Id.Equals(id))
                 .SingleOrDefault();

             if (book is null) 
                return NotFound(); //404

             return Ok(book);
         }

         [HttpPost]
         public IActionResult CreateOneBook([FromBody]Book book)
         {
             try
             {
                 if (book is null)
                     return BadRequest(); //400

                 ApplicationContext.Books.Add(book);
                 return StatusCode(201, book);
             }
             catch (Exception ex)
             {
                 return BadRequest(ex.Message);
             }
         }

         [HttpPut("{id:int}")]
         public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
             [FromBody] Book book)
         { 
             //check book?
             var entity = ApplicationContext
                 .Books
                 .Find(b => b.Id.Equals(id));

             if (entity is null)
                 return NotFound(); //404

             //check id 
             if(id != book.Id)
                 return BadRequest(); //400

             ApplicationContext.Books.Remove(entity);
             book.Id = entity.Id;
             ApplicationContext.Books.Add(book);
             return Ok(book);
         }

         [HttpDelete]
         public IActionResult DeleteAllBooks()
         {
             ApplicationContext.Books.Clear();
             return NoContent(); //204
         }

         [HttpDelete("{id:int}")]
         public IActionResult DeleteOneBook([FromRoute(Name = "id")]int id)
         {
             var entity = ApplicationContext
                 .Books
                 .Find(b => b.Id.Equals(id));

             if (entity is null) 
                 return NotFound(new
                 {
                     statusCode = 404,
                     message = $"Book with id:{id} could not found."
                 }); //404

             ApplicationContext.Books.Remove(entity);
             return NoContent();
         }

         [HttpPatch("{id:int}")]
         public IActionResult PartiallyUptadeOneBook([FromRoute(Name = "id")]int id,
             [FromBody] JsonPatchDocument<Book> bookPatch)
         {
             //check entity
             var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
             if(entity is null)
                 return NotFound(); //404

             bookPatch.ApplyTo(entity);
             return NoContent(); //204
         }*/

        /* private readonly IBookService _bookService;
         private readonly ICategoryService _categoryService;

         public BooksController(IBookService bookService, ICategoryService categoryService)
         {
             _bookService = bookService;
             _categoryService = categoryService;
         }

         [HttpGet]
         public ApiResponse<List<Book>> GetBooks()
         {
             return _bookService.GetAll();
         }

         [HttpGet("{id}")]
         public ApiResponse<Book> GetOneBook(int id)
         {
             return _bookService.GetOneBook(id);
         }

         [HttpPost]
         public ApiResponse<Book> AddBook([FromBody] Book newBook)
         { 
             return _bookService.AddBook(newBook);

         }

         [HttpPut("{id}")]
         public ApiResponse<Book> UpdateOneBook(int id, [FromBody] Book updatedBook)
         {
             return _bookService.UpdateOneBook(id, updatedBook);

         }

         [HttpDelete("{id}")]
         public ApiResponse<Book> DeleteOneBook(int id)
         {
             return _bookService.DeleteOneBook(id); 

         }

         [HttpDelete]
         public ApiResponse<List<Book>> DeleteAllBooks()
         {
             return _bookService.DeleteAllBooks();

         }

         /*         Category Controller           */

        /* [HttpGet("category/{categoryId}")]
         public ApiResponse<List<Book>> GetBooksByCategory(int categoryId)
         {
             return _bookService.GetBooksByCategory(categoryId);

         }

         [HttpGet("categories")]
         public ApiResponse<List<Category>> GetAllCategories()
         {
             return _categoryService.GetAllCategories();

         }

         [HttpPost("categories")]
         public ApiResponse<Category> AddCategory([FromBody] Category newCategory)
         {
             // Kategori ekleme işlemi
             var addedCategory = _categoryService.AddCategory(newCategory);

             // Eğer kategori eklenememişse
             if (addedCategory == null || !addedCategory.Success)
             {
                 return new ApiResponse<Category>(
                     success: false,
                     data: null,
                     message: "Failed to add category. Please try again later.",
                     statusCode: 400
                 );
             }

             // Kategori başarıyla eklenmişse
             return new ApiResponse<Category>(
                 success: true,
                 data: addedCategory.Data,
                 message: "Category successfully added.",
                 statusCode: 201
             );
         }



         [HttpGet("GetBooksWithCategory")]
         public ApiResponse<List<Book>> GetBooksWithCategory() 
         {
             return _bookService.GetBooksWithCategory();

         }*/

        /*private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        public BooksController(IBookService bookService, ICategoryService categoryService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
        }

        // Tüm kitapları getiren endpoint
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookService.GetAll();
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }

        // ID'ye göre bir kitabı getiren endpoint
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {
            var response = await _bookService.GetOneBook(id);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }

        // Yeni bir kitap ekleyen endpoint
        [HttpPost]
        public async Task<IActionResult> CreateOneBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest("Book cannot be null");

            var response = await _bookService.AddBook(book);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return StatusCode(201, response); // 201 Created
        }

        // Bir kitabı güncelleyen endpoint
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            if (id != book.Id)
                return BadRequest("The provided book ID does not match the route ID.");

            var response = await _bookService.UpdateOneBook(id, book);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }

        // Tüm kitapları silen endpoint
        [HttpDelete]
        public async Task<IActionResult> DeleteAllBooks()
        {
            var response = await _bookService.DeleteAllBooks();
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return NoContent(); // 204 No Content
        }

        // ID'ye göre bir kitabı silen endpoint
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var response = await _bookService.DeleteOneBook(id);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return NoContent(); // 204 No Content
        }

        
        


        /*         Category Controller           */

        /* [HttpGet("category/{categoryId}")]
         public async Task<ApiResponse<List<Book>>> GetBooksByCategory(int categoryId)
         {
             return await _bookService.GetBooksByCategory(categoryId);
         }

         [HttpGet("categories")]
         public async Task<ApiResponse<List<Category>>> GetAllCategories()
         {
             return await _categoryService.GetAllCategoriesAsync();
         }

         [HttpPost("categories")]
         public async Task<ApiResponse<Category>> AddCategory([FromBody] Category newCategory)
         {
             var addedCategory = await _categoryService.AddCategoryAsync(newCategory);

             if (addedCategory == null || !addedCategory.Success)
             {
                 return new ApiResponse<Category>(
                     success: false,
                     data: null,
                     message: "Failed to add category. Please try again later.",
                     statusCode: 400
                 );
             }

             return new ApiResponse<Category>(
                 success: true,
                 data: addedCategory.Data,
                 message: "Category successfully added.",
                 statusCode: 201
             );
         }

         [HttpGet("GetBooksWithCategory")]
         public async Task<ApiResponse<List<Book>>> GetBooksWithCategory()
         {
             return await _bookService.GetBooksWithCategory();
         }*/

        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        
        public BooksController(IBookService bookService, ICategoryService categoryService, IMapper mapper, IImageService imageService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _mapper = mapper;
            _imageService = imageService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> UploadImage(ImageUploadDTO ımageUploadDTO)
        {
            var book = await _bookService.GetOneBook(ımageUploadDTO.Id);
            if (book == null) 
                return NotFound();
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var imagePath = await _imageService.UploadImageAsync(ımageUploadDTO.image, uploadsFolder);
                var bookDTO = _mapper.Map<BookDTO>(book.Data); 
                bookDTO.ImagePath = imagePath;
                await _bookService.UpdateOneBook(bookDTO.Id, bookDTO);

                return Ok(new { Message = "İmage uploaded.", ImagePath = bookDTO.ImagePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eror: {ex.Message}");
            }

        }

        // Tüm kitapları getiren endpoint
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookService.GetAll();
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            var bookDTOs = _mapper.Map<List<BookDTO>>(response.Data);
            return Ok(new ApiResponse<List<BookDTO>>(true, bookDTOs, "Books fetched successfully", 200));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook(int id)
        {
            var response = await _bookService.GetOneBook(id);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);


            return Ok(new ApiResponse<BookDTO>(true, response.Data, "Book fetched successfully", 200));
        }


        // ID'ye göre bir kitabı getiren endpoint
        [HttpGet("category/{categoryId}")]
        public async Task<ApiResponse<List<BookDTO>>> GetBooksByCategory(int categoryId)
        {
            var response = await _bookService.GetBooksByCategory(categoryId);
            if (!response.Success)
                return new ApiResponse<List<BookDTO>>(false, null, response.Message, response.StatusCode);

            var bookDTOs = _mapper.Map<List<BookDTO>>(response.Data);
            return new ApiResponse<List<BookDTO>>(true, bookDTOs, "Books fetched successfully for the category", 200);
        }


        // Yeni bir kitap ekleyen endpoint
        [HttpPost]
        public async Task<IActionResult> CreateOneBook([FromBody] BookDTO bookDto)
        {
            if (bookDto == null)
                return BadRequest("Book cannot be null");

            var response = await _bookService.AddBook(bookDto);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            var bookDTO = _mapper.Map<BookDTO>(response.Data);
            return StatusCode(201, new ApiResponse<BookDTO>(true, bookDTO, "Book created successfully", 201));
        }

        // Bir kitabı güncelleyen endpoint
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDTO bookDto)
        {

            var response = await _bookService.UpdateOneBook(id, bookDto);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            var bookDTO = _mapper.Map<BookDTO>(response.Data);
            return Ok(new ApiResponse<BookDTO>(true, bookDTO, "Book updated successfully", 200));
        }


        // Tüm kitapları silen endpoint
        [HttpDelete]
        public async Task<IActionResult> DeleteAllBooks()
        {
            var response = await _bookService.DeleteAllBooks();
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return NoContent();
        }

        // ID'ye göre bir kitabı silen endpoint
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var response = await _bookService.DeleteOneBook(id);
            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return NoContent();
        }


        [HttpGet("categories")]
        public async Task<ApiResponse<List<CategoryDTO>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            var categoryDTO = _mapper.Map<List<CategoryDTO>>(response.Data);
            return new ApiResponse<List<CategoryDTO>>(true, categoryDTO, "Categories fetched successfully", 200);
        }


        [HttpPost("categories")]
        public async Task<ApiResponse<CategoryDTO>> AddCategory([FromBody] CategoryDTO newCategoryDTO)
        {
            var addedCategory = await _categoryService.AddCategoryAsync(newCategoryDTO);
            if (addedCategory == null || !addedCategory.Success)
            {
                return new ApiResponse<CategoryDTO>(false, null, "Failed to add category. Please try again later.", 400);
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(addedCategory.Data);
            return new ApiResponse<CategoryDTO>(true, categoryDTO, "Category successfully added.", 201);
        }

        [HttpDelete("categories")]
        public async Task<ApiResponse<string>> DeleteCategoryById(int id)
        {
            // Kategori bilgilerini servisten al
            var response = await _categoryService.GetCategoryByIdAsync(id);

            // Eğer kategori bulunamazsa hata döndür
            if (response == null || response.Data == null)
            {
                return new ApiResponse<string>(
                    false,
                    null,
                    "Category not found",
                    404
                );
            }

            // Kategori silme işlemini gerçekleştir
            var deleteResult = await _categoryService.DeleteCategoryAsync(id);

            // Silme işlemi başarılıysa
            if (deleteResult.Success)
            {
                return new ApiResponse<string>(
                    true,
                    null,
                    "Category deleted successfully",
                    200
                );
            }

            // Silme işlemi başarısızsa
            return new ApiResponse<string>(
                false,
                null,
                "Failed to delete the category",
                500
            );
        }

    }
}
