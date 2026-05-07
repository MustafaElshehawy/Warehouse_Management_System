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
    public class SaleReturnController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        public SaleReturnController(IUnitOfWork unitOfWork, IStockService stockService)
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
        }

        [HttpPost("CreateReturn")]
        public IActionResult CreateReturn(SaleReturnDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // originalSale ---> to validate quintity and price
            var originalSale = _unitOfWork.SaleHeader.GetFirstOrDefault(
                p => p.Id == dto.SaleHeaderId,
                IncludeWord: "Items"
            );

            if (originalSale == null)
            {
                return BadRequest("الفاتورة الأصلية غير موجودة");
            }


            //Save Return Header ?--> to take id and put in Return Details
            var returnHeader = new SaleReturnHeader
            {
                SaleHeaderId = dto.SaleHeaderId,
                ReturnDate = DateTime.Now,
                TotalRefundAmount = dto.SaleReturnItems.Sum(i => i.Quantity * i.UnitPrice),
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            _unitOfWork.SaleReturnHeader.Add(returnHeader);
            _unitOfWork.Complete();

            //Save detail بقا
            foreach (var item in dto.SaleReturnItems)
            {
                // بتأكد إن الصنف موجود والكمية تسمح
                var originalItem = originalSale.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (originalItem == null)
                {
                    _unitOfWork.SaleReturnHeader.Remove(returnHeader);
                    _unitOfWork.Complete();
                    return BadRequest($"الصنف غير موجود بالفاتورة الأصلية");
                }
                if (item.Quantity > originalItem.Quentity)
                {
                    _unitOfWork.SaleReturnHeader.Remove(returnHeader);
                    _unitOfWork.Complete();
                    return BadRequest($"{item.Quantity}خطأ في كمية الصنف المتاح في الفاتوره الأصلية ");
                }

                // 4- Save Return Detail
                var returnDetail = new SaleReturnDetails
                {
                    SaleReturnHeaderId = returnHeader.Id,
                    ProductId = item.ProductId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalRowCost = item.Quantity * item.UnitPrice
                };
                _unitOfWork.SaleReturnDetails.Add(returnDetail);

                // أهم نقطه update stock 
                var stockResult = _stockService.ProcessMovement(new ProccessMovementDTO
                {
                    ProductId = item.ProductId,
                    WarehouseId = originalSale.WarehouseId, //  استلمنا منه هو اللي هنرجع منه
                    UnitId = item.UnitId,
                    Quantity = item.Quantity,
                    InOut = MovementDirection.In, // مرتجع بيع يعني دخول 
                    ActionType = ActionType.ReturnSale,
                    ReferenceId = returnHeader.Id,
                    ReferenceType = "SaleReturnHeader",
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
