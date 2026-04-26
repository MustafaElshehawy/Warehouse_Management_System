using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class PurchasesResponseDTO
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; } 
        public string WarehouseName { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PurchaseItemResponseDTO> Items { get; set; }
    }
}
