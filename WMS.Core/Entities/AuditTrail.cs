using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WMS.Core.Entities
{
    public abstract class AuditTrail
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(450)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedAt { get; set; }
        [MaxLength(450)]
        public string? ModifiedBy { get; set; }
    }
}
