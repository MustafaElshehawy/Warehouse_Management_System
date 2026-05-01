using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Enums;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class ReportController : Controller
    {
       private readonly IUnitOfWork _unitOfWork;
        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public IActionResult Index(ActionType? ActionType, int? ProductId, int? WarehouseId ,DateTime? StartDate, DateTime? EndDate)
        {
            var movementsfilter = _unitOfWork.StockMovement
                .GetAll(
                sm => (!StartDate.HasValue || sm.CreateAt >= StartDate)
                && (!EndDate.HasValue || sm.CreateAt <= EndDate)
                && (!ProductId.HasValue || sm.ProductId == ProductId)
                && (!WarehouseId.HasValue || sm.UnitId == WarehouseId)
                && (!ActionType.HasValue || sm.ActionType == ActionType)
                ,IncludeWord: "Product,Warehouse,Unit"
                );
            var movementVM = new StockMovementReportVM()
            {
                ActionTypeList = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().Select(e => new SelectListItem {
                    Text = e.ToString(),
                    Value = e.ToString()

                }),

                productList = _unitOfWork.Product.GetAll().Select(p => new SelectListItem {
                    Text =p.Name,
                    Value =p.Id.ToString()
                }),

                WarehouseList =_unitOfWork.Warehouse.GetAll().Select(w=> new SelectListItem
                {
                    Text=w.Name,
                    Value=w.Id.ToString()
                }),
                MovementsList = movementsfilter,



            };
            

           
            return View(movementVM);
        }
    }
}
