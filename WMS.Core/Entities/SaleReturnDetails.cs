using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class SaleReturnDetails
    {
        public int Id { get; set; }
        public int SaleReturnHeaderId { get; set; }

        public SaleReturnHeader SaleReturnHeader { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalRowCost { get; set; }
    }
}
