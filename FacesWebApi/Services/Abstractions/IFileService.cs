using System.IO;
using System.Threading.Tasks;

namespace FacesWebApi.Services.Abstractions
{
    public interface IFileService
    {
        public string LocalRequestImagesPath { get; set; }
        public string GlobalRequestImagesPath { get; set; }

        public string LocalResponseImagesPath { get; set; }
        public string GlobalResponseImagesPath { get; set; }

        public string LocalNewsImagesPath { get; set; }
        public string GlobalNewsImagesPath { get; set; }

        Task<string> SaveFileAsync(Stream stream, string path);
        void DeleteFile(string path);
    }
}
