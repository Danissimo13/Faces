using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface INewsRepository : IRepository
    {
        IEnumerable<News> All(Action<NewsSearchOptions> optionsBuilder);
        Task<News> GetAsync(int id);
        Task<News> CreateAsync(News news);
        News Edit(News news);
        void Delete(News news);
    }
}
