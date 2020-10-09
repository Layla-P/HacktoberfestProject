using HacktoberfestProject.Web.Models.Enums;

namespace HacktoberfestProject.Web.Models.Helpers
{
	public class ServiceResponse<T> : ServiceResponse
	{
		public T Content { get; set; } // the result (e.g. a list of records), or an error message (when in error state)
	}

	/// <summary>
	/// Project intended to provide a Generalised container for the response from a service class.
	/// Typical intended use is a container to relay either:
	/// a) valid data (e.g. a dataset returned from a database query)
	/// b) error information (e.g. why a database call failed)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ServiceResponse
	{
		public ServiceResponseStatus ServiceResponseStatus { get; set; } // general ok/fail status

		public string Message { get; set; } //  optional supplementary message (e.g. "total records found")

		public ServiceResponse()
		{
			ServiceResponseStatus = ServiceResponseStatus.Unset;
			Message = string.Empty;
		}
	}
}
