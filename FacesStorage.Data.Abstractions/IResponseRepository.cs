using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using System;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IResponseRepository : IRepository
    {
        Task<Response> GetAsync(Action<ResponseSearchOptions> optionsBuilder); 
        Task<Response> CreateAsync(Response response);
        Response Edit(Response response);
        void Delete(Response response);
    }
}
