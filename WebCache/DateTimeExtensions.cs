using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCache
{
	public static class DateTimeExtensions
	{
		public static string ToHttpDate(this DateTime date)
		{
			return date.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
		}
	}
}