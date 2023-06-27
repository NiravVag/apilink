using Components.Core.entities;
using FileGenerationComponent.Excel;
using FileGenerationComponent.PPT;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;
namespace FileGenerationComponent.SourceProvider
{
    public abstract class JsonProvider : ISourceProvider
    {
        public Func<string, string> FuncGetMimeType { get; set; }

        public string RootPath { get; set; }

        protected static IConfiguration _configuration = null;

        protected static Action<int, string> _onLog = null; 

        public static JsonProvider GetInstance(FileType type, IConfiguration configuration, Action<int, string> onLog)
        {
            _configuration = configuration;
            _onLog = onLog; 

            switch (type)
            {
                case FileType.PPT:
                    return new JsonPPTProvider();
                case FileType.Excel:
                    return new JsonExcelProvider();
                default:
                    return new JsonPPTProvider();
            }
        }

        protected DataSet GetDataSource(string connecionStringName, string commandText, object model)
        {

            DataSet ds = new DataSet();
            string connecionString = _configuration.GetConnectionString(connecionStringName);

            using (var connection = new SqlConnection(connecionString))
            {
                using (var command = new SqlCommand(commandText, connection))
                {
                    if (model != null)
                    {
                        var properties = model.GetType().GetProperties();

                        if (properties != null)
                            foreach (var property in properties)
                            {
                                string paremeterName = $"@{property.Name}"; 
                                
                                var attribute = (MapDataAttribute)property.GetCustomAttributes(true).FirstOrDefault(x => (x as MapDataAttribute) != null);

                                if (attribute == null && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                                    continue;

                                if (attribute != null && string.IsNullOrWhiteSpace(attribute.Type) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                                    continue; 

                                if(attribute  != null)
                                {

                                    if (!string.IsNullOrWhiteSpace(attribute.Parameter))
                                        paremeterName = attribute.Parameter.StartsWith("@") ? attribute.Parameter : $"@{attribute.Parameter}";
                                    
                                    if(typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                                    {
                                        var dt = new DataTable();

                                        var value = property.GetValue(model); 

                                        var item = (value as IEnumerable).Cast<object>().FirstOrDefault();

                                        if (item != null)
                                        {
                                            var t = item.GetType();
                                            if (t.IsPrimitive || t == typeof(decimal) || t == typeof(string) || t == typeof(DateTime))
                                            {
                                                // Is Primitive, or Decimal, or String or datetime
                                                dt.Columns.Add("Id", t);
                                            }
                                            else
                                            {
                                                foreach(var  subProperty in t.GetProperties())
                                                {
                                                    dt.Columns.Add(subProperty.Name, subProperty.PropertyType);
                                                }
                                            }

                                            foreach(var row in (value as IEnumerable))
                                            {
                                                var dtrow = dt.NewRow();

                                                if (t.IsPrimitive || t == typeof(decimal) || t == typeof(string) || t == typeof(DateTime))
                                                    dtrow["Id"] = row;
                                                else
                                                {
                                                    foreach (var subProperty in t.GetProperties())
                                                        dtrow[subProperty.Name] = subProperty.GetValue(row);                                                
                                                }

                                                dt.Rows.Add(dtrow);
                                            }
                                        }
                                        else//we have only one udt type now. if more than one then need to redefine this structure
                                        {
                                            dt.Columns.Add("Id", typeof(int));
                                        }

                                        var param = command.Parameters.AddWithValue(paremeterName, dt);
                                        param.SqlDbType = SqlDbType.Structured;
                                        param.TypeName = attribute.Type;
                                    }
                                    else
                                        command.Parameters.Add(new SqlParameter { ParameterName = paremeterName, Value = property.GetValue(model) });
                                }
                                else
                                    command.Parameters.Add(new SqlParameter { ParameterName = paremeterName, Value = property.GetValue(model) });

                            }

                    }


                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;//5 min
                    connection.Open();

                    using (var adapter = new SqlDataAdapter(command))
                        adapter.Fill(ds);

                }
            }

            return ds;
        }

        public abstract FileObject GetFileObject(object source);


        protected static Variable GetVariable (ConfVariable fm, string parentName,  DataSet dataSource)
        {
            var variable = new Variable
            {
                ColumnName = fm.ColumnName,
                ParentId = parentName,
                VariableName = fm.Name,
                PropertType = fm.Type,
                IsHorizontal = fm.IsHorizontalCollection,
                MaxRows = fm.MaxRowsNumber ?? 0,
                IndexSource = fm.IndexSource ?? 0,
                IsGroupfield = fm.GroupField,
                IsBoldForFirstRow = fm.IsFirstRowBoldInGroup,
                UsedForFirstRowGroup = fm.UsedForFirstRowGroup,
                IndexGroup = fm.IndexGroup ?? 0,
                DataType = fm.DataType ?? DataType.String,
                FixScale = fm.FixScale != null && fm.FixScale.Value,
                X = fm.X,
                Y = fm.Y 
            };

            if (fm.Type != PropertTypeConst.Single && fm.Variables != null && fm.Variables.Any()) // Multiple
            {
                variable.VariableList = fm.Variables.Select(x => GetVariable(x, fm.Name, dataSource)).ToList();
                variable.IndexSource = fm.IndexSource == null ? 0 : fm.IndexSource.Value;
                variable.DataTable = dataSource.Tables[variable.IndexSource];
            }

            if (fm.Type == PropertTypeConst.Chart)
            {
                if (fm.Series != null)
                    variable.Series = fm.Series;
                if(fm.Categories != null)
                    variable.Categories = fm.Categories;
                variable.ChartType = fm.ChartType;
            }

            return variable; 
        }


        protected static IEnumerable<Variable> GetVariableList(IEnumerable<ConfVariable> fileMappingList, DataSet dataSource)
        {
            var lstVariables = new List<Variable>();

            foreach (var fm in fileMappingList)
                    lstVariables.Add(GetVariable(fm, null, dataSource));

            foreach (var variable in lstVariables)
            {
                SetOuterAndEmpty(variable, lstVariables);
               // variable.ParamOuters = SetOuterParam(variable, lstVariables);
                //variable.EmptyOuters = SetEmpty(variable, lstVariables);
            }

          

            return lstVariables;
        }

        private static void SetOuterAndEmpty(Variable variable, IEnumerable<Variable> lstVariables)
        {
            variable.ParamOuters = SetOuterParam(variable, lstVariables);
            variable.EmptyOuters = SetEmpty(variable, lstVariables);

            if (variable.VariableList != null && variable.VariableList.Any())
                foreach (var child in variable.VariableList)
                    SetOuterAndEmpty(child, variable.VariableList);
        }


        private static IEnumerable<Func<DataRow, string, string>> SetOuterParam(Variable variable, IEnumerable<Variable> variableList)
        {
            switch (variable.PropertType)
            {
                case PropertTypeConst.Picture:
                    return new List<Func<DataRow, string, string>>()
                    { (row,paragraph) => ReplaceSingleText(paragraph,string.Format("[{0}]", variable.VariableName), variable.ColumnName,row, variable.DataType) };
                case PropertTypeConst.Single:
                    var lstFuncs = new List<Func<DataRow, string, string>>() {
                         (row,paragraph) => ReplaceSingleText(paragraph,string.Format("[{0}]", variable.VariableName), variable.ColumnName,row, variable.DataType),
                         (row,paragraph) => ReplaceSumText(paragraph,string.Format("[{0}:{1}]", Functions.Sum,variable.VariableName), variable.ColumnName, row),
                         (row,paragraph) => ReplaceCountText(paragraph,string.Format("[{0}:{1}]", Functions.Count,variable.VariableName),variable.ColumnName, row)
                                              };
                    foreach (var item in variableList.Where(x => x.VariableName != variable.VariableName))
                    {
                        lstFuncs.Add((row, paragraph) => ReplaceSumTextByColum(paragraph, string.Format("[{0}:{1}:{2}]", Functions.Sum, variable.VariableName, item.VariableName), variable.ColumnName, item.ColumnName, row));
                        lstFuncs.Add((row, paragraph) => ReplaceAvgText(paragraph, string.Format("[{0}:{1}:{2}]", Functions.Avg, variable.VariableName, item.VariableName), variable.ColumnName, item.ColumnName, row));

                        // Avg By group
                        //foreach(var subItem in variableList.Where(x => x.VariableName != variable.VariableName && x.VariableName != item.VariableName))
                        //    lstFuncs.Add((row, paragraph) => ReplaceAvgTextByColumn(paragraph, string.Format("[{0}:{1}:{2}:{3}]", Functions.Avg, subItem.VariableName, item.VariableName, variable.VariableName), variable.ColumnName, item.ColumnName, subItem.ColumnName, row));

                    }
                    return lstFuncs;
                case PropertTypeConst.List:
                    return new List<Func<DataRow, string, string>>(){
                         (row, paragraph) => paragraph.Replace(string.Format("[{0}:{1}]",PropertTypeConst.List ,variable.VariableName),""),
                                              };
                case PropertTypeConst.Chart:
                    return new List<Func<DataRow, string, string>>(){
                         (row, paragraph) => paragraph.Replace(string.Format("[{0}:{1}]",PropertTypeConst.Chart ,variable.VariableName),"") };
                case PropertTypeConst.Matrix:
                    return new List<Func<DataRow, string, string>>(){
                         (row, paragraph) => paragraph.Replace(string.Format("[{0}:{1}]",PropertTypeConst.Chart ,variable.VariableName),""),
                         (row,paragraph) => ReplaceSumText(paragraph,string.Format("[{0}:X:{1}]", Functions.Sum,variable.VariableName), variable.ColumnName, row),
                         (row,paragraph) => ReplaceSumText(paragraph,string.Format("[{0}:Y:{1}]", Functions.Sum,variable.VariableName), variable.ColumnName, row),
                    };
            }


            return null;

        }

        private static IEnumerable<Func<string, string>> SetEmpty(Variable variable, IEnumerable<Variable> variableList)
        {

            if (variable.PropertType == PropertTypeConst.Single)
            {
                var lstFuncs = new List<Func<string, string>>() {
                        (paragraph) => EmptyParagraph(string.Format("[{0}]", variable.VariableName), paragraph),
                        (paragraph) => EmptyParagraph(string.Format("[{0}:{1}]", Functions.Sum,variable.VariableName), paragraph),
                        (paragraph) => EmptyParagraph(string.Format("[{0}:{1}]", Functions.Count,variable.VariableName),paragraph) };

                foreach (var item in variableList.Where(x => x.VariableName != variable.VariableName))
                {
                    lstFuncs.Add((paragraph) => EmptyParagraph(string.Format("[{0}:{1}:{2}]", Functions.Sum, variable.VariableName, item.VariableName), paragraph));
                    lstFuncs.Add((paragraph) => EmptyParagraph(string.Format("[{0}:{1}:{2}]", Functions.Avg, variable.VariableName, item.VariableName), paragraph));
                }
                return lstFuncs;
            }

            return null;

        }

        private static string EmptyParagraph(string param, string paragraph)
        {
            return paragraph.Replace(param, "");
        }

        private static string ReplaceSumText(string paragraph, string text, string columnName, DataRow row)
        {
            if (row != null)
            {
                DataColumn column = row.Table.Columns[columnName];

                if (column != null)
                {

                    if (paragraph.Contains(text))
                        paragraph = paragraph.Replace(text, CallDynamicMethod("GetSum", column.DataType, new object[] { row.Table, columnName }).GetString(true));
                }
            }
            else
                paragraph = paragraph.Replace(text, "_");

            return paragraph;
        }

        private static string ReplaceSumTextFilter(string paragraph, string text, string columnName, DataRow row)
        {
            if (row != null)
            {
                DataColumn column = row.Table.Columns[columnName];

                if (column != null)
                {

                    if (paragraph.Contains(text))
                        paragraph = paragraph.Replace(text, CallDynamicMethod("GetSum", column.DataType, new object[] { row.Table, columnName }).GetString(true));
                }
            }
            else
                paragraph = paragraph.Replace(text, "_"); 

            return paragraph;
        }

        public static string GetSum<T>(System.Data.DataTable dataTable, string columnName) where T : struct
        {
            T total = default(T);

            foreach (DataRow row in dataTable.Rows)
            {

                if (row[columnName] != DBNull.Value)
                    total = Add(total, (T)row[columnName]);
            }

            return total.ToString();
        }

        public static string GetSumByGroup<T>(System.Data.DataTable dataTable, string columnName, string ColumnGroupName, string field) where T : struct
        {
            T total = default(T);

            foreach (DataRow row in dataTable.Rows)
            {

                if (row[columnName] != DBNull.Value && row[ColumnGroupName] != DBNull.Value && row[ColumnGroupName].ToString() == field)
                    total = Add(total, (T)row[columnName]);
            }

            return total.ToString();
        }

        private static object CallDynamicMethod(string name, Type typeArg, object[] parameters)
        {
            // Just for simplicity, assume it's public etc
            MethodInfo method = typeof(JsonPPTProvider).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            MethodInfo generic = method.MakeGenericMethod(typeArg);
            return generic.Invoke(null, parameters);
        }

        private static string ReplaceSingleText(string paragraph, string text, string columnName, DataRow row, DataType dataType)
        {
            if (row != null)
            {
                if (Any(row, columnName))
                    paragraph = paragraph.Replace(text, row[columnName].GetString(true));
            }
            else
                paragraph = paragraph.Replace(text, "_");

            if (dataType == DataType.Number)
                paragraph = paragraph.Replace(",", ".");

            return paragraph;

        }

        private static string ReplaceSumTextByColum(string paragraph, string text, string columnName, string ColumnGroup, DataRow row)
        {
            if (row != null)
            {
                if (!string.IsNullOrEmpty(ColumnGroup) && paragraph.Contains(text))
                {
                    DataColumn column = row.Table.Columns[columnName];
                    DataColumn columnGroups = row.Table.Columns[ColumnGroup];

                    if (column != null && columnGroups != null)
                    {
                        string field = row[ColumnGroup].ToString();

                        paragraph = paragraph.Replace(text, CallDynamicMethod("GetSumByGroup", column.DataType, new object[] { row.Table, columnName, ColumnGroup, field }).GetString(true));
                    }
                }
            }
            else
                paragraph = paragraph.Replace(text, "_");

            return paragraph;
        }

        protected static T Add<T>(T number1, T number2)
        {
            dynamic a = number1;
            dynamic b = number2;
            return a + b;
        }




        private static bool Any(System.Data.DataRow row, string columnName)
        {
            foreach (DataColumn column in row.Table.Columns)
                if (column.ColumnName.ToUpper() == columnName.ToUpper())
                    return true;

            return false;
        }

        private static string ReplaceAvgText(string paragraph, string text, string columnName, string ColumnTotal, DataRow row)
        {
            if (row != null)
            {
                if (!string.IsNullOrEmpty(ColumnTotal) && paragraph.Contains(text))
                {
                    DataColumn column = row.Table.Columns[columnName];
                    DataColumn columnGroups = row.Table.Columns[ColumnTotal];

                    if (column != null && columnGroups != null)
                        paragraph = paragraph.Replace(text, CallDynamicMethod("GetAvg", column.DataType, new object[] { row.Table, columnName, ColumnTotal }).GetString(true));
                }
            }
            else
                paragraph = paragraph.Replace(text, "_");

            return paragraph;
        }

        private static string ReplaceCountText(string paragraph, string text, string columnName, DataRow row)
        {
            if (row != null)
                return paragraph.Replace(text, row.Table.Rows.Count.ToString());

            return paragraph.Replace(text, "_");
        }



    }

}
