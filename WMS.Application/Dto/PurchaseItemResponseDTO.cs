using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class PurchaseItemResponseDTO
    {
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitName { get; set; }
        public decimal ExtraCost { get; set; }
    }
}
