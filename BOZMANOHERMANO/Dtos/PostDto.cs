namespace BOZMANOHERMANO.Dtos
{
    public class PostDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TagName { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public int Likes { get; set; }
        public int Retweets { get; set; }
        public int Comments { get; set; }
        public List<CommentsDto>? CommentList { get; set; }
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
}
