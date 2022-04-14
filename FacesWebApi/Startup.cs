using FacesWebApi.Extensions;
using FacesWebApi.Options;
using FacesWebApi.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
<<<<<<< HEAD
=======
using Microsoft.Extensions.Hosting;
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
using Microsoft.IdentityModel.Tokens;

namespace FacesWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurity(),
                        ValidateLifetime = true,
                    };
                });

            services.AddHashService();
            services.AddFileService();
            services.AddFaceDetectionPathSystem();
            services.AddStorageContext();

            services.AddFaceService();

            services.AddDefaultModelsToStorage(Configuration);
        }

<<<<<<< HEAD
=======
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
>>>>>>> 5a02a88a903a252cd896c6ec4fef68b8ce89d3d4
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHashService hash)
        {
            app.UseDeveloperExceptionPage();
            
            var options = new RewriteOptions()
                .AddApacheModRewrite(env.ContentRootFileProvider, "rewrite.txt");
            app.UseRewriter(options);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            var options = new RewriteOptions()
                .AddApacheModRewrite(env.ContentRootFileProvider, "rewrite.txt");
            app.UseRewriter(options);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(configurePolicy => configurePolicy.AllowAnyOrigin());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
