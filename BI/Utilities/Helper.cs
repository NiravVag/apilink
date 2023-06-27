using DTO.TCF;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BI.Utilities
{
    public class Helper : IHelper
    {
        private readonly IHttpClientProviders _httpClientProvider;
        public Helper(IHttpClientProviders httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }
        public async Task<HttpResponseMessage> SendRequestToPartnerAPI(Method method, string request, object jsonvalues, string baseUrl, string token = "", bool isBearer = true)
        {
            var jsonObj = jsonvalues;

            HttpResponseMessage response = new HttpResponseMessage();
            var url = baseUrl;
            Console.WriteLine($"Base Url is: {url} , request {request}");
            HttpClient client = new HttpClient();
            HttpContent content = null;
            if (!string.IsNullOrEmpty(token))
                client = this.GetHttpClient(token, isBearer);
            if (!string.IsNullOrEmpty(url))
            {
                client.BaseAddress = new Uri(url);
            }
            client.DefaultRequestHeaders.Accept.Clear();

            if (jsonObj != null)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            switch (method)
            {
                case Method.Delete:
                    response = client.DeleteAsync(request).Result;
                    break;
                case Method.Get:
                    response = client.GetAsync(request).Result;
                    break;
                case Method.Send:
                    {
                        var requestMessage = new HttpRequestMessage
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(baseUrl + "/" + request),
                            Content = new StringContent(JsonConvert.SerializeObject(jsonvalues), Encoding.UTF8, "application/json")
                        };
                        response = client.SendAsync(requestMessage).Result;
                    }
                    break;
                case Method.Post:
                    {
                        var stringContent = this.GetStringContent(jsonObj);
                        try
                        {
                            response = client.PostAsync(request, stringContent).Result;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        break;
                    }

                case Method.PostForm:
                    {
                        string jsonString = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                        var serializeobject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                        var stringContent = new FormUrlEncodedContent(serializeobject);
                        try
                        {
                            response = client.PostAsync(request, stringContent).Result;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        break;
                    }

                case Method.Patch:
                    {
                        var stringContent = this.GetStringContent(jsonObj);
                        try
                        {
                            response = client.PatchAsync(request, stringContent).Result;
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                        break;
                    }
                case Method.JSONPut:
                    {
                        var stringContent = this.GetStringContent(jsonObj);
                        try
                        {
                            response = client.PutAsync(request, stringContent).Result;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        break;
                    }

            }

            return response;
        }
        public string FromDictionaryToJson(Dictionary<string, string> dictionary)
        {
            var kvs = dictionary.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, string.Join(",", kvp.Value)));
            return string.Concat("{", string.Join(",", kvs), "}");
        }

        private StringContent GetStringContent(object content)
        {
            if (content == null) return null;
            string jsonString = JsonConvert.SerializeObject(content, Formatting.Indented);

            return new StringContent(
                jsonString,
                 Encoding.UTF8,
                 "application/json");
        }
        private HttpClient GetHttpClient(string token, bool isBearer, string contentType = "application/json")
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            if (isBearer)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            else
                _client.DefaultRequestHeaders.Add("Authorization", token);
            return _client;
        }


        public QueryString CreateQueryStringObject(QueryString str, string filterQuery)
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>() { };
            var requestQueryString = new QueryString(str.Value);
            var requestKeyValuePairs = HttpUtility.ParseQueryString(requestQueryString.ToString());
            bool isAddQueryFilter = true;
            foreach (string queryName in requestKeyValuePairs.AllKeys)
            {
                if (queryName == "$filter")
                {
                    isAddQueryFilter = false;
                    string filterQueryValues = requestKeyValuePairs.Get(queryName);
                    filterQuery += " and ( " + filterQueryValues + " )";
                    keyValuePairs.Add(new KeyValuePair<string, string>(queryName, filterQuery.ToString()));
                    continue;
                }
                keyValuePairs.Add(new KeyValuePair<string, string>(queryName, requestKeyValuePairs.Get(queryName)));
            }
            if (isAddQueryFilter)
                keyValuePairs.Add(new KeyValuePair<string, string>("$filter", filterQuery));

            QueryString queryString = QueryString.Create(keyValuePairs);
            return queryString;
        }

        /// <summary>
        /// Upload the document with form data
        /// </summary>
        /// <param name="form"></param>
        /// <param name="baseUrl"></param>
        /// <param name="requestUrl"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> UploadDocument(MultipartFormDataContent form, string baseUrl, string requestUrl, string userToken)
        {
            HttpResponseMessage response;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", userToken);
            response = await client.PostAsync(requestUrl, form);

            return response;
        }

        /// <summary>
        /// Convert list to datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);

                dataTable.Columns.Add(prop.Name, type);

            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public DataTable ConvertToDataTableWithCaptionAndDynamiColumns<T>(List<T> items, int columnIndex, List<string> dynamicColumns)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int actualColumnIndex = 0;
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);

                DataColumn column = dataTable.Columns.Add(prop.Name, type);
                //DataColumn column = dataTable.Columns.Add(prop.Name, typeof(string));

                if (prop.GetCustomAttributes().Any())
                {
                    var caption = ((System.ComponentModel.DescriptionAttribute)((System.Attribute[])prop.GetCustomAttributes())[0])?.Description;

                    if (!string.IsNullOrEmpty(caption))
                    {
                        column.Caption = caption;
                        // check column index or dynamic columns available then inject 
                        if (columnIndex > 0 && dynamicColumns.Any() && actualColumnIndex == columnIndex)
                        {
                            foreach (var columnHeader in dynamicColumns)
                            {
                                dataTable.Columns.Add(columnHeader, typeof(string));
                            }
                        }
                        actualColumnIndex = actualColumnIndex + 1;
                    }
                }
            }

            foreach (T item in items)
            {
                DataRow dataRow = dataTable.NewRow();

                if (dynamicColumns.Any())
                {
                    foreach (var columnHeader in dynamicColumns)
                    {
                        dataRow[columnHeader] = string.Empty;
                    }
                }

                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(item, null) != null)
                    {
                        dataRow[prop.Name] = prop.GetValue(item, null);
                    }
                    //dataRow[prop.Name] = prop.GetValue(item, null) != null ? prop.GetValue(item, null).ToString() : string.Empty;
                }

                dataTable.Rows.Add(dataRow);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        public DataTable ConvertToDataTableWithCaption<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);

                DataColumn column = dataTable.Columns.Add(prop.Name, type);

                if (prop.GetCustomAttributes().Any())
                {
                    var caption = ((System.ComponentModel.DescriptionAttribute)((System.Attribute[])prop.GetCustomAttributes())[0])?.Description;

                    if (!string.IsNullOrEmpty(caption))
                    {
                        column.Caption = caption;
                    }
                }
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public DataTable RemoveCloumnToDataTable(DataTable dataTable, List<string> removedColumns)
        {
            // removed columns 
            foreach (var column in removedColumns)
            {
                dataTable.Columns.Remove(column);
            }
            return dataTable;
        }
    }
}
