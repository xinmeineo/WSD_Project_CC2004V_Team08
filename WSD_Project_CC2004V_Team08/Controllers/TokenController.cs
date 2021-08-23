using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WSD_Project_CC2004V_Team08.Models;

namespace WSD_Project_CC2004V_Team08.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        protected ApplicationDbContext DbContext { get; set; }
        protected RoleManager<IdentityRole> RoleManager { get; set; }
        protected UserManager<IdentityUser> UserManager { get; set; }
        protected IConfiguration Configuration { get; set; }

        public TokenController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            DbContext = context;
            RoleManager = roleManager;
            UserManager = userManager;
            Configuration = configuration;
        }

        private async Task<IActionResult> GetToken(TokenRequestViewModel model)
        {
            try
            {
                // check if there's an user with the given username
                var user = await UserManager.FindByNameAsync(model.username);
                if (user == null && model.username.Contains("@"))
                    user = await UserManager.FindByEmailAsync(model.username);
                // check for authorization
                if (user == null || !await UserManager.CheckPasswordAsync(user, model.password))
                    return new UnauthorizedResult();

                DateTime now = DateTime.UtcNow;
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
                };
                var roles = await UserManager.GetRolesAsync(user);
                claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));
                var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"]));
                var token = new JwtSecurityToken(issuer: Configuration["Auth:Jwt:Issuer"], audience: Configuration["Auth:Jwt:Audience"],
                    claims: claims, notBefore: now, expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)), signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256));
                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                var response = new TokenResponseViewModel()
                {
                    token = encodedToken,
                    expiration = tokenExpirationMins
                };
                return Json(response);
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> Jwt([FromBody] TokenRequestViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            switch (model.grant_type)
            {
                case "password":
                    return await GetToken(model);
                default:
                    return new UnauthorizedResult();
            }
        }
    }
}