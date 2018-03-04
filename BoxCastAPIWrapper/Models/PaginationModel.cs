using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper.Models
{
    public class PaginationModel
    {
        public int First { get; set; }
        public int Previous { get; set; }
        public int Next { get; set; }
        public int Last { get; set; }
        public int Total { get; set; }
    }
}
