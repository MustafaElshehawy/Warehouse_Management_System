using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using WMS.Application.ViewModel;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class StockController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StockController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? WarehouseId)
        {
            var stockfilter=_unitOfWork.Stock.GetAll(s=>s.WarehouseId == WarehouseId || !WarehouseId.HasValue ,IncludeWord: "Warehouse,Product,Unit");

            var stockVM = new StockVM()
            {
                WarehouseList = _unitOfWork.Warehouse.GetAll().Select(w => new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString()
                }),
                StockList =stockfilter,
            };
            return View(stockVM);
        }
    }
}
