using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WMS.Core.Entities
{
    public class ApplicationUser:IdentityUser
    {
        //Will Add New Field
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "رقم الهاتف يجب أن يكون مصرياً صحيحاً")]
        public override string PhoneNumber { get; set; }
    }
}
