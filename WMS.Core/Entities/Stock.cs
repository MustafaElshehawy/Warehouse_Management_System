using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WMS.Core.Entities
{
    public class Stock:AuditTrail
    {
        public int Id { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }


    }
}
