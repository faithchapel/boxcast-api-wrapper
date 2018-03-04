using BoxCastAPIWrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper
{
    public class BoxCastAPIException : Exception
    {
        public BoxCastResponse<ErrorModel> BoxCastError { get; set; }

        public BoxCastAPIException(BoxCastResponse<ErrorModel> error)
        {
            BoxCastError = error;
        }
    }
}
