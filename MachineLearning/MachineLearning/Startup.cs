using System.IO;
using MachineLearning.Prediction;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace MachineLearning
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
            services.AddSingleton<Predictor>(a =>
            {
                IWebHostEnvironment webHost = a.GetService<IWebHostEnvironment>();
                string path = Path.Combine(webHost.ContentRootPath, "Assets", "model.onnx");

                return Predictor
                        .CreateAsync(path)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        }
    }
}
