using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(s =>
                {
                    s.LoginPath = "/Home/Login";
                    s.LogoutPath = "/Home/Login";
                    s.AccessDeniedPath = "/UserHome";
                }
                    );

            //Pobieranie danych konfiguracyjnych z pliku
            var connectionString = builder.Configuration.GetConnectionString("System");
            //Dodawanie do zasob�w klasy kontekstu dla bazy danych
            builder.Services.AddDbContext<TestContext>(x => x.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<Controllers.Services.UserService>();
            builder.Services.AddTransient<Controllers.Services.TagsService>();
            builder.Services.AddTransient<Controllers.Services.PostService>();
            builder.Services.AddTransient<Controllers.Services.GroupService>();
            var app = builder.Build();

            // Run CreateDefaults method
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TestContext>();
                dbContext.CreateDefaults();
            }

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

            app.UseAuthentication();
            app.UseAuthorization();

            // Default route
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserHome}/{action=Index}/{id?}");

            // User profile route
            app.MapControllerRoute(
                    name: "userProfile",
                    pattern: "UserProfile/{id?}",
                    defaults: new { controller = "UserProfile", action = "Index" });

            app.MapControllerRoute(
                    name: "userGroup",
                    pattern: "UserGroup/{id?}",
                    defaults: new { controller = "UserGroup", action = "Index" });

            app.UseCors(
             options => options
             .AllowAnyHeader()
             .AllowAnyOrigin()
             .AllowAnyMethod()
             );
            app.Run();
        }
    }
}
