using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Enums;

namespace WMS.Application.ViewModel
{
    public class StockMovementReportVM
    {
        //parameters 
        public ActionType? ActionType { get; set; }
        public int? ProductId { get; set; }
        public int? WarehouseId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        //view filter + table
        public IEnumerable<SelectListItem> ActionTypeList { get; set; }
        public IEnumerable<SelectListItem> productList { get; set; }

        public IEnumerable<SelectListItem> WarehouseList { get; set; }



        public IEnumerable<StockMovement> MovementsList { get; set; }

    }
}
