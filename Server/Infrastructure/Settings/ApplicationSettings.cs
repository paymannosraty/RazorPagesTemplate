namespace Infrastructure.Settings
{
	public class ApplicationSettings
	{
		public readonly static string KeyName =
			nameof(ApplicationSettings);
		public ApplicationSettings()
		{
		}

		public CultureSettings CultureSettings { get; set; }

		public string[]? ActivationKeys { get; set; }
	}
}
