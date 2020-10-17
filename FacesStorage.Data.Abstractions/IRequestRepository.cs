using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRequestRepository : IRepository
    {
        public IQueryable<TRequest> All<TRequest>() where TRequest : Request;
        public Task<Request> GetByIdAsync(int id);
        public Task<Request> CreateAsync(Request request);
        public Request Edit(Request request);
        public void Delete(Request request);
    }
}
