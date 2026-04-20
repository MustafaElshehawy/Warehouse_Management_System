using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Enums;

namespace WMS.Application.Dto
{
    public class ProccessMovementDTO
    {
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }

        public decimal Quantity { get; set; }
        public MovementDirection InOut { get; set; }

        public ActionType ActionType { get; set; }
        public long ReferenceId { get; set; }
        public string ReferenceType { get; set; }

        public decimal CostPrice { get; set; }

    }
}
