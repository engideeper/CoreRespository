using CNX.Core.Extensions.JWT;
using CNX.Core.Extensions.JWT.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 系统 授权服务 配置
    /// </summary>
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            #region 1、基于角色的API授权

            // 1、这个很简单，其他什么都不用做， 只需要在API层的controller上边，增加特性即可
            // [Authorize(Roles = "Admin,System")]
            #endregion 基于角色的API授权


            #region 2、基于策略的授权（简单版）
            // 2、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            #endregion

            #region 参数

            var jwtSetting = Configuration.GetSection("JwtSetting").Get<JwtParams>();
            if (jwtSetting == null)
            {
                throw new ArgumentNullException(nameof(JwtParams));
            }
            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey)),
                ValidateIssuer = true,
                //发行人
                ValidIssuer = jwtSetting.Issuer,
                ValidateAudience = true,
                //订阅人
                ValidAudience = jwtSetting.Audience,
                ValidateLifetime = true,
                //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                //ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            //2.1【认证】、core自带官方JWT认证
            // 开启Bearer认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(APIResponseHandler);
                o.DefaultForbidScheme = nameof(APIResponseHandler);
            })             // 添加JwtBearer服务
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnChallenge = context =>
                     {
                         context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                         var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                         if (jwtToken.Issuer != jwtSetting.Issuer)
                         {
                             context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                         }

                         if (jwtToken.Audiences.FirstOrDefault() != jwtSetting.Audience)
                         {
                             context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                         }
                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             })
             .AddScheme<AuthenticationSchemeOptions, APIResponseHandler>(nameof(APIResponseHandler), o => { });

            #endregion



        }
    }
}
