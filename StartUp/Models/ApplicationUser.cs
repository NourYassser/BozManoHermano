using Microsoft.AspNetCore.Identity;

namespace StartUp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
