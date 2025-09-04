using BOZMANOHERMANO.Dtos;
using BOZMANOHERMANO.Services.PostServices;
using Microsoft.AspNetCore.Mvc;

namespace BOZMANOHERMANO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        #region PostController
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
            _postService.Post(post);
            return Ok();
        }

        [HttpDelete("DeletePost")]
        public IActionResult DeletePost(int id)
        {
            _postService.DeletePost(id);
            return Ok();
        }
        #endregion


        #region CommentController
        [HttpPost("Comment")]
        public IActionResult Comment(CommentDto comment)
        {
            _postService.Comment(comment);
            return Ok();
        }

        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int id)
        {
            _postService.DeleteComment(id);
            return Ok();
        }
        #endregion


        #region LikeController
        [HttpGet("GetPostLikes")]
        public IActionResult GetPostLikes(int postid)
        {
            return Ok(_postService.PostLikes(postid));
        }

        [HttpPost("Like")]
        public IActionResult Like(int postid)
        {
            _postService.Like(postid);
            return Ok();
        }
        #endregion


        #region RetweetController
        [HttpGet("GetPostRetweets")]
        public IActionResult GetPostRetweets(int postid)
        {
            return Ok(_postService.PostRetweets(postid));
        }

        [HttpPost("Retweet")]
        public IActionResult Retweet(int postid)
        {
            _postService.Retweet(postid);
            return Ok();
        }
        #endregion
    }
}