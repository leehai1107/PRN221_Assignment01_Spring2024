using BusinessObjects;

namespace Repositories.Interface
{
    public interface ITagRepo
    {
        public List<Tag> GetTags();

        public Tag GetTagById(int id);

        public void DeleteTag(int id);

        public void UpdateTag(Tag newTag);

        public void AddTag(Tag newTag);
        public List<Tag> SearchTagsByName(string tagName);

    }
}
