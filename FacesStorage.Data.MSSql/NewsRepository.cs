using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Abstractions.Exceptions;
using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    public class NewsRepository : INewsRepository
    {
        private StorageContext storageContext;
        private DbSet<News> newsDbSet;

        public IEnumerable<News> All(Action<NewsSearchOptions> optionsBuilder)
        {
            NewsSearchOptions searchOptions = new NewsSearchOptions();
            optionsBuilder(searchOptions);

            var news = newsDbSet.OrderByDescending(n => n.PublishDate).AsQueryable<News>();
            news = news.Skip(searchOptions.From);
            news = news.Take(searchOptions.Count);

            news = news.Select(n => new News()
            {
                NewsId = n.NewsId,
                Topic = n.Topic,
                Body = searchOptions.WithBody ? n.Body : string.Empty,
                PublishDate = n.PublishDate,
                ImageName = n.ImageName,
            });

            return news;
        }

        public async Task<News> GetAsync(int id)
        {
            News news = await newsDbSet.FirstOrDefaultAsync(n => n.NewsId == id);
            if (news == null) throw new NewsNotFoundException($"Not found news with id equal {id}.");

            return news;
        }

        public async Task<News> CreateAsync(News news)
        {
            var entityEntry = await newsDbSet.AddAsync(news);
            return entityEntry.Entity;
        }

        public News Edit(News news)
        {
            var entityEntry = newsDbSet.Update(news);
            return entityEntry.Entity;
        }

        public void Delete(News news)
        {
            newsDbSet.Remove(news);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.newsDbSet = this.storageContext.Set<News>();
        }
    }
}
