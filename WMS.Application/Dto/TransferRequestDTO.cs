using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class TransferRequestDTO
    {
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }

        public int FromWarehouseId { get; set; } 
        public int ToWarehouseId { get; set; }   

        public long TransferId { get; set; }   
    }
}
