using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, Player user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
