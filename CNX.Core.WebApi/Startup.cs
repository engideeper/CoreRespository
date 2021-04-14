using Autofac;
using Blog.Core.Extensions;
using CNX.Core.Extensions;
using CNX.Core.Extensions.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CNX.Core.Extensions.Swagger.CustomApiVersion;

namespace CNX.Core.WebApi
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
            services.AddDbContext<DbContextBase>(options =>
            {
                var connectionString = Configuration["ConnectionStrings:Sqlserver"];
                options.UseSqlServer(connectionString);
            });

            services.AddControllers();
            // services.AddScoped<JWTTokenBuilder, JWTTokenBuilder>();

            services.AddSwaggerGen(c =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"CNX.Core.WebApi接口文档",
                        Description = $"CNX.Core.WebApi-- HTTP API " + version,
                        Contact = new OpenApiContact (){ Name = "CNX.Core.WebApi",Url=new Uri("https://www.TCsoft.com"), Email = "" },
                        //License = new OpenApiLicense { Name = "CNX.Core.WebApi" + " 官方文档" }
                    });
                    c.IncludeXmlComments($"{Path.Combine(AppContext.BaseDirectory,"CNX.Core.WebApi.xml")}",true);
                    c.OrderActionsBy(o => o.RelativePath);
                });



                #region Swagger使用鉴权组件
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                 {
                    new OpenApiSecurityScheme
                     {
                       Reference=new OpenApiReference
                       {
                         Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                      },
                     new string[] {}
                  }
                 });
                #endregion


            });


            # region 扩展服务
            services.AddAuthorizationSetup(Configuration);
            #endregion

        }




        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(
                c => {
                    typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{"CNX.Core.WebApi"} {version}");
                    });
                    c.RoutePrefix = "";
                });


            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
