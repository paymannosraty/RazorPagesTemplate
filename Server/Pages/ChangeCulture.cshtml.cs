using Microsoft.AspNetCore.Mvc;

namespace Server.Pages
{
	public class ChangeCultureModel : Infrastructure.BasePageModel
	{
		public ChangeCultureModel()
		{
		}

		public IActionResult OnGet(string name)
		{
			var typeHeader =
				HttpContext.Request.GetTypedHeaders();

			var httpReferer =
				typeHeader.Referer.AbsoluteUri;

			if (string.IsNullOrEmpty(httpReferer))
			{
				return RedirectToPage(pageName: "/Index");
			}

			string defaultCulture = "fa";

			if (string.IsNullOrEmpty(name))
			{
				name = defaultCulture;
			}

			name =
				name.Replace(" ", string.Empty)
				.ToLower();

			switch (name)
			{
				case "fa":
				case "en":
					{
						break;
					}
				default:
					{
						name = defaultCulture;
						break;
					}
			}

			Infrastructure.Middleares.CultureCookieHandlingMiddleware
				.SetCulture(cultureName: name);

			Infrastructure.Middleares.CultureCookieHandlingMiddleware
				.CreateCookie(httpContext: HttpContext, cultureName: name);

			return Redirect(httpReferer);
		}
	}
}
