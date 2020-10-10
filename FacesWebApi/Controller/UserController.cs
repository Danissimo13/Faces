using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using FacesWebApi.ApiModels;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using FacesStorage.Data.Abstractions.SearchOptions;
using System.Collections;

namespace FacesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IStorage storage;
        private readonly IHashService hashService;
        private readonly IFileService fileService;
        private readonly ILogger<UserController> logger;

        public UserController(IStorage storage, IHashService hashService, IFileService fileService, ILogger<UserController> logger)
        {
            this.storage = storage;
            this.hashService = hashService;
            this.fileService = fileService;
            this.logger = logger;
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id,[FromQuery] SearchUserModel searchOptions)
        {
            logger.LogInformation("Get action.");
            var userRepository = storage.GetRepository<IUserRepository>();

            try
            {
                logger.LogInformation("Get user");
                User user = await userRepository.GetAsync((optionsBuilder) =>
                {
                    optionsBuilder.SearchType = UserSearchTypes.ById;
                    optionsBuilder.UserId = id.HasValue ? id.Value : int.Parse(User.FindFirst("Id").Value);
                    optionsBuilder.WithRole = searchOptions.WithRole;
                    optionsBuilder.WithRequests = searchOptions.WithRequests;
                    optionsBuilder.WithRequestResponses = searchOptions.WithRequestResponses;
                    optionsBuilder.WithRequestImages = searchOptions.WithRequestImages;
                    optionsBuilder.WithResponseImages = searchOptions.WithResponseImages;
                    optionsBuilder.FromRequest = searchOptions.FromRequest;
                    optionsBuilder.RequestsCount = searchOptions.RequestsCount;
                    optionsBuilder.WithPassword = false;
                });

                var userModel = new
                {
                    UserId = user.UserId,
                    Nickname = user.Nickname,
                    Email = user.Email,
                    Requests = new ArrayList(),
                    Role = user.Role?.Name
                };

                foreach(Request request in user.Requests)
                {
                    var requestToModel = new
                    {
                        RequestId = request.RequestId,
                        RequestType = request.Discriminator,
                        Images = request.Images?.Select(i => new
                        {
                            ImageSrc = Path.Combine(fileService.LocalRequestImagesPath, i.ImageName)
                        }),
                        Response = new
                        {
                            ResponseId = request.Response?.ResponseId,
                            ResponseType = request.Response?.Discriminator,
                            Images = request.Response?.Images?.Select(i => new
                            {
                                ImageSrc = Path.Combine(fileService.LocalResponseImagesPath, i.ImageName)
                            })
                        }
                    };

                    userModel.Requests.Add(requestToModel);
                }

                logger.LogInformation("Return reposnse.");
                return Ok(userModel);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogInformation($"Error: {ex.Message}");
                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] RegistrationModel registrationModel)
        {
            logger.LogInformation("Post action.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRepository = storage.GetRepository<IUserRepository>();
            var roleRepository = storage.GetRepository<IRoleRepository>();

            try
            {
                logger.LogInformation("Create user and add to db.");
                User user = new User()
                {
                    Nickname = registrationModel.Nickname,
                    Email = registrationModel.Email,
                    Password = Encoding.UTF8.GetString(hashService.GetHash(registrationModel.Password)),
                    Role = await roleRepository.GetByNameAsync(roleRepository.DefaultUserRole)
                };

                await userRepository.CreateAsync(user);
                await storage.SaveAsync();
            }
            catch (DuplicateNameException ex)
            {
                logger.LogInformation($"Error: {ex.Message}");
                ModelState.AddModelError("Email", ex.Message);
                return BadRequest(ModelState);
            }

            logger.LogInformation("Return answer.");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ChangeUserModel changesModel)
        {
            logger.LogInformation("Put action.");
            if (id.ToString() != User.FindFirst("Id")?.Value)
            {
                ModelState.AddModelError("Id", "You do not have the premissions to change this account.");
                return BadRequest(ModelState);
            }

            logger.LogInformation("Check model for valid.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRepository = storage.GetRepository<IUserRepository>();

            User user = null;
            try
            {
                logger.LogInformation("Get a user.");
                user = await userRepository.GetAsync(options =>
                {
                    options.SearchType = UserSearchTypes.ById;
                    options.UserId = id;
                    options.WithPassword = true;
                });
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }

            if (user.Email != changesModel.Email)
            {
                try
                {
                    User userWithSameEmail = await userRepository.GetAsync(options =>
                    {
                        options.SearchType = UserSearchTypes.ByEmail;
                        options.Email = changesModel.Email;
                    });

                    ModelState.AddModelError("Email", "User with same email actually exist.");
                    return BadRequest(ModelState);
                }
                catch { }
            }

            user.Nickname = changesModel.Nickname;
            user.Email = changesModel.Email;

            bool willLogout = false;
            if ((changesModel.Password != null) && (changesModel.Password.Length > 8))
            {
                user.Password = Encoding.UTF8.GetString(hashService.GetHash(changesModel.Password));
                willLogout = true;
            }

            logger.LogInformation("Edit user.");
            userRepository.Edit(user);
            await storage.SaveAsync();

            var userModel = new
            {
                Nickname = changesModel.Nickname,
                Email = changesModel.Email,
                Logout = willLogout
            };

            logger.LogInformation("Return answer.");
            return Ok(userModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id.ToString() != User.FindFirst("Id")?.Value)
            {
                ModelState.AddModelError("Id", "You do not have the premissions to delete this account.");
                return BadRequest(ModelState);
            }

            var userRepository = storage.GetRepository<IUserRepository>();

            try
            {
                User user = await userRepository.GetAsync(options =>
                {
                    options.SearchType = UserSearchTypes.ById;
                    options.UserId = id;
                });

                userRepository.Delete(user);
                await storage.SaveAsync();
            }
            catch(KeyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
