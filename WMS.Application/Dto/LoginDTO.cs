using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WMS.Application.Dto
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "من فضلك ادخل البريد الاليكترني")]
        [EmailAddress]
        public string Email { get; set; }
       
        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور")]
        [MinLength(6, ErrorMessage = "كلمة المرور لازم تكون 6 حروف على الأقل")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "لازم تحتوي على حرف كبير وصغير ورقم")]
        public string Password { get; set; }

    }
}
