using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IResponseRepository<T> : IRepository where T : Response
    {
        IQueryable<T> All();
        Task<T> GetById(int id);
        Task<T> Create(T response);
        T Edit(T response);
        void Delete(T response);
    }
}
