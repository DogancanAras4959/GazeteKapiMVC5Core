using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCore.UnitOfWork;
using DOMAIN.DataAccessLayer;
using DOMAIN.DataAccessLayer.Models;
using GazeteKapiMVC5Core.DataAccessLayer;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Categories newCategory = await _unitOfWork.GetRepository<Categories>().AddAsync(new Categories
            {
                CategoryName = model.CategoryName,
                ParentCategoryId = model.ParentCategoryId,
                Description = model.Description,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
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
            {
                getCategories.IsActive = true;
                Categories model = await _unitOfWork.GetRepository<Categories>().UpdateAsync(getCategories);
                return getCategories != null;
            }
            else
            {
                getCategories.IsActive = false;
                Categories model = await _unitOfWork.GetRepository<Categories>().UpdateAsync(getCategories);
                return getCategories != null;
            }
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
                user = x.user

            }).ToList();
        }

        public CategoryDto GetCategoryById(int id)
        {
            Categories getUser = _unitOfWork.GetRepository<Categories>().FindAsync(x => x.Id == id).Result;

            if (getUser == null)
            {
                return new CategoryDto();
            }

            return new CategoryDto
            {
                Id = getUser.Id,
                CategoryName = getUser.CategoryName,
                Description = getUser.Description,
                ParentCategoryId = getUser.ParentCategoryId,
                IsActive = getUser.IsActive,
                UpdatedTime = getUser.UpdatedTime,
                CreatedTime = getUser.CreatedTime,
                user = getUser.user
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
                CreatedTime = x.CreatedTime,
                user = x.user

            }).ToList();
        }

        public async Task<bool> UpdateCategory(CategoryDto model)
        {
            Categories categoryGet = await _unitOfWork.GetRepository<Categories>().FindAsync(x => x.Id == model.Id);

            Categories getCategory = await _unitOfWork.GetRepository<Categories>().UpdateAsync(new Categories
            {
                Id = categoryGet.Id,
                CategoryName = categoryGet.CategoryName,
                Description = categoryGet.Description,
                ParentCategoryId = categoryGet.ParentCategoryId,
                IsActive = categoryGet.IsActive,
                UpdatedTime = categoryGet.UpdatedTime,
                CreatedTime = categoryGet.CreatedTime,
                user = categoryGet.user,
                UserId = model.UserId,
            });

            return categoryGet != null;
        }
    }
}
