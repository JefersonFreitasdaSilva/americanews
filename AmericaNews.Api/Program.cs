using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Services;
using AmericaNews.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;

namespace AmericaNews.Api
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

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<INoticiaRepository, NoticiaRepository>();
            builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
            builder.Services.AddScoped<IRegistroRepository, RegistroRepository>();

            builder.Services.AddScoped<SHA256>(provider => SHA256.Create());

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<INoticiaService, NoticiaService>();
            builder.Services.AddScoped<IComentarioService, ComentarioService>();
            builder.Services.AddScoped<IRegistroService, RegistroService>();

            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmericaNewsApi", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmericaNewsApi");
                });
            }

            //app cors
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseRouting();
            app.UseCors("corsapp");

            app.Run();
        }
    }
}
