using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    public class UserDataChangeRequestController : Controller
    {
        private readonly IUserDataChangeRequestService _service ;
        public UserDataChangeRequestController(IUserDataChangeRequestService service)
        {
            _service = service;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            return View(_service.GetAllUnapprovedUserDataAsync());
        }
        [HttpPost]
        public JsonResult Reject(string id)
        {
            _service.RejectChangeRequest(id);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> Approve(string id)
        {
            await _service.ApproveChangeRequestAsync(id);
            return Json(true);
        }
        public ActionResult Show(string id)
        {
            var model = _service.ShowChangeRequest(id);
            return View(model.Result);
        }
    }
}
