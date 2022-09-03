namespace Infrastructure.Middleares
{
	public class GlobalExceptionHandlerMiddleware
	{
		public GlobalExceptionHandlerMiddleware(RequestDelegate next)
		{
			Next = next;
		}

		private RequestDelegate Next { get; }

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await Next(httpContext);
			}
			catch (Exception)
			{
				httpContext.Response.Redirect(location: "/Errors/Error500", permanent: false);
			}
		}

	}
}
