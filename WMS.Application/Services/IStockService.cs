using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Dto;

namespace WMS.Application.Services
{
    public interface IStockService
    {
        //Update stock and save record in stockmovement
        public bool ProcessMovement(ProccessMovementDTO dto , string userId);

        public decimal GetCurrentBalance(int productId, int warehouseId, int unitId);

        //will call  ProcessMovementAsync two time one  in and one for out
        //public bool TransferBetweenWarehouses(TransferRequestDTO dto, string userId);
    }
}
