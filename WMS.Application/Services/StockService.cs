using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Dto;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Enums;
using WMS.Core.Repositories;

namespace WMS.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  bool ProcessMovement(ProccessMovementDTO dto,string userId)
        {

            //1-بروح اشوف الصنف دا موجود عندي ولا لا  طبعاا بي  3 اعمده اللي حددناهم للتميز
            //if true  getit
            //if false  add new in stock with quentity zero
            var stockInDB = _unitOfWork.Stock.GetFirstOrDefault(s=>s.ProductId==dto.ProductId && s.WarehouseId == dto.WarehouseId && s.UnitId==dto.UnitId);
            if (stockInDB == null)
            {
                stockInDB = new Stock
                {
                    WarehouseId = dto.WarehouseId,
                    ProductId = dto.ProductId,
                    UnitId = dto.UnitId,
                    Quantity = 0,
                    CreatedBy=userId

                };

                _unitOfWork.Stock.Add(stockInDB);
            }

            //2-calc new quentity 
            //if In  NewQ = current + MovementQuentity
            //if Out NewQ = current - MovementQuentity --note check if (الكمية  اللي موجوده  تكفي ولا لا)
            if (dto.InOut == MovementDirection.In)
            {
                stockInDB.Quantity += dto.Quantity;
                
            }
            else
            {
                if (stockInDB.Quantity < dto.Quantity)
                {
                    return false;

                }
                stockInDB.Quantity -= dto.Quantity;
               
            }
            stockInDB.ModifiedBy = userId;
            stockInDB.ModifiedAt = DateTime.Now;

            //3-الرقم اللي هيطلع من 2 هنحفظه في balanceAfter(new record stockMovement)
            var stockMovement = new StockMovement
            {
                WarehouseId=dto.WarehouseId,
                ProductId=dto.ProductId,
                UnitId=dto.UnitId,
                Quantity=dto.Quantity,
                InOut=dto.InOut,
                ActionType = dto.ActionType,
                ReferenceId = dto.ReferenceId,
                ReferenceType = dto.ReferenceType,
                BalanceAfter = stockInDB.Quantity, //خدت قيمه  النهايه بعد in or out اللي عملنها فوق 
                CostPrice=dto.CostPrice,
                CreatedBy = userId

            };
            _unitOfWork.StockMovement.Add(stockMovement);

            var result = _unitOfWork.Complete();
            //Complete  if save return 1  else return 0
            //4-احفظ كله في حركه واحده 
            return result>0;
        }

        public decimal GetCurrentBalance(int productId, int warehouseId, int unitId)
        {
            var stock = _unitOfWork.Stock.GetFirstOrDefault(s => s.WarehouseId == warehouseId && s.ProductId == productId && s.UnitId == unitId);

            return stock != null ? stock.Quantity : 0 ;
        }

        //public bool TransferBetweenWarehouses(TransferRequestDTO dto, string userId)
        //{
        //    //From Warehouse
            

        //    // To Warehouse
          
            
        //}

    }
}
