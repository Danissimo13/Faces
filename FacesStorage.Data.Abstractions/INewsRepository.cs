using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface INewsRepository : IRepository
    {
        IQueryable<News> All();
        Task<News> GetByIdAsync(int id);
        Task<News> CreateAsync(News news);
        News Edit(News news);
        void Delete(News news);
    }
}
