using dotnet_facebook.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Pobieranie danych konfiguracyjnych z pliku
            var connectionString = builder.Configuration.GetConnectionString("System");
            //Dodawanie do zasob�w klasy kontekstu dla bazy danych
            builder.Services.AddDbContext<TestContext>(x => x.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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

            app.Run();
        }
    }
}
