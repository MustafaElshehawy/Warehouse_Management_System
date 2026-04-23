using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMS.Application.Dto;
using WMS.Core.Entities;

namespace WMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO userFromRequest)
        { 
            if(ModelState.IsValid)
            {
                //saveDb
                ApplicationUser user = new ApplicationUser();
                user.UserName = userFromRequest.UserName;
                user.Email = userFromRequest.Email;
                user.PhoneNumber = userFromRequest.PhoneNumber;
                IdentityResult result = await _userManager.CreateAsync(user, userFromRequest.Password);
                if (result.Succeeded)
                {
                    var roleResult= await _userManager.AddToRoleAsync(user, SD.CustomerRole);
                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user); //data intigrity to sure user added  With Role
                        return BadRequest("فشل تخصيص الصلاحيات، تم إلغاء التسجيل.");
                    }
                    return Ok("تم  إنشاء الحساب بنجاح");
                }
                foreach(var item in result.Errors)
                {

                    ModelState.AddModelError("Password", item.Description);
                }

            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if (ModelState.IsValid)
            { 
                //check user 
                var userInDb = await _userManager.FindByEmailAsync(userFromRequest.Email);
                if (userInDb == null)
                {
                    return Unauthorized("الاسم او البريد الاليكتروني غير صحيح");

                }

                //check pass
                bool found =await _userManager.CheckPasswordAsync(userInDb, userFromRequest.Password);
                if (!found) 
                {
                    return Unauthorized("الاسم او البريد الاليكتروني غير صحيح ");
                }
                //generate token
                

                //معلومات اليوزر اللي بنحطها جوه التوكين جزء payload 
                List<Claim> userClaims =new List<Claim>();
                userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));//but JWT ID Unique
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userInDb.Id));
                userClaims.Add(new Claim(ClaimTypes.Email, userInDb.Email));


                var userRoles = await _userManager.GetRolesAsync(userInDb);
                foreach (var role in userRoles) {
                    userClaims.Add(new Claim(ClaimTypes.Role,role));
                
                }

                //signiture
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);//Hash(Payload + Key)

                //--->desigin token
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires:DateTime.UtcNow.AddHours(1),
                    claims: userClaims,
                    signingCredentials: SigningCredentials

                    );

                //finish design and now generete in token 
                return Ok(new { 
                
                    Token=new JwtSecurityTokenHandler().WriteToken(token),
                    expiration=token.ValidTo,
                });

            }
            return BadRequest(ModelState);
        
        
        
        }



    }
}
