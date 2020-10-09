using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HacktoberfestProject.Web.Extensions
{
	public static class AuthenticationServiceCollectionExtensions
	{
		public static void AddGithubOauthAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = "GitHub";
			})
		   .AddCookie()
		   .AddOAuth("GitHub", options =>
		   {
			   options.ClientId = configuration["GitHub:ClientId"];
			   options.ClientSecret = configuration["GitHub:ClientSecret"];
			   options.CallbackPath = new PathString("/github-oauth");
			   options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
			   options.TokenEndpoint = "https://github.com/login/oauth/access_token";
			   options.UserInformationEndpoint = "https://api.github.com/user";
			   options.SaveTokens = true;
			   options.Scope.Add("user:email");
			   options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
			   options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
			   options.ClaimActions.MapJsonKey(ClaimTypes.Email, "user:email");
			   options.ClaimActions.MapJsonKey("urn:github:login", "login");
			   options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
			   options.Events = new OAuthEvents
			   {
				   OnCreatingTicket = async context =>
				   {
					   var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
					   request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					   request.Headers.Add("X-OAuth-Scopes", "user");
					   request.Headers.Add("X-Accepted-OAuth-Scopes", "user");
					   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
					   var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
					   response.EnsureSuccessStatusCode();
					   var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
					   context.RunClaimActions(json.RootElement);
				   }
			   };
		   });
		}
	}
}
