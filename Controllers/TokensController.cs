using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using back_end_api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace back_end_api
{

    [Route("/api/token")]
    public class TokenController : Controller
    {
        private back_end_apiContext _context;
        private readonly SignInManager<User> _signInManager;


        public TokenController(back_end_apiContext ctx, SignInManager<User> signInManager)
        {
            _context = ctx;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(){
            return new ObjectResult(new {
                Username = User.Identity.Name
            });                
        }
        
        [HttpPut]
        [Authorize]
        public IActionResult Put(){
            
            return new ObjectResult(GenerateToken(User.Identity.Name));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string firstname, string lastname)
        {
            // // Check simplistic username and password validation rules
            
            bool isValid = IsValidUserAndPasswordCombination(username, password);

            if (isValid)
            {
                // Does the user already exist?
                User user = _context.User.SingleOrDefault(u => u.Email == username);

                if (user != null) 
                {
                    // Found the user, verify credentials
                    var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);

                    // Password is correct, generate token and return it
                    if (result.Succeeded)
                    {
                        return new ObjectResult(GenerateToken(user.UserName));
                    }
                } else
                {
                    var userstore = new UserStore<User>(_context);

                    // User does not exist, create one
                    user = new User {
                        FirstName = firstname,
                        LastName = lastname,
                        UserName = username,
                        NormalizedUserName = username.ToUpper(),
                        Email = username,
                        NormalizedEmail = username.ToUpper(),
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };
                    var passwordHash = new PasswordHasher<User>();
                    user.PasswordHash = passwordHash.HashPassword(user, password);
                    await userstore.CreateAsync(user);
                    // await userstore.AddToRoleAsync(user);
                    _context.SaveChanges();
                    return new ObjectResult(GenerateToken(user.UserName));
                }
            }
            return BadRequest();
        }

        private bool IsValidUserAndPasswordCombination(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && username != password;
        }

        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                // new Claim(ClaimTypes.Role),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("7A735D7B-1A19-4D8A-9CFA-99F55483013F")), 
                        SecurityAlgorithms.HmacSha256)
                    ),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
