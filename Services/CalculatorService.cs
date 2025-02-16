using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Context;
using api.Model;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class CalculatorService : ICalculatorService
    {
          private readonly IConfiguration _configuration;
    private readonly apiContext _context; // فرض کنید که DbContext شما این نام است

    public CalculatorService(IConfiguration configuration, apiContext context)
    {
        _configuration = configuration;
        _context = context;
    }

public string GenerateJwtToken(User user)
{
    var claims = new[]
    {
        new Claim("UserId", user.Id.ToString()), // اضافه کردن شناسه کاربر
        new Claim("firstName", user.firstName),
        new Claim("lastName", user.lastName),
        new Claim("phoneNumber", user.phoneNumber),
        new Claim("role", user.role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Issuer"],
        claims: claims,
        expires: DateTime.Now.AddDays(7),
        signingCredentials: creds);

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    // ذخیره توکن در پایگاه داده
    var userToken = new UserToken
    {
        Token = tokenString,
        UserId = user.Id,
        IsExpired = false
    };

    _context.userTokens.Add(userToken);
    _context.SaveChanges();

    return tokenString;
}

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public string GenerateRandomCode()
        {
            Random random = new Random();
            return random.Next(10000, 99999).ToString(); // تولید یک عدد ۵ رقمی
        }
    }
}