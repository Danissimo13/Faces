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
<<<<<<< HEAD
        private readonly IStorage _storage;
        private readonly IHashService _hashService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IStorage storage, IHashService hashService, ILogger<LoginController> logger)
        {
            this._storage = storage;
            this._hashService = hashService;
            this._logger = logger;
        }

=======
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
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

<<<<<<< HEAD
            _logger.LogInformation($"Get identity for {login.Email}.");
            var identity = await GetIdentity(login.Email, login.Password);
            if (identity == null)
            {
                _logger.LogInformation("Auth data invalid.");
=======
            logger.LogInformation($"Get identity for {login.Email}.");
            var identity = await GetIdentity(login.Email, login.Password);
            if (identity == null)
            {
                logger.LogInformation("Auth data invalid.");
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
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

<<<<<<< HEAD
            _logger.LogInformation("Return answer.");
=======
            logger.LogInformation("Return answer.");
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
            return Ok(responseModel);
        }

        [NonAction]
        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
<<<<<<< HEAD
            var userRepository = _storage.GetRepository<IUserRepository>();
=======
            var userRepository = storage.GetRepository<IUserRepository>();
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4

            try
            {
                var user = await userRepository.GetAsync((optionsBuilder) =>
                {
                    optionsBuilder.SearchType = UserSearchTypes.ByEmail;
                    optionsBuilder.Email = email;
                    optionsBuilder.WithRole = true;
                    optionsBuilder.WithPassword = true;
                });

<<<<<<< HEAD
                if (user.Password == Encoding.UTF8.GetString(_hashService.GetHash(password)))
                {
                    _logger.LogInformation("Nice pass");
=======
                if (user.Password == Encoding.UTF8.GetString(hashService.GetHash(password)))
                {
                    logger.LogInformation("Nice pass");
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
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
<<<<<<< HEAD
                _logger.LogInformation($"Error: {ex.Message}"); 
=======
                logger.LogInformation($"Error: {ex.Message}"); 
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
            }

            return null;
        }
    }
}
