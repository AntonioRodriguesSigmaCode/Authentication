using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projetoAPI.Data;
using projetoAPI.Dto.Token;
using projetoAPI.Dto.User;
using projetoAPI.Model;

namespace projetoAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Utilizador?> RegisterAsync(UserRegisterDto request)
        {
            // verificar se já existe algum uername na bd
            if (await _context.Utilizadores.AnyAsync(u => u.Username == request.Username))
                return null;

            var user = new Utilizador
            {
                Username = request.Username
            };

            user.PasswordHash = new PasswordHasher<Utilizador>().HashPassword(user, request.Password);

            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateToken(Utilizador user)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT SecretKey não configurada!");

            var roles = await _context.UtilizadorRoles
                .Where(ur => ur.UtilizadorId == user.Id)
                .Select(ur => ur.Role.Nome)
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:TokenExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}