using EGISZtemplates;
using EGISZtemplates.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Подрубаем нашу БД ёлки-палки
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регаем папочку для файликов (шаблончиков)
FileHelper.EnsureUploadsFolderExists();

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

app.Run();
