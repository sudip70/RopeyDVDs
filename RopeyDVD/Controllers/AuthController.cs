#nullable disable
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RopeyDVD.Data;
using RopeyDVD.Models;
using RopeyDVD.Models.Identity;
using RopeyDVD.Models.ViewModels;

namespace RopeyDVD.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserDetails()
        {
            return View();
        }

        // GET: Authentication/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.UserPassword))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                UserDetailsViewModel userDetails = new UserDetailsViewModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
                CookieOptions loginCookies = new CookieOptions();
                loginCookies.Expires = userDetails.Expiration;
                Response.Cookies.Append("Token", userDetails.Token);
                if (userRoles.Contains("Admin"))
                {
                    //Need some chnages
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("UserDetails", userDetails);
                }
            }
            return RedirectToAction("UnauthorizedAccess");
        }

        // GET: Authentication/RegisterUser
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser( UserRegister model)
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
            return RedirectToAction("Index", "Home");
        }

        // GET: Authentication/RegisterAdmin
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(UserRegister model)
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

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult UnauthorizedAccess()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}

//namespace RopeyDVD.Controllers
//{
//    public class AuthController : Controller
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IConfiguration _configuration;

//        public AuthController(
//            UserManager<IdentityUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _configuration = configuration;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        // GET: Authentication/Login
//        public IActionResult Login()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(UserLoginModel loginModel)
//        {
//            var user = await _userManager.FindByNameAsync(loginModel.Username);
//            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
//            {
//                var userRoles = await _userManager.GetRolesAsync(user);

//                var authClaims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, user.UserName),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//                foreach (var userRole in userRoles)
//                {
//                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//                }

//                var token = GetToken(authClaims);

//                UserDetailsViewModel userDetails = new UserDetailsViewModel()
//                {
//                    UserName = user.UserName,
//                    Email = user.Email,
//                    Token = new JwtSecurityTokenHandler().WriteToken((Microsoft.IdentityModel.Tokens.SecurityToken)token),
//                    Expiration = token.ValidTo
//                };
//                ViewBag.User = userDetails;
//                return RedirectToAction("UserDetails", ViewBag.User);
//            }
//            return RedirectToAction("UnauthorizedAccess");
//        }

////        private object GetToken(List<Claim> authClaims)
////        {
////            throw new NotImplementedException();
////        }

////        private readonly ApplicationDbContext _context;

////        public AuthController(ApplicationDbContext context)
////        {
////            _context = context;
////        }

////        // GET: Auth
////        public async Task<IActionResult> Index()
////        {
////            return View(await _context.Users.ToListAsync());
////        }

////        // GET: Auth/Details/5
////        public async Task<IActionResult> Details(int? id)
////        {
////            if (id == null)
////            {
////                return NotFound();
////            }

////            var user = await _context.Users
////                .FirstOrDefaultAsync(m => m.UserNumber == id);
////            if (user == null)
////            {
////                return NotFound();
////            }

////            return View(user);
////        }

////        // GET: Auth/Create
////        public IActionResult Create()
////        {
////            return View();


////        // POST: Auth/Create
////        // To protect from overposting attacks, enable the specific properties you want to bind to.
////        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> Create([Bind("UserNumber,UserName,UserType,UserPassword")] User user)
////        {
////            if (ModelState.IsValid)
////            {
////                _context.Add(user);
////                await _context.SaveChangesAsync();
////                return RedirectToAction(nameof(Index));
////            }
////            return View(user);
////        }

////        // GET: Auth/Edit/5
////        public async Task<IActionResult> Edit(int? id)
////        {
////            if (id == null)
////            {
////                return NotFound();
////            }

////            var user = await _context.Users.FindAsync(id);
////            if (user == null)
////            {
////                return NotFound();
////            }
////            return View(user);
////        }

////        // POST: Auth/Edit/5
////        // To protect from overposting attacks, enable the specific properties you want to bind to.
////        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> Edit(int id, [Bind("UserNumber,UserName,UserType,UserPassword")] User user)
////        {
////            if (id != user.UserNumber)
////            {
////                return NotFound();
////            }

////            if (ModelState.IsValid)
////            {
////                try
////                {
////                    _context.Update(user);
////                    await _context.SaveChangesAsync();
////                }
////                catch (DbUpdateConcurrencyException)
////                {
////                    if (!UserExists(user.UserNumber))
////                    {
////                        return NotFound();
////                    }
////                    else
////                    {
////                        throw;
////                    }
////                }
////                return RedirectToAction(nameof(Index));
////            }
////            return View(user);
////        }

////        // GET: Auth/Delete/5
////        public async Task<IActionResult> Delete(int? id)
////        {
////            if (id == null)
////            {
////                return NotFound();
////            }

////            var user = await _context.Users
////                .FirstOrDefaultAsync(m => m.UserNumber == id);
////            if (user == null)
////            {
////                return NotFound();
////            }

////            return View(user);
////        }

////        // POST: Auth/Delete/5
////        [HttpPost, ActionName("Delete")]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> DeleteConfirmed(int id)
////        {
////            var user = await _context.Users.FindAsync(id);
////            _context.Users.Remove(user);
////            await _context.SaveChangesAsync();
////            return RedirectToAction(nameof(Index));
////        }

////        private bool UserExists(int id)
////        {
////            return _context.Users.Any(e => e.UserNumber == id);
////        }
////    }
////}
