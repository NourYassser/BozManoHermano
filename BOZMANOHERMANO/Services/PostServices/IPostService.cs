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

        string Post(AddPostDto post);
        string DeletePost(int id);

        List<LikesDto> PostLikes(int postid);
        string Like(int postid);

        List<RetweetsDto> PostRetweets(int postid);
        string Retweet(int postid);

        string Comment(CommentDto comments);
        string DeleteComment(int id);

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

        #region PostService
        public List<PostDto> GetPosts(string username)
        {
            var posts = _postsRepo.GetPosts(username);
            return posts.Select(p => new PostDto
            {
                UserId = p.UserId,
                CreatedDate = p.CreatedAt,
                UserName = p.User.UserName,
                TagName = p.User.TagName,
                Content = p.Content,
                ImagePath = p.ImagePath,
                Likes = p.LikesList.Count,
                Retweets = p.RetweetsList.Count,
                Comments = p.CommentList.Count,
                LikesDto = p.LikesList?.Select(c => new LikesDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    TagName = c.User.TagName,
                    PostId = c.PostId
                }).ToList(),
                RetweetsDto = p.RetweetsList?.Select(c => new RetweetsDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    TagName = c.User.TagName,
                    PostId = c.PostId
                }).ToList(),
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
                CreatedDate = p.CreatedAt,
                UserName = p.User.UserName,
                TagName = p.User.TagName,
                Content = p.Content,
                ImagePath = p.ImagePath,
                Likes = p.LikesList.Count,
                Retweets = p.RetweetsList.Count,
                Comments = p.CommentList.Count,
                LikesDto = p.LikesList?.Select(c => new LikesDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    TagName = c.User.TagName,
                    PostId = c.PostId
                }).ToList(),
                RetweetsDto = p.RetweetsList?.Select(c => new RetweetsDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    TagName = c.User.TagName,
                    PostId = c.PostId
                }).ToList(),
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
        public string Post(AddPostDto postDto)
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
            return _postsRepo.Post(post);
        }

        public string DeletePost(int id)
        {
            var userId = _userContext.GetUserId();

            return _postsRepo.DeletePost(userId, id);
        }
        #endregion

        #region CommentService
        public string Comment(CommentDto comments)
        {
            var userId = _userContext.GetUserId();

            var entity = new Comments()
            {
                UserId = userId,
                PostId = comments.PostId,
                Content = comments.Content,
                CreatedAt = DateTime.UtcNow
            };

            return _postsRepo.Comment(entity);
        }
        public string DeleteComment(int id)
        {
            var userId = _userContext.GetUserId();

            return _postsRepo.DeleteComment(userId, id);
        }
        #endregion

        #region LikeService
        public List<LikesDto> PostLikes(int postid)
        {
            var likes = _postsRepo.PostLikes(postid);
            return likes.Select(p => new LikesDto
            {
                Id = p.Id,
                PostId = p.PostId,
                UserId = p.UserId,
                UserName = p.User.UserName,
                TagName = p.User.TagName
            }).ToList();
        }

        public string Like(int postid)
        {
            return _postsRepo.Like(new Likes
            {
                PostId = postid,
                UserId = _userContext.GetUserId(),
            });
        }
        #endregion

        #region RetweetService
        public List<RetweetsDto> PostRetweets(int postid)
        {
            var likes = _postsRepo.PostRetweets(postid);
            return likes.Select(p => new RetweetsDto
            {
                Id = p.Id,
                PostId = p.PostId,
                UserId = p.UserId,
                UserName = p.User.UserName,
                TagName = p.User.TagName
            }).ToList();
        }
        public string Retweet(int postid)
        {
            return _postsRepo.Retweet(new Retweets
            {
                PostId = postid,
                UserId = _userContext.GetUserId(),
            });
        }
        #endregion

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
