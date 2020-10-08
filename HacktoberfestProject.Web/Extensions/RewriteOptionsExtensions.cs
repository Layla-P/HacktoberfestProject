using HacktoberfestProject.Web.Extensions.Rules;

using Microsoft.AspNetCore.Rewrite;

namespace HacktoberfestProject.Web.Extensions
{
	public static class RewriteOptionsExtensions
	{
		public static RewriteOptions AddRedirectToApex(this RewriteOptions options)
		{
			options.Rules.Add(new RedirectToApexRule());
			return options;
		}
	}
}
