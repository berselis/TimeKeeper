using FileSignatures;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeKeeper;
using TimeKeeper.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TimerKeeperDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});

builder.Services.AddAuthentication()
    .AddCookie(option =>
    {
        option.LoginPath = "/Home/Index";
        option.LogoutPath = "/Home/Index";
        option.AccessDeniedPath = "/Home/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddIdentity<Usuario, IdentityRole>(option =>
    {
        option.SignIn.RequireConfirmedAccount = false;
        option.Password.RequireDigit = false;
        option.Password.RequireNonAlphanumeric = false;
        option.Password.RequireUppercase = false;
        option.Password.RequireLowercase = false;
        option.Password.RequiredLength = 4;

    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TimerKeeperDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IFileFormatInspector>(new FileFormatInspector());

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("IsAdmin", policity =>
        {
            policity.RequireClaim("Role", "Admin");
        });

    })
    .ConfigureApplicationCookie(option =>
    {
        option.LoginPath = "/Home/Index";
        option.LogoutPath = "/Home/Index";
        option.AccessDeniedPath = "/Home/Index";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<TimerKeeperDbContext>();
//    db.Database.Migrate();
//}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
