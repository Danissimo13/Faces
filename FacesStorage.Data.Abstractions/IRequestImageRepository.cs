using FacesStorage.Data.Models;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRequestImageRepository : IRepository
    {
        Task<RequestImage> CreateAsync(RequestImage image);
        RequestImage Edit(RequestImage image);
        void Delete(RequestImage image);
    }
}
