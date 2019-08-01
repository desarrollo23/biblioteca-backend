using System;
using System.Text;
using Biblioteca.DALC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Biblioteca
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
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration["Correo"],
						ValidAudience = Configuration["Correo"],
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
						ClockSkew = TimeSpan.Zero
					};
				});

			services.AddSingleton(Configuration);

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			var connection = Configuration["DataBase:ConnectionString"];

			services.AddDbContext<BibliotecaContext>
				(options => options.UseSqlServer(connection));

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => {
						builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();

						builder.WithOrigins("http://localhost:8080");
					});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddFile("Logs/Log-{Date}.txt");
			app.UseCors("CorsPolicy");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});
		}
	}
}
