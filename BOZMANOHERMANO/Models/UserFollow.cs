using StartUp.Models;

namespace BOZMANOHERMANO.Models
{
    public class UserFollow
    {
        public string FollowerId { get; set; } = null!;
        public ApplicationUser Follower { get; set; } = null!;

        public string FollowedId { get; set; } = null!;
        public ApplicationUser Followed { get; set; } = null!;

        public DateTime FollowedDate { get; set; } = DateTime.UtcNow.Date;
    }
}
