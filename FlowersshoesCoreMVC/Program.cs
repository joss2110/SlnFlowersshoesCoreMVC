using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.DAO;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conexion = builder.Configuration.GetConnectionString("cn1");

builder.Services.AddDbContext<flowersshoesContext>(
    opt => opt.UseSqlServer(conexion));

builder.Services.AddScoped<IngresosDAO>();

builder.Services.AddScoped<VentassDAO>();

builder.Services.AddSession(x => x.IdleTimeout =  TimeSpan.FromMinutes(60));


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
