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
    [Authorize(Roles = SD.CustomerRole)]

    public class PurchasesReturnController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        public PurchasesReturnController(IUnitOfWork unitOfWork, IStockService stockService)
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
        }

        [HttpPost("CreateReturn")]
        public IActionResult CreateReturn(PurchaseReturnDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // orignalPurchase ---> to validate quintity and price
            var originalPurchase = _unitOfWork.PurchaseHeader.GetFirstOrDefault(
                p => p.Id == dto.PurchaseHeaderId,
                IncludeWord: "Items"
            );

            if (originalPurchase == null)
            {
                return BadRequest("الفاتورة الأصلية غير موجودة");
            }
               

            //Save Return Header ?--> to take id and put in Return Details
            var returnHeader = new PurchasesReturnHeader
            {
                PurchaseHeaderId = dto.PurchaseHeaderId,
                ReturnDate = DateTime.Now,
                TotalRefundAmount = dto.Items.Sum(i => i.Quantity * i.UnitPrice),
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            _unitOfWork.PurchaseReturnHeader.Add(returnHeader);
            _unitOfWork.Complete();

            //Save detail بقا
            foreach (var item in dto.Items)
            {
                // بتأكد إن الصنف موجود والكمية تسمح
                var originalItem = originalPurchase.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (originalItem == null )
                {
                    return BadRequest($"الصنف غير موجود بالفاتورة الأصلية");
                }
                if (item.Quantity > originalItem.Quantity)
                {
                    return BadRequest($"{item.Quantity}خطأ في كمية الصنف المتاح في الفاتوره الأصلية ");
                }

                // 4- Save Return Detail
                var returnDetail = new PurchasesReturnDetails
                {
                    PurchasesReturnHeaderId = returnHeader.Id,
                    ProductId = item.ProductId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalRowCost = item.Quantity * item.UnitPrice,
                    CreatedBy = userId
                };
                _unitOfWork.PurchaseReturnDetails.Add(returnDetail);

                // أهم نقطه update stock 
                var stockResult = _stockService.ProcessMovement(new ProccessMovementDTO
                {
                    ProductId = item.ProductId,
                    WarehouseId = originalPurchase.WarehouseId, //  استلمنا فيه هو اللي هنرجع منه
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    InOut = MovementDirection.Out, // مرتجع شراء يعني خروج 
                    ActionType = ActionType.ReturnPurchase,
                    ReferenceId = returnHeader.Id,
                    ReferenceType = "PurchasesReturnHeader",
                    CostPrice = item.UnitPrice
                }, userId);

                if (!stockResult)
                {
                    return BadRequest($"فشل تحديث المخزن للصنف");
                }
                    
            }

            _unitOfWork.Complete(); //for ReturnDetails And StockProccess

            return Ok(new { status = true, message = "تم تسجيل المرتجع بنجاح" });
        }
    }
}
