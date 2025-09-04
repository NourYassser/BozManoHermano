namespace BOZMANOHERMANO.Dtos
{
    public class PostDto
    {
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string TagName { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public int Likes { get; set; }
        public int Retweets { get; set; }
        public int Comments { get; set; }
        public List<LikesDto>? LikesDto { get; set; }
        public List<RetweetsDto>? RetweetsDto { get; set; }
        public List<CommentsDto>? CommentList { get; set; }
    }
    public class CommentDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
    public class CommentsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TagName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class AddPostDto
    {
        public string Content { get; set; }
        public IFormFile? ImagePath { get; set; }
    }

    public class LikesDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TagName { get; set; }
        public int PostId { get; set; }
    }
    public class RetweetsDto : LikesDto { }

}
