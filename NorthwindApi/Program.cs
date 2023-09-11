using Microsoft.EntityFrameworkCore;
using System;

namespace NorthwindApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connection = String.Empty;
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.json");
                connection = builder.Configuration.GetConnectionString("SqlConnString");
            }
            else
            {
                connection = Environment.GetEnvironmentVariable("SqlConnString");
            }

            builder.Services.AddDbContext<PersonDbContext>(options =>
                options.UseSqlServer(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                }).ToArray();

                return forecast;
            }).WithName("GetWeatherForecast");

            app.MapGet("/Person", (PersonDbContext context) =>
            {
                return context.Person.ToList();
            }).WithName("GetPersons")/*.WithOpenApi()*/;

            app.MapPost("/Person", (Person person, PersonDbContext context) =>
            {
                context.Add(person);
                context.SaveChanges();
            }).WithName("CreatePerson")/*.WithOpenApi()*/;

            app.MapPut("/Person/{id}", (int id, Person person, PersonDbContext context) =>
            {
                context.Update(person);
                context.SaveChanges();
            }).WithName("UpdatePerson");

            app.Run();
        }
    }
}