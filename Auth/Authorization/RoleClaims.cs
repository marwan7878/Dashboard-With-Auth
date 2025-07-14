using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auth.Authorization
{
    public class RoleClaimsService 
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleClaimsService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }
        //public async Task<ClaimsModel?> GetClaimsForRoleAsync(int roleId)
        //{
        //    var role = await _roleManager.FindByIdAsync(roleId.ToString());
        //    if (role == null)
        //        return null;

        //    var existingClaims = await _roleManager.GetClaimsAsync(role);

        //    return new ClaimsModel
        //    {
        //        RoleId = role.Id,
        //        //FirstClaimsList = Build(ClaimStore.FirstClaimList, existingClaims),

        //    };
        //}

        //public async Task<bool> UpdateRoleClaimsAsync(int roleId, ClaimsModel model)
        //{
        //    var role = await _roleManager.FindByIdAsync(roleId.ToString());
        //    if (role == null)
        //        return false;

        //    var oldClaims = await _roleManager.GetClaimsAsync(role);
        //    foreach (var claim in oldClaims)
        //        await _roleManager.RemoveClaimAsync(role, claim);

        //    var allGroups = new[]
        //    {
        //    model.FirstClaimsList,
        //};

        //    foreach (var group in allGroups)
        //    {
        //        foreach (var claim in group)
        //        {
        //            if (claim.IsSelected)
        //            {
        //                await _roleManager.AddClaimAsync(role, new Claim(claim.ClaimType, "true"));
        //            }
        //        }
        //    }

        //    return true;
        //}


        //private List<ClaimSelection> Build(List<Claim> storeList, IList<Claim> existingClaims)
        //{
        //    return storeList.Select(c => new ClaimSelection
        //    {
        //        ClaimType = c.Type,
        //        Label = c.Value,
        //        IsSelected = existingClaims.Any(ec => ec.Type == c.Type)
        //    }).ToList();
        //}
    }
}