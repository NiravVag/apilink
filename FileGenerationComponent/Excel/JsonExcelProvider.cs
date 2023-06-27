using Components.Core.entities;
using FileGenerationComponent.PPT;
using FileGenerationComponent.SourceProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileGenerationComponent.Excel
{
    public class JsonExcelProvider : JsonProvider
    {
        public override FileObject GetFileObject(object source)
        {
            var request = source as JsonRequestModel;

            if (request == null)
                throw new Exception("cannot parse object source to JsonRequestModel");

            return GetObjectFromJsonRequest(request);
        }

        private FileObject GetObjectFromJsonRequest(JsonRequestModel request)
        {
            string path = request.JsonPath;

            if (string.IsNullOrEmpty(path))
                throw new Exception("Cannot find the file path");

            if (!path.EndsWith(".json"))
                path = path + ".json";

            if (!System.IO.File.Exists(path))
                throw new Exception("Cannot find json file for configuration");

            string textJson = System.IO.File.ReadAllText(path);

            var objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigurationFile>(textJson);

            if (objJson == null)
                throw new Exception("Cannot parse JSON FILE");

            var dataSource = GetDataSource(objJson.ConnectionStringName, objJson.SourceText, request.Model);
           
            return new ExcelJsonFileObject
            {
                DataSource = dataSource,
                ModelRequest = objJson,
              //  ModelPath = path.Replace(".json", ".xlsx"),
                VariableList = GetVariableList(objJson.Variables, dataSource),
            };
  
        }
    }
}
