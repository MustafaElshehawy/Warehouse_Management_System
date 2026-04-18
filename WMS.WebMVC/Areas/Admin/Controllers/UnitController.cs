using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles=SD.SuperAdminRole)]
    public class UnitController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var units =_unitOfWork.Unit.GetAll();

            var usersDict = _unitOfWork.User.GetAll().ToDictionary(u => u.Id, u => u.UserName);

            var model = units.Select(u => new UnitVM {
                Unit=u,
                CreatedByName = !String.IsNullOrEmpty(u.CreatedBy) && usersDict.ContainsKey(u.CreatedBy) ? usersDict[u.CreatedBy]:"غير معروف",
                ModifiedByName = !string.IsNullOrEmpty(u.ModifiedBy) && usersDict.ContainsKey(u.ModifiedBy)?usersDict[u.ModifiedBy] : "---"

            }).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Unit unit)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                unit.CreatedAt = DateTime.UtcNow;
                unit.CreatedBy = userId;
                _unitOfWork.Unit.Add(unit);
                _unitOfWork.Complete();
                TempData["message"] = "تم إضافة الوحدة الرئسية بنجاح";

                return RedirectToAction("Index");
            }
            return View(unit);
        }


        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var unitIndb = _unitOfWork.Unit.GetFirstOrDefault(x => x.Id == id);
            if (unitIndb == null)
            {
                return NotFound();

            }
            return View(unitIndb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(Unit unit)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                _unitOfWork.Unit.Update(unit, userId);
                _unitOfWork.Complete();

                TempData["message"] = "تم تعديل الوحدة بنجاح";
                return RedirectToAction("Index");

            }
            return View();

        }

        [HttpGet]
        // added restrict  not allaw delete if have product in Fluent API
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var unitIndb = _unitOfWork.Unit.GetFirstOrDefault(x => x.Id == id);
            if (unitIndb == null)
            {
                return NotFound();
            }
            _unitOfWork.Unit.Remove(unitIndb);
            _unitOfWork.Complete();
            TempData["message"] = "تم حذف الوحدة بنجاح";
            return RedirectToAction("Index");

        }
    }
}
