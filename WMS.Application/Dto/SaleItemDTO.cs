using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class SaleItemDTO
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }

        public int UnitId { get; set; }
        public decimal Quentity { get; set; }
        
    }
}
