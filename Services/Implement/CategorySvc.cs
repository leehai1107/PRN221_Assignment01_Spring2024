using BusinessObjects;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class CategorySvc : ICategorySvc
    {
        private ICategoryRepo _categoryRepo;
        public CategorySvc()
        {
            _categoryRepo = new CategoryRepo();
        }

        public List<Category> GetCategories()
        {
            return _categoryRepo.GetCategories();
        }

        public void AddCategory(Category category)
        {
            _categoryRepo.AddCategory(category);
        }

        public Category GetCategoryById(short id)
        {
            return _categoryRepo.GetCategoryById(id);
        }

        public void RemoveCategory(short id)
        {
            _categoryRepo.RemoveCategory(id);
        }

        public void UpdateCategory(Category newCategory)
        {
            _categoryRepo.UpdateCategory(newCategory);
        }
        public List<Category> SearchCategoriesByName(string categoryName)
        {
            return _categoryRepo.SearchCategoriesByName(categoryName);
        }
    }
}
