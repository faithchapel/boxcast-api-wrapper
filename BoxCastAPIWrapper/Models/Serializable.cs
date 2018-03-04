using BoxCastAPIWrapper.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public abstract class Serializable : ISerializable
    {
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
