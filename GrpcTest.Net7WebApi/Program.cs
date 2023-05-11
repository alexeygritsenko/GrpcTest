using Microsoft.AspNetCore.Builder;
using GrpcTest.Clients;
using Microsoft.Extensions.DependencyInjection;
using GrpcTest.Translator;
using Microsoft.Extensions.Hosting;

namespace GrpcTest.Net7WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMicroserviceClient<GrpcTestTranslator.GrpcTestTranslatorClient>("Certificates\\ca.crt", "localhost", 5002);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}