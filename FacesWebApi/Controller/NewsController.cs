using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Abstractions.Exceptions;
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
        public IActionResult Get([FromQuery] SearchNewsModel searchOptions)
        {
            logger.LogInformation("Get action");

            var newsRepository = storage.GetRepository<INewsRepository>();

            IEnumerable<News> allNews;

            logger.LogInformation("Search all news.");
            allNews = newsRepository.All((options) =>
            {
                options.From = searchOptions.From;
                options.Count = searchOptions.Count;
                options.WithBody = searchOptions.WithBody;
            });

            var allNewsModel = allNews.Select(n => new
            {
                Id = n.NewsId,
                Topic = n.Topic,
                Body = n.Body,
                PublishDate = n.PublishDate.ToShortDateString(),
                ImageSrc = Path.Combine(fileService.LocalNewsImagesPath, n.ImageName)
            });

            logger.LogInformation("Return answer.");
            return Ok(allNewsModel);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            logger.LogInformation("Get action");

            var newsRepository = storage.GetRepository<INewsRepository>();

            News news;
            try
            {
                logger.LogInformation("Get news.");
                news = await newsRepository.GetAsync(id);
            }
            catch (NewsNotFoundException ex)
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

            logger.LogInformation("Return answer.");
            return Ok(newsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] NewsModel newsModel)
        {
            logger.LogInformation("Post action.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newsRepository = storage.GetRepository<INewsRepository>();

            logger.LogInformation("Create news and add to db.");
            News news = new News()
            {
                Topic = newsModel.Topic,
                Body = newsModel.Body,
                PublishDate = DateTime.Now.Date
            };
            await newsRepository.CreateAsync(news);
            await storage.SaveAsync();

            string imageName = news.NewsId.ToString() + Path.GetExtension(newsModel.Image.FileName);
            await fileService.SaveFileAsync(newsModel.Image.OpenReadStream(), Path.Combine(fileService.GlobalNewsImagesPath, imageName));
            news.ImageName = imageName;

            newsRepository.Edit(news);
            await storage.SaveAsync();

            logger.LogInformation("Return answer.");

            string uri = "user?id=" + news.NewsId;
            return Created(uri, news.NewsId);
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
            catch (NewsNotFoundException ex)
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
            logger.LogInformation("Delete action.");

            var newsRepository = storage.GetRepository<INewsRepository>();
            try 
            {
                News news = await newsRepository.GetAsync(id);
                newsRepository.Delete(news);
                await storage.SaveAsync();

                logger.LogInformation($"Delete news with id-{id}.");
            }
            catch(NewsNotFoundException ex)
            {
                logger.LogInformation($"Not found news with id-{id}.");

                ModelState.AddModelError("Id", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
