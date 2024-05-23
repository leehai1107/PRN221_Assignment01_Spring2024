using BusinessObjects;

namespace Repositories.Interface
{
    public interface INewsArticleRepo
    {
        public List<NewsArticle> GetNewsArticles();
        public List<NewsArticle> GetNewsArticlesByAccountId(short accountId);
        public List<NewsArticle> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate);
        public List<NewsArticle> SearchNewsArticlesByTitle(string title);
        public void DeleteNewsArticle(string id);
        public void UpdateNewsArticle(NewsArticle updatedNewsArticle, List<Tag> tags);
        public void AddNewsArticle(NewsArticle newNewsArticle, List<Tag> tags);
    }
}
