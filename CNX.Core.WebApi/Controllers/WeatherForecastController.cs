using CNX.Core.Extensions;
using CNX.Core.Extensions.JWT;
using CNX.Core.Extensions.JWT.Model;
using CNX.Core.IServices;
using CNX.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CNX.Core.WebApi.Controllers
{
    /// <summary>
    /// 天气管理
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISampleService _sampleService;
        private readonly JWTTokenBuilder _jWTTokenBuilder;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISampleService sampleService, JWTTokenBuilder jWTTokenBuilder)
        {
            _logger = logger;
            _sampleService = sampleService;
            _jWTTokenBuilder = jWTTokenBuilder;
        }
        


        
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        /// <param name="name">名字</param>
        [Authorize(Policy = "Admin")]
        [HttpGet("GetInfo")]
        public async Task<SampleInfo> Get(int id)
        {


           return await _sampleService.GetSampleById(id);
            //var listSample=  _repository.FindList<SampleInfo>().ToList();
            


            
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }





        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="configuration">依赖</param>
        /// <returns></returns>
        [HttpGet("GetToken")]
        public ApiResult GetToken([FromServices] IConfiguration configuration)
        {
            JwtParams jwtParams = configuration.GetSection("JwtSetting").Get<JwtParams>();
            DateTime expiresAt = DateTime.Now.AddDays(jwtParams.ExpireDays)
                                    .AddHours(jwtParams.ExpireHours)
                                    .AddMinutes(jwtParams.ExpireMinutes)
                                    .AddSeconds(jwtParams.ExpireSeconds);

            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, "CNX"),
                    new Claim(JwtRegisteredClaimNames.Sid,"2"),
                    new Claim(ClaimTypes.Expiration, expiresAt.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("AvatarUrl","3")
            };
            var token = _jWTTokenBuilder.BuildJwtToken(claims.ToArray(), jwtParams);
            return new ApiResult(token);
        }





    }
}
