using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Collections;
using System.Security.Claims;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            //Key->id value->UserName
            var usersDict = _unitOfWork.User.GetAll().ToDictionary(u => u.Id, u => u.UserName);

            var model = categories.Select(c => new CategoryVM
            {
                Category = c,
                CreatedByName = !string.IsNullOrEmpty(c.CreatedBy) && usersDict.ContainsKey(c.CreatedBy)
                        ? usersDict[c.CreatedBy] : "غير معروف",
                ModifiedByName = !string.IsNullOrEmpty(c.ModifiedBy) && usersDict.ContainsKey(c.ModifiedBy)
                        ? usersDict[c.ModifiedBy] : "---"
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
        public IActionResult Create(Category category)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                category.CreatedAt = DateTime.UtcNow;
                category.CreatedBy = userId;
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["message"] = "تم إضافة الصنف بنجاح";

                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                return NotFound();

            }
            return View(categoryIndb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(Category category)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category, userId);
                _unitOfWork.Complete();

                TempData["message"] = "تم تعديل الصنف بنجاح";
                return RedirectToAction("Index");

            }
            return View();

        }

        [HttpGet]
        //will add restrict  not allaw delete if have product
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryIndb);
            _unitOfWork.Complete();
            TempData["message"] = "تم حذف الصنف بنجاح";
            return RedirectToAction("Index");

        }


    }
}
