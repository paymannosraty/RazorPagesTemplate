using Infrastructure.Middlewares;

var webApplicationOptions =
	new WebApplicationOptions
	{
		EnvironmentName =
			System.Diagnostics.Debugger.IsAttached ?
			Environments.Development : Environments.Production,
	};

var builder =
	WebApplication.CreateBuilder(options: webApplicationOptions);

builder.Services.AddRazorPages();

builder.Services.Configure<Infrastructure.Settings.ApplicationSettings>
	(builder.Configuration.GetSection(Infrastructure.Settings.ApplicationSettings.KeyName))
	.AddSingleton
	(implementationFactory: serviceType =>
	{
		var result =
			serviceType.GetRequiredService
			<Microsoft.Extensions.Options.IOptions
			<Infrastructure.Settings.ApplicationSettings>>().Value;

		return result;
	});

var app =
	builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseGlobalException();

	app.UseExceptionHandler("/Errors/Error");

	// The default HSTS value is 30 days.
	// You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseActivationKeys();

app.UseCultureCookie();

app.UseRouting();

app.MapRazorPages();

app.Run();
