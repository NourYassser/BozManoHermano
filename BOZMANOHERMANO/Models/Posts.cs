namespace StartUp.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ImagePath { get; set; } = string.Empty;
        public int Likes { get; set; } = 0;
        public int Retweets { get; set; } = 0;
        public int Comments { get; set; } = 0;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public List<Comments> CommentList { get; set; }
        public List<Likes> LikesList { get; set; }
        public List<Retweets> RetweetsList { get; set; }
    }
    public class Comments
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int PostId { get; set; }
        public Posts? Post { get; set; }
        public ApplicationUser? User { get; set; }
    }
    public class Likes
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public Posts? Post { get; set; }
        public ApplicationUser? User { get; set; }
    }
    public class Retweets
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public Posts? Post { get; set; }
        public ApplicationUser? User { get; set; }
    }
}

