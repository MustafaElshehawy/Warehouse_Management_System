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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            var products = _unitOfWork.Product.GetAll(IncludeWord: "ProductUnits");

            var usersDict = _unitOfWork.User.GetAll().ToDictionary(u => u.Id, u => u.UserName);
            var unitsDict = _unitOfWork.Unit.GetAll().ToDictionary(u => u.Id, u => u.Name);
            var catgDict = _unitOfWork.Category.GetAll().ToDictionary(u => u.Id, u => u.Name);

            var model = products.Select(p => new ProductVM
            {
                Product = p,
                CreatedByName = !String.IsNullOrEmpty(p.CreatedBy) && usersDict.ContainsKey(p.CreatedBy) ? usersDict[p.CreatedBy] : "غير معروف",
                ModifiedByName = !string.IsNullOrEmpty(p.ModifiedBy) && usersDict.ContainsKey(p.ModifiedBy) ? usersDict[p.ModifiedBy] : "---",
                CategorybyName = catgDict[p.CategoryId],
                UnitByName = unitsDict[p.SmallestUnitId]

            }).ToList();
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString(),

                }),
                UnitList = _unitOfWork.Unit.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })

            };
            return View(productVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(ProductVM productVM, List<IFormFile> files)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                productVM.Product.ModifiedAt = DateTime.UtcNow;
                productVM.Product.CreatedBy = userId;
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complete();
                string RootPath = _webHostEnvironment.WebRootPath;//وصلت لفولدر wwwroot

                if (files != null && files.Count > 0 && files.Count < 5)
                {
                    foreach (var galleryFile in files)
                    {
                        string filename = Guid.NewGuid().ToString();
                        var upload = Path.Combine(RootPath, @"Images/Products/Gallery");
                        if (!Directory.Exists(upload))
                        {
                            Directory.CreateDirectory(upload);
                        }
                        var ext = Path.GetExtension(galleryFile.FileName);

                        using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                        {
                            galleryFile.CopyTo(filestream);
                        }
                        Image Image = new Image
                        {
                            ImageUrl = @"Images/Products/Gallery/" + filename + ext,
                            ProductId = productVM.Product.Id
                        };
                        _unitOfWork.Image.Add(Image);
                    }
                }

                _unitOfWork.Complete();

                TempData["message"] = "تم إضافة المنتج والصور بنجاح";

                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            productVM.UnitList = _unitOfWork.Unit.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            return View(productVM);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == 0) return NotFound();

            var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, IncludeWord: "Images");
            if (product == null) return NotFound();

            ProductVM productVM = new ProductVM()
            {
                Product = product,
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                UnitList = _unitOfWork.Unit.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductVM productVM, List<IFormFile> files)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(productVM.Product, userId);
                _unitOfWork.Complete();

                string RootPath = _webHostEnvironment.WebRootPath;

                if (files != null && files.Count > 0 && files.Count < 5)
                {
                    foreach (var galleryFile in files)
                    {
                        string filename = Guid.NewGuid().ToString();
                        var upload = Path.Combine(RootPath, @"Images/Products/Gallery");
                        if (!Directory.Exists(upload)) Directory.CreateDirectory(upload);

                        var ext = Path.GetExtension(galleryFile.FileName);
                        using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                        {
                            galleryFile.CopyTo(filestream);
                        }

                        Image newImage = new Image
                        {
                            ImageUrl = @"/Images/Products/Gallery/" + filename + ext,
                            ProductId = productVM.Product.Id
                        };
                        _unitOfWork.Image.Add(newImage);
                    }
                    _unitOfWork.Complete();
                }

                TempData["message"] = "تم تحديث المنتج بنجاح";
                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            productVM.UnitList = _unitOfWork.Unit.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVM);
        }
    }
}
