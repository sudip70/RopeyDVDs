//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using RopeyDVD.Models;
//using RopeyDVD.Models.Identity;
//using RopeyDVD.Models.ViewModels;

//namespace RopeyDVD.Controllers
//{
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    public class UserController : Controller
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IConfiguration _configuration;

//        public UserController(
//            UserManager<IdentityUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _configuration = configuration;
//        }

//        [Authorize(Roles = "Manager, Assistant")]
//        public IActionResult Profile()
//        {
//            return View();
//        }

//        [Authorize(Roles = "Manager, Assistant")]
//        public IActionResult PasswordChange()
//        {
//            return View();
//        }
//        [HttpPost]
//        [Authorize(Roles = "Manager, Assistant")]
//        public async Task<IActionResult> ChangePassword(UpdatePassword model)
//        {
//            var authUser = _configuration.GetUser();
//            var user = await _userManager.FindByNameAsync(authUser);
//            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

//            if (result.Succeeded)
//            {
//                TempData["SuccessAlert"] = "Password changed successfully";
//                return RedirectToAction("Profile"); //without error
//            }
//            else
//            {
//                TempData["DangerAlert"] = "Password couldn\'t be changed";
//                return RedirectToAction("Profile"); //with error
//            }
//        }

//        [Authorize(Roles = "Manager")]
//        public IActionResult ViewUsers()
//        {
//            var users = _userManager.Users.Select(c => new UserDetailsViewModel()
//            {
//                Id = c.Id,
//                UserName = c.UserName,
//                Email = c.Email,
//            }).ToList();
//            return View(users);
//        }

//        [Authorize(Roles = "Manager")]
//        public async Task<IActionResult> EditUser(string id)
//        {
//            UpdateUserDetails updateUserDetails = new UpdateUserDetails();
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var user = await _userManager.FindByIdAsync(id);

//            if (user == null)
//            {
//                return NotFound();
//            }
//            updateUserDetails.User = user;
//            ViewData["Username"] = user.UserName;
//            ViewData["Email"] = user.Email;

//            return View(updateUserDetails);
//        }



//        [HttpPost]
//        [Authorize(Roles = "Manager")]
//        public async Task<IActionResult> UpdateUser(string id, UpdateUserDetails detailModel)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }
//            var user = await _userManager.FindByIdAsync(id);
//            if (user == null)
//            {
//                return NotFound();
//            }
//            else
//            {
//                var result = await _userManager.ChangePasswordAsync(user, detailModel.CurrentPassword, detailModel.NewPassword);
//                user.UserName = detailModel.UserName;
//                user.Email = detailModel.Email;


//                if (result.Succeeded)
//                {
//                    var result2 = _userManager.UpdateAsync(user);
//                    if (result2.IsCompleted)
//                    {
//                        TempData["SuccessAlert"] = "User Details was updated successfully.";

//                        return RedirectToAction("ViewUsers");  //without error
//                    }
//                    else
//                    {
//                        TempData["SuccessAlert"] = "User Details wasn\'t updated.";
//                        return RedirectToAction("ViewUsers");  //without error
//                    }

//                }
//                else
//                {
//                    TempData["DangerAlert"] = "Password couldn\'t be updated.";
//                    return RedirectToAction("ViewUsers");//with error
//                }

//            }

//        }

//        [Authorize(Roles = "Manager")]
//        public async Task<IActionResult> DeleteUser(string? id)
//        {
//            UpdateUserDetails updateUserDetails = new UpdateUserDetails();
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var user = await _userManager.FindByIdAsync(id);

//            if (user == null)
//            {
//                return NotFound();
//            }
//            updateUserDetails.User = user;

//            return View(updateUserDetails);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Manager")]
//        public async Task<IActionResult> DeleteConfirmed(string? id)
//        {
//            var user = await _userManager.FindByIdAsync(id);
//            var result = _userManager.DeleteAsync(user);
//            TempData["SuccessAlert"] = "User was Deleted Successfully";
//            return RedirectToAction("ViewUsers"); //show message
//        }

//        [Authorize(Roles = "Manager")]
//        public IActionResult CreateUser()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Manager")]
//        public async Task<IActionResult> StoreUser(UserRegister model)
//        {
//            var userExists = await _userManager.FindByNameAsync(model.UserName);
//            if (userExists != null)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

//            IdentityUser user = new()
//            {
//                Email = model.Email,
//                SecurityStamp = Guid.NewGuid().ToString(),
//                UserName = model.UserName
//            };
//            var result = await _userManager.CreateAsync(user, model.UserPassword);
//            if (!result.Succeeded)
//                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
//            if (!await _roleManager.RoleExistsAsync(UserRoles.Assistant))
//                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Assistant));


//            if (await _roleManager.RoleExistsAsync(UserRoles.Assistant))
//            {
//                await _userManager.AddToRoleAsync(user, UserRoles.Assistant);
//            }
//            return RedirectToAction("Index", "Home");
//        }
//    }
//}
