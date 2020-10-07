using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Models.Enums
{
	public enum PrStatus
	{
		[Description("Meets the Criteria to be included in Hacktoberfest.")]
		Valid = 1,
		[Description("Does not meet Crteria to be included in Hacktoberfest.")]
		Invalid = 10,
		[Description("The Repository is not tagged as Hacktoberfest.")]
		TopicInvalid = 20,
		[Description("The PR has not been merged,labelled or approved.")]
		Awaiting  = 30,
		[Description("The PR is not within the allowed dates.")]
		InvalidDate = 40

	}
}
