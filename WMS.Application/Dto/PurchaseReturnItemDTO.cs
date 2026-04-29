using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class PurchaseReturnItemDTO
    {
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
