using FaceDetection.Core;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.MSSql;
using FacesWebApi.Services.Abstractions;
using FacesWebApi.Services.Implemetations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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

        public static IServiceCollection AddStorageContext(this IServiceCollection services)
        {
            services.AddScoped<IStorage, Storage>(provider =>
            {
                IConfiguration config = provider.GetRequiredService<IConfiguration>();
                return new Storage(config.GetConnectionString("DbConnection"));
            });

            return services;
        }

        public static IServiceCollection AddHashService(this IServiceCollection services)
        {
            services.AddSingleton<IHashService, HashService>();

            return services;
        }
    }
}
