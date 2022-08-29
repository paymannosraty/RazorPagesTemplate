using Infrastructure.Middleares;

var builder =
	WebApplication.CreateBuilder();

builder.Services.AddRazorPages();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[]
	{
		new System.Globalization.CultureInfo("en-US"),
		new System.Globalization.CultureInfo("fa-IR"),
	};

	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;

	options.DefaultRequestCulture =
		new Microsoft.AspNetCore.Localization
		.RequestCulture(uiCulture: "fa-IR", culture: "fa-IR");
});

var app =
	builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Errors/Error");

	// The default HSTS value is 30 days.
	// You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCultureCookie();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
