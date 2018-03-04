using BoxCastAPIWrapper.Interfaces;
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

    }
}
