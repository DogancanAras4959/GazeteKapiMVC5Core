using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO.FooterTypeDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO.StylePageDTO;
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
        Task<bool> EditIsMenuById(int id);
        bool DeleteCategoryById(int id);
        List<CategoryListItemDto> GetParentCategoryList();
        List<StylePageListItemDto> GetAllStyleTypes();
        List<FooterTypeListItemDto> GetAllFooterTypes();
        CategoryDto GetCategoryByName(string categoryName);
    }
}
