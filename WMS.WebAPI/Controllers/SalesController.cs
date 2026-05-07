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
    public class SalesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        public SalesController(IUnitOfWork unitOfWork, IStockService stockService)
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
        }
        [HttpPost("CreateSale")]
        public IActionResult CreateSale(SaleDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //1-Save header
            var header = new SaleHeader
            {
                CustomerName = dto.CustomerName,
                BrancheId = dto.BranchId,
                WarehouseId = dto.WareHouseId,
                TotalCost = 0,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };
            _unitOfWork.SaleHeader.Add(header);
            _unitOfWork.Complete();//to take id of header to put in details table

            decimal totalHeaderCost = 0;

            foreach (var item in dto.Items)
            {
                //to calc netprofit and check price
                var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == item.ProductId);
                if (product == null) return BadRequest($"المنتج {item.ProductId} غير موجود");

                var detail = new SaleDetails
                {
                    SaleHeaderId = header.Id,
                    ProductId = item.ProductId,
                    UnitId = item.UnitId,
                    Quentity = item.Quentity,
                    CostAtTime = product.SellingPrice,
                    NetProfit = (product.SellingPrice -product.CostPrice)*item.Quentity,
                    CreatedBy = userId,

                };
                _unitOfWork.SaleDetails.Add(detail);
                totalHeaderCost += (product.SellingPrice * item.Quentity);

                var stockResult = _stockService.ProcessMovement(new ProccessMovementDTO
                {
                    ProductId = item.ProductId,
                    WarehouseId = dto.WareHouseId,
                    UnitId = item.UnitId,
                    Quantity = item.Quentity,
                    InOut = MovementDirection.Out, // بيع يعني خروج
                    ActionType = ActionType.Sale,
                    ReferenceId = header.Id,
                    ReferenceType = "SaleHeader",
                    CostPrice = product.SellingPrice
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
            header.TotalCost = totalHeaderCost;
            _unitOfWork.Complete();


            return Ok(new
            {
                status = true,
                message = "تم تسجيل المبيعات وتحديث المخزن بنجاح",
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
