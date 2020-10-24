using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using FacesWebApi.ApiModels;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using FacesStorage.Data.Abstractions.Exceptions;

namespace FacesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestController : ControllerBase
    {
        private readonly IStorage storage;
        private readonly IFileService fileService;
        private readonly IFaceService faceService;
        private readonly ILogger<RequestController> logger;

        public RequestController(IStorage storage, IFileService fileService, IFaceService faceService,ILogger<RequestController> logger)
        {
            this.storage = storage;
            this.fileService = fileService;
            this.faceService = faceService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            logger.LogInformation("Get action.");

            var requestRepository = storage.GetRepository<IRequestRepository>();
            var responseRepository = storage.GetRepository<IResponseRepository>();

            Request request;
            Response response;
            try
            {
                request = await requestRepository.GetAsync((options) =>
                {
                    options.RequestId = id;
                    options.WithImages = true;
                    options.WithUser = true;
                });
                response = await responseRepository.GetAsync((options) =>
                {
                    options.ResponseId = request.ResponseId.Value;
                    options.WithImages = true;
                });
            }
            catch(RequestNotFoundException ex)
            {
                ModelState.AddModelError("Request", ex.Message);
                return BadRequest(ModelState);
            }
            catch (ResponseNotFoundException ex)
            {
                ModelState.AddModelError("Response", ex.Message);
                return BadRequest(ModelState);
            }

            var responseModel = new
            {
                Request = new
                {
                    RequestId = request.RequestId,
                    RequestType = request.Discriminator,
                    Images = request.Images.Select(i => new { ImageSrc = Path.Combine(fileService.LocalRequestImagesPath, i.ImageName) })
                },
                Response = new
                {
                    Images = response.Images.Select(i => new { ImageSrc = Path.Combine(fileService.LocalResponseImagesPath, i.ImageName) })
                },
                User = request.UserId.HasValue ? new
                {
                    UserId = request.UserId,
                    Username = request.User.Nickname,
                } : null,
            };

            return Ok(responseModel);
        }
    
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] RequestModel requestModel)
        {
            logger.LogInformation($"Post action. Nick: {User.Identity.IsAuthenticated} !");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Request request;
            try
            {
                logger.LogInformation("Create request.");
                int? userId = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst("Id").Value) : new int?();
                request = await faceService.CreateRequestAsync(requestModel.RequestType, requestModel.FromImage, requestModel.ToImage, userId);

                logger.LogInformation("Create response.");
                Response response = await faceService.CreateResponseAsync(request);
            }
            catch(KeyNotFoundException ex)
            {
                ModelState.AddModelError("RequestType", ex.Message);
                return BadRequest(ModelState);
            }
            catch(NullReferenceException ex)
            {
                ModelState.AddModelError("ToImage", ex.Message);
                return BadRequest(ModelState);
            }
            catch (InvalidDataException ex)
            {
                ModelState.AddModelError("Images", ex.Message);
                return BadRequest(ModelState);
            }

            logger.LogInformation("Return response.");
            var responseModel = new { Id = request.RequestId };
            return Ok(responseModel);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var requestRepository = storage.GetRepository<IRequestRepository>();
            var responseRepository = storage.GetRepository<IResponseRepository>();
        }
    }
}
