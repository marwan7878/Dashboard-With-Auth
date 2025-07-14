using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Auth.Authorization
{
    public class PermissionScanner
    {
        public static List<string> GetAllActionPermissions()
        {
            var permissions = new List<string>();

            var controllers = Assembly.GetEntryAssembly()?
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type));

            foreach (var controller in controllers!)
            {
                bool controllerHasAutoAuthorize = controller.IsDefined(typeof(AutoAuthorizeAttribute), inherit: true);
                if (controllerHasAutoAuthorize)
                {
                    var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                        .Where(m => !m.IsDefined(typeof(NonActionAttribute)));
                    foreach (var action in actions)
                    {
                        var controllerName = controller.Name.Replace("Controller", "");
                        var actionName = action.Name;

                        permissions.Add($"{controllerName}.{actionName}");
                    }
                }
            }

            return permissions.OrderBy(x => x).ToList();
        }
    }

}
