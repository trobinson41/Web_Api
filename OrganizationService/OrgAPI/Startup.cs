using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrgDAL;

namespace OrgAPI
{
    public class Startup
    {
        //IConfiguration configuration;

        //public Startup(IConfiguration _configuration)
        //{
        //    configuration = _configuration;
        //}
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(
            //    x => x.AddPolicy("myPolicy",
            //    p => p.AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .WithOrigins("")
            //        .AllowCredentials()
            //    ));

            //services.AddMvc(
            //    config => config.Filters.Add(new MyExceptionFilter())
            //    );
            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
                //x.Filters.Add(new AuthorizeFilter());
            }).AddXmlSerializerFormatters()
                              .AddXmlDataContractSerializerFormatters();
            services.AddDbContext<OrganizationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<OrganizationDbContext>()
                    .AddDefaultTokenProviders();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));
            var tokenValidationParameter = new TokenValidationParameters()
            {
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                //ValidateLifetime=false,
                //ClockSkew=TimeSpan.Zero
            };

            services.AddAuthentication(x => x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwt =>
                    {
                        //jwt.SaveToken = true;
                        //jwt.RequireHttpsMetadata = true;
                        jwt.TokenValidationParameters = tokenValidationParameter;
                    }
                    );

            //services.ConfigureApplicationCookie(opt =>
            //           {
            //               opt.Events = new CookieAuthenticationEvents
            //               {                               
            //                   OnRedirectToLogin = redirectContext =>
            //                   {
            //                       redirectContext.HttpContext.Response.StatusCode = 401;
            //                       return Task.CompletedTask;
            //                   },
            //                   OnRedirectToAccessDenied = redirectContext =>
            //                   {
            //                       redirectContext.HttpContext.Response.StatusCode = 401;
            //                       return Task.CompletedTask;
            //                   }
            //               };
            //           });
            //var conStr = configuration.GetConnectionString("OrgConStr");            
            //services.AddDbContext<OrganizationDbContext>(option => option.UseSqlServer(conStr));

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseExceptionHandler(
                           options =>
                           {
                               options.Run(async context =>
                               {

                                   context.Response.StatusCode = 500;//Internal Server Error
                                   context.Response.ContentType = "application/json";
                                   await context.Response.WriteAsync("We are woirking on it");
                                   //var ex = context.Features.Get<IExceptionHandlerFeature>();
                                   //if (ex != null)
                                   //{
                                   //    await context.Response.WriteAsync(ex.Error.Message);
                                   //}
                               });
                           }
                           );
            //app.UseCors("myPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
