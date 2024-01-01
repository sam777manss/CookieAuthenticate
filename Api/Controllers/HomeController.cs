using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration configuration) {
        _config = configuration;    
        }

        // GET: api/<HomeController>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            //A claim is a statement about a subject by an issuer and
            //represent attributes of the subject that are useful in the context of authentication and authorization operations.
            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(1)),
                    new Claim(ClaimTypes.Name,"Sameer"),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim("FavoriteDrink","Tea")
                    };
            //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
            var principal = new ClaimsPrincipal(identity);
            //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal, new AuthenticationProperties() { IsPersistent = true });

            return new string[] { "value1", "value2" };
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var token = GenerateToken();

            // set cookie
            //CookieOptions cookie = new CookieOptions();

            //var claims = GetClaimsFromToken(token);
            //var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            //Response.Cookies.Append("APIEmail", emailClaim, cookie);
            //string cookieValueFromReq = Request.Cookies["APIEmail"];

            //var user = HttpContext.User;
            //var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            return "value";
        }

        // POST api/<HomeController>
        /// <summary>
        /// [Authorize]
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [Authorize(Roles = "User2")]

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private List<Claim> GetClaimsFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.CanReadToken(token))
            {
                SecurityToken securityToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                }, out securityToken);

                return claimsPrincipal.Claims.ToList();
            }

            return new List<Claim>();
        }
        // To generate token
        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,"sam777manss@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
                //new Claim(ClaimTypes.Name,user.Email),
				//new Claim(ClaimTypes.Role,user.Role)
			};
            var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
