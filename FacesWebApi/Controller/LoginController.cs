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
using FacesStorage.Data.Abstractions.Exceptions;

namespace FacesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IStorage _storage;
        private readonly IHashService _hashService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IStorage storage, IHashService hashService, ILogger<LoginController> logger)
        {
            this._storage = storage;
            this._hashService = hashService;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation($"Get identity for {login.Email}.");
            var identity = await GetIdentity(login.Email, login.Password);
            if (identity == null)
            {
                _logger.LogInformation("Auth data invalid.");
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

            var responseModel = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            _logger.LogInformation("Return answer.");
            return Ok(responseModel);
        }

        [NonAction]
        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var userRepository = _storage.GetRepository<IUserRepository>();

            try
            {
                var user = await userRepository.GetAsync((optionsBuilder) =>
                {
                    optionsBuilder.SearchType = UserSearchTypes.ByEmail;
                    optionsBuilder.Email = email;
                    optionsBuilder.WithRole = true;
                    optionsBuilder.WithPassword = true;
                });

                if (user.Password == Encoding.UTF8.GetString(_hashService.GetHash(password)))
                {
                    _logger.LogInformation("Nice pass");
                    var claims = new List<Claim>
                    {
                        new Claim("Id", user.UserId.ToString()),
                        new Claim("Name", user.Nickname),
                        new Claim("Role", user.Role.Name),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    return claimsIdentity;
                }
            }
            catch(UserNotFoundException ex) 
            { 
                _logger.LogInformation($"Error: {ex.Message}"); 
            }

            return null;
        }
    }
}
