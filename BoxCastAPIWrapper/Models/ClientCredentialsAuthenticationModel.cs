using BoxCastAPIWrapper.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public class ClientCredentialsAuthenticationModel : Serializable, IAuthenticationModel
    {
        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        public override string ToString()
        {
            return string.Format("grant_type={0}&scope={1}", GrantType, Scope);
        }

        public Dictionary<string, string> ToDictionary()
        {
            var d = new Dictionary<string, string>();

            d.Add("grant_type", GrantType);
            d.Add("scope", Scope);

            return d;
        }
    }
}
