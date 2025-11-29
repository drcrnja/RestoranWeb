using Microsoft.EntityFrameworkCore;
using Restoran.web.Models;
using Restoran.web.Data;


var builder = WebApplication.CreateBuilder(args);

// Dodaj DbContext
builder.Services.AddDbContext<RestoranContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RestoranContext"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));
// Dodaj MVC
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");
app.UseSession();
app.Run();