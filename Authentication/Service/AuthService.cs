using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Authentication.Dto.Token;
using Authentication.Dto.User;
using Authentication.Models;
using Authentication.Data;
using Authentication.Model;
using System.Runtime.CompilerServices;

namespace Authentication.Service
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
            if (await _context.Utilizadores.AnyAsync(u => u.UserName == request.Username))
                return null;

            var user = new Utilizador
            {
                UserName = request.Username
            };

            user.PasswordHash = new PasswordHasher<Utilizador>().HashPassword(user, request.Password);

            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
        {
			var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserName == request.Username);
			if (user == null) return null;

			var passwordCheck = new PasswordHasher<Utilizador>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
			if (passwordCheck == PasswordVerificationResult.Failed) return null;

			return await CreateTokenResponse(user);
		}

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
			var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
			if (user == null) return null;
			return await CreateTokenResponse(user);
		}

        public async Task<string> CreateToken(Utilizador user)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT SecretKey não configurada!");

            var roles = await _context.UtilizadorRole
                .Where(ur => ur.Utilizador == user)
                .Select(ur => ur.Role.Nome)
                .ToListAsync();*/

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            /*foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }*/

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

		private async Task<TokenResponseDto> CreateTokenResponse(Utilizador user)
		{
			return new TokenResponseDto
			{
				AcessToken = await CreateToken(user),
				RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
			};
		}

		private async Task<string> GenerateAndSaveRefreshTokenAsync(Utilizador user)
		{
			var refreshToken = GenerateRefreshToken();
			user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
			_context.Utilizadores.Update(user);
			await _context.SaveChangesAsync();
			return refreshToken;
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private async Task<Utilizador?> ValidateRefreshTokenAsync(int userId, string refreshToken)
		{
			var user = await _context.Utilizadores.FindAsync(userId);
			if (user == null ||user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow
            )
				return null;

			return user;
		}

	}
}