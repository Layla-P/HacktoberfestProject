using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HacktoberfestProject.Web.Extensions
{
	public static class EnumerationExtensions
	{
		public static string GetDescription(this Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

			if (attributes != null && attributes.Any())
			{
				return attributes.First().Description;
			}

			return value.ToString();
		}
	}
}
