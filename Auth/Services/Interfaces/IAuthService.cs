using Auth.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationViewModel> GetTokenAsync(TokenRequestViewModel model);
        Task<JwtSecurityToken> CreateJwtToken(string email);
        Task<string> AssignRoleAsync(AssignRoleViewModel model);
    }
}
