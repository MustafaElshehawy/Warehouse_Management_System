using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;
using WMS.Application.Dto;
using WMS.Application.Services;
using WMS.Core.Entities;
using WMS.Core.Enums;
using WMS.Core.Repositories;

namespace WMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =SD.CustomerRole)]
    public class PurchasesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        public PurchasesController(IUnitOfWork unitOfWork,IStockService stockService)
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
        }

        [HttpPost("CreatePurchase")]
        public IActionResult CreatePurchase(PurchaseDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //1-Save header
            var header = new PurchasesHeader
            {
                SupplierName= dto.SupplierName,
                BrancheId = dto.BranchId,
                WarehouseId =dto.WarehouseId,
                TotalCost=dto.Items.Sum(i=>i.Quantity * i.UnitPrice),
                CreatedAt=DateTime.Now,
                CreatedBy=userId
            };
            _unitOfWork.PurchaseHeader.Add(header);
            _unitOfWork.Complete();//to take id of header to put in details table

            foreach (var item in dto.Items) 
            {
                var detail = new PurchasesDetails
                {
                    PurchaseId=header.Id,
                    ProductId=item.ProductId,
                    UnitId=item.UnitId,
                    Quantity =item.Quantity,
                    UnitPrice=item.UnitPrice,
                    TotalRowCost=item.Quantity*item.UnitPrice,
                    ExtraCost=item.ExtraCost,
                    CreatedBy=userId,
                    
                };
                _unitOfWork.PurchaseDetails.Add(detail);

                // أهم خطوة: نستخدم الـ StockService اللي إنت عملتها
                var stockResult=_stockService.ProcessMovement(new ProccessMovementDTO
                {
                    ProductId = item.ProductId,
                    WarehouseId = dto.WarehouseId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    InOut = MovementDirection.In, // شراء يعني دخول
                    ActionType = ActionType.Purchase,
                    ReferenceId = header.Id,
                    ReferenceType = "PurchasesHeader",
                    CostPrice = item.UnitPrice
                }, userId);
                if (!stockResult)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = $"فشل في تحديث المخزن للمنتج رقم {item.ProductId}"
                    });
                }
                _unitOfWork.Complete();
            }


            return Ok(new
            {
                status = true,
                message = "تم تسجيل المشتريات وتحديث المخزن بنجاح",
                data = new
                {
                    purchaseId = header.Id,
                    totalAmount = header.TotalCost,
                    itemsCount = dto.Items.Count
                }
            });

        }
    }
}
