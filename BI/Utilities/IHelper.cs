using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI.Utilities
{
    public interface IHelper
    {

        Task<HttpResponseMessage> SendRequestToPartnerAPI(Method method, string request, object jsonvalues, string baseUrl, string token = "", bool isBearer = true);
        string FromDictionaryToJson(Dictionary<string, string> dictionary);
        QueryString CreateQueryStringObject(QueryString str, string filterQuery);
        Task<HttpResponseMessage> UploadDocument(MultipartFormDataContent form, string baseUrl, string requestUrl, string userToken);
        DataTable ConvertToDataTable<T>(List<T> items);
        DataTable ConvertToDataTableWithCaption<T>(List<T> items);
        DataTable ConvertToDataTableWithCaptionAndDynamiColumns<T>(List<T> items, int columnIndex, List<string> dynamicColumns);

        DataTable RemoveCloumnToDataTable(DataTable dataTable, List<string> removedColumns);
    }
}
