using Microsoft.AspNetCore.Mvc;

namespace Auth.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AutoAuthorizeAttribute : TypeFilterAttribute
    {
        public AutoAuthorizeAttribute() : base(typeof(AutoAuthorizeFilter))
        {
        }
    }
}
