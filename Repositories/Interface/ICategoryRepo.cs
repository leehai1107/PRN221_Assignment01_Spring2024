using BusinessObjects;

namespace Repositories.Interface
{
    public interface ICategoryRepo
    {
        public List<Category> GetCategories(); // <>
        public Category GetCategoryById(short id);
        public void RemoveCategory(short id);
        public void UpdateCategory(Category newCategory);
        public void AddCategory(Category Category);
        public List<Category> SearchCategoriesByName(string categoryName);
    }
}
