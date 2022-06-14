using System.Data;
using Coderr.Client;
using Coderr.Client.AdoNet;
using Coderr.Client.AspNetCore.Mvc;
using Coderr.Client.Serilog;
using Microsoft.AspNetCore.Authentication.Negotiate;
using MvcDemo.App.TodoItems;
using MvcDemo.Data;
using MvcDemo.Data.TodoItems;
using MvcDemo.WebSite.Infrastructure;
using Serilog;

// A standard ASP.NET Core application running .NET 6 (3.1x and above are supported by Coderr).
// Works the same in .NET Core 3.1 and above (but configured in Startup.cs).
var url = new Uri("https://localhost:44393/");
Err.Configuration.Credentials(url,
    "5d1330ee179541818fee99046357154e",
    "3fec9e64b392475eb056cc2067fb22ca");

Err.Configuration.AttachUserPrincipal();

Err.Configuration.UseErrorPage("/Views/Shared/Error.cshtml");


//Err.Configuration.ReportInvalidModelStates();

Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Coderr(Err.Configuration)
    .CreateLogger();
Log.Debug("Hello world");

var builder = WebApplication.CreateBuilder(args);

// This will provide the latest log entries to Coderr
// and report logged exceptions (if not yet reported).
builder.Logging.AddCoderr();

builder.Services
    .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services
    .AddControllersWithViews(options =>
    {
        // Collects telemetry from the MVC pipeline.
        options.CatchMvcExceptions();
    })
    .AddRazorRuntimeCompilation();


builder.Services.AddScoped(_ => DataBuilder.OpenConnection(builder.Configuration).ToCoderrConnection());
builder.Services.AddScoped(x => x.GetRequiredService<IDbConnection>().BeginTransaction());
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

// Catch all errors (and telemetry) outside the MVC pipeline.
app.CatchMiddlewareExceptions();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

IdentityMiddleware.UserId = 3;
app.UseAuthentication();
app.UseFakeIdentity();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
