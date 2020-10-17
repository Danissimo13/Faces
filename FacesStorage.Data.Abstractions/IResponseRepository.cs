using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IResponseRepository : IRepository
    {
        IQueryable<TResponse> All<TResponse>() where TResponse : Response;
        Task<Response> GetByIdAsync(int id); 
        Task<Response> CreateAsync(Response response);
        Response Edit(Response response);
        void Delete(Response response);
    }
}
