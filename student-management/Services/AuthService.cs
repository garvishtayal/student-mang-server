using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using student_management.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public async Task<AuthResult> SignInAsync(LoginModel model)
        {
            dynamic user = null;
            if (model.Role == "student")
            {
                user = await _context.Students.FirstOrDefaultAsync(u => u.Email == model.Email);
            }
            else if (model.Role == "parent")
            {
                user = await _context.Parents.FirstOrDefaultAsync(u => u.Email == model.Email);
            }
            else if (model.Role == "teacher" || model.Role == "admin")
            {
                user = await _context.Managements.FirstOrDefaultAsync(u => u.Email == model.Email);
            }

            if (user != null && user.Password == model.Password)
            {
                var token = JwtHelper.GenerateToken(user);
                return new AuthResult { Success = true, Token = token };
            }

            return new AuthResult { Success = false, Message = "Invalid email or password" };
        }

        public async Task<IdentityResult> AddUserAsync(RegisterModel model)
        {

            // Save the user in the corresponding table based on role
            if (model.Role == "student")
            {
                var student = new Student
                {
                    Email = model.Email,
                    Password = model.Password
                };
                await _context.Students.AddAsync(student);
            }
            else if (model.Role == "parent")
            {

                var student = await _context.Students.FirstOrDefaultAsync(u => u.Email == model.StudentMail);

                var parent = new Parent
                {
                    Email = model.Email,
                    Password = model.Password,
                    StudentId = student.Id,
                   
                };
                await _context.Parents.AddAsync(parent);
            }
            else if (model.Role == "teacher" || model.Role == "admin")
            {
                var teacher = new Management
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role
                };
                await _context.Managements.AddAsync(teacher);
            }

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
        public async Task<AuthResult> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_secret_key_here_test_key_123xyz");

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "your_issuer_here",
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Cast to JwtSecurityToken to access Claims
                var jwtToken = validatedToken as JwtSecurityToken;

                if (jwtToken == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                // Perform any additional checks on the roleClaim if needed

                return new AuthResult { Success = true, Message = roleClaim };
            }

            catch (Exception ex)
            {
                // Log the error to the console or a logging framework
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return new AuthResult { Success = false, Message = $"Invalid token: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentEmails()
        {
            return await _context.Students.ToListAsync();
        }


    }


    //function for JWT generation
    public static class JwtHelper
    {
        private static readonly string _secretKey = "your_secret_key_here_test_key_123xyz";
        private static readonly string _issuer = "your_issuer_here";

        public static string GenerateToken(Management user, int expireMinutes = 30)
        {
            return GenerateToken(user.Email, user.Role, expireMinutes);
        }

        public static string GenerateToken(Student user, int expireMinutes = 30)
        {
            return GenerateToken(user.Email, "student", expireMinutes);
        }

        public static string GenerateToken(Parent user, int expireMinutes = 30)
        {
            return GenerateToken(user.Email, "parent", expireMinutes);
        }

        private static string GenerateToken(string email, string role, int expireMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }




}