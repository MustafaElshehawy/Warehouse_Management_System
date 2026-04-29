using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class PurchaseReturnDTO
    {
        public int PurchaseHeaderId { get; set; }

        public List<PurchaseReturnItemDTO> Items { get; set; } = new List<PurchaseReturnItemDTO>();
    }
}
