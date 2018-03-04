using BoxCastAPIWrapper.Interfaces;
using BoxCastAPIWrapper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BoxCastAPIWrapper
{
    public class BoxCast
    {
        private static string clientId = "";
        private static string secret = "";
        public static string oAuthTokenEndPoint = "https://login.boxcast.com/oauth2/token";
        public static string apiEndPoint = "https://api.boxcast.com/";
        public static string apiVersionHeader = "";

        private static TokenModel token;

        public static bool HasToken { get { return token != null; } }


        /// <summary>
        /// Set the client id and secret for communicating with the API
        /// </summary>
        /// <param name="apiClientId"></param>
        /// <param name="apiSecret"></param>
        public static void SetAPICredentials(string apiClientId, string apiSecret)
        {
            clientId = apiClientId;
            secret = apiSecret;
        }


        /// <summary>
        /// Authenticate with BoxCast using Password Grant Flow
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task Authenticate(string username, string password)
        {
            var authModel = new PasswordAuthenticationModel() { Username = username, Password = password };
            token = await getToken(authModel);
        }


        /// <summary>
        /// Authenticate with BoxCast using Authorization Code Grant Flow
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public static async Task Authenticate(string authCode)
        {
            var authModel = new AuthCodeAuthenticationModel() { Code = authCode };
            token = await getToken(authModel);
        }

        /// <summary>
        /// Clear the oAuth token
        /// </summary>
        public static void ClearToken()
        {
            token = null;
        }


        /// <summary>
        /// Set up the client for making requests
        /// </summary>
        /// <param name="client"></param>
        private static void initializeClient(HttpClient client)
        {
            client.BaseAddress = new Uri(apiEndPoint);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            if (apiVersionHeader != "")
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(apiVersionHeader));
            }

        }

        /// <summary>
        /// Handle responses for all HTTP requests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private static async Task<BoxCastResponse<T>> handleResponse<T>(HttpResponseMessage httpResponse)
        {            
            if (httpResponse.IsSuccessStatusCode)
            {
                return await BoxCastResponse<T>.CreateResponse(httpResponse);
            }
            else
            {
                BoxCastResponse<ErrorModel> errorResponse = null;

                try
                {
                    errorResponse = await BoxCastResponse<ErrorModel>.CreateResponse(httpResponse);                    
                }
                catch
                {
                    throw new WebException((int)httpResponse.StatusCode + " - " + httpResponse.ReasonPhrase);
                }

                throw new BoxCastAPIException(errorResponse);
            }
        }

        /// <summary>
        /// Submit a GET request to the API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<BoxCastResponse<T>> Get<T>(BoxCastRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                initializeClient(client);

                if(request.ETag?.Length > 0)
                {
                    client.DefaultRequestHeaders.IfNoneMatch.Add(new EntityTagHeaderValue(request.ETag));
                }

                HttpResponseMessage response = await client.GetAsync(request.ToString());
                return await handleResponse<T>(response);
            }
        }

        /// <summary>
        /// Submit a POST request to the API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<BoxCastResponse<T>> Post<T>(string resource, object content)
        {
            using (HttpClient client = new HttpClient())
            {
                initializeClient(client);

                string json = JsonConvert.SerializeObject(content);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(resource, requestContent);
                return await handleResponse<T>(response);
            }
        }

        /// <summary>
        /// Submit a PUT request to the API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="content"></param>
        /// <param name="eTag"></param>
        /// <returns></returns>
        public static async Task<BoxCastResponse<T>> Put<T>(string resource, object content, string eTag = "")
        {
            using (HttpClient client = new HttpClient())
            {
                initializeClient(client);

                if (eTag?.Length > 0)
                {
                    client.DefaultRequestHeaders.IfMatch.Add(new EntityTagHeaderValue(eTag));
                }

                string json = JsonConvert.SerializeObject(content);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(resource, requestContent);
                return await handleResponse<T>(response);
            }
        }

        /// <summary>
        /// Submit a DELETE request to the API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<BoxCastResponse<T>> Delete<T>(string resource)
        {
            using (HttpClient client = new HttpClient())
            {
                initializeClient(client);

                HttpResponseMessage response = await client.DeleteAsync(resource);
                return await handleResponse<T>(response);
            }
        }


        /// <summary>
        /// Get an oAuth token for a given user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static async Task<TokenModel> getToken(IAuthenticationModel authModel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(clientId + ":" + secret));


                string requestJson = authModel.Serialize();
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(new Uri(oAuthTokenEndPoint), content);
                return (await handleResponse<TokenModel>(response)).Content;
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }

}
