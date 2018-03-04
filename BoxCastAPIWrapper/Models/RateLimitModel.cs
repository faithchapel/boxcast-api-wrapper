using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public class RateLimitModel
    {
        public int? Limit { get; set; }
        public int? Remaining { get; set; }
        public DateTime? Reset { get; set; }
    }
}
