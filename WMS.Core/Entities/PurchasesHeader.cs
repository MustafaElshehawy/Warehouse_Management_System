using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Entities
{
    public class PurchasesHeader:AuditTrail
    {
        public int Id { get; set; }

        public string SupplierName { get; set; }

        public int BrancheId { get; set; }
        public Branche Branche { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public decimal TotalCost { get; set; }
    }
}
