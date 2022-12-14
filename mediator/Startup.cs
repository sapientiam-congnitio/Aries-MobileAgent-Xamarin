using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace mediator
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAriesFramework(builder =>
            {
                builder.RegisterMediatorAgent(options =>
                {
                    options.EndpointUri = "http://localhost:5000";
                    
                    options.WalletConfiguration.Id = "MediatorWallet";
                    options.WalletCredentials.Key = "SecretWalletKey";
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Register Aries and Mediator middlwares with the ASP.NET Core pipeline
            app.UseAriesFramework();
            app.UseMediatorDiscovery();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    // Convenience middleware to redirect everything from root 
                    // to the well known mediator agent configuration
                    context.Response.Redirect("/.well-known/agent-configuration", permanent: false);
                });
            });
        }
    }
}
