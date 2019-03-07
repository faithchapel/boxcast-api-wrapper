using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Interfaces
{
    public interface IAuthenticationModel : ISerializable
    {
        [JsonProperty(PropertyName = "grant_type")]
        string GrantType { get; set; }

        Dictionary<string, string> ToDictionary();
    }
}
