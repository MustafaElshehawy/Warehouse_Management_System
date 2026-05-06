using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class SaleReturnHeader:AuditTrail
    {
        public int Id { get; set; }

        public int SaleHeaderId { get; set; }
        public PurchasesHeader SaleHeader { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TotalRefundAmount { get; set; }

        public ICollection<SaleReturnDetails> Items { get; set; } = new HashSet<SaleReturnDetails>();
    }
}
