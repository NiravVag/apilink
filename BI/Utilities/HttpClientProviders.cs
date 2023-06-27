using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BI.Utilities
{
    public class HttpClientProviders : IHttpClientProviders
    {
        public HttpResponseMessage PostJsonAsync(HttpClient client, string request, HttpContent jsonObj)
        {
            return client.PostAsync(request, jsonObj).Result;
        }
    }
}
