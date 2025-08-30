using Microsoft.AspNetCore.Identity;

namespace StartUp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string TagName { get; set; } = string.Empty;
        public string? ProfilePicPath { get; set; } = string.Empty;
        public string? HeaderPath { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public int Followers { get; set; } = 0;
        public int Following { get; set; } = 0;
        public ICollection<Posts> Posts { get; set; } = new HashSet<Posts>();

    }
}
