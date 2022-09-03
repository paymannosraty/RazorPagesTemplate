namespace Infrastructure.Middleares
{
	public static class ExtentionMethods
	{
		static ExtentionMethods()
		{
		}

		public static IApplicationBuilder UseCultureCookie(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CultureCookieHandlerMiddleware>();
		}

		public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
		{
			return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		}
	}
}
