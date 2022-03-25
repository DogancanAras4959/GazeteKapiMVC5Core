using CORE.ApplicationCommon.DTOS.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.Engine.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> CreateCategory(CategoryDto model);
        Task<bool> CategoryIfExists(string categoryName);
        Task<bool> CategoryIfExists(string categoryName, int id);
        CategoryDto GetCategoryById(int id);
        List<CategoryListItemDto> GetAllCategory();
        Task<bool> UpdateCategory(CategoryDto model);
        Task<bool> EditIsActiveById(int id);
        bool DeleteCategoryById(int id);
        List<CategoryListItemDto> GetParentCategoryList();
    }
}
