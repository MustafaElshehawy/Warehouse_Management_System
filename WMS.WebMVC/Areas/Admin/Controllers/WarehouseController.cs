using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using System.Reflection.Metadata;
using System.Security.Claims;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.SuperAdminRole)]
    public class WarehouseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public WarehouseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Warehouses = _unitOfWork.Warehouse.GetAll();

            var usersDict = _unitOfWork.User.GetAll().ToDictionary(u => u.Id, u => u.UserName);

            var model = Warehouses.Select(w=>new WarehouseVM { 
            Warehouse= w,
                CreatedByName = !string.IsNullOrEmpty(w.CreatedBy) && usersDict.ContainsKey(w.CreatedBy) ? usersDict[w.CreatedBy] : "غير معروف",
                ModifiedByName = !string.IsNullOrEmpty(w.ModifiedBy) && usersDict.ContainsKey(w.ModifiedBy) ? usersDict[w.ModifiedBy] : "---",

            });

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            WarehouseVM warehouseVM = new WarehouseVM()
            {
                Warehouse = new Warehouse(),
                BrancheList =_unitOfWork.Branche.GetAll().Select(w =>new SelectListItem { 
                    Text = w.Name,
                    Value =w.Id.ToString(),
                }),
            };

            return View(warehouseVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Warehouse warehouse)
        {
            var claimsIdentity =(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                warehouse.CreatedBy = userId;
                _unitOfWork.Warehouse.Add(warehouse);
                _unitOfWork.Complete();

                TempData["message"] = "تم إضافة المخزن بنجاح";
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
            var WarehouseInDb = _unitOfWork.Warehouse.GetFirstOrDefault(w => w.Id == id);
            if (WarehouseInDb == null)
            {
                return NotFound();

            }
            var branches = _unitOfWork.Branche.GetAll().Select(b=>new SelectListItem { 
            Text =b.Name,
            Value =b.Id.ToString(),
            
            });
            WarehouseVM model = new WarehouseVM()
            {
                Warehouse=WarehouseInDb,
                BrancheList= branches
            };
            return View(model);
            
        }

        public IActionResult Update(Warehouse warehouse)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState != null)
            {
                _unitOfWork.Warehouse.Update(warehouse, userId);
                _unitOfWork.Complete();
                TempData["message"] = "تم تعديل المخزن بنجاح";
                return RedirectToAction("Index");

            }
            return View();

        }

    }
}
