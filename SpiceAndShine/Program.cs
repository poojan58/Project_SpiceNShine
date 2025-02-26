using SpiceAndShine.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.IgnoreNullValues = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".SpiceAndShine.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(360);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(typeof(Program));

var config = builder.Configuration;
var services = builder.Services;
#if DEBUG
services.AddMvc(options => options.EnableEndpointRouting = false);//.AddRazorRuntimeCompilation();
#else
    services.AddMvc(options => options.EnableEndpointRouting = false);
#endif

Common.DBConnectionString = config["ConnectionStrings:DefaultConnection"];
Common.SiteURL = config["AppStrings:SiteURL"];
Common.SiteCDNBaseURL = config["AppStrings:SiteCDNBaseURL"];


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseStatusCodePagesWithRedirects("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseSession();



app.UseMvc(routes =>
{
    routes.MapRoute(
       "Manager",
       "manager/{controller}/{action}/{id?}",
       new { area = "manager", controller = "Login", action = "Index" }
    );
   
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}"
    );
});

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/

app.Run();
