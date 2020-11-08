using FacesStorage.Data.Models;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IResponseImageRepository : IRepository
    {
        Task<ResponseImage> CreateAsync(ResponseImage image);
        ResponseImage Edit(ResponseImage image);
    }
}
