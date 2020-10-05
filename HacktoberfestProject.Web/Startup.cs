using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HacktoberfestProject.Web.Extensions;
using HacktoberfestProject.Web.Data.Configuration;
using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Services;
using Microsoft.AspNetCore.Rewrite;

namespace HacktoberfestProject.Web
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Configure CosmosDB Table API
            services.Configure<TableConfiguration>(Configuration.GetSection("CosmosTableStorage"));
            services.AddSingleton<ITableContext, TableContext>();
            //services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSingleton<IGithubService, GithubService>();
            services.AddSingleton<ITableService, TableService>();

            services.AddControllersWithViews();
            
            services.AddGithubOauthAuthentication(Configuration);
            services.AddLogging();

            //CosmosTableTest.RunTableStorageTests(services);
            //GithubAPITests.RunTableStorageTests(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = new RewriteOptions();
            options.AddRedirectToApex();
            app.UseRewriter(options);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");               
            }
         
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
