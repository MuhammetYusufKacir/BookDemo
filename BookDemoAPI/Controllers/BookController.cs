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

        [HttpGet("GetPage")]
        public async Task<IActionResult> GetPage(int pageNumber = 1, int pageSize = 10)
        {
            
            var response = await _bookService.GetPage(pageNumber, pageSize);

            
            if (response.Success)
            {
                return Ok(response); 
            }
            else
            {
                return StatusCode(response.StatusCode, response); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBook()
        {
            var response = await _bookService.GetAll();

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
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
