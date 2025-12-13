using Microsoft.EntityFrameworkCore;
using Restoran.BLL.Mappings;
using Restoran.BLL.Services;
using Restoran.DAL;
using Restoran.DAL.UnitOfWork;
using Restoran.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<RestoranContext>(opts =>
    opts.UseSqlite("Data Source=restoran.db"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRezervacijaService, RezervacijaService>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<RestoranContext>();
    ctx.Database.EnsureCreated();

    if (!ctx.Stolovi.Any())
    {
        var lokacije = new[] { "Prozor", "Sredina", "Kraj", "Ugao", "Terasa" };
        var rnd = new Random();

        for (int i = 1; i <= 20; i++)
        {
            ctx.Stolovi.Add(new Sto
            {
                BrojStola = i,
                BrojMesta = 4,
                Lokacija = lokacije[rnd.Next(lokacije.Length)]
            });
        }
        ctx.SaveChanges();
    }
}

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