using DTO.Common;
using DTO.KPI;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public  class KpiMap: ApiCommonData
    {
        public  ModuleItem GetModule(ApModule entity)
        {
            if (entity == null)
                return null;

            return new ModuleItem
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public  ModuleItem GetSubModule(ApSubModule entity)
        {
            if (entity == null)
                return null;

            return new ModuleItem
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        //public  KpiColumnItem GetColumn(Entities.KpiColumn entity)
        //{
        //    if (entity == null)
        //        return null;

        //    return new KpiColumnItem
        //    {
        //        Id = entity.Id,
        //        FieldLabel = entity.FieldLabel,
        //        IdSubModule = entity.IdSubModule,
        //        Type = GetFieldType(entity.FieldType),
        //        FieldName = entity.FieldName
        //    };

        //}

        //public  KpiFilterItem GetFilter(Entities.KpiColumn entity)
        //{
        //    if (entity == null)
        //        return null;

        //    return new KpiFilterItem
        //    {
        //        Id = entity.Id,
        //        FieldLabel = entity.FieldLabel,
        //        FieldName = entity.FieldName,
        //        IdSubModule = entity.IdSubModule,
        //        Required = entity.FilterRequired,
        //        IsMultiple = entity.FilterIsMultiple != null && entity.FilterIsMultiple.Value
        //    };

        //}

        public  KpiTemplateItem GetTemplate(Entities.KpiTemplate entity)
        {
            if (entity == null)
                return null;

            return new KpiTemplateItem
            {
                Id = entity.Id,
                Name = entity.Name,
                IdSubModuleList = entity.KpiTemplateSubModules?.Select(x => x.IdSubModule),
                IdModule = entity.IdModule,
                SubModuleName = $"{entity.IdModuleNavigation.Name} / {string.Join(", ", entity.KpiTemplateSubModules.Select(x => x.IdSubModuleNavigation.Name))}",
                UserName = entity.User?.FullName,
                Shared = entity.IsShared,
                UseXlsFormulas = entity.UseXlsFormulas
            };

        }

        public  TemplateColumn GetTemplateColumn(Entities.KpiTemplateColumn entity)
        {
            if (entity == null)
                return null;

            return new DTO.KPI.TemplateColumn
            {
                Id = entity.Id,
                ColumnName = entity.ColumnName,
                IdColumn = entity.IdColumn == null ? 0 : entity.IdColumn.Value,
                Type = entity.IdColumnNavigation == null ? FieldType.String :  GetFieldType(entity.IdColumnNavigation.FieldType),
                IdSubModule = entity.IdColumnNavigation == null ? 0 : entity.IdColumnNavigation.IdSubModule,
                IdModule = entity.IdColumnNavigation == null ? 0 : entity.IdColumnNavigation.IdModule,
                OriginalLabel = entity.IdColumnNavigation == null ? "" :  entity.IdColumnNavigation.FieldLabel,
                FieldName = entity.IdColumnNavigation == null ? "" :  entity.IdColumnNavigation.FieldLabel,
                Group = entity.Group != null && entity.Group.Value,
                SumFooter = entity.SumFooter != null && entity.SumFooter.Value,
                Valuecolumn = entity.Valuecolumn
            };

        }

        public  DTO.KPI.TemplateFilter GetTemplateFilter(Entities.KpiTemplateColumn entity)
        {
            if (entity == null)
                return null;

            return new DTO.KPI.TemplateFilter
            {
                Id = entity.Id,
                ColumnName = entity.ColumnName,
                IdColumn = entity.IdColumn.Value,
                IsMultiple = entity.IdColumnNavigation.FilterIsMultiple != null && entity.IdColumnNavigation.FilterIsMultiple.Value,
                Required = entity.Required,               
                Type = GetFieldType(entity.IdColumnNavigation?.FieldType),
                IdSubModule = entity.IdColumnNavigation.IdSubModule,
                OriginalLabel = entity.IdColumnNavigation.FieldLabel,
                FieldName = entity.IdColumnNavigation.FieldName,
                SelectMultiple = entity.SelectMultiple != null && entity.SelectMultiple.Value,
                IdModule = entity.IdColumnNavigation.IdModule,
                FilterLazy = entity.FilterLazy != null && entity.FilterLazy.Value
            };

        }

        public  FieldType GetFieldType(string type)
        {
            switch(type)
            {
                case "DATETIME":
                    return FieldType.DateTime;
                case "INT":
                    return FieldType.Number;
                case "DATE":
                    return FieldType.Date;
                case "VARCHAR":
                default:
                    return FieldType.String;
            }
        }


    }
}
