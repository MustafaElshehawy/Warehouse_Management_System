using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WMS.Core.Entities
{
    public class Category:AuditTrail
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }= string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

    }
}
