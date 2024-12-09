using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace api.Services
{
    public interface ICalculatorService
    {
        string GenerateJwtToken(User user);
        string HashPassword(string password);
        string GenerateRandomCode();
    }
}