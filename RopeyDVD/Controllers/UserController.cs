
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RopeyDVD.Models;
using RopeyDVD.Models.Identity;
using RopeyDVD.Models.ViewModels;

namespace RopeyDVD.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize(Roles = "Manager, Assistant")]
        public IActionResult PasswordChange()
        { 
            return View();
        }
        //[HttpPost]
        //[Authorize(Roles = "Manager, Assistant")]
        //public async Task<IActionResult> ChangePassword(UpdatePassword model)
        //{
        //    var authUser = _configuration.GetUser();
        //    var user = await _userManager.FindByNameAsync(authUser);
        //    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        //    if (result.Succeeded)
        //    {
        //        TempData["SuccessAlert"] = "Password changed successfully";
        //       return RedirectToAction("Profile"); //without error
        //   }
        //   else
        //    {
        //        TempData["DangerAlert"] = "Password couldn\'t be changed";
        //        return RedirectToAction("Profile"); //with error
        //    }
        //}

      
   }
}
