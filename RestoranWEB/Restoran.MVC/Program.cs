using Microsoft.EntityFrameworkCore;
using Restoran.BLL.Mappings;
using Restoran.BLL.Services;
using Restoran.DAL;
using Restoran.DAL.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// EF + AutoMapper + UnitOfWork + Servisi
builder.Services.AddDbContext<RestoranContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("RestoranConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRezervacijaService, RezervacijaService>();

// NOVI AutoMapper 15.1.0 – lambda umesto typeof(Program)
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Rezervacija}/{action=Index}/{id?}");

app.Run();