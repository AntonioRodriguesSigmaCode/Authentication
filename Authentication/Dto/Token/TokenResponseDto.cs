using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Dto.Token
{
    public class TokenResponseDto
    {
        public required string AcessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}