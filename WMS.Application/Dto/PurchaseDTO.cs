using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class PurchaseDTO
    {
        public string SupplierName { get; set; }
        public int BranchId { get; set; }
        public int WarehouseId { get; set; }

        public List<PurchaseItemDTO> Items { get; set; } = new List<PurchaseItemDTO>();
    }
}
