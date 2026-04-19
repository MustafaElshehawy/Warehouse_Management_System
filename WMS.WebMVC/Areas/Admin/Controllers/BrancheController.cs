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
    [Authorize(Roles =SD.SuperAdminRole)]
    public class BrancheController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrancheController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Brances = _unitOfWork.Branche.GetAll();

            var usersDict = _unitOfWork.User.GetAll().ToDictionary(u => u.Id, u => u.UserName);

            var model = Brances.Select(b => new BrancheVM { 
            Branche = b,
            CreatedByName = !string.IsNullOrEmpty(b.CreatedBy) && usersDict.ContainsKey(b.CreatedBy) ? usersDict[b.CreatedBy]:"غير معروف",
            ModifiedByName = !string.IsNullOrEmpty(b.ModifiedBy) && usersDict.ContainsKey(b.ModifiedBy) ? usersDict[b.ModifiedBy] : "---",

            }).ToList(); 

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Branche branche)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                branche.CreatedBy=userId;
                _unitOfWork.Branche.Add(branche);
                _unitOfWork.Complete();
                TempData["message"] = "تم إضافة الفرع بنجاح";

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var brancheInDb = _unitOfWork.Branche.GetFirstOrDefault(b => b.Id == id);
            if (brancheInDb == null)
            {
                return NotFound();

            }
            return View(brancheInDb);
        }

        [HttpPost]
        public IActionResult Update(Branche branche)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState != null)
            {
                _unitOfWork.Branche.Update(branche, userId);
                _unitOfWork.Complete();
                TempData["message"] = "تم تعديل الفرع بنجاح";
                return RedirectToAction("Index");
            
            }
            return View();
        
        
        }
    }
}
