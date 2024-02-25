using Flights_Application.Data;
using Flights_Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flights_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<Entities>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FlightsConnectionString")));
            builder.Services.AddControllersWithViews();
            builder.Services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
                {

                    Description="Development Server",
                    Url= "http://localhost:5158"
                });

                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]+e.ActionDescriptor.RouteValues["controller"]}");
            }



                );

           builder.Services.AddScoped<Entities>();

           
            var app = builder.Build();
            var entitiesService = app.Services.CreateScope().ServiceProvider.GetService<Entities>();

            entitiesService.Database.EnsureCreated();
            Random random = new Random();

            if (!entitiesService.Flights.Any())
            {

                Flight[] flightsToSeed = new Flight[]
                {
                  new (   Guid.NewGuid(),
         "American Airlines",
         random.Next(90, 5000).ToString(),
         new TimePlace("Zurich",DateTime.Now.AddHours(random.Next(1, 23))),
         new TimePlace("Baku",DateTime.Now.AddHours(random.Next(4, 25))),
             random.Next(1, 853)),
 new (   Guid.NewGuid(),
         "Adria Airways",
         random.Next(90, 5000).ToString(),
         new TimePlace("Ljubljana",DateTime.Now.AddHours(random.Next(1, 15))),
         new     ("Warsaw",DateTime.Now.AddHours(random.Next(4, 19))),
             random.Next(1, 254)),
 new (   Guid.NewGuid(),
         "ABA Air",
         random.Next(90, 5000).ToString(),
         new TimePlace("Praha Ruzyne",DateTime.Now.AddHours(random.Next(1, 55))),
         new TimePlace("Paris",DateTime.Now.AddHours(random.Next(4, 58))),
             random.Next(1, 254)),
 new (   Guid.NewGuid(),
         "AB Corporate Aviation",
         random.Next(90, 5000).ToString(),
         new TimePlace("Le Bourget",DateTime.Now.AddHours(random.Next(1, 58))),
         new TimePlace("Zagreb",DateTime.Now.AddHours(random.Next(4, 60))),
             random.Next(1, 254))
                };
                entitiesService.Flights.AddRange(flightsToSeed);
                entitiesService.SaveChanges();
            }


            app.UseCors(builder=> builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
            app.UseSwagger().UseSwaggerUI();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
            }

            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}