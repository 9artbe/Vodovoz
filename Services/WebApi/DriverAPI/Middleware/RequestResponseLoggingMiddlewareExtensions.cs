﻿using Microsoft.AspNetCore.Builder;

namespace DriverAPI.Middleware
{
	public static class RequestResponseLoggingMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
		}
	}
}
