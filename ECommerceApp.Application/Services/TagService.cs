using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Tag>> GetActiveTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Where(t => t.IsActive).OrderBy(t => t.SortOrder);
        }

        public async Task<IEnumerable<Tag>> GetTagsByTypeAsync(TagType type)
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Where(t => t.IsActive && t.Type == type).OrderBy(t => t.SortOrder);
        }

        public async Task<Tag> AddTagAsync(Tag tag)
        {
            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveChangesAsync();
            return tag;
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _tagRepository.Update(tag);
            await _tagRepository.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag != null)
            {
                // Soft delete
                tag.DeletedAt = DateTime.UtcNow;
                tag.IsActive = false;
                _tagRepository.Update(tag);
                await _tagRepository.SaveChangesAsync();
            }
        }
    }
} 