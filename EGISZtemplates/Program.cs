using EGISZtemplates;
using EGISZtemplates.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EGISZtemplates.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Подрубаем нашу БД ёлки-палки
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// Добавление пользовательского сервиса
builder.Services.AddScoped<UserService>();

// Настройка печенек (Cookie аутентификации)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Бла-бла-бла, отловщик-отладчик-фиксатор ошибок на стадии разработки
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Подрубаем маршрутизацию для странички с управлением шаблонами
app.MapControllerRoute(
    name: "templates",
    pattern: "{controller=Templates}/{action=Manage}/");

// ...и на страничку авторизации
app.MapControllerRoute(
    name: "templates",
    pattern: "{controller=Account}/{action=Login}/");

app.Run();  // ПОООГНАЛИ!
