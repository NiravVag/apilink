using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class StaffDetails
    {
        public int Id { get; set;  }

        public int UserId { get; set;  }

        public string EmployeeNo { get; set; }

        public DateObject DateBirth { get; set; }

        public int? CountryId { get; set; }

        public string CountryName { get; set; }

        public string StaffName { get; set; }

        public int? QualificationId { get; set; }

        public string LocalLanguage {get ;set; }

        public string Graduate { get; set;  }

        public string EmergencyContact { get; set;  }

        public string Gender { get; set;  }

        public DateObject GraduateDate { get; set;  }

        public string EmergencyContactPhone { get; set;  }

        public string Martial { get; set;  }
        
        public string PassportNo { get; set;  }

        public string SkypeId { get; set;  }

        public string Email { get; set; }

        public string Phone { get; set;  }

        public DateObject JoinDate { get; set;  }

        public int? PositionId { get; set; }

        public string CompanyEmail { get; set;  }

        public string BankName { get; set;  }
        
        public int? ReportHeadId { get; set;  }

        public int? ManagerId { get; set;  }

        public string CompanyMobile { get; set;  }

        public string BankAccount { get; set;  }
        [RequiredGreaterThanZero(ErrorMessage = "EDIT_STAFF.EMPLOYEE_TYPE_REQ")]
        public int EmployeeTypeId { get; set;  }

        public string AnnualLeave { get; set;  }

        public string AssCardNo { get; set;  }

        public int? OfficeId { get; set;  }

        public DateObject ProbExpDate { get; set;  }

        public string HousingFundCard { get; set;  }

        public int QcStartPlaceId { get; set;  }

        public int? ProbatonPeriod { get; set;  }

        public string PlacePurchSiHf { get; set;  }

        public int? DepartmentId { get; set;  }

        public int? SubDepartmentId { get; set;  }

        public IEnumerable<int> OpCountryValues { get; set;  }

        public IEnumerable<int> ProfileValues { get; set;  }

        public IEnumerable<int> ApiEntityIds { get; set; }

        public IEnumerable<int> ApiServiceIds { get; set; }

        public IEnumerable<int> MarketSegmentValues { get; set;  }

        public IEnumerable<int> ProductCategoryValues { get; set;  }

        public IEnumerable<int> ExpertiseValues { get; set;  }

        public int? PayrollCurrencyId { get; set;  }
        
        public bool IsAuditor { get; set;  }

        public int? HomeCountryId { get; set;  }

        public int? HomeStateId { get; set;  }
         
        public int? HomeCityId { get; set;  }

        public string HomeAddress { get; set;  }

        public int? CurrentCountryId { get; set;  }

        public int? CurrentStateId { get; set;  }

        public int? CurrentCityId { get; set;  }

        public int? CurrentCountyId { get; set; }

        public string CurrentAddress { get; set;  }

        public DateObject StartWkDate { get; set;  }

        public int? WorkingYears { get; set;  }

        public int? TotWkYearsGarment { get; set;  }

       public bool HasServerPicture { get; set;  }
                 
        public bool HomeIsCurrent { get; set;  }

        public bool? IsForecastApplicable { get; set; }

        public IEnumerable<RenewModel> RenewList { get; set;  }

        public IEnumerable<JobModel> JobList { get; set;  }

        public IEnumerable<TrainingModel> TrainingList { get; set;  }

        public IEnumerable<AttachedFileModel> AttachedList { get; set;  }

        public string StatusName { get; set; }

        public int? StatusId { get; set; }

        public string MajorSubject { get; set; }

        public string EmergencyContactRelationship { get; set; }

        public string GlobalGrading { get; set; }

        public int? NoticePeriod { get; set; }

        public int? BandId { get; set; }

        public int? SocialInsuranceTypeId { get; set; }

        public int? HukoLocationId { get; set; }

        public IEnumerable<EntityService> EntityServiceIds { get; set; }

        public int PrimaryEntity { get; set; }

        public int? StartPortId { get; set; }

        public int? HrOutSourceCompanyId { get; set; }

        public HrPhotoModel HrPhoto { get; set; }
        public int? CompanyId { get; set; }
        public int? PayrollCompany { get; set; }
    }
    public class HrPhotoModel
    {
        public Guid? GuidId { get; set; }
        public string FileName { get; set; }
        public string UniqueId { get; set; }
        public string FileUrl { get; set; }
        public int UserId { get; set; }
    }

    public class EntityService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ServiceId { get; set; }
        public int? EntityId { get; set; }
    }

    public class RenewModel
    {
        public int Id { get; set;  }

        public DateObject StartDate { get; set;  }

        public DateObject EndDate { get; set;  }
    }

    public class JobModel
    {
        public int Id { get; set;  }

        public string Company { get; set;  }

        public string Position { get; set;  }

        public double? Salary { get; set;  }

        public int? CurrencyId { get; set;  }

        public DateObject StartDate { get; set;  }

        public DateObject EndDate { get; set;  }
    }

    public class TrainingModel
    {
        public int Id { get; set;  }

        public string TrainingTopic { get; set;  }

        public DateObject StartDate { get; set;  }

        public DateObject EndDate { get; set;  }

        public string Trainer { get; set;  }

        public string Comments { get; set; }

    }

    public class AttachedFileModel
    {
        public int Id { get; set; }
        public string FileName { get; set;  }

        public int FileTypeId { get; set;  }

        public int UserId { get; set;  }

        public string UserName { get; set;  }

        public string UploadedDate { get; set;  }
        
       // public byte[] File { get; set;  }

        public bool IsNew { get; set;  }

        public string MimeType { get; set;  }

        public string UniqueId { get; set; }
        public string FileUrl { get; set; }
    }

}
