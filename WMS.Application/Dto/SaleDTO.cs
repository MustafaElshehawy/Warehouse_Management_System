using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class SaleDTO
    {
        public String CustomerName {  get; set; }

        public int BranchId { get; set; }

        public int WareHouseId { get; set; }


        public List<SaleItemDTO> Items { get; set; }= new List<SaleItemDTO>();
    }
}
