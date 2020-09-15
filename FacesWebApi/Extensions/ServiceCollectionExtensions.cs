using FaceDetection.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;

namespace FacesWebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFaceDetectionPathSystem(this IServiceCollection services)
        {
            IWebHostEnvironment environment = services
                .First(service => service.ServiceType == typeof(IWebHostEnvironment))
                .ImplementationInstance as IWebHostEnvironment;

            string inputFacesPath = Path.Combine(environment.WebRootPath, "faces", "requests");
            string outputFacesPath = Path.Combine(environment.WebRootPath, "faces", "responses");
            ModelPathSystem pathSystem = new ModelPathSystem(inputFacesPath, outputFacesPath);
            services.AddSingleton<ModelPathSystem>(pathSystem);

            return services;
        }
    }
}
