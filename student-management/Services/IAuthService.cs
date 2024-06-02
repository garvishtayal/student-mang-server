using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using student_management.Models;

namespace student_management.Services
{
    public interface IAuthService
    {
        Task<AuthResult> SignInAsync(LoginModel model);
        Task<IdentityResult> AddUserAsync(RegisterModel model);
        Task<AuthResult> ValidateTokenAsync(string token);
        Task<IEnumerable<Student>> GetAllStudentEmails();
    }
}
