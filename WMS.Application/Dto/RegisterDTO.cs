using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WMS.Application.Dto
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "من فضلك ادخل اسم المستخدم")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="من فضلك ادخل البريد الاليكترني")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "رقم الهاتف يجب أن يكون مصرياً صحيحاً")]
        public  string PhoneNumber { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور")]
        [MinLength(6, ErrorMessage = "كلمة المرور لازم تكون 6 حروف على الأقل")] 
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",ErrorMessage = "لازم تحتوي على حرف كبير وصغير ورقم")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string ConfirmPassword { get; set; }
    }
}
