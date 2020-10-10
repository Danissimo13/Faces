using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FacesStorage.Data.Abstractions;
using FacesWebApi.ApiModels;
using FacesWebApi.Options;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using FacesStorage.Data.Abstractions.SearchOptions;

namespace FacesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IStorage storage;
        private readonly IHashService hashService;
        private readonly ILogger<LoginController> logger;

        public LoginController(IStorage storage, IHashService hashService, ILogger<LoginController> logger)
        {
            this.storage = storage;
            this.hashService = hashService;
            this.logger = logger;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            logger.LogInformation($"Get identity for {login.Email}.");
            var identity = await GetIdentity(login.Email, login.Password);
            if (identity == null)
            {
                logger.LogInformation("Auth data invalid.");
                ModelState.AddModelError("Password", "Invalid email or password.");
                return BadRequest(ModelState);
            }

            var timeNow = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: timeNow,
                claims: identity.Claims,
                expires: timeNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurity(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            logger.LogInformation("Return answer.");
            return Ok(response);
        }

        [NonAction]
        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var userRepository = storage.GetRepository<IUserRepository>();

            try
            {
                var user = await userRepository.GetAsync((optionsBuilder) =>
                {
                    optionsBuilder.SearchType = UserSearchTypes.ByEmail;
                    optionsBuilder.Email = email;
                    optionsBuilder.WithRole = true;
                    optionsBuilder.WithPassword = true;
                });

                if (user.Password == Encoding.UTF8.GetString(hashService.GetHash(password)))
                {
                    logger.LogInformation("Nice pass");
                    var claims = new List<Claim>
                    {
                        new Claim("Id", user.UserId.ToString()),
                        new Claim("Name", user.Nickname),
                        new Claim("Role", user.Role.Name)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    return claimsIdentity;
                }
            }
            catch(KeyNotFoundException ex) 
            { 
                logger.LogInformation($"Error: {ex.Message}"); 
            }

            return null;
        }
    }
}
