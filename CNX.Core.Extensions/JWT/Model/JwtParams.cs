using System;
using System.Collections.Generic;
using System.Text;

namespace CNX.Core.Extensions.JWT.Model
{
   public class JwtParams
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }

        public int ExpireDays { get; set; } = 0;

        public int ExpireHours { get; set; } = 0;

        public int ExpireMinutes { get; set; } = 0;

        public int ExpireSeconds { get; set; } = 0;
    }
}
