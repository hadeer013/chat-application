using ChatWithAuth.DAL.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace chatWithAuthentication.Token
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser appUser, UserManager<ApplicationUser> userManager);
    }
}
