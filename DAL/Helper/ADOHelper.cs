using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DTO.Kpi;
using System.Reflection;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public class ADOHelper
    {
        public static async Task<DataSet> GetLinkDataSource(IConfiguration _configuration, string connecionStringName, string commandText, object model)
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

                                if (attribute != null)
                                {
                                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                                    {
                                        var dt = new DataTable();

                                        var value = property.GetValue(model);

                                        var item = value != null ? (value as IEnumerable).Cast<object>().FirstOrDefault() : null;

                                        if (item != null)
                                        {
                                            var t = item.GetType();
                                            //if (t.IsPrimitive || t == typeof(decimal) || t == typeof(string) || t == typeof(DateTime))
                                            //{
                                            //    // Is Primitive, or Decimal, or String or datetime
                                            //    dt.Columns.Add("Id", t);
                                            //}
                                            //else
                                            //{
                                                foreach (var subProperty in t.GetProperties())
                                                {
                                                    dt.Columns.Add(subProperty.Name, subProperty.PropertyType);
                                                }
                                            //}

                                            foreach (var row in (value as IEnumerable))
                                            {
                                                var dtrow = dt.NewRow();

                                                //if (t.IsPrimitive || t == typeof(decimal) || t == typeof(string) || t == typeof(DateTime))
                                                //    dtrow["Id"] = row;
                                                //else
                                                //{
                                                    foreach (var subProperty in t.GetProperties())
                                                        dtrow[subProperty.Name] = subProperty.GetValue(row);
                                                //}

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
                    command.CommandTimeout = 120;
                    connection.Open();

                    using (var adapter = new SqlDataAdapter(command))
                        adapter.Fill(ds);

                }
            }

            return ds;
        }

        /// <summary>
        /// frame the List from the query result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static async Task<List<T>> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = await GetItem<T>(row);
                    data.Add(item);
                }
            }
            return data;
        }


        /// <summary>
        /// frame the List from the query list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static async Task<T> GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            if (dr != null)
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name.ToLower() == column.ColumnName.ToLower())
                            pro.SetValue(obj, dr.IsNull(pro.Name) ? null : dr[column.ColumnName], null);
                    }
                }
            }
            return obj;
        }
    }
}
