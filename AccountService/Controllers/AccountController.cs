using AccountService.Dtos;
using AccountService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AccountService.Controllers
{
    
   
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            _roleManager = roleManager;
            this.config = config;
        }
        [HttpGet("current")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await userManager.FindByEmailAsync(email);
            return new UserDto
            {
                Email = user.Email,

                DisplayName = user.DisplayName
            };

        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginDto { ReturnUrl = returnUrl });
        }
       // [HttpGet]
        
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            var user1 = await userManager.FindByEmailAsync(loginDto.Email);
            var userRole = await userManager.GetRolesAsync(user1);
            if(result.Succeeded)
            return Redirect(loginDto.ReturnUrl);
            else
            {
                return View();
            }
            //if (userRole.Count == 0) await userManager.AddToRoleAsync(user1, "user");

            //if (!result.Succeeded)
            //{
            //    return Unauthorized(StatusCode(401));
            //}
            //var authClaims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name,user1.DisplayName),
            //    new Claim(ClaimTypes.Email,loginDto.Email),
            //    new Claim(ClaimTypes.MobilePhone,user1.PhoneNumber),
            //      new Claim(ClaimTypes.Role, userRole[0]),
            //    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            //};
            //var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["JWT:Secret"]));
            //var token = new JwtSecurityToken(issuer: config["JWT:ValidIssuer"]
            //    , audience: config["JWT:ValidAudience"],
            //    expires: DateTime.Now.AddDays(7),
            //    claims: authClaims,
            //    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

            //var Sectoken = new JwtSecurityTokenHandler().WriteToken(token);
            //if (string.IsNullOrEmpty(Sectoken))
            //{
            //    return Unauthorized();
            //}

            //var user = new UserDto
            //{
            //    Email = loginDto.Email,
            //    Token = Sectoken,
            //    DisplayName = user1.DisplayName,
            //    Role = userRole[0]

            //};
            //return Ok(user);
        }


        [HttpGet("currentuser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> getUser()
        {

            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await userManager.FindByEmailAsync(email);
            var userRole = await userManager.GetRolesAsync(user);
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Phone = user.PhoneNumber,
                Role = userRole[0]
            };
        }
        [HttpPost("isSigned")]
        public async Task<bool> IsSignedIn(LoginDto login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);
            var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
            return signInManager.IsSignedIn(claimsPrincipal);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var User = new AppUser
            {
                DisplayName = registerDto.DisplayName,

                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.Phone
            };
            var result = await userManager.CreateAsync(User, registerDto.Password);
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                var role = new IdentityRole();
                role.Name = "admin";
                await _roleManager.CreateAsync(role);
            }
            if (!await _roleManager.RoleExistsAsync("user"))
            {
                var role = new IdentityRole();
                role.Name = "user";
                await _roleManager.CreateAsync(role);
            }
            if (!result.Succeeded) return BadRequest(StatusCode(400));
            var adminUser = await userManager.FindByEmailAsync("admin@ecommerce.com");

            if (adminUser != null)
            {
                await userManager.AddToRoleAsync(adminUser, "admin");
            }


            if (User.Email != "admin@ecommerce.com")
            {
                var user1 = await userManager.FindByEmailAsync(User.Email);
                await userManager.AddToRoleAsync(user1, "user");
            }
            return new UserDto
            {
                DisplayName = User.DisplayName,

                Email = User.Email,
                Phone = User.PhoneNumber
            };
        }

        [HttpGet("isAdmin")]
        [Authorize]
        public async Task<ActionResult<bool>> IsAdmin(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var role = await userManager.IsInRoleAsync(user, "admin");
                return role;
            }
            return false;
        }
        [HttpGet("ExistEmail")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            return await userManager.FindByEmailAsync(Email) != null;
        }
    }
}
