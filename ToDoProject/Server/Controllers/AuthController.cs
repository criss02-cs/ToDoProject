using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoProject.EmailService;
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
        //private readonly IEmailSender _emailSender;

        public AuthController(IConfiguration conf, DatabaseContext ctx)
        {
            _configuration = conf;
            _ctx = ctx;
            //_emailSender = emailSender;
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
                //this.SendConfirmationEmail(result.User.Id, result.User?.Email);
            }
            return Ok(result);
        }


        [HttpPost, Route("Login")]
        public ActionResult<RegistrationResponse> Login([FromBody] LoginRequest request)
        {
            var manager = new AuthManager(_ctx);
            var result = manager.Login(request);
            if (result.IsSuccesfulRegistration)
            {
                //do some stuff to take token
                result.Token = this.CreateToken(result.User);
                this.SendConfirmationEmail(result.User.Email, result.User.Id);
            }
            return Ok(result);
        }
        [HttpPost, Route("ConfirmEmail")]
        public ActionResult<ConfirmEmail> ConfirmEmail([FromBody] ConfirmEmail request)
        {
            // Verifico la validità del token, non controllo se sia scaduto dato che
            // sono sicuro che l'abbia già controllato la parte di front end
            var isValid = this.ValidateToken(request.Token);
            // Se il token è valido vado a confermare l'email
            if (isValid)
            {
                var manager = new UserManager(_ctx);
                var result = manager.ConfirmEmail(request.Email);
                request.IsEmailConfirmed = result;
                request.IsTokenValid = isValid;
                return Ok(request);
            }
            var error = new ConfirmEmail { Email = request.Email, Token = request.Token, IsEmailConfirmed = false, IsTokenValid = false };
            return Ok(error);
        }

        [HttpPost, Route("SendConfirmationEmail")]
        public async Task<ActionResult<bool>> SendConfirmationEmailAsync([FromBody] ConfirmationEmailRequest request)
        {
            IEmailSender emailSender;
            var token = this.GenerateEmailConfirmationToken(request.Email, request.UserId);
            var confirmationLink = Url.Action("ConfirmEmail", "auth", new { token, email = request.Email }, Request.Scheme);
            var apiKey = _configuration["API:SendGridApiKey"];
            // Codice per inviare l'email
            emailSender = new EmailSender(apiKey);
            await emailSender.SendEmailAsync(request.Email, "Conferma Email", confirmationLink);
            return Ok(true);
        }

        private async void SendConfirmationEmail(string email, Guid userId)
        {
            IEmailSender emailSender;
            var token = this.GenerateEmailConfirmationToken(email, userId);
            var confirmationLink = Url.Action("ConfirmEmail", "auth", new { token, email }, Request.Scheme);
            var apiKey = _configuration["API:SendGridApiKey"];
            // Codice per inviare l'email
            emailSender = new EmailSender(apiKey);
            await emailSender.SendEmailAsync(email, "Conferma Email", confirmationLink);
        }

        private bool ValidateToken(string token)
        {
            var secret = _configuration["JWT:Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string CreateToken(UserDTO? user)
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

        private string GenerateEmailConfirmationToken(string email, Guid userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, userId.ToString())
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            //recupero da Web.Config issuer e secret
            var issuer = _configuration["JWT:Issuer"];
            var secret = _configuration["JWT:Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return tokenHandler.WriteToken(token);
        }
    }
}
