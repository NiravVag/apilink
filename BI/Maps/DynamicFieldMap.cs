using DTO.Common;
using DTO.DynamicFields;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public  class DynamicFieldMap: ApiCommonData
    {
        public  APIModule GetModules(RefModule entity)
        {
            if (entity == null)
                return null;
            return new APIModule
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active
            };
        }

        public  ControlType GetControlTypes(DfControlType entity)
        {
            if (entity == null)
                return null;
            return new ControlType
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active
            };
        }

        public  DDLSourceType GetDDLSourceTypes(DfDdlSourceType entity)
        {
            if (entity == null)
                return null;
            return new DDLSourceType
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active
            };
        }

        public  DDLSource GetDDLSource(DfDdlSource entity)
        {
            if (entity == null)
                return null;
            return new DDLSource
            {
                Id = entity.Id,
                Name = entity.Name,
                Active = entity.Active
            };
        }

        public static DfCuConfiguration MapDFCustomerConfigurationEntity(DfCustomerConfiguration request, int userId,int? entityId)
        {
            if (request == null)
                return null;

            var entity = new DfCuConfiguration
            {
                Id = request.Id,
                CustomerId = request.CustomerId,
                ModuleId = request.ModuleId,
                ControlTypeId = request.ControlTypeId,
                Label = request.Label,
                Type = request.Type,
                DataSourceType = request.DataSourceType,
                DisplayOrder = request.DisplayOrder,
                Fbreference=request.FbReference,
                EntityId= entityId,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            };

            return entity;
        }

        public static DfCuConfiguration UpdateDFCustomerConfigurationEntity(DfCustomerConfiguration request, DfCuConfiguration entity, int userId,int? entityId)
        {
            if (request == null)
                return null;

            entity.Id = request.Id;
            entity.CustomerId = request.CustomerId;
            entity.ModuleId = request.ModuleId;
            entity.ControlTypeId = request.ControlTypeId;
            entity.Label = request.Label;
            entity.Type = request.Type;
            //entity.DataType = request.DataType;
            entity.DataSourceType = request.DataSourceType;
            entity.DisplayOrder = request.DisplayOrder;
            entity.Fbreference = request.FbReference;
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.EntityId = entityId;

            return entity;
        }

        public  DfControlAttribute MapDFControlAttributes(DfControlAttributes request)
        {
            var dfControlAttribute = new DfControlAttribute
            {
                Id = request.Id,
                ControlAttributeId = request.ControlAttributeId,
                //Key = request.Key,
                Value = request.Value,
                Active = true,
            };
            return dfControlAttribute;
        }

        public  DfCustomerConfiguration MapDfCustomerConfigurationRequest(DfCuConfiguration entity)
        {
            if (entity == null)
                return null;

            var customerConfiguration = new DfCustomerConfiguration
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                ModuleId = entity.ModuleId,
                ControlTypeId = entity.ControlTypeId,
                Label = entity.Label,
                Type = entity.Type,
                //DataType = entity.DataType,
                DataSourceType = entity.DataSourceType,
                DisplayOrder = entity.DisplayOrder,
                ControlAttributeList = entity.DfControlAttributes.Select(GetDfControlAttribute),
                DDLSourceList = entity.DataSourceTypeNavigation?.DfDdlSources?.Select(GetDfDDLSource),
                Active = true,
            };

            return customerConfiguration;
        }

        public  DfControlAttributes GetDfControlAttribute(DfControlAttribute entity)
        {
            if (entity == null)
                return null;

            return new DfControlAttributes
            {
                Id = entity.Id,
                ControlAttributeId = entity.ControlAttributeId,
                AttributeId = entity.ControlAttribute?.AttributeId,
                Value = entity.Value,
                Active = entity.Active,
            };

        }

        public  DfDDLSource GetDfDDLSource(DfDdlSource entity)
        {
            if (entity == null & entity.ParentId != null)
                return null;

            return new DfDDLSource
            {
                Id = entity.Id,
                Name = entity.Name,
                ParentId = entity.ParentId,
                Active = entity.Active,
            };

        }

        public  DfControlTypeAttributes GetDfControlAttribute(DfControlTypeAttribute entity)
        {
            if (entity == null)
                return null;
            return new DfControlTypeAttributes
            {
                Id = entity.Id,
                //Name = entity.Name,
                //Value=entity.Value,
                Active = entity.Active
            };
        }

        public  DfControlTypeAttributes GetDfControlsAttribute(DfControlTypeAttribute entity)
        {
            if (entity == null)
                return null;

            return new DfControlTypeAttributes
            {
                Id = entity.Id,
                Name = entity.Attribute?.Name,
                DataType = entity.Attribute?.DataType,
                DefaultValue = entity.DefaultValue,
                ControlTypeId = entity.ControlTypeId,
                AttributeId = entity.AttributeId,
                Active = entity.Active,
            };

        }

        public  EditDfCustomerConfiguration MapEditDfCustomerConfigData(DfCustomerConfigBaseData baseData
                                                , List<EditDfControlAttributes> attributeList,bool IsBooking)
        {
            var data = new EditDfCustomerConfiguration();
            if (baseData != null && attributeList != null && attributeList.Any())
            {
                data.Id = baseData.Id;
                data.CustomerId = baseData.CustomerId;
                data.ModuleId = baseData.ModuleId;
                data.ControlTypeId = baseData.ControlTypeId;
                data.Label = baseData.Label;
                data.Type = baseData.Type;
                data.DataType = baseData.DataType;
                data.DataSourceType = baseData.DataSourceType;
                data.DisplayOrder = baseData.DisplayOrder;
                data.IsBooking = IsBooking;
                data.FbReference = baseData.FbReference;
                data.ControlAttributeList = attributeList;
            }
            
            return data;
        }

        public  List<InspectionBookingDFData> MapBookingDFData(List<InspectionBookingDFRepo> bookingDFList, List<DFDataSourceRepo> dataSourceList)
        {
            var bookingDFDataList = new List<InspectionBookingDFData>();

            //Process non dropdown control values
            //var bookingDFNonDropDownDataList = bookingDFList.Where(x => x.ControlType != (int)DfControlTypeEnum.DropDown);
            foreach (var bookingData in bookingDFList)
            {
                InspectionBookingDFData data = new InspectionBookingDFData();
                data.BookingNo = bookingData.BookingNo;
                data.DFName = bookingData.DFName;
                data.FbReference = bookingData.FbReference;
                data.ControlConfigId = bookingData.ControlConfigurationId;
                //if control is datepicker format the date
                if (bookingData.ControlType == (int)DfControlTypeEnum.DatePicker)
                {
                    data.DFValue = bookingData.DFValue!=null?Convert.ToDateTime(bookingData.DFValue).ToString(StandardDateFormat):"";
                }
                if (bookingData.ControlType == (int)DfControlTypeEnum.DropDown)
                {
                    data.DFValue = dataSourceList.Where(x => x.Id == Convert.ToInt32(bookingData.DFValue)).FirstOrDefault()?.Name;
                }
                else
                {
                    data.DFValue = bookingData.DFValue;
                }
                bookingDFDataList.Add(data);
            }
            return bookingDFDataList;
        }

    }
}
