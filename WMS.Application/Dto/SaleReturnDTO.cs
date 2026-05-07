using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Dto
{
    public class SaleReturnDTO
    {
        public int SaleHeaderId { get; set; } //For oraginal sale invoice

        public List<SaleReturnItemsDto> SaleReturnItems { get; set;} = new List<SaleReturnItemsDto>();
    }
}
