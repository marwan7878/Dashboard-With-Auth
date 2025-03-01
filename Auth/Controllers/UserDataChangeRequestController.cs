using Auth.Interfaces;
using Auth.Models;
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
        [ValidateAntiForgeryToken]
        public IActionResult Reject(string id)
        {
            _service.RejectChangeRequest(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(string id)
        {
            _service.ApproveChangeRequest(id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Show(string id)
        {
            var model = _service.ShowChangeRequest(id);
            return View(model.Result);
        }




        // GET: UserDataChangeRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDataChangeRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserDataChangeRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserDataChangeRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserDataChangeRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserDataChangeRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
