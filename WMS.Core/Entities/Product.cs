using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WMS.Core.Entities
{
    public class Product:AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم المنتج")]
        public string Name { get; set; }
        public string? Description { get; set; } = "N/A";

        [Required(ErrorMessage = "من فضلك اختر صنف المنتج ")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }


        //لسه  لوجيك بتاعه بفكر اعمل  محرك للسعر  من عمليات الشراء لحساب التكلفه والبيع
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public int SmallestUnitId { get; set; }
        [ForeignKey("SmallestUnitId")]
        public Unit? Unit { get; set; }


        [ValidateNever]
        public List<Image> Images { get; set; }
        public ICollection<ProductUnit> ProductUnits { get; set; } = new HashSet<ProductUnit>();
    }
}
