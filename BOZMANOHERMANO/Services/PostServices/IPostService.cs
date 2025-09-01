using BOZMANOHERMANO.Dtos;
using BOZMANOHERMANO.Repo;
using StartUp.HiddenServices;
using StartUp.Models;

namespace BOZMANOHERMANO.Services.PostServices
{
    public interface IPostService
    {
        List<PostDto> GetPosts(string username);
        List<PostDto> GetFollowingPosts();

        void Post(AddPostDto post);
    }
    public class PostService : IPostService
    {
        private readonly IPostsRepo _postsRepo;
        private readonly IUserContext _userContext;
        public PostService(IPostsRepo postsRepo, IUserContext userContext)
        {
            _postsRepo = postsRepo;
            _userContext = userContext;
        }
        public List<PostDto> GetPosts(string username)
        {
            var posts = _postsRepo.GetPosts(username);
            return posts.Select(p => new PostDto
            {
                UserId = p.UserId,
                TagName = p.User.TagName,
                UserName = p.User.UserName,
                Content = p.Content,
                ImagePath = p.ImagePath,
                Likes = p.Likes,
                Retweets = p.Retweets,
                Comments = p.Comments,
                CommentList = p.CommentList?.Select(c => new CommentsDto
                {
                    UserId = c.UserId,
                    UserName = c.UserName,
                    TagName = c.TagName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToList()
            }).ToList();
        }
        public List<PostDto> GetFollowingPosts()
        {
            var userId = _userContext.GetUserId();

            var followingList = _postsRepo.Following(userId);

            var posts = _postsRepo.GetFollowingPosts(followingList);
            return posts.Select(p => new PostDto
            {
                UserId = p.UserId,
                TagName = p.User.TagName,
                UserName = p.User.UserName,
                Content = p.Content,
                ImagePath = p.ImagePath,
                Likes = p.Likes,
                Retweets = p.Retweets,
                Comments = p.Comments,
                CommentList = p.CommentList?.Select(c => new CommentsDto
                {
                    UserId = c.UserId,
                    UserName = c.UserName,
                    TagName = c.TagName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToList()
            }).ToList();
        }
        public void Post(AddPostDto postDto)
        {
            var userId = _userContext.GetUserId();

            var post = new Posts
            {
                Content = postDto.Content,
                ImagePath = postDto.ImagePath != null ? SaveImage(postDto.ImagePath) : null,
                Likes = 0,
                Retweets = 0,
                Comments = 0,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            _postsRepo.Post(post);
        }
        string? SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0) return null;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return "/images/" + uniqueFileName;
        }
    }
}
