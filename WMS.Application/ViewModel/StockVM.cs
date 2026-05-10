using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Enums;

namespace WMS.Application.ViewModel
{
    public class StockVM
    {
        //parameters 
        public int? WarehouseId { get; set; }


        //view filter + table
        public IEnumerable<SelectListItem> WarehouseList { get; set; }



        public IEnumerable<Stock> StockList { get; set; }
    }
}
