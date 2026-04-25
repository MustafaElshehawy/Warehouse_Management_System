using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class PurchaseItemDTO
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExtraCost { get; set; }
        public decimal TotalRowCost { get; set; }
    }
}
