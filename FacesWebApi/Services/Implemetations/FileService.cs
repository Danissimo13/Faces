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

        public string LocalNewsImagesPath { get; set; }
        public string GlobalNewsImagesPath { get; set; }

        public FileService(IWebHostEnvironment environment)
        {
            LocalRequestImagesPath = Path.Combine("faces", "requests");
            GlobalRequestImagesPath = Path.Combine(environment.WebRootPath, LocalRequestImagesPath);

            LocalResponseImagesPath = Path.Combine("faces", "responses");
            GlobalResponseImagesPath = Path.Combine(environment.WebRootPath, LocalResponseImagesPath);

            LocalNewsImagesPath = Path.Combine("img", "news");
            GlobalNewsImagesPath = Path.Combine(environment.WebRootPath, LocalNewsImagesPath);
        }

        public async Task<string> SaveFileAsync(Stream stream, string path)
        {
            using(FileStream writer = new FileStream(path, FileMode.Create))
            {
                await stream.CopyToAsync(writer);
            }

            return path;
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
