using Microsoft.Extensions.Options;

namespace Infrastructure.Middleares
{
	public class CultureCookieHandlerMiddleware
	{
		#region Static members
		public readonly static string CookieName = "Culture.Cookie";

		public static void SetCulture(string? cultureName)
		{
			if (string.IsNullOrEmpty(cultureName) == false)
			{
				var cultureInfo =
					new System.Globalization.CultureInfo(name: cultureName);

				Thread.CurrentThread.CurrentCulture = cultureInfo;
				Thread.CurrentThread.CurrentUICulture = cultureInfo;
			}
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
				httpContext.Response.Cookies
					.Append(key: CookieName, value: cultureName, options: cookieOptions);
			}
		}

		public static string? GetCultureNameByCookie(HttpContext httpContext, IList<string>? supportedCultures)
		{
			if (supportedCultures == null ||
				supportedCultures.Count == 0)
			{
				return null;
			}

			var cultureName =
				httpContext.Request.Cookies[key: CookieName];

			if (cultureName == null)
			{
				return null;
			}

			if (supportedCultures.Contains(cultureName) == false)
			{
				return null;
			}

			return cultureName;

		}
		#endregion /Static members

		public CultureCookieHandlerMiddleware(RequestDelegate next,
			IOptions<RequestLocalizationOptions>? requestLocalizationOptions)
		{
			Next = next;
			RequestLocalizationOptions = requestLocalizationOptions?.Value;
		}

		private RequestDelegate Next { get; }
		private RequestLocalizationOptions? RequestLocalizationOptions { get; }

		public async Task InvokeAsync(HttpContext httpContext)
		{
			var defaultCulture =
				RequestLocalizationOptions?
				.DefaultRequestCulture.UICulture.Name;

			var supportedCultures =
				RequestLocalizationOptions?
				.SupportedCultures?
				.Select(current => current.Name)
				.ToList();

			var currentCulture =
				GetCultureNameByCookie(httpContext: httpContext, supportedCultures: supportedCultures);

			if (currentCulture == null)
			{
				currentCulture = defaultCulture;
			}

			SetCulture(cultureName: currentCulture);

			await Next(context: httpContext);
		}
	}
}
