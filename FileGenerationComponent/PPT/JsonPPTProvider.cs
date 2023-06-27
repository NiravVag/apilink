using Components.Core.entities;
using FileGenerationComponent.SourceProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FileGenerationComponent.PPT
{
    public class JsonPPTProvider : JsonProvider
    {
        public override FileObject GetFileObject(object source)
        {
            var request = source as JsonRequestModel;

            if (request == null)
                throw new Exception("cannot parse object source to JsonRequestModel");

           

            string path = request.JsonPath;

            if (string.IsNullOrEmpty(path))
                throw new Exception("Cannot find the file path");

            if (!path.EndsWith(".json"))
                path = path + ".json";

            if (!System.IO.File.Exists(path))
                throw new Exception("Cannot find json file for configuration");

            _onLog?.Invoke(12, "reading Json file");
            string textJson = System.IO.File.ReadAllText(path);

            _onLog?.Invoke(14, "parsing Json file to object");
            var objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigurationFile>(textJson); 

            if(objJson == null)
                throw new Exception("Cannot parse JSON FILE");

            _onLog?.Invoke(15, "BEGIN Get data from database");
            var dataSource = GetDataSource(objJson.ConnectionStringName, objJson.SourceText,request.Model);


            if(string.IsNullOrEmpty(objJson.ModelPath))
                   objJson.ModelPath = path.Replace(".json", ".pptx");

            _onLog?.Invoke(18, "END Get data from database");

            return new PPTObject
            {
                DataSource = dataSource,
                FileName = objJson.FileNameExt,
                VariableList = GetVariableList(objJson.Variables, dataSource),
                ModelPath = objJson.ModelPath
            };
        }



        public static string GetAvg<T>(System.Data.DataTable dataTable, string columnName, string ColumnTotalName) where T : struct
        {
            T total1 = default(T);
            T total2 = default(T);

            foreach (DataRow row in dataTable.Rows)
            {
                if (row[columnName] != DBNull.Value)
                    total1 = Add(total1, (T)row[columnName]);

                if (row[ColumnTotalName] != DBNull.Value)
                    try
                    {
                        total2 = Add(total2, (T)row[ColumnTotalName]);
                    }                       
                    catch(InvalidCastException exception)
                    {
         
                        total2 = Add(total2, (T)Convert.ChangeType(row[ColumnTotalName], typeof(T)));
                    }
                        
            }

            // return (Convert.ToInt32(total1 ) * 100 / Convert.ToInt32(total2)).ToString() ;
            return Convert.ToInt32(Math.Round(Convert.ToDouble(total1) * 100 / Convert.ToDouble(total2),
                    MidpointRounding.AwayFromZero)).ToString();
        }

        public static string GetAvgByGroup<T>(System.Data.DataTable dataTable, string columnName, string ColumnTotalName, string columnGroupName, string field) where T : struct
        {
            T total1 = default(T);
            T total2 = default(T);

            foreach (DataRow row in dataTable.Rows)
            {

                if (row[columnName] != DBNull.Value && row[columnGroupName] != DBNull.Value && row[columnGroupName].ToString() == field)
                    total1 = Add(total1, (T)row[columnName]);

                if (row[ColumnTotalName] != DBNull.Value && row[columnGroupName] != DBNull.Value && row[columnGroupName].ToString() == field)
                    total2 = Add(total2, (T)row[ColumnTotalName]);

            }

            return
                Convert.ToInt32(Math.Round(Convert.ToDouble(total1) * 100 / Convert.ToDouble(total2),
                    MidpointRounding.AwayFromZero)).ToString();
            //return (Convert.ToInt32(total1) * 100 / Convert.ToInt32(total2)).ToString();
        }
          
    }
}
