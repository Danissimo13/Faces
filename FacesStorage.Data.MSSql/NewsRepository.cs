using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    public class NewsRepository : INewsRepository
    {
        private StorageContext storageContext;
        private DbSet<News> dbSet;

        public IQueryable<News> All()
        {
            return dbSet.AsQueryable<News>();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            News news = await dbSet.FirstOrDefaultAsync(n => n.NewsId == id);
            if (news == null) throw new KeyNotFoundException($"Not found news with id equal {id}.");

            return news;
        }

        public async Task<News> CreateAsync(News news)
        {
            var entityEntry = await dbSet.AddAsync(news);
            return entityEntry.Entity;
        }

        public News Edit(News news)
        {
            var entityEntry = dbSet.Update(news);
            return entityEntry.Entity;
        }

        public void Delete(News news)
        {
            dbSet.Remove(news);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.dbSet = this.storageContext.Set<News>();
        }
    }
}
