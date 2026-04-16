using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using System.Security.Claims;
using WMS.Application.ViewModel;
using WMS.Core.Entities;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManger;

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger)
        {
            _unitOfWork = unitOfWork;
            _userManger = userManager;
            _roleManger = roleManger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var users = _unitOfWork.User.GetAll(userId);

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UserVM userVM = new UserVM()
            {
                User = new ApplicationUser(),
                RoleList = _roleManger.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };
            return View(userVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(UserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = model.User.UserName,
                    Email = model.User.Email,
                    PhoneNumber = model.User.PhoneNumber
                };

                var userResult = await _userManger.CreateAsync(user, model.Password);

                if (userResult.Succeeded)
                {
                    //try to add role if fail delete user
                    try
                    {
                        var roleResult = await _userManger.AddToRoleAsync(user, model.Role);

                        if (!roleResult.Succeeded)
                        {
                            await _userManger.DeleteAsync(user);
                            ModelState.AddModelError("", "فشل إضافة صلحية المستخدم");
                        }
                        else
                        {
                            TempData["message"] = "تم إضافة المستخدم بنجاح";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        await _userManger.DeleteAsync(user);
                        ModelState.AddModelError("", "Error Exception : " + ex.Message);
                    }
                }

                foreach (var error in userResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.RoleList = _roleManger.Roles.Select(i => new SelectListItem { Text = i.Name, Value = i.Name });
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = _unitOfWork.User.GetFirstOrDefault(id);
            if (user == null)
            {

                return NotFound();
            }
            var roles = await _userManger.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault();

            var userVM = new UserVM
            {
                User = user,
                Role = currentRole,
                RoleList = _roleManger.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };
            var userRoles = _roleManger.Roles.Select(i => new SelectListItem { Text = i.Name, Value = i.Name });
            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserVM model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                // 1. GetuserFromDb
                var userFromDb = _unitOfWork.User.GetFirstOrDefault(model.User.Id);

                if (userFromDb == null) return NotFound();

                // 2. catch data from model
                userFromDb.UserName = model.User.UserName;
                userFromDb.Email = model.User.Email;
                userFromDb.PhoneNumber = model.User.PhoneNumber;


                var result = await _userManger.UpdateAsync(userFromDb);

                if (result.Succeeded)
                {
                    // 3. OldRole
                    var oldRoles = await _userManger.GetRolesAsync(userFromDb);

                    if (!oldRoles.Contains(model.Role))
                    {
                        // Removeold role and add new
                        await _userManger.RemoveFromRolesAsync(userFromDb, oldRoles);
                        await _userManger.AddToRoleAsync(userFromDb, model.Role);
                    }

                    TempData["success"] = "تم تحديث بيانات المستخدم بنجاح";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            model.RoleList = _roleManger.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            return View(model);
        }

        public IActionResult LockUnlock(string id)
        {
            var user = _unitOfWork.User.GetFirstOrDefault(id);
            if (user == null)
            {

                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddDays(30);
                TempData["message"] = "تم حظر المستخدم بنجاح"; 
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
                TempData["message"] = "تم إلغاء الحظر بنجاح";

            }

            _unitOfWork.Complete();


            return RedirectToAction("Index", "User");
        }


    }
}
