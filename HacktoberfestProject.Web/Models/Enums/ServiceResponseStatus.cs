using System.ComponentModel;

namespace HacktoberfestProject.Web.Models.Enums
{
	public enum ServiceResponseStatus
	{
		[Description("Unset")]
		Unset = 0,

		[Description("Ok")]
		Ok = 1,

		[Description("No Content")]
		NoContent = 2,

		[Description("Not Found")]
		NotFound = 3,

		[Description("Created")]
		Created = 4,

		[Description("Bad request")]
		BadRequest = 5,

		// typical expected usage: some major or unexpected fault - would return an http500 error
		[Description("Failure, was not Handled")]
		FailUnhandled = 6,

		// typical expected usage: some minor or handled fault - would return an http200 ok, but supply an error message as the body - for example, a data problem where a business-rule could not be met, etc
		[Description("Failure, was Handled")]
		FailHandled = 7,

		[Description("Duplicate Found")]
		DuplicateFound = 8

	}
}
