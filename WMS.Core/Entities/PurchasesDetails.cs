using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WMS.Core.Entities
{
    public class PurchasesDetails:AuditTrail
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public PurchasesHeader Purchase { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExtraCost { get; set; }
        public decimal TotalRowCost { get; set; }
    }
}
