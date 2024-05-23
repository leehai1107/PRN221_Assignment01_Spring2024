using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class NewsArticleRepo : INewsArticleRepo
    {
        public void AddNewsArticle(NewsArticle newNewsArticle, List<Tag> tags)
        {
            using (FunewsManagementDbContext _context = new FunewsManagementDbContext())
            {
                // Check if the tags already exist in the database
                foreach (var tag in tags)
                {
                    var existingTag = _context.Tags.FirstOrDefault(t => t.TagId == tag.TagId);
                    if (existingTag != null)
                    {
                        // If the tag already exists, use the existing tag
                        newNewsArticle.Tags.Add(existingTag);
                    }
                    else
                    {
                        // If the tag doesn't exist, add it to the context
                        _context.Tags.Add(tag);
                        newNewsArticle.Tags.Add(tag);
                    }
                }

                // Add the newNewsArticle to the context
                _context.NewsArticles.Add(newNewsArticle);

                // Save the changes to the database
                _context.SaveChanges();
            }
        }


        public void DeleteNewsArticle(string id)
        {
            using (FunewsManagementDbContext _context = new())
            {
                NewsArticle existData = _context.NewsArticles
                                    .Include(x => x.Tags)
                                    .FirstOrDefault(x => x.NewsArticleId == id);
                if (existData != null)
                {
                    existData.Tags.Clear();
                    _context.NewsArticles.Update(existData);
                    _context.SaveChanges();
                }
                else
                {
                    // Handle the case where the category with the specified ID is not found
                    throw new ArgumentException("NewsArticle not found", nameof(id));
                }
            }

            using (FunewsManagementDbContext _context = new())
            {
                var existData = _context.NewsArticles.Find(id);
                if (existData != null)
                {
                    _context.NewsArticles.Remove(existData);
                    _context.SaveChanges();
                    _context.Entry(existData).State = EntityState.Detached;

                }
                else
                {
                    // Handle the case where the category with the specified ID is not found
                    throw new ArgumentException("NewsArticle not found", nameof(id));
                }
            }
        }

        public List<NewsArticle> GetNewsArticles()
        {
            using (FunewsManagementDbContext _context = new())
                return _context.NewsArticles.Include(x => x.Tags).Include(z => z.Category).Include(y => y.CreatedBy).ToList();
        }

        public List<NewsArticle> GetNewsArticlesByAccountId(short accountId)
        {
            using (FunewsManagementDbContext _context = new())
                return _context.NewsArticles.Include(x => x.Tags).Include(z => z.Category).Include(y => y.CreatedBy).Where(x => x.CreatedById == accountId).ToList();
        }

        public void UpdateNewsArticle(NewsArticle updatedNewsArticle, List<Tag> tags)
        {
            using (FunewsManagementDbContext _context = new FunewsManagementDbContext())
            {
                // Load the existing NewsArticle entity from the database
                var existingNewsArticle = _context.NewsArticles
                    .Include(n => n.Tags) // Include the Tags navigation property
                    .FirstOrDefault(n => n.NewsArticleId == updatedNewsArticle.NewsArticleId);

                if (existingNewsArticle != null)
                {
                    // Clear existing tags from the NewsArticle entity
                    existingNewsArticle.Tags.Clear();

                    // Update other properties of the NewsArticle entity if needed
                    existingNewsArticle.NewsTitle = updatedNewsArticle.NewsTitle;
                    existingNewsArticle.NewsContent = updatedNewsArticle.NewsContent;
                    existingNewsArticle.CreatedDate = updatedNewsArticle.CreatedDate;
                    existingNewsArticle.CategoryId = updatedNewsArticle.CategoryId;
                    existingNewsArticle.CreatedDate = updatedNewsArticle.CreatedDate;
                    existingNewsArticle.ModifiedDate = updatedNewsArticle.ModifiedDate;
                    existingNewsArticle.CreatedById = updatedNewsArticle.CreatedById;
                    existingNewsArticle.NewsStatus = updatedNewsArticle.NewsStatus;


                    // Associate the updated tags with the existing NewsArticle entity
                    foreach (var tag in tags)
                    {
                        var existingTag = _context.Tags.FirstOrDefault(t => t.TagId == tag.TagId);
                        if (existingTag != null)
                        {
                            // If the tag already exists, use the existing tag
                            existingNewsArticle.Tags.Add(existingTag);
                        }
                        else
                        {
                            // If the tag doesn't exist, add it to the context and associate it with the NewsArticle
                            _context.Tags.Add(tag);
                            existingNewsArticle.Tags.Add(tag);
                        }
                    }

                    // Save the changes to the database
                    _context.SaveChanges();
                }
            }
        }

        public List<NewsArticle> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate)
        {
            using (FunewsManagementDbContext _context = new FunewsManagementDbContext())
            {
                return _context.NewsArticles
                    .Include(x => x.Tags)
                    .Include(z => z.Category)
                    .Include(y => y.CreatedBy)
                    .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
                    .ToList();
            }
        }

        public List<NewsArticle> SearchNewsArticlesByTitle(string title)
        {
            using (FunewsManagementDbContext _context = new())
            {
                // Query the database for categories matching the provided name
                return _context.NewsArticles
                    .Include(x => x.Tags)
                    .Include(z => z.Category)
                    .Include(y => y.CreatedBy)
                               .Where(n => n.NewsTitle.Contains(title))
                               .ToList();
            }
        }

    }
}
