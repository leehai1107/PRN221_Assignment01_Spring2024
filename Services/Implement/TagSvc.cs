using BusinessObjects;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class TagSvc : ITagSvc
    {
        private ITagRepo _tagRepo;
        public TagSvc()
        {
            _tagRepo = new TagRepo();
        }

        public void AddTag(Tag newTag)
        {
            _tagRepo.AddTag(newTag);
        }

        public void DeleteTag(int id)
        {
            _tagRepo.DeleteTag(id);
        }

        public Tag GetTagById(int id)
        {
            return _tagRepo.GetTagById(id);
        }

        public List<Tag> GetTags()
        {
            return _tagRepo.GetTags();
        }

        public void UpdateTag(Tag newTag)
        {
            _tagRepo.UpdateTag(newTag);
        }
        public List<Tag> SearchTagsByName(string tagName)
        {
            return _tagRepo.SearchTagsByName(tagName);
        }

    }
}
