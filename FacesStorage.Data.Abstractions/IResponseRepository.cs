using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IResponseRepository<T> : IRepository where T : Response
    {
        IQueryable<T> All();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T response);
        T Edit(T response);
        void Delete(T response);
    }
}
