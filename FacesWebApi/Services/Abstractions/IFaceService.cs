using FacesStorage.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FacesWebApi.Services.Abstractions
{
    public interface IFaceService
    {
        public Task<Request> CreateRequestAsync(string requestType, IFormFile fromImage, IFormFile toImage, int? userId);
        public Task<Response> CreateResponseAsync(Request request);
    }
}
