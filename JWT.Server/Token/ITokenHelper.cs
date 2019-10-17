using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWT.Server
{
    public interface ITokenHelper
    {
        ComplexToken CreateToken(User user);
        ComplexToken CreateToken(Claim[] claims);
        Token RefreshToken(ClaimsPrincipal claimsPrincipal);
    }
}
