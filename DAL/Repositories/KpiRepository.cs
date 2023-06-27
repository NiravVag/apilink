using Contracts.Repositories;
using DTO.Common;
using DTO.KPI;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class KpiRepository : Repository, IKpiRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;
        private IDictionary<SignEquality, string> _dictSigns = new Dictionary<SignEquality, string>()
        {
            { SignEquality.Equal, "=" },
            { SignEquality.Greater, ">" },
            { SignEquality.GreaterOrEqual, ">=" },
            { SignEquality.Less, "<" },
            { SignEquality.LessOrEqual, "<=" }
        };

        public KpiRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        public async Task<IEnumerable<KpiColumnItem>> GetColumns(int idSubModule)
        {
            if (_ApplicationContext.RoleList != null && _ApplicationContext.RoleList.Any())
                return await _context.KpiColumns
                .Where(x => x.Active && x.CanShowInResult && x.IdSubModule == idSubModule
                     && x.IdSubModuleNavigation.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)))
                .Select(x => new KpiColumnItem
                {
                    Id = x.Id,
                    FieldLabel = x.FieldLabel,
                    IdSubModule = x.IdSubModule,
                    IdModule = x.IdModule,
                    Type = GetFieldTypeEnum(x.FieldType),
                    FieldName = x.FieldName
                }).ToListAsync();

            return null; 
        }

        public async Task<IEnumerable<KpiColumnItem>> GetColumnsByModule(int idModule)
        {
            if (_ApplicationContext.RoleList != null && _ApplicationContext.RoleList.Any())
                return await _context.KpiColumns
                .Where(x => x.Active && x.CanShowInResult && x.IdModule == idModule
                     && x.IdModuleNavigation.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)))
                .Select(x => new KpiColumnItem
                {
                    Id = x.Id,
                    FieldLabel = x.FieldLabel,
                    IdSubModule = x.IdSubModule,
                    IdModule = x.IdModule,
                    Type = GetFieldTypeEnum(x.FieldType),
                    FieldName = x.FieldName
                }).ToListAsync();

            return null;
        }

        public async Task<IEnumerable<KpiFilterItem>> GetFilters(int idSubModule)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            var data = _context.KpiColumns
                 .Where(x => x.Active && x.CanFilter && x.IdSubModule == idSubModule
                             && x.IdSubModuleNavigation.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)));

            if (_ApplicationContext.UserType == UserTypeEnum.Customer)
                data = data.Where(x => !x.IsCustomerId && x.FilterDataSourceFieldCondition == null);

           return await data.Select(x => new KpiFilterItem
                {
                    Id = x.Id,
                    FieldLabel = x.FieldLabel,
                    FieldName = x.FieldName,
                    IdSubModule = x.IdSubModule,
                    IdModule = x.IdModule,
                    Required = x.FilterRequired,
                    IsMultiple = x.FilterIsMultiple != null && x.FilterIsMultiple.Value
                }).ToListAsync(); 
        }

        public async Task<IEnumerable<KpiFilterItem>> GetFiltersByModule(int idModule)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            var data = _context.KpiColumns
                 .Where(x => x.Active && x.CanFilter && x.IdModule == idModule
                             && x.IdModuleNavigation.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)));

            if (_ApplicationContext.UserType == UserTypeEnum.Customer)
                data = data.Where(x => !x.IsCustomerId && x.FilterDataSourceFieldCondition == null);

            return await data.Select(x => new KpiFilterItem
            {
                Id = x.Id,
                FieldLabel = x.FieldLabel,
                FieldName = x.FieldName,
                IdSubModule = x.IdSubModule,
                IdModule = x.IdModule,
                Required = x.FilterRequired,
                IsMultiple = x.FilterIsMultiple != null && x.FilterIsMultiple.Value
            }).ToListAsync();
        }

        public async Task<IEnumerable<ApModule>> GetModules()
        {
            if (_ApplicationContext.RoleList != null && _ApplicationContext.RoleList.Any())
                return await _context.ApModules.Where(x => x.Active
                    && x.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole))
                ).ToListAsync();

            return null;
        }

        public async Task<IEnumerable<ApSubModule>> GetSubModules(int idmodule)
        {
            if(_ApplicationContext.RoleList != null && _ApplicationContext.RoleList.Any())
                return await _context.ApSubModules
                     .Include(x => x.ApSubModuleRoles)
                     .Where(x => x.Active && x.IdModule == idmodule 
                        && x.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)))
                     .ToListAsync();

            return null;
        }

        public async Task<IEnumerable<ApSubModule>> GetSubModuleList(IEnumerable<int> idList)
        {
            if (_ApplicationContext.RoleList != null && _ApplicationContext.RoleList.Any())
                return await _context.ApSubModules
                     .Include(x => x.ApSubModuleRoles)
                     .Where(x => idList.Contains(x.Id) && x.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)))
                     .ToListAsync();

            return null;
        }

        public async Task<IEnumerable<KpiTemplateItem>> GetTemplates(int? idSubModule = null)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            var data = _context.KpiTemplates
                .Where(x => (x.IsShared || x.UserId == _ApplicationContext.UserId)
                    && x.KpiTemplateSubModules.Any( y =>  y.IdSubModuleNavigation.ApSubModuleRoles.Any(z => _ApplicationContext.RoleList.Contains(z.IdRole))));

            if (idSubModule != null)
                data = data.Where(x => x.KpiTemplateSubModules.Any(y => y.IdSubModule == idSubModule.Value));

            return await data.Select(x => new KpiTemplateItem {

            }).ToListAsync(); 
        }

        public async Task<KpiTemplate> GetTemplate(int id)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            return await _context.KpiTemplates
                .Include(x => x.KpiTemplateColumns)
                 .ThenInclude(x => x.IdColumnNavigation)
                .Include(x => x.User)
                .Include(x => x.IdModuleNavigation)
                .Include(x => x.KpiTemplateSubModules) 
                   .ThenInclude(x => x.IdSubModuleNavigation)
                    .ThenInclude(x => x.ApSubModuleRoles)                    
                .FirstOrDefaultAsync(x => x.Id == id
                    && x.IdModuleNavigation.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole))
                    && (!x.KpiTemplateSubModules.Any() || x.KpiTemplateSubModules.Any(y => y.IdSubModuleNavigation.ApSubModuleRoles.Any(z => _ApplicationContext.RoleList.Contains(z.IdRole)))));
        }


        public async Task<IEnumerable<KpiTemplateColumn>> GetTemplateColumns(int idTemplate)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            return await _context.KpiTemplateColumns
                .Include(x => x.IdTemplateNavigation)
                .Include(x => x.IdColumnNavigation)
                    .ThenInclude(x => x.IdSubModuleNavigation)
                        .ThenInclude(x => x.ApSubModuleRoles)
                .Where(x => x.IdTemplate == idTemplate &&  x.OrderColumn != null && ( x.IdColumnNavigation == null || (x.IdColumnNavigation.CanShowInResult
                    && (x.IdColumnNavigation.IdSubModuleNavigation == null || x.IdColumnNavigation.IdSubModuleNavigation.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole))))))
                    .OrderBy(x => x.OrderColumn)
                .ToListAsync();
        }

        public async Task<IEnumerable<KpiTemplateColumn>> GetTemplateFilters(int idTemplate)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            return await _context.KpiTemplateColumns
            .Include(x => x.IdTemplateNavigation)
            .Include(x => x.IdColumnNavigation)
            .ThenInclude(x => x.IdSubModuleNavigation)
                        .ThenInclude(x => x.ApSubModuleRoles)
            .Where(x => x.IdTemplate == idTemplate && x.IdColumnNavigation.CanFilter && x.OrderFilter != null
             && (x.IdColumnNavigation.IdSubModule == null || x.IdColumnNavigation.IdSubModuleNavigation.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole))))
             .OrderBy(x => x.OrderFilter)
            .ToListAsync();
        }

        public async Task<KpiTemplateColumn> GetFieldById(int id)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            var item = await _context.KpiTemplateColumns
                .Include(x => x.IdColumnNavigation)
                .ThenInclude(x => x.IdSubModuleNavigation)
                  .ThenInclude(x => x.ApSubModuleRoles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return null;

            if (item.IdColumnNavigation.IdSubModuleNavigation != null && !item.IdColumnNavigation.IdSubModuleNavigation.ApSubModuleRoles.Any(x => _ApplicationContext.RoleList.Contains(x.IdRole)))
                return null;

            return item; 
        }

        public async Task<IEnumerable<dynamic>> GetDataSourceByName(string dataSourceName, string fieldName, string term, IDictionary<string, object> parameters = null)
        {
            var list = new List<dynamic>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                string sSql = $"SELECT * From {dataSourceName} ";
                string sqlWhere = "";

                if (parameters != null && parameters.Any())
                {                    
                    foreach(var parameter in parameters)
                    {
                        var items = parameter.Value as IEnumerable;

                        if (items == null)
                        {
                            var param = command.CreateParameter();
                            param.ParameterName = $"@{parameter.Key}";
                            param.Value = parameter.Value;
                            command.Parameters.Add(param);
                        }

                        if (sqlWhere != "")
                            sqlWhere += " AND ";                      

                        if (items != null)
                            sqlWhere += $"{parameter.Key} IN ({string.Join(",", items.Cast<object>().Select(x => $"'{x.ToString()}'"))})";
                        else
                            sqlWhere += $"{parameter.Key} = @{parameter.Key}";
                    }

                    sSql = $"{sSql} WHERE {sqlWhere}";            
                }

                if (!string.IsNullOrWhiteSpace(fieldName) && !string.IsNullOrWhiteSpace(term))
                {
                    var param = command.CreateParameter();
                    param.ParameterName = $"@{fieldName}_lazy";
                    param.Value = term;
                    command.Parameters.Add(param);

                    sSql = $"{sSql} {(string.IsNullOrEmpty(sqlWhere) ? " WHERE " : " AND ")} {fieldName} LIKE @{fieldName}_lazy + '%'";
                }
                command.CommandText = sSql;
               _context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        dynamic expando = new ExpandoObject();
                        var expandoDict = expando as IDictionary<string, object>;

                        for (int i = 0; i < result.FieldCount; i++)
                            expandoDict.Add(result.GetName(i), result[i]);

                        list.Add(expando);
                    }

                }

                _context.Database.CloseConnection();
            }

            return list;
        }

        public async Task<(IEnumerable<KpiTemplateItem>, int)> SearchTemplates(KpiTemplateListRequest request)
        {

            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return (null,0);

            int skip = (request.Index - 1) * request.PageSize;


            IQueryable<InternalKpiTemplate>  query ;


            if(request.IdSubModule > 0)
                query = _context.KpiTemplates
                    .Where(x => (x.IsShared || x.UserId == _ApplicationContext.UserId)
                && x.KpiTemplateSubModules.Any(y => y.IdSubModule == request.IdSubModule && y.IdSubModuleNavigation.ApSubModuleRoles.Any(z => _ApplicationContext.RoleList.Contains(z.IdRole))))
                .Select(x => new InternalKpiTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Shared = x.IsShared,
                    UserName = x.User.FullName,
                    UseXlsFormulas = x.UseXlsFormulas,
                    Iduser = x.UserId,
                    IdModule = x.IdModule,
                    ModuleName = x.IdModuleNavigation.Name,
                    SubModuleList = x.KpiTemplateSubModules.Select(y => new InternalSubModule
                     {
                        Id = y.IdSubModule,
                        Name = y.IdSubModuleNavigation.Name
                    })
                });
            else
                query = _context.KpiTemplates
                    .Where(x => (x.IsShared || x.UserId == _ApplicationContext.UserId)
                && x.IdModuleNavigation.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)))
                .Select(x => new InternalKpiTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Shared = x.IsShared,
                    UserName = x.User.FullName,
                    UseXlsFormulas = x.UseXlsFormulas, 
                    Iduser = x.UserId,
                    IdModule = x.IdModule,
                    ModuleName = x.IdModuleNavigation.Name,
                    SubModuleList = x.KpiTemplateSubModules.Select(y => new InternalSubModule
                    {
                        Id = y.IdSubModule,
                        Name = y.IdSubModuleNavigation.Name                       
                    })
                });
         

            int count = await query.CountAsync();

            if (count <= 0)
                return (null, 0);

            var userCustomers = new List<int>();

            if(_ApplicationContext.CustomerId > 0)
                userCustomers = await _context.ItUserMasters.Where(x => x.CustomerId == _ApplicationContext.CustomerId).Select(x => x.Id).ToListAsync();
            

            return ((await query.OrderBy(x => x.Id).Skip(skip).Take(request.PageSize).ToListAsync()).Select(x => new KpiTemplateItem {
                Id = x.Id,
                IdModule = x.IdModule,
                IdSubModuleList = x.SubModuleList.Select(y => y.Id),
                Name = x.Name,
                Shared = x.Shared,
                CanSave = x.Iduser == _ApplicationContext.UserId || userCustomers.Contains(x.Iduser),
                SubModuleName = $"{x.ModuleName} / {string.Join(", ",  x.SubModuleList.Select(y => y.Name))}",
                UserName = x.UserName,
                UseXlsFormulas = x.UseXlsFormulas
            }), count);
        }

        public async Task<KpiTemplateView> GetViewTemplate(int id)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;
            //           

            var item = await _context.KpiTemplates.Where(x => x.Id == id
                   && (x.IsShared || x.UserId == _ApplicationContext.UserId)
                   && x.IdModuleNavigation.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole))
                   && (!x.KpiTemplateSubModules.Any() || x.KpiTemplateSubModules.Any(y => y.IdSubModuleNavigation.ApSubModuleRoles.Any(z => _ApplicationContext.RoleList.Contains(z.IdRole)))))
                    .Select(x => new
                    {
                        IdTemplate = x.Id,
                        SubModuleList = x.KpiTemplateSubModules.Any() ?  x.KpiTemplateSubModules.Select(y => new {
                            Id = y.IdSubModule,
                            Name = y.IdSubModuleNavigation.Name,
                            IdModule = y.IdSubModuleNavigation.IdModule,
                            ModuleName = y.IdSubModuleNavigation.IdModuleNavigation.Name
                        }) : null,
                        TemplateName = x.Name,
                        ModuleName =x.IdModuleNavigation.Name,
                        ColumnList = x.KpiTemplateColumns.Where(y => (y.IdColumn == null || y.IdColumnNavigation.CanShowInResult) && y.OrderColumn != null)
                            .OrderBy(y => y.OrderColumn)
                            .Select(y => new TemplateColumn {
                                Id = y.Id,
                                ColumnName = y.ColumnName,
                                IdSubModule = y.IdColumnNavigation.IdSubModule,
                                IdModule = y.IdColumnNavigation.IdModule,
                                IdColumn = y.IdColumn == null ? 0 : y.IdColumn.Value,
                                Group = y.Group != null && y.Group.Value,
                                OriginalLabel = y.IdColumnNavigation.FieldLabel,
                                FieldName = y.IdColumnNavigation.FieldName,
                                SumFooter = y.SumFooter != null && y.SumFooter.Value,
                                Type = GetFieldTypeEnum(y.IdColumnNavigation.FieldType)
                            }),
                        FilterList = x.KpiTemplateColumns.Where(y => y.IdColumnNavigation.CanFilter && y.OrderFilter != null)
                            .OrderBy(y => y.OrderFilter)
                            .Select(y => new TemplateFilter
                            {
                                Id = y.Id,
                                ColumnName = y.ColumnName,
                                IdSubModule = y.IdColumnNavigation.IdSubModule,
                                IdModule = y.IdColumnNavigation.IdModule,
                                IsMultiple = y.IdColumnNavigation.FilterIsMultiple != null && y.IdColumnNavigation.FilterIsMultiple.Value,
                                Required = y.Required,
                                IdColumn = y.IdColumn.Value,
                                OriginalLabel = y.IdColumnNavigation.FieldLabel,
                                FieldName = y.IdColumnNavigation.FieldName,
                                Type = GetFieldTypeEnum(y.IdColumnNavigation.FieldType),
                                SelectMultiple = y.SelectMultiple != null && y.SelectMultiple.Value,
                                FilterLazy = y.FilterLazy != null && y.FilterLazy.Value
                            })
                    }).FirstOrDefaultAsync();

            if (item == null)
                return null;

            return new KpiTemplateView
            {
                ColumnList = item.ColumnList,
                FilterList = item.FilterList,
                IdTemplate = item.IdTemplate,
                TemplateName = item.TemplateName,
                Module = $"{item.ModuleName} {(item.SubModuleList != null ? "/" : "")} {(item.SubModuleList != null  ? string.Join(", ", item.SubModuleList.Select(y => y.Name)) : "")}"
            };
        }

        public async Task<IEnumerable<TemplateColumn>> GetColumnsByTemplate(int idTemplate)
        {
            if (_ApplicationContext.RoleList == null || !_ApplicationContext.RoleList.Any())
                return null;

            return await _context.KpiTemplateColumns
                .Where(x => x.IdTemplate == idTemplate 
                        &&  x.IdColumnNavigation.CanShowInResult && x.OrderColumn != null
                        && (x.IdTemplateNavigation.IsShared || x.IdTemplateNavigation.UserId == _ApplicationContext.UserId)
                         && x.IdColumnNavigation.IdSubModuleNavigation.ApSubModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole)))
                 .OrderBy(x => x.OrderColumn)
                 .Select(x => new TemplateColumn
                 {
                     Id = x.Id,
                     ColumnName = x.ColumnName,
                     IdSubModule =x.IdColumnNavigation.IdSubModule,
                     IdModule = x.IdColumnNavigation.IdModule,
                     IdColumn = x.IdColumn == null ? 0 : x.IdColumn.Value,
                     Group = x.Group != null && x.Group.Value,
                     OriginalLabel = x.IdColumnNavigation.FieldLabel,
                     FieldName = x.IdColumnNavigation.FieldName,
                     SumFooter = x.SumFooter != null && x.SumFooter.Value,
                     Type = GetFieldTypeEnum(x.IdColumnNavigation.FieldType)
                 }).ToListAsync();
                        

        }

        public async Task<ViewData> DataSourceResult(KpiTemplateViewRequest request)
        {
            var template = await _context.KpiTemplates
                .Where(x => x.Id == request.IdTemplate && (x.IsShared || x.UserId == _ApplicationContext.UserId)
                    && x.IdModuleNavigation.ApModuleRoles.Any(y => _ApplicationContext.RoleList.Contains(y.IdRole))
                    && (!x.KpiTemplateSubModules.Any() || x.KpiTemplateSubModules.Any(y => y.IdSubModuleNavigation.ApSubModuleRoles.Any(z => _ApplicationContext.RoleList.Contains(z.IdRole)))))
                    .Select(x => new {
                        Name = x.Name,
                        UseXlx = x.UseXlsFormulas,
                        DataSourceName = x.IdModuleNavigation.DataSourceName,
                        IdModule = x.IdModule,
                        SubModuleList = x.KpiTemplateSubModules.Select(y => new {
                            DataSourceName = y.IdSubModuleNavigation.DataSourceName,
                            IdSubModule = y.IdSubModule,
                            KeyList = y.IdSubModuleNavigation.KpiColumns.Where(z => z.IsKey).Select(z => new {
                                Id = z.Id,
                                Name = z.FieldName,
                                FieldType = z.FieldType
                            })
                        }),
                        Filters = x.KpiTemplateColumns
                                    .Where(y => y.IdColumnNavigation.CanFilter  && y.OrderFilter != null)
                                    .Select(y => new {
                                        Id = y.Id,
                                        Name = y.IdColumnNavigation.FieldName,
                                        Label = y.ColumnName,
                                        Required = y.IdColumnNavigation.FilterRequired,
                                        Multiple = y.IdColumnNavigation.FilterIsMultiple,
                                        IsCustomer = y.IdColumnNavigation.IsCustomerId,
                                        IsLocation = y.IdColumnNavigation.IsLocationId,
                                        FieldType = y.IdColumnNavigation.FieldType,
                                        SignEquality = y.IdColumnNavigation.FilterSignEqualityId,
                                        SelectMultiple = y.SelectMultiple,
                                        IdSubModule = y.IdColumnNavigation.IdSubModule,
                                        IdModule = y.IdColumnNavigation.IdModule,
                                        DataSourceName = y.IdColumnNavigation.IdSubModule != null ? y.IdColumnNavigation.IdSubModuleNavigation.DataSourceName : ""
                                    }),
                        Columns = x.KpiTemplateColumns
                                    .Where(y => (y.IdColumnNavigation == null || y.IdColumnNavigation.CanShowInResult) && y.OrderColumn != null)
                                    .OrderBy(y => y.OrderColumn)
                                    .Select(y => new TemplateColumn {
                                        Id = y.Id,
                                        FieldName = y.IdColumnNavigation == null ? y.ColumnName : y.IdColumnNavigation.FieldName,
                                        ColumnName = y.ColumnName,
                                        Group = y.Group != null && y.Group.Value,
                                        SumFooter = y.SumFooter != null && y.SumFooter.Value,
                                        Type = y.IdColumnNavigation == null ? FieldType.String :  GetFieldTypeEnum(y.IdColumnNavigation.FieldType),
                                        IdSubModule = y.IdColumnNavigation == null ? (int?) null :  y.IdColumnNavigation.IdSubModule,
                                        IdModule = y.IdColumnNavigation == null ? (int?)null  :y.IdColumnNavigation.IdModule,
                                        IsKey = y.IdColumnNavigation != null && y.IdColumnNavigation.IsKey,
                                        Valuecolumn = y.Valuecolumn,
                                        IdColumn = y.IdColumn == null ? 0 : y.IdColumn.Value
                                    })
                    }).FirstOrDefaultAsync();

            if (template == null)   
                return null;

            
            //if (template.SubModuleList == null || !template.SubModuleList.Any())
            //    return null;

            string datasourceName = template.DataSourceName;

            if (string.IsNullOrEmpty(datasourceName))
                return null; 

            var list = new List<dynamic>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                string sSqlSelect = "";
                string sSqlFrom = "";
                var lstFroms = new List<int>();
                string mainKeyField = ""; 

                var mainKeyColumn = template.Columns.FirstOrDefault(x => x.IsKey && x.IdModule != null);

                if (mainKeyColumn == null)
                {
                    // search it in database
                    var mainKey = await _context.KpiColumns.FirstOrDefaultAsync(x => x.IdModule == template.IdModule && x.IsKey);

                    if (mainKey == null)
                        return null;

                    mainKeyField = mainKey.FieldName;
                }
                else
                    mainKeyField = mainKeyColumn.FieldName;

                foreach (var column in template.Columns)
                {
                    string sourceName = "";

                    if (column.IdColumn == 0)
                    {
                        if (sSqlSelect != "")
                            sSqlSelect += ", ";

                        sSqlSelect += $"'{column.Valuecolumn}' AS [{column.FieldName}]";
                    }
                    else
                    { 
                        if (column.IdModule != null)
                            sourceName = datasourceName;
                        else
                        {
                            var subModule = template.SubModuleList.FirstOrDefault(x => x.IdSubModule == column.IdSubModule);

                            if (subModule == null)
                                continue;

                            if (subModule.KeyList == null || !subModule.KeyList.Any())
                                continue;

                            sourceName = subModule.DataSourceName;

                            if (!lstFroms.Contains(subModule.IdSubModule))
                            {
                                string andLeft = "";

                                foreach (var key in subModule.KeyList)
                                {
                                    if (andLeft != "")
                                        andLeft += $" AND ";

                                    andLeft += $" [{sourceName}].[{key.Name}] = [{datasourceName}].[{mainKeyField}]";
                                }

                                sSqlFrom = sSqlFrom + $" LEFT JOIN [{sourceName}] ON {andLeft} ";
                                lstFroms.Add(subModule.IdSubModule);
                            }
                        }


                        if (sSqlSelect != "")
                            sSqlSelect += ", ";

                        sSqlSelect += $"[{sourceName}].[{column.FieldName}]";
                    }
                }

                string sSql = $"SELECT {sSqlSelect} From {datasourceName} {sSqlFrom} ";

                if (request.FilterValues != null && request.FilterValues.Any())
                {                   
                    string sqlWhere = "";
                    bool findCustomer = false;
                    bool findLocation = false;

                    foreach (var parameter in request.FilterValues)
                    {
                        var item = template.Filters.FirstOrDefault(x => x.Id == parameter.Id);

                        if (item == null) 
                            continue;

                        if (item.Required && parameter.Value == null)
                            return null;

                        if (parameter.Value == null)
                            continue; 

                        var type = GetFieldType(item.FieldType);
                        object value = null; 

                        if ((item.SelectMultiple == null || !item.SelectMultiple.Value) &&  !TryChangeType(parameter.Value, type, out value))
                            return null; 

                        if(item.SelectMultiple != null && item.SelectMultiple.Value)
                        {
                            IEnumerable items = parameter.Value as IEnumerable;

                            if (items == null)
                                return null;

                            if (item.Required && !items.Cast<object>().Any())
                                return null ;

                            if (!items.Cast<object>().Any())
                                continue;

                            foreach (var selectItem in items)
                                if (!TryChangeType(selectItem, type, out object selectValue))
                                    return null;

                            value = items;
                        }
                                               
                        string paramName = $"@{item.Name}";
                        string signString = "=";

                        if (item.SelectMultiple != null && item.SelectMultiple.Value)
                            signString = "IN";
   
                        if (item.SignEquality != null && _dictSigns.TryGetValue((SignEquality)item.SignEquality.Value, out signString))
                            paramName = $"@{item.Name}_{item.Id}";


                        if (item.IsCustomer && _ApplicationContext.UserType == UserTypeEnum.Customer)
                        {
                            value = _ApplicationContext.CustomerId;
                            findCustomer = true; 
                        }

                        
                        if (item.IsLocation && _ApplicationContext.UserType == UserTypeEnum.InternalUser)
                        {
                            if (_ApplicationContext.LocationList != null && !_ApplicationContext.LocationList.Contains((int)value))
                                return null; 

                            findLocation = true;
                        }


                        if (sqlWhere != "")
                            sqlWhere += " AND ";

                        if (item.SelectMultiple == null || !item.SelectMultiple.Value)
                        {

                            var param = command.CreateParameter();
                            param.ParameterName = paramName;
                            param.Value = value;
                            command.Parameters.Add(param);

                            sqlWhere += $"[{(string.IsNullOrEmpty(item.DataSourceName) ?  datasourceName : item.DataSourceName)}].[{item.Name}] {signString} {paramName}";
                        }
                        else
                             sqlWhere += $"[{(string.IsNullOrEmpty(item.DataSourceName) ? datasourceName : item.DataSourceName)}].[{item.Name}] IN ({string.Join(",", (value as IEnumerable).Cast<object>().Select(x => $"'{x.ToString()}'"))}) ";
                    }

                    if(_ApplicationContext.UserType == UserTypeEnum.Customer && !findCustomer)
                    {
                        var item = template.Filters.FirstOrDefault(x => x.IsCustomer);

                        if(item != null)
                        {
                            if (sqlWhere != "")
                                sqlWhere += " AND ";

                            sqlWhere += $"[{(string.IsNullOrEmpty(item.DataSourceName) ? datasourceName : item.DataSourceName)}].[{item.Name}] = @{item.Name}";
                        }

                        var param = command.CreateParameter();
                        param.ParameterName = $"@{item.Name}";
                        param.Value = _ApplicationContext.CustomerId;
                        command.Parameters.Add(param);
                    }

                    if (_ApplicationContext.UserType == UserTypeEnum.InternalUser && !findLocation)
                    {
                        var item = template.Filters.FirstOrDefault(x => x.IsLocation);

                        if (item != null)
                        {
                            if (sqlWhere != "")
                                sqlWhere += " AND ";

                            if (_ApplicationContext.LocationList == null || !_ApplicationContext.LocationList.Any())
                                return null; 

                            sqlWhere += $"[{(string.IsNullOrEmpty(item.DataSourceName) ? datasourceName : item.DataSourceName)}].[{item.Name}] IN ({string.Join(",",_ApplicationContext.LocationList)}) ";
                        }

                    }

                    sSql = $"{sSql} WHERE {sqlWhere}";
                }

                command.CommandText = sSql;
               _context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        dynamic expando = new ExpandoObject();
                        var expandoDict = expando as IDictionary<string, object>;

                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            if(result[i] != null && result[i].GetType() == typeof(DateTime) || result[i].GetType() == typeof(DateTime?))
                                expandoDict.Add(result.GetName(i), ((DateTime)result[i]).ToString("dd/MM/yyyy"));
                            else
                                expandoDict.Add(result.GetName(i), result[i]);
                        }                           

                        list.Add(expando);
                    }
                }
                
                _context.Database.CloseConnection();

                if(list.Count >0)
                {
                    var lstGroups = template.Columns.Where(x => x.Group ).ToList();
                    IEnumerable<dynamic> data = list; 


                    if(lstGroups != null && lstGroups.Any())
                    {                                               
                         data = GroupBy(data, lstGroups);
                        return new ViewData
                        {
                            ColumnList = template.Columns,
                            Data = data,
                            TemplateName = template.Name,
                            UseXls = template.UseXlx,
                            BasicData = list
                        };
                    }
                }
            }

            return new ViewData
            {
                ColumnList = template.Columns,
                Data = list,
                TemplateName = template.Name,
                UseXls = template.UseXlx,
                BasicData = list
            }; 
        }

        private IEnumerable<object> GroupBy(IEnumerable<object> data,  List<TemplateColumn> templates)
        {

            if (templates.Count <= 0)
                return data;

            var first = templates.First();
            var newTemplates = templates.ToList();

            newTemplates.Remove(first);

          Func<object, TemplateColumn, string> func = (currentModel, templateColumn) =>
          {
               var expandoDict = currentModel as IDictionary<string, object>;

               if (expandoDict.TryGetValue(templateColumn.FieldName, out object value))
                   return value.ToString();

               return ""; 

           };

            var groups = data.GroupBy(x => func(x, first)).ToList();

            var lst = new List<dynamic>();
            foreach(var group in  groups)
            {
                dynamic expando = new ExpandoObject();
                var expandoDict = expando as IDictionary<string, object>;

                var result = GroupBy(group.AsEnumerable(), newTemplates);
                expandoDict.Add(group.Key, result);
                lst.Add(expando);
            }

            return lst;
                       
           //return groups.Select(x => {
           //    dynamic expando = new ExpandoObject();
           //    var expandoDict = expando as IDictionary<string, object>;

           //    var result = GroupBy(x.AsEnumerable(), templates);
           //    expandoDict.Add(x.Key, result);

           //    return expando;
           //});
        }

        private Type GetFieldType(string type)
        {
            switch (type)
            {
                case "DATETIME":
                    return typeof(DateTime);
                case "INT":
                    return typeof(int);
                case "DATE":
                    return typeof(DateTime);
                case "VARCHAR":
                default:
                    return  typeof(string);
            }
        }

        private static FieldType GetFieldTypeEnum(string type)
        {
            switch (type)
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


        private bool TryChangeType(object value, Type type, out object result)
        {
            try
            {
                if (type == typeof(DateTime))
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<DateObject>(value.ToString()).ToDateTime(); 
                    return true; 
                }

                result = Convert.ChangeType(value, type);
                return true; 
            }
            catch(Exception ex)
            {
                result = null;
                return false; 
            }
        }

    }

    internal class InternalKpiTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<InternalSubModule> SubModuleList { get; set; }

        public bool Shared { get; set; }

        public bool UseXlsFormulas { get; set; }

        public int Iduser { get; set;  }

        public string UserName { get; set; }

        public int IdModule { get; set;  }

        public string ModuleName { get; set;  }
    }

    internal class InternalSubModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
