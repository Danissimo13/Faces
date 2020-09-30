using FacesStorage.Data.Models;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IResponseImageRepository : IRepository
    {
        Task<ResponseImage> Create(ResponseImage image);
        ResponseImage Edit(ResponseImage image);
        void Delete(ResponseImage image);
    }
}
