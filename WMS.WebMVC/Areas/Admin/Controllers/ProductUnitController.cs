using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using System.Security.Claims;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class ProductUnitController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductUnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var productUnits = _unitOfWork.ProductUnit.GetAll(IncludeWord: "ParentUnit,Product,ChildUnit");
            var usersDict = _unitOfWork.User.GetAll().ToDictionary(u => u.Id, u => u.UserName);

            var model = productUnits.Select(u => new ProductUnitCreateVM
            {
                ProductUnit = u,
                CreatedByName = !string.IsNullOrEmpty(u.CreatedBy) && usersDict.ContainsKey(u.CreatedBy) ? usersDict[u.CreatedBy] : "غير معروف",
                ModifiedByName = !string.IsNullOrEmpty(u.ModifiedBy) && usersDict.ContainsKey(u.ModifiedBy) ? usersDict[u.ModifiedBy] : "---"

            }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetAvailableChildUnits(int productId)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, IncludeWord: "Unit");
            if (product == null) return Json(new List<object>());

            var allOptions = new List<object>();

            // 1. الوحدة الأساسية  - السعر البيع بتاعها 
            allOptions.Add(new
            {
                id = product.SmallestUnitId,
                name = product.Unit.Name,
                price = product.SellingPrice
            });

            // 2. التحويلات اللي  عررفتها قبل كدا  الخاص بالمنتج 
            var existingUnits = _unitOfWork.ProductUnit.GetAll(u => u.ProductId == productId, IncludeWord: "ParentUnit");

            foreach (var item in existingUnits)
            {
                allOptions.Add(new
                {
                    id = item.ParentUnitId,
                    name = item.ParentUnit.Name,
                    price = item.ParentUnitPrice 
                });
            }

            
            var result = allOptions.GroupBy(u => u.GetType().GetProperty("id").GetValue(u))
                                  .Select(g => g.First())
                                  .ToList();

            return Json(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ProductUnitCreateVM productUnitCreateVM = new()
            {
                ProductUnit = new ProductUnit(),
                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                ParentUnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                ChildUnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(productUnitCreateVM);
        }

        [HttpPost]
        public IActionResult Create(ProductUnit productUnit)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                productUnit.CreatedBy = userId;
                _unitOfWork.ProductUnit.Add(productUnit);
                _unitOfWork.Complete();
                TempData["message"] = "تم إضافة الوحدة الفرعيه بنجاح";

                return RedirectToAction("Index");
            
            }
            ProductUnitCreateVM productUnitCreateVM = new()
            {
                ProductUnit = new ProductUnit(),
                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                ParentUnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                ChildUnitList = new List<SelectListItem>()
            };
            return View(productUnitCreateVM);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var productUnit = _unitOfWork.ProductUnit.GetFirstOrDefault(u => u.Id == id, IncludeWord: "ChildUnit");
            if (productUnit == null) return NotFound();

            // بنجيب المنتج عشان نعرف سعر الوحدة الصغرى الحالية كام
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productUnit.ProductId);

            ProductUnitCreateVM vm = new()
            {
                ProductUnit = productUnit,
                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                ParentUnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                // هنا بنملاها بالوحدة الصغرى المسجلة حالياً عشان تظهر لليوزر
                ChildUnitList = new List<SelectListItem> {
            new SelectListItem { Text = productUnit.ChildUnit.Name, Value = productUnit.ChildUnitId.ToString() }
        }
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductUnitCreateVM vm)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductUnit.Update(vm.ProductUnit,userId);
                _unitOfWork.Complete();
                TempData["message"] = "تم تحديث الوحدة بنجاح";
                return RedirectToAction("Index");
            }
           
            return View(vm);
        }

    }
}
