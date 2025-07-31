using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Services
{
    public interface ITagService
    {
        Task<Tag> GetTagByIdAsync(int id);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<IEnumerable<Tag>> GetActiveTagsAsync();
        Task<IEnumerable<Tag>> GetTagsByTypeAsync(TagType type);
        Task<Tag> AddTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
    }
} 