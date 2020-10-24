using FaceDetection.Core;
using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using FacesStorage.Data.MSSql;
using FacesWebApi.Services.Abstractions;
using FacesWebApi.Services.Implemetations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;

namespace FacesWebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFaceDetectionPathSystem(this IServiceCollection services)
        {
            services.AddSingleton<ModelPathSystem>(provider => {
                var fileService = provider.GetRequiredService<IFileService>();
                string inputFacesPath = fileService.GlobalRequestImagesPath;
                string outputFacesPath = fileService.GlobalResponseImagesPath;
                ModelPathSystem pathSystem = new ModelPathSystem(inputFacesPath, outputFacesPath);
                return pathSystem;
            });

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

        public static IServiceCollection AddDefaultModelsToStorage(this IServiceCollection services, IConfiguration config)
        {
            var hashService = new HashService();
            var storage = new Storage(config.GetConnectionString("DbConnection"));

            var roleRepository = storage.GetRepository<IRoleRepository>();
            var userRepository = storage.GetRepository<IUserRepository>();

            if (!roleRepository.Any())
            {
                roleRepository.CreateAsync(new Role() { Name = roleRepository.DefaultUserRole });
                roleRepository.CreateAsync(new Role() { Name = roleRepository.DefaultAdminRole });
                storage.Save();
            }

            var adminData = config.GetSection("Admin");
            if (!userRepository.All(options => { }).Any())
            {
                User user = userRepository.CreateAsync(new User()
                {
                    Nickname = adminData.GetValue<string>("Nickname"),
                    Email = adminData.GetValue<string>("Email"),
                    Password = Encoding.UTF8.GetString(hashService.GetHash(adminData.GetValue<string>("Password"))),
                    Role = roleRepository.GetAsync(roleRepository.DefaultAdminRole).Result
                }).Result;
                storage.Save();
            }

            return services;
        }

        public static IServiceCollection AddHashService(this IServiceCollection services)
        {
            services.AddSingleton<IHashService, HashService>();

            return services;
        }

        public static IServiceCollection AddFileService(this IServiceCollection services)
        {
            services.AddSingleton<IFileService, FileService>(provider => new FileService(provider.GetRequiredService<IWebHostEnvironment>()));

            return services;
        }

        public static IServiceCollection AddFaceService(this IServiceCollection services)
        {
            services.AddScoped<IFaceService, FaceService>();

            return services;
        }
    }
}
