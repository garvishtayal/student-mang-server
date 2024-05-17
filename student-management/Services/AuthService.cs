using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using student_management.Models;
using System.Threading.Tasks;
using test_server.Data;

namespace student_management.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> SignInAsync(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null && user.Password == model.Password)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Invalid email or password" });
        }

        public async Task<IdentityResult> AddUserAsync(RegisterModel model)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}
