using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class SaleHeader:AuditTrail
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }

        public int BrancheId { get; set; }
        public Branche Branche { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public decimal TotalCost { get; set; }


        public ICollection<SaleDetails> Items { get; set; } = new HashSet<SaleDetails>();

    }
}
