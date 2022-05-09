
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RopeyDVD.Models;
using RopeyDVD.Models.Identity;
using RopeyDVD.Models.ViewModels;
using RopeyDVD.Service;

namespace RopeyDVD.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        public UserController(IUserService userService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _userService = userService;
            }

            //[Authorize(Roles = "Manager, Assistant")]
            public IActionResult Profile()
            {
                return View();
            }

            //[Authorize(Roles = "Manager, Assistant")]
            public IActionResult ChangePassword()
            { 
                return View();
            }
            [HttpPost]
            //[Authorize(Roles = "Manager, Assistant")]
            public async Task<IActionResult> ChangePassword(UpdatePassword model)
            {
                var authUser = _userService.GetUser();
                var user = await _userManager.FindByNameAsync(authUser);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    TempData["SuccessAlert"] = "Password has been changed successfully.";
                        return RedirectToAction("Profile");
                }
                else
                {
                    TempData["DangerAlert"] = "Password couldn't be altered.";
                    return RedirectToAction("Profile");
                }
            }
            //[Authorize(Roles ="Manager")]
            public IActionResult ViewUsers()
            {
                var users = _userManager.Users.Select(c => new UserDetailsViewModel()
                {
                    UserName = c.UserName,
                    Email = c.Email,
                }).ToList();
                return View(users);
            }
            //[Authorize(Roles ="Manager")]
            public async Task<IActionResult> EditUser(string id)
            {
                UpdateUserDetails updateUserDetails = new UpdateUserDetails();
                if (id == null)
                {
                    return NotFound();
                }
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }
                updateUserDetails.User = user;
                ViewData["UserName"] = user.UserName;
                ViewData["Email"] = user.Email;

                return View(updateUserDetails);
            }

            [HttpPost]
            //[Authorize(Roles = "Manager")]
            public async Task<IActionResult> UpdateUser(string id, UpdateUserDetails detailModel)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    var result = await _userManager.ChangePasswordAsync(user, detailModel.CurrentPassword, detailModel.NewPassword);
                    user.UserName = detailModel.UserName;
                    user.Email = detailModel.Email;


                    if (result.Succeeded)
                    {
                        var result2 = _userManager.UpdateAsync(user);
                        if (result2.IsCompleted)
                        {
                            TempData["SuccessAlert"] = "User Details has been updated successfully.";
                            return RedirectToAction("ViewUsers");
                        }
                        else
                        {
                            TempData["SuccessAlert"] = "User Details update unsuccessful.";
                            return RedirectToAction("ViewUsers"); 
                        }

                    }
                    else
                    {
                        TempData["DangerAlert"] = "Password update unsuccessful.";
                        return RedirectToAction("ViewUsers");
                    }

                }

            }
            //[Authorize(Roles = "Manager")]
            public async Task<IActionResult> DeleteUser(string? id)
            {
                UpdateUserDetails updateUserDetails = new UpdateUserDetails();
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }
                updateUserDetails.User = user;

                return View(updateUserDetails);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            //[Authorize(Roles = "Manager")]
            public async Task<IActionResult> DeleteConfirmed(string? id)
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = _userManager.DeleteAsync(user);
                TempData["SuccessAlert"] = "User was Deleted Successfully";
                return RedirectToAction("ViewUsers"); //show message
            }

            //[Authorize(Roles = "Manager")]
            public IActionResult CreateUser()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            //[Authorize(Roles = "Manager")]
            public async Task<IActionResult> StoreUser(UserRegister model)
            {
                var userExists = await _userManager.FindByNameAsync(model.UserName);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.UserPassword);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                if (!await _roleManager.RoleExistsAsync(UserRoles.Assistant))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Assistant));


                if (await _roleManager.RoleExistsAsync(UserRoles.Assistant))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Assistant);
                }
                return RedirectToAction("Index", "Home");
        }

    }
}