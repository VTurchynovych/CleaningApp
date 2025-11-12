using Microsoft.AspNetCore.Identity;

namespace CleaningApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}
