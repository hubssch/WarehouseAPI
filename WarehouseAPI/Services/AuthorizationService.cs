using WarehouseAPI.Interfaces;
using WarehouseAPI.Data;
using WarehouseAPI.Models;
using WarehouseAPI.Classes;
using WarehouseAPI.Models.Dto;
using WarehouseAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Data.Entity;

namespace WarehouseAPI.Services
{
    public class AuthorizationService : IAuthenticationService
    {
        private readonly WarehouseAppDbContext _dbcontext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AuthorizationService(WarehouseAppDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbcontext = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterDataDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                RoleId = dto.RoleId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _dbcontext.Users.Add(newUser);
            _dbcontext.SaveChanges();
        }

        public LoggedUserRecordDto GenerateJwtAndGetUser(LoginDto dto)
        {
            var user = _dbcontext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new AuthenticationException("Invalid username or password");
            }

            Role r = _dbcontext.Roles.FirstOrDefault(r => r.Id == user.RoleId);

            if (r is not null) user.Role = r;
                
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new AuthenticationException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, $"{user.Email}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();

            user.PasswordHash = String.Empty;

            return new LoggedUserRecordDto {
                IsLogged = true,
                Token = tokenHandler.WriteToken(token),
                UserItem = user,
                Message = "Logged sucessfully"
            };
        }

        public User GetLoggedUser(StringValues token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new TokenException("No token provided");
            }

            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var decodedToken = jwtHandler.ReadJwtToken(token.ToString().Substring("Bearer ".Length));
                var userId = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var user = _dbcontext.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Id == Convert.ToInt32(userId));

                if (user != null) user.PasswordHash = String.Empty;

                Role r = _dbcontext.Roles.FirstOrDefault(r => r.Id == user.RoleId);

                if (r is not null) user.Role = r;

                return user;
            }
            catch (Exception)
            {
                throw new TokenException("Invalid token");
            }
        }
    }
}
