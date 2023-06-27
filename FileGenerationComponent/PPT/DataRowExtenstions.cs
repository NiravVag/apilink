using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FileGenerationComponent.PPT
{
    public static class DataRowExtenstions
    {
        public static bool IsFirstElementInGroup(this DataRow row, IEnumerable<string> groupColumns, int index)
        {
            if (row.Table != null)
            {
                var labelFields = new Dictionary<string, string>();

                foreach (string groupColumn in groupColumns)
                    labelFields.Add(groupColumn, row[groupColumn].ToString());


                for (int i = index; i < row.Table.Rows.Count - 1; i++)
                {
                    bool find = true;

                    foreach (var item in labelFields)
                    {
                        if (item.Value != row.Table.Rows[i][item.Key].ToString())
                            find = false;
                    }

                    if (find)
                    {
                        return row.Table.Rows[i].Equals(row);
                    }

                }
                return false;

                //var firstRow = row.Table.AsEnumerable().Where(x => x[GroupColumnName].ToString() ==  LabelField).First();

                //return firstRow.Equals(row) ;
            }

            return true;
        }

        public static int GetCountByGroup(this DataRow row, IEnumerable<string> groupColumnNameList, int index, int count)
        {
            if (row.Table != null)
            {
                var labelFields = new Dictionary<string, string>();

                foreach (string groupColumnName in groupColumnNameList)
                    labelFields.Add(groupColumnName, row[groupColumnName].ToString());

                int ALlcount = 0;
                for (int i = index; i < index + count; i++)
                {
                    if (row.Table.Rows.Count > i)
                    {
                        bool find = true;
                        foreach (var labelField in labelFields)
                            if (labelField.Value != row.Table.Rows[i][labelField.Key].ToString())
                                find = false;
                        if (find)
                            ALlcount++;
                    }

                }

                return ALlcount;
                //return row.Table.AsEnumerable().Where(x => x[GroupColumnName].ToString() == LabelField).Count(); 
            }

            return 0;
        }
    }
}
