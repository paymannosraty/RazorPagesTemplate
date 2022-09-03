using Infrastructure.Middleares;

var builder =
	WebApplication.CreateBuilder();

builder.Services.AddRazorPages();

builder.Services.Configure<Infrastructure.Settings.ApplicationSettings>
	(builder.Configuration.GetSection(Infrastructure.Settings.ApplicationSettings.KeyName));

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

app.UseCultureCookie();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
