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
    /* public interface ICategoryService
      {
          ApiResponse<List<Category>> GetAllCategories();
          ApiResponse<Category> GetCategoryById(int id);
          ApiResponse<Category> AddCategory(Category newCategory);
          ApiResponse<Category> UpdateCategory(int id, Category updatedCategory);
          ApiResponse<Category> DeleteCategory(int id);
      }*/

    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync(Expression<Func<Category, bool>> filter = null);
        Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<CategoryDTO>> AddCategoryAsync(CategoryDTO newCategoryDto);
        Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(int id, Category updatedCategory);
        Task<ApiResponse<CategoryDTO>> DeleteCategoryAsync(int id);
    }

}
