﻿using System;
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
using FacesStorage.Data.Abstractions.Exceptions;

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
                User user = await userRepository.GetAsync((options) =>
                {
                    options.SearchType = UserSearchTypes.ById;
                    options.UserId = id.HasValue ? id.Value : int.Parse(User.FindFirst("Id").Value);
                    options.WithRole = searchOptions.WithRole;
                    options.WithRequests = searchOptions.WithRequests;
                    options.WithRequestResponses = searchOptions.WithRequestResponses;
                    options.WithRequestImages = searchOptions.WithRequestImages;
                    options.WithResponseImages = searchOptions.WithResponseImages;
                    options.FromRequest = searchOptions.FromRequest;
                    options.RequestsCount = searchOptions.RequestsCount;
                    options.WithPassword = false;
                });

                var responseModel = new
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

                    responseModel.Requests.Add(requestToModel);
                }

                logger.LogInformation("Return reposnse.");
                return Ok(responseModel);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogInformation($"Error: {ex.Message}");
                ModelState.AddModelError("Id", ex.Message);
                return NotFound(ModelState);
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

            User user;
            try
            {
                logger.LogInformation("Create user and add to db.");
                user = new User()
                {
                    Nickname = registrationModel.Nickname,
                    Email = registrationModel.Email,
                    Password = Encoding.UTF8.GetString(hashService.GetHash(registrationModel.Password)),
                    Role = await roleRepository.GetAsync(roleRepository.DefaultUserRole)
                };

                await userRepository.CreateAsync(user);
                await storage.SaveAsync();
            }
            catch (UserAlreadyExistException ex)
            {
                logger.LogInformation($"Error: {ex.Message}");
                ModelState.AddModelError("Email", ex.Message);
                return Conflict(ModelState);
            }

            string userUrl = $"acc?id={user.UserId}";

            logger.LogInformation("Return answer.");
            return Created(userUrl, user.UserId);
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
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                return NotFound(ModelState);
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
                    return Conflict(ModelState);
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

            var responseModel = new
            {
                Nickname = changesModel.Nickname,
                Email = changesModel.Email,
                Logout = willLogout
            };

            logger.LogInformation("Return answer.");
            return Accepted(responseModel);
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
            catch(UserNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
