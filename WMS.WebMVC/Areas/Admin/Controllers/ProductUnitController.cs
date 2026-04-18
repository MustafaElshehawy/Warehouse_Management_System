using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
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
            var productUnits = _unitOfWork.ProductUnit.GetAll();
            return View(productUnits);
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
        
    }
}
