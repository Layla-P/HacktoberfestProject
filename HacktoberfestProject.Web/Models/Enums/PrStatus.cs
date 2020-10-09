using System.ComponentModel;

namespace HacktoberfestProject.Web.Models.Enums
{
	public enum PrStatus
	{
		[Description("Meets Criteria to be included in Hacktoberfest.")]
		Valid = 1,
		[Description("Does not meet Criteria to be included in Hacktoberfest.")]
		Invalid = 10,
		[Description("The Repository does not have the Hacktoberfest topic.")]
		TopicInvalid = 20,
		[Description("The PR has not been merged, labelled or approved.")]
		Awaiting = 30,
		[Description("The PR is not within the allowed dates.")]
		InvalidDate = 40
	}
}
