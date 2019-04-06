using System.Buffers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using OptWebApi.Formatters;

namespace OptWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            int _50mb = 5 * 1024 * 1024;
            var pool = ArrayPool<char>.Create(_50mb, 25);
            
            services.AddMvcCore()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddMvcOptions(p =>
                    {
                        
                        p.OutputFormatters.Add(new JsonOutputFormatter(pool));
                        p.InputFormatters.Add(new JsonInputFormatter(pool));
                    }
                    );

//            services.AddSingleton(ArrayPool<char>.Create(_50mb, 25));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
