using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Server.Pages
{
	public class ChangeCultureModel : Infrastructure.BasePageModel
	{
		public ChangeCultureModel(Infrastructure.Settings.ApplicationSettings applicationSettings)
		{
			ApplicationSettings = applicationSettings;
		}

		private Infrastructure.Settings.ApplicationSettings ApplicationSettings { get; }

		public IActionResult OnGet(string? cultureName)
		{
			var typeHeader =
				HttpContext.Request.GetTypedHeaders();

			var httpReferer =
				typeHeader?.Referer?.AbsoluteUri;

			if (string.IsNullOrEmpty(httpReferer))
			{
				return RedirectToPage(pageName: "/Index");
			}

			var defaultCulture =
				ApplicationSettings
				.CultureSettings?
				.DefaultCutrueName;

			var supportedCultures =
				ApplicationSettings
				.CultureSettings?
				.SupportedCultureNames?
				.ToList();

			if (string.IsNullOrEmpty(cultureName))
			{
				cultureName = defaultCulture;
			}

			if (supportedCultures?.Contains(item: cultureName!) == false)
			{
				cultureName = defaultCulture;
			}

			Infrastructure.Middleares.CultureCookieHandlerMiddleware
				.SetCulture(cultureName: cultureName);

			Infrastructure.Middleares.CultureCookieHandlerMiddleware
				.CreateCookie(httpContext: HttpContext, cultureName: cultureName!);

			return Redirect(httpReferer);
		}
	}
}
