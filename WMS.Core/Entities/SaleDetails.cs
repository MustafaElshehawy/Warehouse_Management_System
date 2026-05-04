using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class SaleDetails:AuditTrail
    {
        public int Id { get; set; }

        public int SaleHeaderId { get; set; }
        public SaleHeader SaleHeader{ get; set; }

        public int ProductId { get; set; }
        public Product Product {  get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public decimal Quentity { get; set; }
        public decimal CostAtTime { get; set; }
        public decimal NetProfit { get; set; }

    }
}
