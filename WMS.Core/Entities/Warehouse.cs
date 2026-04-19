using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WMS.Core.Entities
{
    public class Warehouse:AuditTrail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك  ادخل اسم الفرع")]
        public string Name { get; set; }

        [Required(ErrorMessage = "من فضلك  ادخل عنوان الفرع")]
        public string Address { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branche? Branch { get; set; }


    }
}
