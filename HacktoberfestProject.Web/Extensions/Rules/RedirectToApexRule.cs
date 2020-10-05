using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Extensions.Rules
{
    public class RedirectToApexRule : IRule
    {
        public virtual void ApplyRule(RewriteContext context)
        {
            var req = context.HttpContext.Request;

            if (req.Host.Value.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                var apex = req.Host.Value.Replace("www.", "");
                var host = new HostString(apex);
                var newUrl = UriHelper.BuildAbsolute(req.Scheme, host, req.PathBase, req.Path, req.QueryString);
                var response = context.HttpContext.Response;
                response.StatusCode = 301;
                response.Headers[HeaderNames.Location] = newUrl;
                context.Result = RuleResult.EndResponse;
            }
            context.Result = RuleResult.ContinueRules;
            return;
        }
    }
}
