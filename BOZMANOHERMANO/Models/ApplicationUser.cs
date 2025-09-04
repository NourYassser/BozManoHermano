using BOZMANOHERMANO.Models;
using Microsoft.AspNetCore.Identity;

namespace StartUp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string TagName { get; set; } = string.Empty;
        public string? ProfilePicPath { get; set; } = string.Empty;
        public string? HeaderPath { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public ICollection<Posts> Posts { get; set; } = new HashSet<Posts>();

        public ICollection<UserFollow> Followings { get; set; } = new HashSet<UserFollow>();
        public ICollection<UserFollow> Followers { get; set; } = new HashSet<UserFollow>();

        public ICollection<Likes> Likes { get; set; } = new HashSet<Likes>();
        public ICollection<Retweets> Retweets { get; set; } = new HashSet<Retweets>();
        public ICollection<Comments> Comments { get; set; } = new HashSet<Comments>();
    }
}
