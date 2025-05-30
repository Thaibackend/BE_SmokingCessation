using AutoMapper;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Models.DTOs.Post;
using SmokingQuitSupportAPI.Models.Entities;

namespace SmokingQuitSupportAPI.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostRequestDto request, int userId)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdPost = await _postRepository.AddAsync(post);
            return _mapper.Map<PostDto>(createdPost);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDto?> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return post == null ? null : _mapper.Map<PostDto>(post);
        }

        public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(int userId)
        {
            var posts = await _postRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDto?> UpdatePostAsync(int id, UpdatePostRequestDto request, int userId)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null || post.UserId != userId)
                return null;

            post.Title = request.Title;
            post.Content = request.Content;
            post.UpdatedAt = DateTime.UtcNow;

            var updatedPost = await _postRepository.UpdateAsync(post);
            return _mapper.Map<PostDto>(updatedPost);
        }

        public async Task<bool> DeletePostAsync(int id, int userId)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null || post.UserId != userId)
                return false;

            await _postRepository.DeleteAsync(id);
            return true;
        }
    }
} 