using System;

using HacktoberfestProject.Web.Tools;

using Xunit;

namespace HacktoberfestProject.Tests.Tools
{
	public class NullCheckerTests
	{
		#region Fields

		private static readonly object _nullValue = null;
		private static readonly object _nonNullValue = new object();

		#endregion

		[Fact]
		public void IsNotNull_WithArgumentValueNull_ShouldThrowArgumentNullException()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => NullChecker.IsNotNull(_nullValue, nameof(_nullValue)));

			Assert.Equal(nameof(_nullValue), exception.ParamName);
		}

		[Fact]
		public void IsNotNull_WithArgumentValueNotNull_ShouldNotThrowArgumentNullException()
		{
			NullChecker.IsNotNull(_nonNullValue, nameof(_nonNullValue));
			//The fact the testcase was executed up until here means it was successful
		}
	}
}
