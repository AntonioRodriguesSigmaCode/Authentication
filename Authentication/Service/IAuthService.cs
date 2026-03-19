using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Authentication.Dto.Token;
using Authentication.Models;
using Authentication.Dto.User;
using Authentication.Model;

namespace Authentication.Service
{
    public interface IAuthService
    {
        Task<Utilizador?> RegisterAsync(UserRegisterDto request);
        Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}