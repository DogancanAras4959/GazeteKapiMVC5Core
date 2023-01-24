using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCore.UnitOfWork;
using DOMAIN.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using GazeteKapiMVC5Core.DataAccessLayer;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.ApplicationCommon.DTOS.CategoryDTO.FooterTypeDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO.StylePageDTO;

namespace SERVICES.Engine.Engines
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;

        public CategoryService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CategoryIfExists(string categoryName) =>
            await _unitOfWork.GetRepository<Categories>().FindAsync(x => x.CategoryName == categoryName) != null;

        public async Task<bool> CategoryIfExists(string categoryName, int id) =>
            await _unitOfWork.GetRepository<Categories>().FindAsync(x => x.CategoryName == categoryName && x.Id == id) != null;
     
        public async Task<bool> CreateCategory(CategoryDto model)
        {
            if (model.ParentCategoryId == 999)
            {
                model.ParentCategoryId = 0;
            }

            Categories newCategory = await _unitOfWork.GetRepository<Categories>().AddAsync(new Categories
            {
                CategoryName = model.CategoryName,
                ParentCategoryId = model.ParentCategoryId,
                Description = model.Description,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                StyleId = model.StyleId,
                TypeId = model.TypeId,
                isMenu = model.isMenu,
                Sorted = model.Sorted,
                UserId = model.UserId,
                Position = model.Position,
                Image = model.Image,
                IsActive = true,
            });

            return newCategory != null && newCategory.Id != 0;
        }

        public bool DeleteCategoryById(int id)
        {
            Task<int> result = _unitOfWork.GetRepository<Categories>().DeleteAsync(new Categories { Id = id });
            return Convert.ToBoolean(result.Result);
        }

        public async Task<bool> EditIsActiveById(int id)
        {
            Categories getCategories = _unitOfWork.GetRepository<Categories>().FindAsync(x => x.Id == id).Result;
           
            if (getCategories.IsActive == false)
                getCategories.IsActive = true;
            else
                getCategories.IsActive = false;

            Categories model = await _unitOfWork.GetRepository<Categories>().UpdateAsync(getCategories);
            return getCategories != null;
        }

        public async Task<bool> EditIsMenuById(int id)
        {
            Categories getCategories = _unitOfWork.GetRepository<Categories>().FindAsync(x => x.Id == id).Result;

            if (getCategories.isMenu == false)
                getCategories.isMenu = true;
            else
                getCategories.isMenu = false;

            Categories model = await _unitOfWork.GetRepository<Categories>().UpdateAsync(getCategories);
            return getCategories != null;
        }

        public List<CategoryListItemDto> GetAllCategory()
        {
            IEnumerable<Categories> categories = _unitOfWork.GetRepository<Categories>().Filter(null, x => x.OrderBy(y => y.Id), "user", 1, 100);

            return categories.Select(x => new CategoryListItemDto
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                Description = x.Description,
                ParentCategoryId = x.ParentCategoryId,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime,
                StyleId = x.StyleId,
                isMenu = x.isMenu,
                Sorted = x.Sorted,
                TypeId = x.TypeId,
                Position = x.Position,
                styles = x.stylePosts,
                typeFooter = x.typeToCategories,
                user = x.user,
                Image = x.Image,
                
            }).ToList();
        }

        public List<FooterTypeListItemDto> GetAllFooterTypes()
        {
            IEnumerable<FooterType> footerTypes = _unitOfWork.GetRepository<FooterType>().Filter(null, x => x.OrderBy(y => y.Id), "", 1, 100);

            return footerTypes.Select(x => new FooterTypeListItemDto
            {
                Id = x.Id,
                IsActive = x.IsActive,
                TypeName = x.TypeName,

            }).ToList();
        }

        public List<StylePageListItemDto> GetAllStyleTypes()
        {
            IEnumerable<StylePosts> footerTypes = _unitOfWork.GetRepository<StylePosts>().Filter(null, x => x.OrderBy(y => y.Id), "", 1, 100);

            return footerTypes.Select(x => new StylePageListItemDto
            {
                Id = x.Id,
                isActive = x.IsActive,
                StyleName = x.StyleName,

            }).ToList();
        }

        public CategoryDto GetCategoryById(int id)
        {
            Categories getCategory = _unitOfWork.GetRepository<Categories>().FindAsync(x => x.Id == id).Result;

            if (getCategory == null)
            {
                return new CategoryDto();
            }

            return new CategoryDto
            {
                Id = getCategory.Id,
                CategoryName = getCategory.CategoryName,
                Description = getCategory.Description,
                ParentCategoryId = getCategory.ParentCategoryId,
                IsActive = getCategory.IsActive,
                UpdatedTime = getCategory.UpdatedTime,
                CreatedTime = getCategory.CreatedTime,
                isMenu = getCategory.isMenu,
                Image = getCategory.Image,
                TypeId = getCategory.TypeId,
                StyleId = getCategory.StyleId,
                Sorted = getCategory.Sorted,
                Position = getCategory.Position,
                UserId = getCategory.UserId,
            };
        }

        public CategoryDto GetCategoryByName(string categoryName)
        {
            Categories getCategory = _unitOfWork.GetRepository<Categories>().FindAsync(x => x.CategoryName == categoryName).Result;

            if (getCategory == null)
            {
                return new CategoryDto();
            }

            return new CategoryDto
            {
                Id = getCategory.Id,
                CategoryName = getCategory.CategoryName,
                Description = getCategory.Description,
                ParentCategoryId = getCategory.ParentCategoryId,
                IsActive = getCategory.IsActive,
                isMenu = getCategory.isMenu,
                UpdatedTime = getCategory.UpdatedTime,
                Sorted = getCategory.Sorted,
                styles = getCategory.stylePosts,
                StyleId = getCategory.StyleId,
                typeFooter = getCategory.typeToCategories,
                TypeId = getCategory.TypeId,
                CreatedTime = getCategory.CreatedTime,
                Image = getCategory.Image,
                Position = getCategory.Position,
                UserId = getCategory.UserId,
                user = getCategory.user
            };
        }

        public CategoryDto GetCategoryFirstOrDefault()
        {
            Categories getCategory = _unitOfWork.GetRepository<Categories>().Filter(x=> x.IsActive == true,x=> x.OrderBy(x=> x.Id),"",null,null).Take(1).FirstOrDefault();

            if (getCategory == null)
            {
                return new CategoryDto();
            }

            return new CategoryDto
            {
                Id = getCategory.Id,
                CategoryName = getCategory.CategoryName,
                Description = getCategory.Description,
                ParentCategoryId = getCategory.ParentCategoryId,
                IsActive = getCategory.IsActive,
                UpdatedTime = getCategory.UpdatedTime,
                CreatedTime = getCategory.CreatedTime,
                isMenu = getCategory.isMenu,
                Image = getCategory.Image,
                TypeId = getCategory.TypeId,
                StyleId = getCategory.StyleId,
                Sorted = getCategory.Sorted,
                Position = getCategory.Position,
                UserId = getCategory.UserId,
            };
        }

        public CategoryDto GetCategoryStyleId(int styleId)
        {
            Categories getCategory = _unitOfWork.GetRepository<Categories>().FindAsync(x => x.StyleId == styleId).Result;

            if (getCategory == null)
            {
                return new CategoryDto();
            }

            return new CategoryDto
            {
                Id = getCategory.Id,
                CategoryName = getCategory.CategoryName,
                Description = getCategory.Description,
                ParentCategoryId = getCategory.ParentCategoryId,
                IsActive = getCategory.IsActive,
                UpdatedTime = getCategory.UpdatedTime,
                CreatedTime = getCategory.CreatedTime,
                isMenu = getCategory.isMenu,
                Image = getCategory.Image,
                TypeId = getCategory.TypeId,
                StyleId = getCategory.StyleId,
                Sorted = getCategory.Sorted,
                Position = getCategory.Position,
                UserId = getCategory.UserId,
            };
        }

        public List<CategoryListItemDto> GetParentCategoryList()
        {
            IEnumerable<Categories> parentCategories = _unitOfWork.GetRepository<Categories>().Filter(x => x.ParentCategoryId == 0, y => y.OrderBy(t => t.Id), "user", 1, 100);

            return parentCategories.Select(x => new CategoryListItemDto
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                Description = x.Description,
                ParentCategoryId = x.ParentCategoryId,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                isMenu = x.isMenu,
                CreatedTime = x.CreatedTime,
                TypeId = x.TypeId,
                Sorted = x.Sorted,
                StyleId = x.StyleId,
                user = x.user,
                Position = x.Position,
                Image = x.Image,
                UserId = x.UserId,

            }).ToList();
        }

        public async Task<bool> UpdateCategory(CategoryDto model)
        {
            Categories categoryGet = await _unitOfWork.GetRepository<Categories>().FindAsync(x => x.Id == model.Id);
            if (model.ParentCategoryId == 999)
            {
                categoryGet.ParentCategoryId = 0;
            }

            if (model.Image == null)
            {
                model.Image = categoryGet.Image;
            }

            Categories getCategory = await _unitOfWork.GetRepository<Categories>().UpdateAsync(new Categories
            {
                Id = categoryGet.Id,
                CategoryName = model.CategoryName,
                Description = model.Description,
                ParentCategoryId = model.ParentCategoryId,
                IsActive = categoryGet.IsActive,
                isMenu = model.isMenu,
                UpdatedTime = DateTime.Now,
                TypeId = model.TypeId,
                Sorted = categoryGet.Sorted,
                StyleId = model.StyleId,
                CreatedTime = categoryGet.CreatedTime,
                user = categoryGet.user,
                stylePosts = categoryGet.stylePosts,
                typeToCategories = categoryGet.typeToCategories,
                Position = model.Position,
                Image = model.Image,
                UserId = model.UserId,
            });

            return getCategory != null;
        }

    }
}
