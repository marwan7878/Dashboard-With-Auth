namespace Auth.Authorization
{
    public interface IRolePermissionService
    {

        Task<List<string>> GetPermissionsByRoleAsync(string roleName);
        Task<bool> UpdateRolePermissionsAsync(string roleName, List<string> selectedPermissions);
    }
}

