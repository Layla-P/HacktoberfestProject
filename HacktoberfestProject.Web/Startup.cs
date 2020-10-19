using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Data.Configuration;
using HacktoberfestProject.Web.Extensions;
using HacktoberfestProject.Web.Services;
using HacktoberfestProject.Web.Services.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;

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
			services.Configure<GithubConfiguration>(Configuration.GetSection("GitHub"));
			
			
			services.AddSingleton<ITableContext, TableContext>();
			services.AddSingleton(c => { return new GitHubClient(new ProductHeaderValue("HacktoberfestProject")); });
			services.AddSingleton<IGithubService, GithubService>();
			services.AddSingleton<ITrackerEntryService, TrackerEntryService>();
			services.AddSingleton<IProjectService, ProjectService>();

			services.AddControllersWithViews();

			services.AddGithubOauthAuthentication(Configuration);
			services.AddLogging();

			//TableContextTests.RunTableStorageTests(services);
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

      app.Use(async (ctx, next) =>
      {
          ctx.Response.Headers.Add("Content-Security-Policy", "default-src 'self' 'unsafe-inline'; connect-src 'self' https://api.github.com;frame-src *.youtube.com; img-src 'self' *.githubusercontent.com; style-src-elem 'self';");
          await next();
      });
      

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
