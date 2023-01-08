using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoProject.Models.DTO;
using ToDoProject.Models.DTO.Auth;
using ToDoProject.Server.Business;

namespace ToDoProject.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;
        private DatabaseContext _ctx;

        public AuthController(IConfiguration conf, DatabaseContext ctx)
        {
            _configuration = conf;
            _ctx = ctx;
        }
        [HttpPost, Route("Register")]
        public ActionResult<RegistrationResponse> Register([FromBody] RegistrationRequest body)
        {
            // Non faccio controlli sulla validazione perché
            // li farò nella parte di FE
            var manager = new AuthManager(_ctx);
            var result = manager.Register(body);
            if (result.IsSuccesfulRegistration)
            {
                //do some stuff to take token
                result.Token = this.CreateToken(result.User);
            }
            return Ok(result);
        }


        private string CreateToken(UserDTO user)
        {
            DateTime issuedAt = DateTime.Now;
            DateTime expires = DateTime.Now.AddMinutes(60);

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity cIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Actor, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            });
            //recupero da Web.Config issuer e secret
            var issuer = _configuration["JWT:Issuer"];
            var secret = _configuration["JWT:Secret"];

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
            var singingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                subject: cIdentity,
                notBefore: issuedAt,
                expires: expires,
                signingCredentials: singingCredentials
                );
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
