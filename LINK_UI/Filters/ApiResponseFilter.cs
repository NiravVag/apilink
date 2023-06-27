using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;

namespace LINK_UI.Filters
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return "Bad Request";
                default:
                    return null;
            }
        }
    }
    public class ApiBadRequestResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration = null;

        public ApiBadRequestResponse(ModelStateDictionary modelState,
            IHostingEnvironment env, 
            IConfiguration configuration,
            string lang
            )
            : base(400)
        {
            _env = env;
            _configuration = configuration;
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }
            try
            {

           
            var basePath = env.WebRootPath;
            var translateFolderPath = _configuration["TranslateFolderPath"];

            var filePath = string.Concat(basePath, translateFolderPath, lang+".json");
            string json = "";
            Dictionary<string, JObject> module = null;
            using (StreamReader r = new StreamReader(filePath))
            {
                  json = r.ReadToEnd();
                 module = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
            }

                Errors = modelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                var keyError = modelState.Select(x => x.Key).ToArray();

                Errors = Errors.Select(x => GetTranslateValue(module, x, keyError));
            }
            catch (Exception)
            {

                throw new ArgumentException("Invalid ApiBadRequestResponse", nameof(modelState));
            }
        }
        protected static string GetTranslateValue (Dictionary<string, JObject> module, string searchLabel, string[] keyError)
        {
            string translateValue = searchLabel;
            if (!string.IsNullOrEmpty(searchLabel) && searchLabel.Contains(".")){
                string[] moduleLabel = searchLabel.Split(".");
                if (module.Keys.Contains(moduleLabel[0]))
                {
                    JObject labels = module[moduleLabel[0]];
                    if (!string.IsNullOrEmpty(Convert.ToString(labels[moduleLabel[1]])))
                        translateValue = Convert.ToString(labels[moduleLabel[1]]);
                }
            }
            return translateValue;
        }
    }
}
 
