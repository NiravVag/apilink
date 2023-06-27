using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BI.Utilities
{
    public interface IGenericAPI
    {
        Task<dynamic> GetRequest(string uri, string token);

        //Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content, string token);

        Task<dynamic> PostRequest(string uri, dynamic content, string token);
    }
}
