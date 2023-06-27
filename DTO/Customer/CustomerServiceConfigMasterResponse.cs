using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServiceConfigMasterResponse
    {
        public IEnumerable<Service> ServiceList { get; set; }        public IEnumerable<ServiceType> ServiceTypeList { get; set; }        public IEnumerable<ProductCategory> ProductCategoryList { get; set; }        public IEnumerable<PickType> PickTypeList { get; set; }        public IEnumerable<LevelPick1> LevelPick1List { get; set; }        public IEnumerable<LevelPick2> LevelPick2List { get; set; }        public IEnumerable<Pick1> Pick1List { get; set; }        public IEnumerable<Pick2> Pick2List { get; set; }        public IEnumerable<DefectClassification> DefectClassificationList { get; set; }        public IEnumerable<ReportUnit> ReportUnitList { get; set; }
        public CustomerServiceConfigMasterResult Result { get; set; }
        public IEnumerable<DpPoint> DpPointList { get; set; }
    }

    public enum CustomerServiceConfigMasterResult    {        Success = 1,        CannotGetService = 2,        CannotGetServiceType = 3,        CannotGetProductCategory= 4,        CannotGetPickType = 5,        CannotGetLevelPick1=6,        CannotGetLevelPick2=7,        CannotGetPick1=8,        CannotGetPick2=9,        CannotGetDefectClassification=10,        CannotGetReportUnit=11,        CannotGetDpPoint = 12,    }
}
