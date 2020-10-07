using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using FacesStorage.Data.MSSql;
using FacesWebApi.ApiModels;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FacesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class NewsController : ControllerBase
    {
        private readonly IStorage storage;
        private readonly IFileService fileService;
        private readonly ILogger<NewsController> logger;

        public NewsController(IStorage storage, IFileService fileService, ILogger<NewsController> logger)
        {
            this.storage = storage;
            this.fileService = fileService;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var newsRepository = storage.GetRepository<NewsRepository>();

            News news;
            try
            {
                logger.LogInformation("Get news.");
                news = await newsRepository.GetLastAsync();
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogInformation($"Error: {ex.Message}");
                ModelState.AddModelError("News", ex.Message);
                return BadRequest(ModelState);
            }

            var newsModel = new
            {
                Id = news.NewsId,
                Topic = news.Topic,
                Body = news.Body,
                PublishDate = news.PublishDate.ToShortDateString(),
                ImageSrc = Path.Combine(fileService.LocalNewsImagesPath, news.ImageName)
            };

            return Ok(newsModel);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var newsRepository = storage.GetRepository<NewsRepository>();

            News news;
            try
            {
                logger.LogInformation("Get news.");
                news = await newsRepository.GetAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogInformation($"Error: {ex.Message}");
                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }

            var newsModel = new
            {
                Id = news.NewsId,
                Topic = news.Topic,
                Body = news.Body,
                PublishDate = news.PublishDate.ToShortDateString(),
                ImageSrc = Path.Combine(fileService.LocalNewsImagesPath, news.ImageName)
            };

            return Ok(newsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewsModel newsModel)
        {
            logger.LogInformation("Post action.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newsRepository = storage.GetRepository<NewsRepository>();

            logger.LogInformation("Create news and add to db.");
            News news = new News()
            {
                Topic = newsModel.Topic,
                Body = newsModel.Body,
                PublishDate = DateTime.Now.Date
            };
            await newsRepository.CreateAsync(news);

            string imageName = Path.Combine(news.NewsId.ToString(), Path.GetExtension(newsModel.Image.FileName));
            await fileService.SaveFileAsync(newsModel.Image.OpenReadStream(), Path.Combine(fileService.GlobalNewsImagesPath, imageName));
            news.ImageName = imageName;

            newsRepository.Edit(news);
            await storage.SaveAsync();

            logger.LogInformation("Return answer.");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewsModel newsModel)
        {
            logger.LogInformation("Put action.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newsRepository = storage.GetRepository<INewsRepository>();

            News news;
            try
            {
                logger.LogInformation("Get a news.");
                news = await newsRepository.GetAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }

            logger.LogInformation("Edit news.");

            string imageName = Path.Combine(news.NewsId.ToString(), Path.GetExtension(newsModel.Image.FileName));
            fileService.DeleteFile(Path.Combine(fileService.GlobalNewsImagesPath, news.ImageName));
            await fileService.SaveFileAsync(newsModel.Image.OpenReadStream(), Path.Combine(fileService.GlobalNewsImagesPath, imageName));
            news.ImageName = imageName;

            newsRepository.Edit(news);
            await storage.SaveAsync();

            logger.LogInformation("Return answer.");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var newsRepository = storage.GetRepository<INewsRepository>();
            try 
            { 
                News news = await newsRepository.GetAsync(id);
                newsRepository.Delete(news);
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
