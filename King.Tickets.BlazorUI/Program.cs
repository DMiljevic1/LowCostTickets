using King.Tickets.BlazorUI.IServices;
using King.Tickets.BlazorUI.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
.AddCircuitOptions(options =>
{
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(15);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(15);
});
builder.Services.AddScoped(provider =>
{
	var handler = new HttpClientHandler
	{
		ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => true
	};
	return new HttpClient(handler);
});
builder.Services.AddScoped<ILowCostTicketService, LowCostTicketService>();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
