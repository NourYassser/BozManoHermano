using StartUp.Models;

namespace BOZMANOHERMANO.Models
{
    public class PostView
    {
        public int Id { get; set; }
        public int PostsId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

        public Posts Post { get; set; }
        public ApplicationUser User { get; set; }
    }

}
