using ChatWithAuth.DAL.Data;
using chatWithAuthentication.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace VHW.BLL.Token
{
    public class TokenServices: ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateToken(ApplicationUser appUser, UserManager<ApplicationUser> userManager)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,appUser.UserName),
                new Claim(ClaimTypes.Email,appUser.Email)
            };
            var roles = await userManager.GetRolesAsync(appUser);
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:Duration"])),
                    claims: authClaims,
                    //HmacSha256   -   HmacSha512Signature
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
                ) ;
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
