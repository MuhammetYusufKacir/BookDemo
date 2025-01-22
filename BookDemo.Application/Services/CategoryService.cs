using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookDemo.Application.Services
{

    /* public class CategoryService : ICategoryService
      {
          private readonly string _configuration;
          private readonly ISqlManager _sqlManager;

          public CategoryService(IConfiguration configuration, ISqlManager sqlManager)
          {
              _configuration = configuration.GetConnectionString("DefaultConnection");
              _sqlManager = sqlManager;
          }

          public List<Category> GetAllCategories()
          {
              string query = _sqlManager.GetAllCategoriesQuery();
              return _sqlManager.ExecuteReader(query, reader => new Category
              {
                  Id = Convert.ToInt32(reader["Id"]),
                  Name = reader["Name"].ToString()
              });
          }

          public Category GetCategoryById(int id)
          {
              string query = _sqlManager.GetCategoryByIdQuery();
              var parameters = new[]
              {
                  new SqlParameter("@Id", id)
              };

              var categories = _sqlManager.ExecuteReader(query, reader => new Category
              {
                  Id = Convert.ToInt32(reader["Id"]),
                  Name = reader["Name"].ToString()
              }, parameters);

              return categories.FirstOrDefault();
          }

          public Category AddCategory(Category newCategory)
          {
              string query = _sqlManager.InsertCategoryQuery();
              var parameters = new[]
              {
                  new SqlParameter("@Name", newCategory.Name)
              };

              int insertedId = _sqlManager.ExecuteScalar<int>(query, parameters);
              newCategory.Id = insertedId;

              return newCategory;
          }

          public Category UpdateCategory(int id, Category updatedCategory)
          {
              string query = _sqlManager.UpdateCategoryQuery();
              var parameters = new[]
              {
                  new SqlParameter("@Id", id),
                  new SqlParameter("@Name", updatedCategory.Name)
              };

              _sqlManager.ExecuteNonQuery(query, parameters);
              return updatedCategory;
          }

          public Category DeleteCategory(int id)
          {
              Category deletedCategory = GetCategoryById(id);
              if (deletedCategory != null)
              {
                  string query = _sqlManager.DeleteCategoryQuery();
                  var parameters = new[]
                  {
                      new SqlParameter("@Id", id)
                  };

                  _sqlManager.ExecuteNonQuery(query, parameters);
              }

              return deletedCategory;
          }

      }*/

    /* public class CategoryService : ICategoryService
    {
        private readonly string _configuration;
        private readonly ISqlManager _sqlManager;

        public CategoryService(IConfiguration configuration, ISqlManager sqlManager)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
            _sqlManager = sqlManager;
        }

        public ApiResponse<List<Category>> GetAllCategories()
        {
            try
            {
                string query = _sqlManager.GetAllCategoriesQuery();
                var categories = _sqlManager.ExecuteReader(query, reader => new Category
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                });

                return new ApiResponse<List<Category>>(true, categories, "Categories retrieved successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Category>>(false, null, $"Error: {ex.Message}", 500);
            }
        }

        public ApiResponse<Category> GetCategoryById(int id)
        {
            try
            {
                string query = _sqlManager.GetCategoryByIdQuery();
                var parameters = new[]
                {
                    new SqlParameter("@Id", id)
                };

                var categories = _sqlManager.ExecuteReader(query, reader => new Category
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                }, parameters);

                var category = categories.FirstOrDefault();

                if (category == null)
                    return new ApiResponse<Category>(false, null, "Category not found.", 404);

                return new ApiResponse<Category>(true, category, "Category retrieved successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Category>(false, null, $"Error: {ex.Message}", 500);
            }
        }

        public ApiResponse<Category> AddCategory(Category newCategory)
        {
            try
            {
                string query = _sqlManager.InsertCategoryQuery();
                var parameters = new[]
                {
                    new SqlParameter("@Name", newCategory.Name)
                };

                int insertedId = _sqlManager.ExecuteScalar<int>(query, parameters);
                newCategory.Id = insertedId;

                return new ApiResponse<Category>(true, newCategory, "Category added successfully.", 201);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Category>(false, null, $"Error: {ex.Message}", 500);
            }
        }

        public ApiResponse<Category> UpdateCategory(int id, Category updatedCategory)
        {
            try
            {
                string query = _sqlManager.UpdateCategoryQuery();
                var parameters = new[]
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Name", updatedCategory.Name)
                };

                _sqlManager.ExecuteNonQuery(query, parameters);

                return new ApiResponse<Category>(true, updatedCategory, "Category updated successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Category>(false, null, $"Error: {ex.Message}", 500);
            }
        }

        public ApiResponse<Category> DeleteCategory(int id)
        {
            try
            {
                var deletedCategory = GetCategoryById(id).Data;
                if (deletedCategory == null)
                    return new ApiResponse<Category>(false, null, "Category not found.", 404);

                string query = _sqlManager.DeleteCategoryQuery();
                var parameters = new[]
                {
                    new SqlParameter("@Id", id)
                };

                _sqlManager.ExecuteNonQuery(query, parameters);

                return new ApiResponse<Category>(true, deletedCategory, "Category deleted successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Category>(false, null, $"Error: {ex.Message}", 500);
            }
        }
    }*/

    /* public class CategoryService : ICategoryService
     {

         private readonly AppDbContext _context;

         public CategoryService(AppDbContext context )
         {
             _context = context;
         }

         public ApiResponse<List<Category>> GetAllCategories()
         {
             try
             {
                 var category = _context.Categories.ToList();
                 return new ApiResponse<List<Category>>(true, category, "Category retrieved successfully.", 200);
             }
             catch (Exception ex)
             {
                 return new ApiResponse<List<Category>>(false, null, ex.Message, 500);
             }
         }

         public ApiResponse<Category> GetCategoryById(int id)
         {
             try
             {
                 var category = _context.Categories.FirstOrDefault(c => c.Id == id);
                 if (category == null) 
                     return new ApiResponse<Category>(false, null, "Category not found", 400);

                 return new ApiResponse<Category>(true, category, "Category retrieved successfully", 200);

             }
             catch (Exception ex)
             {
                 return new ApiResponse<Category>(false, null,ex.Message, 500);
             }
         }
         public ApiResponse<Category> AddCategory(Category newCategory)
         {
             try
             {
                 _context.Categories.Add(newCategory);
                 _context.SaveChanges();

                 return new ApiResponse<Category>(true, newCategory, "Book added successfully.",201);
             }
             catch (Exception ex)
             {
                 return new ApiResponse<Category>(false, null, ex.Message, 500);
             }
         }
         public ApiResponse<Category> UpdateCategory(int id, Category updatedCategory)
         {
             try
             {
                 var category = _context.Categories.FirstOrDefault(c => c.Id == id);
                 if (category == null)
                     return new ApiResponse<Category>(false, null, "Category not found.", 404);

                 category.Name = updatedCategory.Name;
                 _context.SaveChanges();

                 return new ApiResponse<Category>(true, category, "Category updated successfully.", 200);
             }
             catch (Exception ex)
             {
                 return new ApiResponse<Category>(false, null, ex.Message, 500);
             }
         }
         public ApiResponse<Category> DeleteCategory(int id)
         {
             try
             {
                 var category = _context.Categories.FirstOrDefault(c => c.Id == id);
                 if (category == null)
                     return new ApiResponse<Category>(false, null, "Category not found.", 404);

                 _context.Categories.Remove(category);
                 _context.SaveChanges();

                 return new ApiResponse<Category>(true, category, "Category deleted successfully.", 200);
             }
             catch (Exception ex)
             {
                 return new ApiResponse<Category>(false, null, ex.Message, 500);
             }
         }
     }*/

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public static class CacheKeyHelper
        {
            public const string CacheKey = "Category_List";
        }

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync(Expression<Func<Category, bool>> filter = null)
        {
            try
            {
                string cacheKey = CacheKeyHelper.CacheKey;
                var cacheData = await _cacheService.GetAsync<ApiResponse<List<CategoryDTO>>>(cacheKey);
                if (cacheData != null)
                {
                    return cacheData;
                }

                var categories = await _categoryRepository.GetAllAsync(c => c.Status == EntityStatus.Active);
                var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
                await _cacheService.SetAsync(cacheKey,categoryDTOs, TimeSpan.FromMinutes(10));
                return new ApiResponse<List<CategoryDTO>>(true, categoryDTOs, "Categories retrieved successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoryDTO>>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id)
        {
            try
            {
                string cacheKey = CacheKeyHelper.CacheKey;
                var cachedCategories = await _cacheService.GetAsync<List<CategoryDTO>>(cacheKey);
                if (cachedCategories != null)
                {
                    var cachedCategory = cachedCategories.FirstOrDefault(c => c.Id == id);
                    if (cachedCategory != null)
                    {
                        return new ApiResponse<CategoryDTO>(true, cachedCategory, "Category retrieved from cache successfully", 200);
                    }
                }

                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                    return new ApiResponse<CategoryDTO>(false, null, "Category not found", 404);

                var categoryDTOs = _mapper.Map<CategoryDTO>(category);
                if (cachedCategories != null)
                {
                    cachedCategories.Add(categoryDTOs);
                    await _cacheService.SetAsync(cacheKey, cachedCategories, TimeSpan.FromMinutes(10));
                }
                else
                {
                    await _cacheService.SetAsync(cacheKey, new List<CategoryDTO>{categoryDTOs}, TimeSpan.FromMinutes(10));
                }
              
                return new ApiResponse<CategoryDTO>(true, categoryDTOs, "Category retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDTO>(false, null, ex.Message, 500);
            }
        }



        public async Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(int id, Category updatedCategory)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                    return new ApiResponse<CategoryDTO>(false, null, "Category not found.", 404);

                _mapper.Map(updatedCategory, category);
                await _categoryRepository.UpdateAsync(category);

                var updatedCategoryDTO = _mapper.Map<CategoryDTO>(category);
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);

                return new ApiResponse<CategoryDTO>(true, updatedCategoryDTO, "Category updated successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDTO>(false, null, ex.Message, 500);
            }
        }


        public async Task<ApiResponse<CategoryDTO>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                    return new ApiResponse<CategoryDTO>(false, null, "Category not found.", 404);

                await _categoryRepository.DeleteAsync(category);

                var categoryDTO = _mapper.Map<CategoryDTO>(category);
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);

                return new ApiResponse<CategoryDTO>(true, categoryDTO, "Category deleted successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDTO>(false, null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<CategoryDTO>> AddCategoryAsync(CategoryDTO newCategoryDto)
        {

            try
            {
                var newCategory = _mapper.Map<Category>(newCategoryDto);
                newCategory.Status = EntityStatus.Active;

                await _categoryRepository.AddAsync(newCategory);

                var createdCategoryDto = _mapper.Map<CategoryDTO>(newCategory);
                string cacheKey = CacheKeyHelper.CacheKey;
                await _cacheService.RemoveAsync(cacheKey);

                return new ApiResponse<CategoryDTO>(true, createdCategoryDto, "Category added successfully.", 201);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDTO>(false, null, ex.Message, 500);
            }
        }
    }
}
