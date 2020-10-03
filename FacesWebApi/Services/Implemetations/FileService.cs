using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace FacesWebApi.Services.Implemetations
{
    public class FileService : IFileService
    {
        public string LocalRequestImagesPath { get; set; }
        public string GlobalRequestImagesPath { get; set; }

        public string LocalResponseImagesPath { get; set; }
        public string GlobalResponseImagesPath { get; set; }

        public FileService(IWebHostEnvironment environment)
        {
            GlobalRequestImagesPath = Path.Combine(environment.WebRootPath, "faces", "requests");
            LocalRequestImagesPath = Path.Combine("faces", "requests");
            GlobalResponseImagesPath = Path.Combine(environment.WebRootPath, "faces", "responses");
            LocalResponseImagesPath = Path.Combine("faces", "responses");
        }

        public async Task<string> SaveFileAsync(Stream stream, string path)
        {
            using(FileStream writer = new FileStream(path, FileMode.Create))
            {
                await stream.CopyToAsync(writer);
            }

            return path;
        }
    }
}
