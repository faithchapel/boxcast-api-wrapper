using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public class ErrorModel : Serializable
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }

        [JsonProperty(PropertyName = "invalid_fields")]
        public Dictionary<string, string> InvalidFields { get; set; }
    }
}
