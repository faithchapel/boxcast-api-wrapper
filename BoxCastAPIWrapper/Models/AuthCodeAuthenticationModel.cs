﻿using BoxCastAPIWrapper.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public class AuthCodeAuthenticationModel : Serializable, IAuthenticationModel
    {
        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; } = "authorization_code";
        
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var d = new Dictionary<string, string>();

            d.Add("grant_type", GrantType);
            d.Add("code", Code);

            return d;
        }
    }
}
