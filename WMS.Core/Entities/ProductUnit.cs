using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WMS.Core.Entities
{
    public class ProductUnit:AuditTrail
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        public int ParentUnitId { get; set; }
        [ForeignKey("ParentUnitId")]
        [ValidateNever]
        public Unit ParentUnit { get; set; }

        public int ChildUnitId { get; set; }
        [ForeignKey("ChildUnitId")]
        [ValidateNever]
        public Unit ChildUnit { get; set; }
        //number of unit from parent to small
        public decimal UnitFactor { get; set; }
        public decimal ParentUnitPrice { get; set; }



    }
}
