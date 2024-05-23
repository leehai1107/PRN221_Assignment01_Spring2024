using BusinessObjects;

namespace Services.Interface
{
    public interface ICategorySvc
    {
        public List<Category> GetCategories(); //<-- <List>
        public Category GetCategoryById(short id);
        public void RemoveCategory(short id);
        public void UpdateCategory(Category newCategory);
        public void AddCategory(Category Category);
        public List<Category> SearchCategoriesByName(string categoryName);
    }
}
