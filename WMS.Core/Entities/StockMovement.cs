using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WMS.Core.Enums;

namespace WMS.Core.Entities
{
    public class StockMovement
    {
        public long Id { get; set; }//many many tranciaction
        [Required(ErrorMessage ="من فضلك اختر المخزن")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        [Required(ErrorMessage = "من فضلك اختر المنتج")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required(ErrorMessage = "من فضلك اختر الوحدة")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        [Required(ErrorMessage = "من فضلك اكتب الكمية")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public MovementDirection InOut { get; set; }

        public ActionType ActionType { get; set; }
        public long ReferenceId { get; set; } 
        public string ReferenceType { get; set; } // "Sales", "Purchase", .. ## save const variable in Share layer
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        



        //AuditTrial only created
        public DateTime CreateAt { get; set; }=DateTime.Now;
        [MaxLength(450)]
        public string CreatedBy { get; set; } = string.Empty;

    }
}
