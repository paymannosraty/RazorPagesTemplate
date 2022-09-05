namespace Infrastructure.Middleares;
public class ActivationKeysHandlerMiddleware
{
	#region Static Member(s)
	private static string GetSha256(string value)
	{
		using var mySHA256 = System.Security.Cryptography.SHA256.Create();

		var stringBuilder =
			new System.Text.StringBuilder();

		try
		{
			var valueBytes =
				System.Text.Encoding.UTF8.GetBytes(s: value);

			// Compute the hash of the fileStream.
			byte[] hashBytes =
				mySHA256.ComputeHash(buffer: valueBytes);

			foreach (byte theByte in hashBytes)
			{
				stringBuilder.Append
					(value: theByte.ToString("x2"));
			}

			return stringBuilder.ToString();
		}
		catch
		{
			return string.Empty;
		}
	}

	private static string GetValidActivationKeyByDomain(string domain)
	{
		var result =
			GetSha256(value: domain);

		return result;
	}
	#endregion /Static Member(s)
	public ActivationKeysHandlerMiddleware(RequestDelegate next)
	{
		Next = next;
	}

	public RequestDelegate Next { get; }

	public async Task InvokeAsync(HttpContext httpContext, Settings.ApplicationSettings applicationSettings)
	{
		if (applicationSettings == null ||
			applicationSettings.ActivationKeys == null ||
			applicationSettings.ActivationKeys.Length == 0)
		{
			httpContext.Response.Redirect("ActivationKeyNotFound.html");
			return;
		}

		var domain =
			httpContext.Request.Host.Value;

		var valiActivationKey =
			GetValidActivationKeyByDomain(domain: domain);

		if (valiActivationKey == null)
		{
			httpContext.Response.Redirect("ActivationKeyNotFound.html");
			return;
		}

		var isContains =
			applicationSettings
			.ActivationKeys?
			.Where(current => current.ToLower() == valiActivationKey?.ToLower())
			.Any();

		if (isContains == false)
		{
			httpContext.Response.Redirect("ActivationKeyNotFound.html");
			return;
		}

		await Next(httpContext);
	}
}
