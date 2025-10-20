using Microsoft.AspNetCore.Identity;

namespace CleaningApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
