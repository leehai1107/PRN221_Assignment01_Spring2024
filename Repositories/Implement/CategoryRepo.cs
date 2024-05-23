using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class CategoryRepo : ICategoryRepo
    {
        public List<Category> GetCategories()
        {
            using (FunewsManagementDbContext _context = new())
                return _context.Categories.Include(x => x.NewsArticles).ToList();
        }

        public Category GetCategoryById(short id)
        {
            using (FunewsManagementDbContext _context = new())
                return _context.Categories.Find(id);
        }

        public void AddCategory(Category category)
        {
            using (FunewsManagementDbContext _context = new())
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
        }

        public void RemoveCategory(short id)
        {
            using (FunewsManagementDbContext _context = new())

            {  // Find the category in the database based on its ID
                var categoryToRemove = _context.Categories.Find(id);

                if (categoryToRemove != null)
                {
                    try
                    {
                        // Remove the category from the context
                        _context.Categories.Remove(categoryToRemove);

                        // Save the changes to the database
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Can not delete this category", ex);
                    }
                }
                else
                {
                    // Handle the case where the category with the specified ID is not found
                    throw new ArgumentException("Category not found", nameof(id));
                }
            }
        }

        public void UpdateCategory(Category newCategory)
        {
            using (FunewsManagementDbContext _context = new())

            {  // Retrieve the existing category from the database based on its ID
                var existingCategory = GetCategoryById(newCategory.CategoryId);

                if (existingCategory != null)
                {
                    // Update the properties of the existing category with the properties of the new category
                    existingCategory.CategoryName = newCategory.CategoryName;
                    existingCategory.CategoryDesciption = newCategory.CategoryDesciption;

                    _context.Categories.Update(existingCategory);
                    // Save the changes to the database
                    _context.SaveChanges();
                }
                else
                {
                    // Handle the case where the category with the specified ID is not found
                    throw new ArgumentException("Category not found", nameof(newCategory));
                }
            }
        }

        public List<Category> SearchCategoriesByName(string categoryName)
        {
            using (FunewsManagementDbContext _context = new())
            {
                // Query the database for categories matching the provided name
                return _context.Categories
                               .Where(c => c.CategoryName.Contains(categoryName))
                               .ToList();
            }
        }

    }
}
