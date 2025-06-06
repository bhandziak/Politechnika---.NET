
using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.Helpers;
using CarWorkshopProjekt.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

namespace CarWorkshopProjekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Konfiguracja NLog z pliku nlog.config
            //var logger = app.Services.GetRequiredService<ILogger<Program>>();
            //logger.LogError("Testowy b³¹d: sprawdzam NLog");
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Information);
            // Œcie¿ka do katalogu projektu dla nloga
            var projectDir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            Environment.SetEnvironmentVariable("PROJECT_DIR", projectDir);
            builder.Host.UseNLog();
//            builder.Logging.AddNLog("nlog.config");

            // CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactLocalhost3010", policy =>
                {
                    policy.WithOrigins("http://localhost:3010")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Baza danych
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;


                // zwraca 401 (niezalogowany)
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                // zwraca 403 (brak uprawnieñ)
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            });

            builder.Services.AddScoped<ICustomerService, CustomerService>(); //Service do CustomerController
            builder.Services.AddScoped<IUserService, UserService>(); //Service do UserController
            builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>(); // Service do ServiceOrder


            var app = builder.Build();
            app.UseCors("AllowReactLocalhost3010");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles(); //obs³uga zdjêæ

            app.MapControllers();

            app.Run();
        }
    }
}