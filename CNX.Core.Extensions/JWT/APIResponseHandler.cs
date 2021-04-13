using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CNX.Core.Extensions.JWT
{
    public class APIResponseHandler:AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private JsonSerializerSettings setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public APIResponseHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            //await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult<string>(null, statusCode:StatusCodes.Status401Unauthorized ,success:false, msg: "很抱歉，您无权访问该接口，请确保已经登录!"), setting));
            await Response.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult<string>(msg: "很抱歉，您无权访问该接口，请确保已经登录!", statusCode: StatusCodes.Status401Unauthorized), setting));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult<string>(msg: "很抱歉，您的访问权限等级不够，联系管理员!", statusCode: StatusCodes.Status403Forbidden), setting));
        }

    }
}
