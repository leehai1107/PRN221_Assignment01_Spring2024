using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class TagRepo : ITagRepo
    {
        public void AddTag(Tag newTag)
        {
            using (FunewsManagementDbContext _context = new())
            {
                try
                {
                    _context.Tags.Add(newTag);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Can not add this tag", ex);
                }
            }
        }

        public void DeleteTag(int id)
        {
            using (FunewsManagementDbContext _context = new())

            {
                var existTag = _context.Tags.Find(id);
                if (existTag != null)
                {
                    try
                    {
                        _context.Tags.Remove(existTag);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Can not delete this tag", ex);

                    }
                }
                else
                {
                    // Handle the case where the category with the specified ID is not found
                    throw new ArgumentException("Tag not found", nameof(id));
                }
            }
        }

        public Tag GetTagById(int id)
        {
            using (FunewsManagementDbContext _context = new())
                return _context.Tags.Find(id);
        }

        public List<Tag> GetTags()
        {
            using (FunewsManagementDbContext _context = new())
                return _context.Tags.ToList();
        }

        public void UpdateTag(Tag newTag)
        {
            using (FunewsManagementDbContext _context = new())

            {
                var existTag = _context.Tags.Find(newTag.TagId);
                if (existTag != null)
                {
                    try
                    {
                        existTag.TagName = newTag.TagName;
                        existTag.Note = newTag.Note;
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Can not update this tag", ex);

                    }
                }
                else
                {
                    // Handle the case where the category with the specified ID is not found
                    throw new ArgumentException("Tag not found", nameof(newTag.TagId));
                }
            }
        }

        public List<Tag> SearchTagsByName(string tagName)
        {
            using (FunewsManagementDbContext _context = new())
            {
                // Query the database for categories matching the provided name
                return _context.Tags
                               .Where(t => t.TagName.Contains(tagName))
                               .ToList();
            }
        }
    }
}
