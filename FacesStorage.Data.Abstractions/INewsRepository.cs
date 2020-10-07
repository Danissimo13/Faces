using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface INewsRepository : IRepository
    {
        IQueryable<News> All();
        Task<News> GetAsync(int id);
        Task<News> GetLastAsync();
        Task<News> CreateAsync(News news);
        News Edit(News news);
        void Delete(News news);
    }
}
