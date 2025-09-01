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
        void DeletePost(int id);

        void Comment(CommentDto comments);
        void DeleteComment(int id);

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
                Comments = p.CommentList.Count,
                CommentList = p.CommentList?.Select(c => new CommentsDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    TagName = c.User.TagName,
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
                Comments = p.CommentList.Count,
                CommentList = p.CommentList?.Select(c => new CommentsDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    TagName = c.User.TagName,
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

        public void DeletePost(int id)
        {
            var userId = _userContext.GetUserId();

            _postsRepo.DeletePost(userId, id);
        }

        public void Comment(CommentDto comments)
        {
            var userId = _userContext.GetUserId();

            var entity = new Comments()
            {
                UserId = userId,
                PostId = comments.PostId,
                Content = comments.Content,
                CreatedAt = DateTime.UtcNow
            };

            _postsRepo.Comment(entity);
        }
        public void DeleteComment(int id)
        {
            var userId = _userContext.GetUserId();

            _postsRepo.DeleteComment(userId, id);
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
