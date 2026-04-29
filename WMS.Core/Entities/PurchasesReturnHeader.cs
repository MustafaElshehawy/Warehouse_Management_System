using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class PurchasesReturnHeader : AuditTrail
    {
        public int Id { get; set; }

        public int PurchaseHeaderId { get; set; }
        public PurchasesHeader PurchasesHeader { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TotalRefundAmount { get; set; }

        public ICollection<PurchasesReturnDetails> Items { get; set; } = new HashSet<PurchasesReturnDetails>();
    }
}
