// `DateTime` extension methods.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCache
{
	public static class DateTimeExtensions
	{
		// Generate an "[HTTP date](http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.3.1)" from a `DateTime`.
		public static string ToHttpDate(this DateTime date)
		{
			return date.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
		}
	}
}