using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using projetoAPI.Dto.Token;
using projetoAPI.Model;
using projetoAPI.Dto.User;

namespace projetoAPI.Service
{
    public interface IAuthService
    {
        Task<Utilizador?> RegisterAsync(UserRegisterDto request);
        Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}