using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projetoAPI.Dto.Token
{
    public class RefreshTokenRequestDto
    {
        public int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}