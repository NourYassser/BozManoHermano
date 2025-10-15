using StartUp.Models;

namespace BOZMANOHERMANO.Models
{
    public class Hashtag
    {
        public int Id { get; set; }
        public string Tag { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PostsHashtag> PostsHashtags { get; set; }
    }

    public class PostsHashtag
    {
        public int PostId { get; set; }
        public Posts Post { get; set; }
        public int HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }

}
