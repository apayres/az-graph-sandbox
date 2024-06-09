using GraphSandbox.Web.Services;
using GraphSandbox.Web.Services.Contracts;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// WEB & POLICIES
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddMvcOptions(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    }).AddMicrosoftIdentityUI();

// AUTH
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddMicrosoftGraph(opts =>
    {
        opts.Scopes = "user.read tasks.readwrite calendars.readwrite";
    })
    .AddInMemoryTokenCaches();

// LOCAL SERVICES
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();


var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
