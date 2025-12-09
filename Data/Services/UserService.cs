using CleaningApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleaningApp.Data.Services
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Pobierz listę wszystkich użytkowników wraz z informacją o rolach
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName ?? "Brak imienia",
                    Email = user.Email,
                    IsEmployee = roles.Contains("Employee"),
                    IsAdmin = roles.Contains("Admin")
                });
            }

            return userList;
        }

        // Przełącz rolę pracownika (Dodaj lub Usuń)
        public async Task ToggleEmployeeRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

            if (isEmployee)
            {
                await _userManager.RemoveFromRoleAsync(user, "Employee");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Employee");
            }
        }
        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}