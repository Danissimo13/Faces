using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceDetection.Core;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using FacesStorage.Data.MSSql;
using FacesWebApi.ApiModels;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FacesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RequestController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            logger.LogInformation("Get action.");

            var requestRepository = storage.GetRepository<IRequestRepository>();
            var responseRepository = storage.GetRepository<IResponseRepository>();

            Request request;
            Response response;
            try
            {
                request = await requestRepository.GetByIdAsync(id);
                response = await responseRepository.GetByIdAsync(id);
            }
            catch(KeyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
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
        public async Task<IActionResult> Post([FromForm] RequestModel requestModel)
        {
            foreach(var head in Request.Headers)
            {
                Console.WriteLine(head.Key + " - " + head.Value);
            }

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

        // PUT api/<RequestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RequestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
