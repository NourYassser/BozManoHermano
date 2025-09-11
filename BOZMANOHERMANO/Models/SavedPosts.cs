using StartUp.Models;

namespace BOZMANOHERMANO.Models
{
    public class SavedPosts
    {
        public int Id { get; set; }

        public DateTime SaveDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public Posts Post { get; set; }

    }
}
