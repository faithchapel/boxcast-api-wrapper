using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper
{
    public class BoxCastRequest
    {
        public string Resource { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSizeLimit { get; set; }

        public string SortBy { get; set; }

        public bool SortDescending { get; set; }

        public string Search { get; set; }

        public string ETag { get; set; }

        /// <summary>
        /// Used for supplying query string parameters that are either undocumented or were added since the api documentation this wrapper was based upon.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; set; } = new Dictionary<string, string>();

        public override string ToString()
        {

            string request = Resource;

            string queryString = "";

            if(PageNumber != null)
            {
                queryString = appendQueryString(queryString, "p=" + PageNumber);
            }

            if (PageSizeLimit != null)
            {
                queryString = appendQueryString(queryString, "l=" + PageSizeLimit);
            }

            if (SortBy?.Length > 0)
            {
                string order = "";
                if (SortDescending)
                {
                    order = "-";
                }
                queryString = appendQueryString(queryString, "s=" + order + SortBy);
            }

            if (Search?.Length > 0)
            {
                queryString = appendQueryString(queryString, "q=" + Search);
            }


            if(QueryParameters.Count > 0)
            {
                foreach(var key in QueryParameters.Keys)
                {
                    queryString = appendQueryString(queryString, key + "=" + QueryParameters[key]);
                }
            }

            return request + queryString;

        }

        private string appendQueryString(string queryString, string value)
        {
            if(queryString.Length > 0)
            {
                return  queryString + "&" + value;
            }
            else
            {
                return "?" + value;
            }
        }

    }
}
