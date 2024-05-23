using BusinessObjects;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class NewsArticleSvc : INewsArticleSvc
    {
        private INewsArticleRepo _newsArticleRepo;
        public NewsArticleSvc()
        {
            _newsArticleRepo = new NewsArticleRepo();
        }

        public void AddNewsArticle(NewsArticle newNewsArticle, List<Tag> tags)
        {
            _newsArticleRepo.AddNewsArticle(newNewsArticle, tags);
        }

        public void DeleteNewsArticle(string id)
        {
            _newsArticleRepo.DeleteNewsArticle(id);
        }

        public List<NewsArticle> GetNewsArticles()
        {
            return _newsArticleRepo.GetNewsArticles();
        }

        public List<NewsArticle> GetNewsArticlesByAccountId(short accountId)
        {
            return _newsArticleRepo.GetNewsArticlesByAccountId(accountId);
        }

        public void UpdateNewsArticle(NewsArticle updatedNewsArticle, List<Tag> tags)
        {
            _newsArticleRepo.UpdateNewsArticle(updatedNewsArticle, tags);
        }
        public List<NewsArticle> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate)
        {
            return _newsArticleRepo.GetNewsArticlesByPeriod(startDate, endDate);
        }
        public List<NewsArticle> SearchNewsArticlesByTitle(string title)
        {
            return _newsArticleRepo.SearchNewsArticlesByTitle(title);
        }
    }
}
