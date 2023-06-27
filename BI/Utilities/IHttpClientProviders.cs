using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI.Utilities
{
    public interface IHttpClientProviders
    {     
        HttpResponseMessage PostJsonAsync(HttpClient client, string request, HttpContent jsonObj);
    }
}
