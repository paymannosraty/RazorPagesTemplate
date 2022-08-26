namespace Infrastructure.Middleares
{
	public class CultureCookieHandlingMiddleware
	{
		public readonly static string CookieName = "Culture.Cookie";

		public static void SetCulture(string cultureName)
		{
			var cultureInfo =
				new System.Globalization.CultureInfo(name: cultureName);

			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
		}

		public static void CreateCookie(HttpContext httpContext, string cultureName)
		{
			httpContext.Response.Cookies.Delete(key: CookieName);

			var cookieOptions =
				new CookieOptions
				{
					Path = "/",
					Secure = false,
					HttpOnly = false,
					IsEssential = false,
					MaxAge = null,
					Expires = DateTimeOffset.UtcNow.AddYears(1),
					SameSite = SameSiteMode.Unspecified,
				};

			if (string.IsNullOrEmpty(cultureName) == false)
			{
				cultureName =
					cultureName
					.Substring(startIndex: 0, length: 2)
					.ToLower();

				httpContext.Response.Cookies
					.Append(key: CookieName, value: cultureName, options: cookieOptions);
			}
		}

		public CultureCookieHandlingMiddleware(RequestDelegate next)
		{
			Next = next;
		}

		private RequestDelegate Next { get; set; }

		public async Task InvokeAsync(HttpContext httpContext)
		{
			var cultureName =
				httpContext.Request.Cookies[key: CookieName]?
				.Substring(startIndex: 0, length: 2)
				.ToLower();

			switch (cultureName)
			{
				case "fa":
				case "en":
					{
						SetCulture(cultureName: cultureName);
						break;
					}

				default:
					SetCulture("fa");
					break;
			}

			await Next(context: httpContext);
		}
	}
}
