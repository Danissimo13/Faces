using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRequestRepository<T> : IRepository where T : Request
    {
        public IQueryable<T> All();
        public Task<T> GetById(int id);
        public Task<T> Create(T request);
        public T Edit(T request);
        public void Delete(T request);
    }
}
