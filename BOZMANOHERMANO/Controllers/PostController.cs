using BOZMANOHERMANO.Dtos;
using BOZMANOHERMANO.Services.Notifications;
using BOZMANOHERMANO.Services.PostServices;
using Microsoft.AspNetCore.Mvc;

namespace BOZMANOHERMANO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly INotificationService _notificationService;
        public PostController(IPostService postService,
            INotificationService notificationService)
        {
            _postService = postService;
            _notificationService = notificationService;
        }

        #region PostController
        [HttpGet("GetPost")]
        public IActionResult GetPost(int id)
        {
            return Ok(_postService.GetPostById(id));
        }

        [HttpGet("GetPosts")]
        public IActionResult GetPosts(string username)
        {
            return Ok(_postService.GetPosts(username));
        }

        [HttpGet("GetFollowingPosts")]
        public IActionResult GetFollowingPosts()
        {
            return Ok(_postService.GetFollowingPosts());
        }


        [HttpPost("Post")]
        public IActionResult Post(AddPostDto post)
        {
            var x = _postService.Post(post);
            return Ok(x);
        }

        [HttpDelete("DeletePost")]
        public IActionResult DeletePost(int id)
        {
            var x = _postService.DeletePost(id);
            return Ok(x);
        }
        #endregion


        #region SavedPostsController
        [HttpPost("Save")]
        public IActionResult Save(int postId)
        {
            var x = _postService.Save(postId);
            return Ok(x);
        }
        [HttpGet("GetSavedPosts")]
        public IActionResult GetSavedPosts()
        {
            return Ok(_postService.GetSavedPosts());
        }
        #endregion


        #region CommentController
        [HttpPost("Comment")]
        public async Task<IActionResult> Comment(CommentDto comment)
        {
            var x = _postService.Comment(comment);
            await _notificationService.AddNotificationAsync(_postService.GetPostUserId(comment.PostId), "Comment");
            return Ok(x);
        }

        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int id)
        {
            var x = _postService.DeleteComment(id);
            return Ok(x);
        }
        #endregion


        #region LikeController
        [HttpGet("GetPostLikes")]
        public IActionResult GetPostLikes(int postid)
        {
            return Ok(_postService.PostLikes(postid));
        }

        [HttpPost("Like")]
        public async Task<IActionResult> Like(int postid)
        {
            var x = _postService.Like(postid);

            await _notificationService.AddNotificationAsync(_postService.GetPostUserId(postid), "Like", postid.ToString());

            return Ok(x);
        }
        #endregion


        #region RetweetController
        [HttpGet("GetPostRetweets")]
        public IActionResult GetPostRetweets(int postid)
        {
            return Ok(_postService.PostRetweets(postid));
        }

        [HttpPost("Retweet")]
        public async Task<IActionResult> Retweet(int postid)
        {
            var x = _postService.Retweet(postid);

            await _notificationService.AddNotificationAsync(_postService.GetPostUserId(postid), "Retweet", postid.ToString());

            return Ok(x);
        }

        [HttpPost("RetweetWithThoughts")]
        public async Task<IActionResult> RetweetWithThoughts(RetweetWithThoughtsDto retweet)
        {
            var x = _postService.RetweetWithThoughts(retweet);

            await _notificationService.AddNotificationAsync(_postService.GetPostUserId(retweet.PostId), "Retweet", retweet.PostId.ToString());

            return Ok(x);
        }
        #endregion
    }
}