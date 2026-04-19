using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WMS.Core.Entities
{
    public class Branche:AuditTrail
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="من فضلك  ادخل اسم الفرع")]
        public string Name { get; set; }

        [Required(ErrorMessage = "من فضلك  ادخل عنوان الفرع")]
        public string Address { get; set; }

        public Boolean IsActive { get; set; } = true;

    }
}
