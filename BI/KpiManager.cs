using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.KPI;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class KpiManager : IKpiManager
    {

        private readonly IKpiRepository _repository = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IUserRightsManager _userRightsManager = null;
        private readonly KpiMap KpiMap = null;

        public KpiManager(IKpiRepository repository, IAPIUserContext applicationContext, IUserRightsManager userRightsManager)
        {
            _repository = repository;
            _ApplicationContext = applicationContext;
            _userRightsManager = userRightsManager;
            KpiMap = new KpiMap();
        }

        public async Task<KpiColumnListResponse> GetColumnList(int idSubModule)
        {
            var data = await _repository.GetColumns(idSubModule);

            if (data == null || !data.Any())
                return new KpiColumnListResponse { Result = KpiColumnListResult.NotFound };

            return new KpiColumnListResponse
            {
                Data = data,
                Result = KpiColumnListResult.Success
            };
        }



        public async Task<KpiFilterListResponse> GetFilterList(int idSubModule)
        {
            var data = await _repository.GetFilters(idSubModule);

            if (data == null || !data.Any())
                return new KpiFilterListResponse { Result = KpiFilterListResult.NoFound };

            return new KpiFilterListResponse
            {
                Data = data,
                Result = KpiFilterListResult.Success
            };
        }

        public async Task<KpiColumnListResponse> GetColumnListByModule(int idModule)
        {
            var data = await _repository.GetColumnsByModule(idModule);

            if (data == null || !data.Any())
                return new KpiColumnListResponse { Result = KpiColumnListResult.NotFound };

            return new KpiColumnListResponse
            {
                Data = data,
                Result = KpiColumnListResult.Success
            };
        }



        public async Task<KpiFilterListResponse> GetFilterListByModule(int idModule)
        {
            var data = await _repository.GetFiltersByModule(idModule);

            if (data == null || !data.Any())
                return new KpiFilterListResponse { Result = KpiFilterListResult.NoFound };

            return new KpiFilterListResponse
            {
                Data = data,
                Result = KpiFilterListResult.Success
            };
        }

        public async Task<ModuleListResponse> GetModuleList()
        {
            var data = await _repository.GetModules();

            if (data == null || !data.Any())
                return new ModuleListResponse { Result = ModuleListResult.NotFound };

            return new ModuleListResponse
            {
                Data = data.Select(KpiMap.GetModule),
                Result = ModuleListResult.Success
            };
        }

        public async Task<ModuleListResponse> GetSubModuleList(int idmodule)
        {
            var data = await _repository.GetSubModules(idmodule);

            if (data == null || !data.Any())
                return new ModuleListResponse { Result = ModuleListResult.NotFound };

            return new ModuleListResponse
            {
                Data = data.Select(KpiMap.GetSubModule),
                Result = ModuleListResult.Success
            };
        }

        public async Task<KpiTemplateColumnListResponse> GetTemplateColumnList(int idTemplate)
        {
            var data = await _repository.GetTemplateColumns(idTemplate);

            if (data == null || !data.Any())
                return new KpiTemplateColumnListResponse { Result = KpiTemplateColumnListResult.NotFound };

            return new KpiTemplateColumnListResponse
            {
                Data = data.Select(KpiMap.GetTemplateColumn),
                Result = KpiTemplateColumnListResult.Success
            };
        }

        public async Task<KpiTemplateFilterListResponse> GetTemplateFilterList(int idTemplate)
        {
            var data = await _repository.GetTemplateFilters(idTemplate);

            if (data == null || !data.Any())
                return new KpiTemplateFilterListResponse { Result = KpiTemplateFilterListResult.NotFound };

            return new KpiTemplateFilterListResponse
            {
                Data = data.Select(KpiMap.GetTemplateFilter),
                Result = KpiTemplateFilterListResult.Success
            };
        }

        public async Task<KpiTemplateListResponse> GetTemplateList(int? idSubModule)
        {
            var data = await _repository.GetTemplates(idSubModule);

            if (data == null || !data.Any())
                return new KpiTemplateListResponse { Result = KpiTemplateListResult.NotFound };

            return new KpiTemplateListResponse
            {
                Data = data,
                Result = KpiTemplateListResult.Success
            };
        }

        public async Task<KpiTemplateItemResponse> GetTemplate(int id)
        {
            var item = await _repository.GetTemplate(id);

            if (item == null)
                return new KpiTemplateItemResponse
                {
                    Result = KpiTemplateListResult.NotFound
                };

            bool canSave = false;

            if (_ApplicationContext.UserId == item.UserId)
                canSave = true;
            else if (_ApplicationContext.CustomerId > 0)
            {
                var userList = await _userRightsManager.GetUsersIdListByCustomer(_ApplicationContext.CustomerId);

                canSave = userList != null && userList.Contains(_ApplicationContext.UserId);
            }

            var currentItem = KpiMap.GetTemplate(item);
            currentItem.CanSave = canSave;

            return new KpiTemplateItemResponse
            {
                Result = KpiTemplateListResult.Success,
                Item = currentItem
            };
        }

        public async Task<KpiDataSourceResponse> GetDataSource(int id)
        {
            var filter = await _repository.GetFieldById(id);

            if (filter == null)
                return new KpiDataSourceResponse { Result = DataSourceResult.FilterNotFound };

            IEnumerable<dynamic> data = null;

            if (filter.FilterLazy == null || !filter.FilterLazy.Value)
            {
                data = await GetDataSource(filter.IdColumnNavigation, "", "");

                if (data == null)
                    return new KpiDataSourceResponse { Result = DataSourceResult.NotFound };
            }

            return new KpiDataSourceResponse
            {
                DataSource = data,
                DataSourceFieldName = filter.IdColumnNavigation.FilterDataSourceFieldName,
                DataSourceFieldValue = filter.IdColumnNavigation.FilterDataSourceFieldValue,
                Result = DataSourceResult.Success
            };
        }

        public async Task<IEnumerable<object>> GetDataLazy(int id, string fieldName, string term)
        {
            var filter = await _repository.GetFieldById(id);

            if (filter == null)
                return null;

            return await GetDataSource(filter.IdColumnNavigation, fieldName, term);
        }

        public async Task<KpiSavetemplateResponse> SaveTemplate(KpiTemplateRequest request)
        {
            
            if (request.TemplateColumnList == null || !request.TemplateColumnList.Any())
                return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.TemplateColumnsRequired };

            if (request.Id <= 0)
                return await AddTemplate(request);

            return await UpdateTemplate(request);
        }

        public async Task<KpiTemplateListResponse> SearchTemplates(KpiTemplateListRequest request)
        {
            if (request.Index <= 0)
                request.Index = 1;

            if (request.PageSize <= 0)
                request.PageSize = 10;

            var data = await _repository.SearchTemplates(request);

            if (data.Item2 <= 0)
                return new KpiTemplateListResponse { Result = KpiTemplateListResult.NotFound };

            return new KpiTemplateListResponse
            {
                Data = data.Item1,
                Index = request.Index,
                PageSize = request.PageSize,
                PageCount = (data.Item2 / request.PageSize) + (data.Item2 % request.PageSize > 0 ? 1 : 0),
                TotalCount = data.Item2,
                Result = KpiTemplateListResult.Success
            };


        }

        public async Task<KpiTemplateViewResponse> GetViewTemplate(int id)
        {
            var item = await _repository.GetViewTemplate(id);

            if (item == null)
                return new KpiTemplateViewResponse { Result = KpiTemplateViewResult.NotFound };

            return new KpiTemplateViewResponse
            {
                Result = KpiTemplateViewResult.Success,
                Item = item
            };
        }


        private async Task<KpiSavetemplateResponse> AddTemplate(KpiTemplateRequest request)
        {
            // Form Validation
            if (request.Module == null || request.Module.Id <= 0)
                return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.ModuleIsrequired };

            //if (request.SubmoduleList == null || !request.SubmoduleList.Any())
            //    return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.SubModuleIsrequired };

            if(string.IsNullOrWhiteSpace(request.Name))
                return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.TemplateNameIsRequired };

            // Check  ability
            var moduleList = await _repository.GetModules(); 

            if(moduleList == null || !moduleList.Any(x => x.Id == request.Module.Id))
                return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.UnAuthorized };

            IEnumerable<Entities.ApSubModule> subModuleList = null;

            if (request.SubmoduleList != null && request.SubmoduleList.Any())
            {
                subModuleList = await _repository.GetSubModuleList(request.SubmoduleList.Select(x => x.Id));

                if (subModuleList == null || !subModuleList.Any() || subModuleList.Count() != request.SubmoduleList.Count())
                    return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.UnAuthorized };
            }

            // Add Entity
            var entity = new Entities.KpiTemplate
            {
                IsShared = request.Shared,
                Name = request.Name,
                UserId = _ApplicationContext.UserId,
                UseXlsFormulas = request.UseXlsFormulas,
                CreatedDate = DateTime.Now,
                IdModule = request.Module.Id
            };

            _repository.AddEntity(entity);

            if (subModuleList != null)
            {
                foreach (var submodule in request.SubmoduleList)
                {
                    var entitySubModule = new Entities.KpiTemplateSubModule
                    {
                        IdSubModule = submodule.Id,
                        IdTemplateNavigation = entity
                    };

                    _repository.AddEntity(entitySubModule);
                }
            }
            

            // Add columns
            int i = 0;

            foreach (var item in request.TemplateColumnList)
            {
                i++; 
                var columnEntity = new Entities.KpiTemplateColumn
                {
                    IdColumn = item.IdColumn ==  0 ? (int?)null : item.IdColumn,
                    ColumnName = item.ColumnName,
                    OrderColumn = i,
                    SumFooter = item.SumFooter,
                    Group = item.Group,
                    Required = false,
                    Valuecolumn = item.Valuecolumn
                };

                entity.KpiTemplateColumns.Add(columnEntity);
                _repository.AddEntity(columnEntity);
            }

            // Add filters
            i = 0; 
            if(request.TemplateFilterList != null)
                foreach (var item in request.TemplateFilterList)
                {
                    i++;
                    var columnEntity = new Entities.KpiTemplateColumn
                    {
                        IdColumn = item.IdColumn,
                        ColumnName = item.ColumnName,
                        OrderFilter = i,
                        SelectMultiple = item.SelectMultiple,
                        Required = item.Required, 
                        FilterLazy = item.FilterLazy
                    };

                    entity.KpiTemplateColumns.Add(columnEntity);
                    _repository.AddEntity(columnEntity);
                }

            // Check if we have filter customer if user type is customer
            if(_ApplicationContext.UserType == Entities.Enums.UserTypeEnum.Customer)
            {
                //check if customer filter exists
                IEnumerable<Entities.KpiColumn> itemList = null;

                if (subModuleList != null && subModuleList.Any())
                {
                    itemList = await _repository.GetListAsync<Entities.KpiColumn>(x => x.IdSubModule != null
                        && subModuleList.Select(y => y.Id).Contains(x.IdSubModule.Value)
                        && x.CanFilter && x.IsCustomerId && x.FilterDataSourceFieldCondition == null);

                    if (itemList != null && itemList.Any() && !entity.KpiTemplateColumns.Any(x => x.IdColumn != null && itemList.Select(y => y.Id).Contains(x.IdColumn.Value)))
                    {
                        foreach (var item in itemList)
                        {
                            i++;
                            var columnEntity = new Entities.KpiTemplateColumn
                            {
                                IdColumn = item.Id,
                                ColumnName = item.FieldLabel,
                                OrderFilter = i,
                                Required = item.FilterRequired
                            };

                            entity.KpiTemplateColumns.Add(columnEntity);
                            _repository.AddEntity(columnEntity);
                        }
                    }
                }

                itemList = await _repository.GetListAsync<Entities.KpiColumn>(x => x.IdModule != null
                    && request.Module.Id == x.IdModule
                    && x.CanFilter && x.IsCustomerId && x.FilterDataSourceFieldCondition == null);

                if (itemList != null && itemList.Any() && !entity.KpiTemplateColumns.Any(x => x.IdColumn != null && itemList.Select(y => y.Id).Contains(x.IdColumn.Value)))
                {
                    foreach (var item in itemList)
                    {
                        i++;
                        var columnEntity = new Entities.KpiTemplateColumn
                        {
                            IdColumn = item.Id,
                            ColumnName = item.FieldLabel,
                            OrderFilter = i,
                            Required = item.FilterRequired
                        };

                        entity.KpiTemplateColumns.Add(columnEntity);
                        _repository.AddEntity(columnEntity);
                    }
                }

            }

            // Check if we have filter location if user type is internal
            if (_ApplicationContext.UserType == Entities.Enums.UserTypeEnum.InternalUser)
            {
                //check if customer filter exists
                IEnumerable<Entities.KpiColumn> itemList = null;

                if (subModuleList != null && subModuleList.Any())
                {
                    itemList = await _repository.GetListAsync<Entities.KpiColumn>(x => x.IdSubModule != null
                    && subModuleList.Select(y => y.Id).Contains(x.IdSubModule.Value)
                    && x.CanFilter && x.IsLocationId);

                    if (itemList != null && itemList.Any() && !entity.KpiTemplateColumns.Any(x => x.IdColumn != null && itemList.Select(y => y.Id).Contains(x.IdColumn.Value)))
                    {
                        foreach (var item in itemList)
                        {
                            i++;
                            var columnEntity = new Entities.KpiTemplateColumn
                            {
                                IdColumn = item.Id,
                                ColumnName = item.FieldLabel,
                                OrderFilter = i,
                                Required = item.FilterRequired
                            };

                            entity.KpiTemplateColumns.Add(columnEntity);
                            _repository.AddEntity(columnEntity);
                        }
                    }
                }

                itemList = await _repository.GetListAsync<Entities.KpiColumn>(x => x.IdModule != null
                && request.Module.Id == x.IdModule
                && x.CanFilter && x.IsLocationId);

                if (itemList != null && itemList.Any() && !entity.KpiTemplateColumns.Any(x => x.IdColumn != null && itemList.Select(y => y.Id).Contains(x.IdColumn.Value)))
                {
                    foreach (var item in itemList)
                    {
                        i++;
                        var columnEntity = new Entities.KpiTemplateColumn
                        {
                            IdColumn = item.Id,
                            ColumnName = item.FieldLabel,
                            OrderFilter = i,
                            Required = item.FilterRequired
                        };

                        entity.KpiTemplateColumns.Add(columnEntity);
                        _repository.AddEntity(columnEntity);
                    }
                }
            }

            await _repository.Save();

            return new KpiSavetemplateResponse
            {
                Id = entity.Id,
                Result = KpiSavetemplateResult.Success
            };
        }

        private async Task<KpiSavetemplateResponse> UpdateTemplate(KpiTemplateRequest request)
        {
            var entity = await _repository.GetTemplate(request.Id);

            if (entity == null)
                return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.UnAuthorized };

            if(entity.UserId != _ApplicationContext.UserId)
            {
                if(_ApplicationContext.CustomerId <= 0)
                    return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.UnAuthorized };
              
                var users = await _userRightsManager.GetUsersIdListByCustomer(_ApplicationContext.CustomerId);

                if(users == null || !users.Contains(_ApplicationContext.UserId))
                    return new KpiSavetemplateResponse { Result = KpiSavetemplateResult.UnAuthorized };
            }                
            
            entity.IsShared = request.Shared;
            entity.UpdatedDate = DateTime.Now;

            // columns
            int i = 0; 
            foreach(var column in request.TemplateColumnList)
            {
                i++;
                Entities.KpiTemplateColumn columnEntity = null; 

                if (column.Id <= 0)
                {                    
                    columnEntity = new Entities.KpiTemplateColumn
                    {
                        IdColumn = column.IdColumn == 0 ? (int?) null : column.IdColumn,
                        ColumnName = column.ColumnName,
                        SumFooter = column.SumFooter,
                        Group = column.Group,
                        OrderColumn = i,
                        Required = false,                        
                        Valuecolumn = column.Valuecolumn
                    };

                    entity.KpiTemplateColumns.Add(columnEntity);
                    _repository.AddEntity(columnEntity);
                    continue;
                }

                columnEntity = entity.KpiTemplateColumns.FirstOrDefault(x => x.Id == column.Id);

                if (columnEntity == null)
                    continue;

                columnEntity.ColumnName = column.ColumnName;
                columnEntity.SumFooter = column.SumFooter;
                columnEntity.Group = column.Group;
                columnEntity.OrderColumn = i;
                columnEntity.Valuecolumn = column.Valuecolumn;

                _repository.EditEntity(columnEntity);
            }

            //columns to remove
            if(entity.KpiTemplateColumns.Any(x => x.IdColumnNavigation != null && x.IdColumnNavigation.CanShowInResult && !request.TemplateColumnList.Any(y => y.Id == x.Id)))
                _repository.RemoveEntities(entity.KpiTemplateColumns.Where(x => x.IdColumnNavigation != null && x.IdColumnNavigation.CanShowInResult 
                    && !request.TemplateColumnList.Any(y => y.Id == x.Id)));

            // new filters
            i = 0; 
            foreach (var filter in request.TemplateFilterList)
            {
                i++;
                Entities.KpiTemplateColumn columnEntity = null;

                if (filter.Id <= 0)
                {
                    columnEntity = new Entities.KpiTemplateColumn
                    {
                        IdColumn = filter.IdColumn,
                        ColumnName = filter.ColumnName,
                        OrderFilter = i,
                        SelectMultiple = filter.SelectMultiple,
                        Required = filter.Required,
                        FilterLazy = filter.FilterLazy
                    };

                    entity.KpiTemplateColumns.Add(columnEntity);
                    _repository.AddEntity(columnEntity);
                    continue;
                }

                columnEntity = entity.KpiTemplateColumns.FirstOrDefault(x => x.Id == filter.Id);

                if (columnEntity == null)
                    continue;

                columnEntity.ColumnName = filter.ColumnName;
                columnEntity.OrderFilter = i;
                columnEntity.SelectMultiple = filter.SelectMultiple;
                columnEntity.Required = filter.Required;
                columnEntity.FilterLazy = filter.FilterLazy;

                _repository.EditEntity(columnEntity);
            }

            //filters to remove
            if (entity.KpiTemplateColumns.Any(x => x.IdColumnNavigation != null && x.IdColumnNavigation.CanFilter  && !request.TemplateFilterList.Any(y => y.Id == x.Id)))
                _repository.RemoveEntities(entity.KpiTemplateColumns.Where(x => x.IdColumnNavigation != null && x.IdColumnNavigation.CanFilter 
                    && !request.TemplateFilterList.Any(y => y.Id == x.Id)));

            await _repository.Save();

            return new KpiSavetemplateResponse
            {
                Id = entity.Id,
                Result = KpiSavetemplateResult.Success
            };
        }

        private async Task<IEnumerable<dynamic>> GetDataSource(Entities.KpiColumn filter, string fieldName, string term)
        {
            var dict = new Dictionary<string, object>();

            if (filter.FilterIsMultiple == null || !filter.FilterIsMultiple.Value)
                return null;

            if (string.IsNullOrEmpty(filter.FilterDataSourceName))
                return null;

            if (_ApplicationContext.UserType == Entities.Enums.UserTypeEnum.Customer && filter.IsCustomerId)
            {
                if (!string.IsNullOrEmpty(filter.FilterDataSourceFieldCondition))
                    dict.Add(filter.FilterDataSourceFieldCondition, _ApplicationContext.CustomerId);
                else
                    dict.Add(filter.FilterDataSourceFieldValue, _ApplicationContext.CustomerId);
            }

            if (_ApplicationContext.UserType == Entities.Enums.UserTypeEnum.InternalUser && filter.IsLocationId && _ApplicationContext.LocationList != null)
            {
                if (!string.IsNullOrEmpty(filter.FilterDataSourceFieldCondition))
                    dict.Add(filter.FilterDataSourceFieldCondition, _ApplicationContext.LocationList);
                else
                    dict.Add(filter.FilterDataSourceFieldValue, _ApplicationContext.LocationList);
            }
                
            return await _repository.GetDataSourceByName(filter.FilterDataSourceName, fieldName, term, dict);
        }


        public async Task<ViewDataResponse> ViewResult(KpiTemplateViewRequest request)
        {
            var data = await _repository.DataSourceResult(request);

            if (data == null || data.Data == null || !data.Data.Any())
                return new ViewDataResponse { Result = ViewDataResult.NotFound };
                                  
            var result = GetRows(data.Data, data.ColumnList.ToList(), data.UseXls, request.ForExport, data.BasicData);

            if (result == null || !result.Any())
                return new ViewDataResponse { Result = ViewDataResult.NotFound };

            return new ViewDataResponse
            {
                Result = ViewDataResult.Success,
                ColumnList = request.ForExport ? data.ColumnList : null,
                Rows = result,
                UseXls = data.UseXls
            };
        }

        public async Task<IEnumerable<TemplateColumn>> GetColumnListByTemplate(int templateId)
        {
            return await _repository.GetColumnsByTemplate(templateId);
        }

        public async Task<DeleteTemplateResponse> DeleteTemplate(int id)
        {
            var item = await _repository.GetTemplate(id);

            if (item == null)
                return new DeleteTemplateResponse { Result = DeleteTemplateResult.NotFound };

            _repository.RemoveEntities(item.KpiTemplateColumns);
            _repository.RemoveEntities(item.KpiTemplateSubModules);
            _repository.RemoveEntities(new HashSet<Entities.KpiTemplate>() { item });

            try
            {
                await _repository.Save();
            }
            catch(Exception ex)
            {

            }
           

            return new DeleteTemplateResponse
            {
                Id = id,
                Result = DeleteTemplateResult.Success
            };             
            
        }

        private IEnumerable<HtmlRow> GetRows(IEnumerable<object> items, List<TemplateColumn> columns, bool useXls, bool forExport, IEnumerable<object> basicData)
        {
            var positions = new List<int>();
            var counts = new List<int>();
            var rows = AddRows(items, positions, 0, counts, columns);

            if (forExport && useXls)
                return rows;

            bool findSum = false;
            var row = new HtmlRow
            {
                Cells = new List<HtmlCell>(),
                IsSum = true
            };

           foreach(var column in columns)
           {
                if(column.SumFooter && column.Type == Entities.Enums.FieldType.Number)
                {
                    findSum = true;
                    row.Cells.Add(new HtmlCell
                    {
                        Type = Entities.Enums.FieldType.Number,
                        Value = basicData.Sum(x => GetElement<decimal>(x, column.FieldName)).ToString()
                    });
                }
                else
                {
                    row.Cells.Add(new HtmlCell
                    {
                        Value = ""
                    });
                }
           }

            if (findSum)
            {
                var cells = new List<HtmlCell>();
                int beginCellsToMerge = 0;
                int endCellsToMerge = -1;
                bool lastHasValue = false;
                bool addTotalTitle = false;

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if(string.IsNullOrWhiteSpace(row.Cells[i].Value))
                    {
                        endCellsToMerge = i;

                        if (lastHasValue)
                            beginCellsToMerge = i;

                        lastHasValue = false;

                        continue; 
                    }

                    // merge the last Cells
                    if (beginCellsToMerge >= 0 && endCellsToMerge >= 0)
                        cells.Add(new HtmlCell
                        {
                            Value = addTotalTitle ? "" : "Total",
                            ColSpan = endCellsToMerge - beginCellsToMerge +1,
                            Type = Entities.Enums.FieldType.String
                        });

                    // Add current cell
                    cells.Add(row.Cells[i]);

                    // init;
                    beginCellsToMerge = -1;
                    endCellsToMerge = -1;
                    addTotalTitle = true;
                    lastHasValue = true;
                }

                row.Cells = cells; 
                rows.Add(row);
            }               
        
          return rows;
        }

        private  T GetElement<T>(object item, string fieldName)
        {
            var dictItem = new RouteValueDictionary(item);

            if (!dictItem.TryGetValue(fieldName, out object result))
                return default; 

            
            return(T) Convert.ChangeType(result, typeof(T));

        }

        private List<HtmlRow> AddRows(IEnumerable<object> items, List<int> positions, int pos, List<int> counts, List<TemplateColumn> columns)
        {

            var list = new List<HtmlRow>();

            foreach (var item in items)
            {
                var dictItem = new RouteValueDictionary(item);

                if (IsEnumerable(dictItem.First().Value, out IEnumerable<object> itemList))
                {

                    if (positions.Count > pos)
                    {
                        for (int i = pos; i < positions.Count; i++)
                            positions[i] = 0;
                    }
                    else
                        positions.Add(0);

                    int count = this.GetCount(itemList);

                    if (counts.Count > pos)
                    {
                        for (int i = pos; i < counts.Count; i++)
                            counts[i] = count;
                    }
                    else
                        counts.Add(count);


                    list.AddRange(this.AddRows(itemList, positions, (pos + 1), counts, columns));
                }
                else
                {

                    var tr = new HtmlRow { Cells = new List<HtmlCell>() };

                    for (int i = 0; i < columns.Count(); i++)
                    {
                        TemplateColumn column = columns[i];

                        if (positions.Count > i)
                        {
                            int posIndex = positions[i];

                            if (posIndex == 0)
                                tr.Cells.Add(new HtmlCell { RowSpan = counts[i], Value = dictItem[column.FieldName].ToString(), Type = column.Type });

                            posIndex++;
                            positions[i] = posIndex;
                        }
                        else
                            tr.Cells.Add(new HtmlCell { Value = dictItem[column.FieldName].ToString(), Type = column.Type });

                    }

                    list.Add(tr);
                }
            }

            return list;
        }

        private bool IsEnumerable(object obj, out IEnumerable<object> result)
        {
            result = null;

            if (obj == null)
                return false;

            if (obj.GetType() == typeof(string))
                return false;

            var itemList = obj as IEnumerable;

            if (itemList == null)
                return false;

            result = itemList.Cast<object>();

            return true;
        }

        private int GetCount(IEnumerable<object> items)
        {

            int count = 0;

            foreach (var item in items)
            {
                var dictItem = new RouteValueDictionary(item);

                if (IsEnumerable(dictItem.First().Value, out IEnumerable<object> itemList))
                    count = count + this.GetCount(itemList);
                else
                    count = items.Count();
            }

            return count;
        }
    }
}
