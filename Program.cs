using CalendarChanger.Infrastructure;
using CalendarChanger.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CalendarChanger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CalendarContext>(options =>
            {
                try
                {
                    var dataDir = Path.Combine("/app", "data");
                    Directory.CreateDirectory(dataDir);
                    var dataPath = Path.Combine(dataDir, "data.db");
                    options.UseSqlite($"Data Source={dataPath}");
                }
                catch (Exception e)
                {
                    var dataDir = "data";
                    Directory.CreateDirectory(dataDir);
                    var dataPath = Path.Combine(dataDir, "data.db");
                    options.UseSqlite($"Data Source={dataPath}");
                }

            });
            builder.Services.AddHttpClient<TimeEditClient>();
            builder.Services.AddScoped<ICalendarService, CalendarService>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(6);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CalendarChanger API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Enter your API key"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();
            
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CalendarContext>();
                db.Database.Migrate();
            }
            
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=EventModifier}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
