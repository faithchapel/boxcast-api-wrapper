using BoxCastAPIWrapper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper
{
    public class BoxCastResponse<T>
    {
        public string ETag { get; set; }
        public PaginationModel Pagination { get; set; } = new PaginationModel();
        public RateLimitModel RateLimit { get; set; } = new RateLimitModel();
        public Dictionary<string, IEnumerable<string>> Headers { get; set; } = new Dictionary<string, IEnumerable<string>>();
        public HttpStatusCode ResponseCode;
        public string ResponsePhrase;
        public T Content { get; set; }


        /// <summary>
        /// Create a BoxCastResponse based on the given HttpResponseMessage
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public static async Task<BoxCastResponse<T>> CreateResponse(HttpResponseMessage httpResponse)
        {

            var boxCastResponse = new BoxCastResponse<T>();
            string json = await httpResponse.Content.ReadAsStringAsync();

            //populate consistent response data
            boxCastResponse.ETag = httpResponse.Headers.ETag?.Tag;
            boxCastResponse.ResponseCode = httpResponse.StatusCode;
            boxCastResponse.ResponsePhrase = httpResponse.ReasonPhrase;

            IEnumerable<string> headerValues = null;

            httpResponse.Headers.TryGetValues("X-RateLimit-Limit", out headerValues);
            if (headerValues?.First() != null)
            {
                boxCastResponse.RateLimit.Limit = Convert.ToInt32(headerValues.First());
            }

            httpResponse.Headers.TryGetValues("X-RateLimit-Remaining", out headerValues);
            if (headerValues?.First() != null)
            {
                boxCastResponse.RateLimit.Remaining = Convert.ToInt32(headerValues.First());
            }

            httpResponse.Headers.TryGetValues("X-RateLimit-Reset", out headerValues);
            if (headerValues?.First() != null)
            {
                DateTime reset;
                DateTime.TryParse(headerValues.First(), out reset);
                boxCastResponse.RateLimit.Reset = reset;
            }

            httpResponse.Headers.TryGetValues("X-Pagination", out headerValues);
            if(headerValues?.First() != null)
            {
                string paginationJson = headerValues.First();
                boxCastResponse.Pagination = JsonConvert.DeserializeObject<PaginationModel>(paginationJson);
            }

            //dump all header data into the headers dictionary.
            boxCastResponse.Headers = httpResponse.Headers.ToDictionary(x => x.Key, x => x.Value);


            //deserialize json content
            boxCastResponse.Content = JsonConvert.DeserializeObject<T>(json);

            return boxCastResponse;
        }
    }
}
