using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_API_Escuela.Filters;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.


        public void ConfigureServices(IServiceCollection services)
        {
            //Configuramos el servicio automapper
            services.AddAutoMapper(typeof(Startup));

            //Para el servicio almacenar archivos de manera local
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            services.AddHttpContextAccessor();


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"), builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });

            });

            //services.AddControllers();

            //Registra el log de errores como filtro global
            services.AddControllers(options =>
           {
               options.Filters.Add(typeof(ExceptionFilter));
           });

            //Desarrollo

            services.AddCors(options => options.AddDefaultPolicy(buider =>
            {
                buider.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });

            }));

            //Producci�n
            //services.AddCors(options =>
            //{
            //    var frontend_url = Configuration.GetValue<string>("frontend_url");
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.WithOrigins(frontend_url).AllowAnyMethod().AllowAnyHeader()
            //        .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
            //    });

            //});

            //Agregar autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
            opciones.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true, //valida el tiempo de vida
                ValidateIssuerSigningKey = true, //valida la firma con la llave privada
                IssuerSigningKey = new SymmetricSecurityKey( //configuramos la llave
                Encoding.UTF8.GetBytes(Configuration["keyjwt"])),
                ClockSkew = TimeSpan.Zero //para no tener problemas con diferencias de tiempo al calcular que el token ha vencido.

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web_API_Escuela", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web_API_Escuela v1"));
            }

            app.UseHttpsRedirection();

            //Permite servir archivos estaticos
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            //para autorizarte primero tienes que autorizarte, as� que va antes este middlware
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
