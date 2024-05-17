using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using student_management.Models;

namespace student_management.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> SignInAsync(LoginModel model);
        Task<IdentityResult> AddUserAsync(RegisterModel model);
    }
}
