using BoxCastAPIWrapper.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public class PasswordAuthenticationModel : Serializable, IAuthenticationModel
    {
        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; } = "password";

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var d = new Dictionary<string, string>();

            d.Add("grant_type", GrantType);
            d.Add("username", Username);
            d.Add("password", Password);

            return d;
        }
    }
}
