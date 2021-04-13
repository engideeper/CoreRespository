using CNX.Core.Extensions.JWT.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CNX.Core.Extensions.JWT
{
   public class JWTTokenBuilder
    {
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="jwtSetting"></param>
        /// <returns></returns>
        public virtual string BuildJwtToken(Claim[] claims, JwtParams jwtParams)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 实例化JwtSecurityToken
            var jwtToken = new JwtSecurityToken(
                issuer: jwtParams.Issuer,
                audience: jwtParams.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(jwtParams.ExpireDays)
                                     .AddHours(jwtParams.ExpireHours)
                                     .AddMinutes(jwtParams.ExpireMinutes)
                                     .AddSeconds(jwtParams.ExpireSeconds),
                signingCredentials: creds
            );
            // 生成 Token
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

    }
}
