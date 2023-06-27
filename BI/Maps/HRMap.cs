using DTO;
using DTO.Audit;
using DTO.HumanResource;
using DTO.Location;
using DTO.References;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DTO.Common;
namespace BI.Maps
{
    public class HRMap : ApiCommonData
    {

        public Position GetPosition(HrPosition entity)
        {
            if (entity == null)
                return null;

            return new Position
            {
                Id = entity.Id,
                Name = entity.PositionName
            };

        }
        public Auditor GetAuditor(HrStaff entity)
        {
            if (entity == null)
                return null;
            return new Auditor
            {
                Id = entity.Id,
                Name = entity.PersonName
            };
        }

        public CustomerCS GetCustomerCS(HrStaff entity)
        {
            if (entity == null)
                return null;
            return new CustomerCS
            {
                Id = entity.Id,
                Name = entity.PersonName
            };
        }
        public Department GetDepartment(HrDepartment entity)
        {
            if (entity == null)
                return null;
            return new Department
            {
                Id = entity.Id,
                Name = entity.DepartmentName,
                DepartParentId = entity.DeptParentId
            };
        }

        public StaffItem MapStaffItem(HrStaff entity)
        {
            if (entity == null)
                return null;

            return new StaffItem
            {
                CountryName = entity.NationalityCountry?.CountryName,
                DepartmentName = entity.Department?.DepartmentName,
                EmployeeType = entity.EmployeeType?.EmployeeTypeName,
                JoinDate = entity.JoinDate?.ToString(StandardDateFormat),
                OfficeName = entity.Location?.LocationName,
                PositionName = entity.Position?.PositionName,
                StaffName = entity.PersonName,
                Id = entity.Id,
                StatusName = entity?.Status?.Name,
                StatusId = entity?.StatusId
            };

        }

        public StaffInfo GetStaffInfo(HrStaff entity)
        {
            if (entity == null)
                return null;

            return new StaffInfo
            {
                Id = entity.Id,
                CurrencyId = entity.PayrollCurrencyId,
                CurrencyName = entity.PayrollCurrency?.CurrencyName,
                LocationId = entity.LocationId,
                LocationName = entity.Location?.LocationName,
                StaffName = entity.PersonName,
                CountryId = entity.NationalityCountryId == null ? 0 : entity.NationalityCountryId.Value,
                Email = entity.CompanyEmail,
                UserTypeId = entity.ItUserMasters.Select(x => x.UserTypeId).FirstOrDefault(),
                EmployeeTypeId = entity.EmployeeTypeId
            };

        }

        public EmployeeType GetEmployeeType(HrEmployeeType entity)
        {
            return new EmployeeType
            {
                Id = entity.Id,
                Name = entity.EmployeeTypeName
            };
        }

        public Qualification GetQualification(HrQualification entity)
        {
            if (entity == null)
                return null;

            return new Qualification
            {
                Id = entity.Id,
                Name = entity.QualificationName
            };

        }

        public Profile GetProfile(HrProfile entity)
        {
            if (entity == null)
                return null;

            return new Profile
            {
                Id = entity.Id,
                Name = entity.ProfileName
            };
        }

        public MarketSegment GetMarketSegment(RefMarketSegment entity)
        {
            if (entity == null)
                return null;

            return new MarketSegment
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public ProductCategory GetProductCategory(RefProductCategory entity)
        {
            if (entity == null)
                return null;

            return new ProductCategory
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Expertise GetExpertise(RefExpertise entity)
        {
            if (entity == null)
                return null;

            return new Expertise
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
        public FileType GetFileType(HrFileType entity)
        {
            if (entity == null)
                return null;

            return new FileType
            {
                Id = entity.Id,
                Name = entity.FileTypeName
            };
        }


        public StaffDetails GetStaffDetails(HrStaff entity, Func<string, string> _funcGetMimeType, bool hasPhoto)
        {
            if (entity == null)
                return null;

            int? deptId = entity.Department?.DeptParentId;

            if (deptId == null)
                deptId = entity.DepartmentId;

            return new StaffDetails
            {
                Id = entity.Id,
                StaffName = entity.PersonName?.Trim(),
                AnnualLeave = entity.AnnualLeave?.Trim(),
                AssCardNo = entity.SocialInsuranceCardNo?.Trim(),
                BankAccount = entity.BankAccountNo?.Trim(),
                BankName = entity.BankName?.Trim(),
                CompanyEmail = entity.CompanyEmail?.Trim(),
                CompanyMobile = entity.CompanyMobileNo?.Trim(),
                CountryId = entity.NationalityCountryId,
                CountryName = entity.NationalityCountry?.CountryName,
                CurrentAddress = entity.CurrentAddress?.Trim(),
                CurrentCityId = entity.CurrentCityId,
                CurrentCountryId = entity.CurrentCity?.Province?.CountryId,
                CurrentCountyId = entity.CurrentCountyId == null ? null : entity.CurrentCountyId,
                CurrentStateId = entity.CurrentCity?.ProvinceId,
                DateBirth = Static_Data_Common.GetCustomDate(entity.BirthDate),
                DepartmentId = deptId,
                SubDepartmentId = entity.Department?.DeptParentId == null ? null : entity.DepartmentId,
                Email = entity.EmaiLaddress?.Trim(),
                EmergencyContact = entity.EmergencyContactName?.Trim(),
                EmergencyContactPhone = entity.EmergencyContactPhone?.Trim(),
                EmployeeNo = entity.EmpNo?.Trim(),
                EmployeeTypeId = entity.EmployeeTypeId,
                Gender = entity.Gender?.Trim(),
                Graduate = entity.GraduateSchool?.Trim(),
                GraduateDate = Static_Data_Common.GetCustomDate(entity.GraduateDate),
                HomeAddress = entity.HomeAddress?.Trim(),
                HomeCityId = entity.HomeCityId,
                HomeCountryId = entity.HomeCity?.Province?.CountryId,
                HomeIsCurrent = !string.IsNullOrEmpty(entity.HomeAddress) && entity.HomeAddress == entity.CurrentAddress,
                HomeStateId = entity.HomeCity?.ProvinceId,
                HousingFundCard = entity.HousingFuncard,
                JoinDate = Static_Data_Common.GetCustomDate(entity.JoinDate),
                LocalLanguage = entity.LocalLanguage?.Trim(),
                Martial = entity.MaritalStatus?.Trim(),
                OfficeId = entity.LocationId,
                PassportNo = entity.PassportNo?.Trim(),
                PayrollCurrencyId = entity.PayrollCurrencyId,
                Phone = entity.EmergencyCall,
                PlacePurchSiHf = entity.PlacePurchasingSihf,
                PositionId = entity.PositionId,
                ProbatonPeriod = entity.ProbationPeriod,
                ProbExpDate = Static_Data_Common.GetCustomDate(entity.ProbationExpiredDate),
                QualificationId = entity.QualificationId,
                ReportHeadId = entity.ParentStaffId,
                ManagerId = entity.ManagerId,
                SkypeId = entity.SkypeId,
                StartWkDate = Static_Data_Common.GetCustomDate(entity.StartWorkingDate),
                TotWkYearsGarment = entity.TotalWorkingYears,
                WorkingYears = entity.TotalWorkingYears,
                HasServerPicture = hasPhoto,
                IsForecastApplicable = entity.IsForecastApplicable,
                StatusName = entity?.Status?.Name,
                StatusId = entity.StatusId.GetValueOrDefault(),
                PrimaryEntity = entity.PrimaryEntity.GetValueOrDefault(),
                MajorSubject = entity.MajorSubject,
                EmergencyContactRelationship = entity.EmergencyContactRelationship,
                GlobalGrading = entity.GlobalGrading,
                BandId = entity.BandId,
                SocialInsuranceTypeId = entity.SocialInsuranceTypeId,
                HukoLocationId = entity.HukoLocationId,
                NoticePeriod = entity.NoticePeriod,
                AttachedList = entity.HrAttachments?.Where(x => x.Active)?.Select(x => new AttachedFileModel
                {
                    Id = x.Id,
                    UniqueId = x.UniqueId,
                    FileUrl = x.FileUrl,
                    FileTypeId = x.FileTypeId,
                    FileName = x.FileName,
                    UserId = x.UserId,
                    UserName = x.User?.FullName,
                    UploadedDate = x.UploadDate.ToString(StandardDateFormat),
                    MimeType = _funcGetMimeType(Path.GetExtension(x.FileName))
                }).ToArray(),
                HrPhoto = entity.HrPhotos?.Where(x => x.Active)?.Select(x => new HrPhotoModel
                {
                    GuidId = x.GuidId,
                    FileName = x.FileName,
                    UserId = x.UserId,
                    UniqueId = x.UniqueId,
                    FileUrl = x.FileUrl
                }).FirstOrDefault(),
                EntityServiceIds = entity.HrStaffEntityServiceMaps?.Select(x => new EntityService
                {
                    Id = x.Id,
                    EntityId = x.EntityId,
                    ServiceId = x.ServiceId,
                    Name = x.Entity.Name + "(" + x.Service.Name + ")"
                }).ToArray(),
                OpCountryValues = entity.HrStaffOpCountries?.Select(x => x.CountryId).ToArray(),
                ProfileValues = entity.HrStaffProfiles?.Select(x => x.ProfileId).ToArray(),
                ApiEntityIds = entity.HrEntityMaps?.Select(x => x.EntityId).ToArray(),
                ApiServiceIds = entity.HrStaffServices?.Select(x => x.ServiceId.GetValueOrDefault()).ToArray(),
                MarketSegmentValues = entity.HrStaffMarketSegments?.Select(x => x.MarketSegmentId).ToArray(),
                ProductCategoryValues = entity.HrStaffProductCategories?.Select(x => x.ProductCategoryId).ToArray(),
                ExpertiseValues = entity.HrStaffExpertises?.Select(x => x.ExpertiseId).ToArray(),
                RenewList = entity.HrRenews?.Select(x => new RenewModel
                {
                    Id = x.Id,
                    StartDate = Static_Data_Common.GetCustomDate(x.StartDate),
                    EndDate = Static_Data_Common.GetCustomDate(x.EndDate),
                }).ToArray(),
                JobList = entity.HrStaffHistories.Select(x => new JobModel
                {
                    Id = x.Id,
                    Company = x.Company,
                    CurrencyId = x.CurrencyId,
                    EndDate = Static_Data_Common.GetCustomDate(x.DateEnd),
                    Position = x.Position,
                    Salary = x.Salary,
                    StartDate = Static_Data_Common.GetCustomDate(x.Datebegin),

                }).ToArray(),
                TrainingList = entity.HrStaffTrainings.Select(x => new TrainingModel
                {
                    Id = x.Id,
                    Comments = x.Comment,
                    StartDate = Static_Data_Common.GetCustomDate(x.DateStart),
                    EndDate = Static_Data_Common.GetCustomDate(x.DateEnd),
                    Trainer = x.Trainer,
                    TrainingTopic = x.TrainingTopic
                }).ToArray(),
                StartPortId = entity.StartPortId,
                HrOutSourceCompanyId = entity.HroutSourceCompanyId,
                CompanyId = entity.CompanyId.GetValueOrDefault(),
                PayrollCompany = entity.PayrollCompany.GetValueOrDefault()
            };

        }
        public HrStaff MapStaffEntity(StaffDetails request, int entityId, int userId)
        {
            if (request == null)
                return null;

            var staff = new HrStaff
            {
                Active = true,
                AnnualLeave = request.AnnualLeave?.Trim(),
                BankAccountNo = request.BankAccount?.Trim(),
                BankName = request.BankName?.Trim(),
                BirthDate = request.DateBirth?.ToDateTime(),
                CompanyEmail = request.CompanyEmail?.Trim(),
                CompanyMobileNo = request.CompanyMobile?.Trim(),
                CreatedAt = DateTime.Now,
                DepartmentId = request.SubDepartmentId == null || request.SubDepartmentId == 0 ? SetIntDefaultValue(request.DepartmentId) : request.SubDepartmentId,
                EmaiLaddress = request.Email?.Trim(),
                EmergencyCall = request.Phone?.Trim(),
                EmergencyContactName = request.EmergencyContact?.Trim(),
                EmergencyContactPhone = request.EmergencyContactPhone?.Trim(),
                EmployeeTypeId = request.EmployeeTypeId,
                EmpNo = request.EmployeeNo?.Trim(),
                GraduateDate = request.GraduateDate?.ToDateTime(),
                GraduateSchool = request.Graduate?.Trim(),
                Gender = request.Gender?.Trim(),
                HousingFuncard = request.HousingFundCard?.Trim(),
                JoinDate = request.JoinDate?.ToDateTime(),
                LaborContractExpiredDate = request.ProbExpDate?.ToDateTime(),
                LocationId = SetIntDefaultValue(request.OfficeId),
                LocalLanguage = request.LocalLanguage?.Trim(),
                ManagerId = SetIntDefaultValue(request.ManagerId),
                MaritalStatus = request.Martial,
                NationalityCountryId = SetIntDefaultValue(request.CountryId),
                PayrollCurrencyId = SetIntDefaultValue(request.PayrollCurrencyId),
                PersonName = request.StaffName?.Trim(),
                PassportNo = request.PassportNo?.Trim(),
                ParentStaffId = SetIntDefaultValue(request.ReportHeadId),
                PositionId = SetIntDefaultValue(request.PositionId),
                PlacePurchasingSihf = request.PlacePurchSiHf?.Trim(),
                ProbationExpiredDate = request.ProbExpDate?.ToDateTime(),
                ProbationPeriod = request.ProbatonPeriod,
                QualificationId = SetIntDefaultValue(request.QualificationId),
                SkypeId = request.SkypeId,
                SocialInsuranceCardNo = request.AssCardNo?.Trim(),
                StartWorkingDate = request.StartWkDate?.ToDateTime(),
                TotalWorkingYears = request.WorkingYears,
                CreatedBy = request.UserId,
                HomeCityId = SetIntDefaultValue(request.HomeCityId),
                HomeAddress = request.HomeAddress?.Trim(),
                CurrentCityId = request.HomeIsCurrent ? SetIntDefaultValue(request.HomeCityId) : request.CurrentCityId,
                CurrentCountyId = request.CurrentCountyId,
                CurrentAddress = request.HomeIsCurrent ? request.HomeAddress : request.CurrentAddress,
                IsForecastApplicable = request.IsForecastApplicable,
                BandId = request.BandId,
                HukoLocationId = request.HukoLocationId,
                SocialInsuranceTypeId = request.SocialInsuranceTypeId,
                StatusId = (int)StaffStatus.OnJob,
                MajorSubject = request.MajorSubject,
                EmergencyContactRelationship = request.EmergencyContactRelationship,
                GlobalGrading = request.GlobalGrading,
                NoticePeriod = request.NoticePeriod,
                PrimaryEntity = request.PrimaryEntity,
                StartPortId = request.StartPortId,
                HroutSourceCompanyId = request.HrOutSourceCompanyId,
                CompanyId = request.CompanyId,
                PayrollCompany = request.PayrollCompany
            };

            if (request.MarketSegmentValues != null)
            {
                staff.HrStaffMarketSegments = new HashSet<HrStaffMarketSegment>();
                foreach (var item in request.MarketSegmentValues)
                {
                    staff.HrStaffMarketSegments.Add(new HrStaffMarketSegment
                    {
                        MarketSegmentId = item
                    });
                }

            }

            if (request.ProductCategoryValues != null)
            {
                staff.HrStaffProductCategories = new HashSet<HrStaffProductCategory>();
                foreach (var item in request.ProductCategoryValues)
                {
                    staff.HrStaffProductCategories.Add(new HrStaffProductCategory
                    {
                        ProductCategoryId = item
                    });
                }
            }

            if (request.ExpertiseValues != null)
            {
                staff.HrStaffExpertises = new HashSet<HrStaffExpertise>();
                foreach (var item in request.ExpertiseValues)
                {
                    staff.HrStaffExpertises.Add(new HrStaffExpertise
                    {
                        ExpertiseId = item
                    });
                }
            }

            if (request.ProfileValues != null)
            {
                staff.HrStaffProfiles = new HashSet<HrStaffProfile>();
                foreach (var item in request.ProfileValues)
                {
                    staff.HrStaffProfiles.Add(new HrStaffProfile
                    {
                        ProfileId = item
                    });
                }
            }
            // Added entity mapping for user level
            if (request.ApiEntityIds != null)
            {
                staff.HrEntityMaps = new HashSet<HrEntityMap>();
                foreach (var item in request.ApiEntityIds)
                {
                    staff.HrEntityMaps.Add(new HrEntityMap
                    {
                        EntityId = item
                    });
                }
            }

            if (request.ApiServiceIds != null)
            {
                staff.HrStaffServices = new HashSet<HrStaffService>();
                foreach (var item in request.ApiServiceIds)
                {
                    staff.HrStaffServices.Add(new HrStaffService
                    {
                        ServiceId = item,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        CreatedBy = userId
                    });
                }
            }

            if (request.EntityServiceIds != null)
            {
                staff.HrStaffEntityServiceMaps = new HashSet<HrStaffEntityServiceMap>();
                foreach (var item in request.EntityServiceIds)
                {
                    staff.HrStaffEntityServiceMaps.Add(new HrStaffEntityServiceMap
                    {
                        ServiceId = item.ServiceId,
                        EntityId = item.EntityId,
                    });
                }
            }
            else if (request.ApiEntityIds != null && request.ApiServiceIds != null)
            {
                foreach (var apiEntityId in request.ApiEntityIds)
                {
                    foreach (var serviceId in request.ApiServiceIds)
                    {
                        staff.HrStaffEntityServiceMaps.Add(new HrStaffEntityServiceMap
                        {
                            EntityId = entityId,
                            ServiceId = serviceId
                        });
                    }
                }
            }

            if (request.OpCountryValues != null)
            {
                staff.HrStaffOpCountries = new HashSet<HrStaffOpCountry>();
                foreach (var item in request.OpCountryValues)
                {
                    staff.HrStaffOpCountries.Add(new HrStaffOpCountry
                    {
                        CountryId = item
                    });
                }
            }

            if (request.RenewList != null)
            {
                int i = 0;
                staff.HrRenews = new HashSet<HrRenew>();
                foreach (var item in request.RenewList.Where(x => x.StartDate != null && x.EndDate != null))
                {
                    i++;
                    staff.HrRenews.Add(new HrRenew
                    {
                        StartDate = item.StartDate.ToDateTime(),
                        EndDate = item.EndDate.ToDateTime(),
                        Number = i
                    });
                }
            }

            if (request.TrainingList != null)
            {
                staff.HrStaffTrainings = new HashSet<HrStaffTraining>();
                foreach (var item in request.TrainingList.Where(x => !string.IsNullOrEmpty(x.TrainingTopic)))
                {
                    staff.HrStaffTrainings.Add(new HrStaffTraining
                    {
                        TrainingTopic = item.TrainingTopic,
                        Comment = item.Comments,
                        DateStart = item.StartDate?.ToDateTime(),
                        DateEnd = item.EndDate?.ToDateTime(),
                        Trainer = item.Trainer
                    });
                }
            }

            if (request.JobList != null)
            {
                staff.HrStaffHistories = new HashSet<HrStaffHistory>();
                foreach (var item in request.JobList.Where(x => !string.IsNullOrEmpty(x.Company)))
                {
                    staff.HrStaffHistories.Add(new HrStaffHistory
                    {
                        Company = item.Company,
                        CurrencyId = SetIntDefaultValue(item.CurrencyId),
                        Datebegin = item.StartDate?.ToDateTime(),
                        DateEnd = item.EndDate?.ToDateTime(),
                        Position = item.Position,
                        Salary = item.Salary
                    });

                }
            }

            if (request.AttachedList != null)
            {
                staff.HrAttachments = new HashSet<HrAttachment>();
                foreach (var item in request.AttachedList.Where(x => x.FileTypeId > 0 && !string.IsNullOrEmpty(x.UniqueId)))
                {
                    staff.HrAttachments.Add(new HrAttachment
                    {
                        FileTypeId = item.FileTypeId,
                        FileName = item.FileName,
                        GuidId = Guid.NewGuid(),
                        UniqueId = item.UniqueId,
                        FileUrl = item.FileUrl,
                        Active = true,
                        UploadDate = DateTime.Now,
                        UserId = request.UserId
                    });

                }
            }

            if (request.HrPhoto != null && !string.IsNullOrEmpty(request.HrPhoto.UniqueId))
            {
                staff.HrPhotos.Add(new HrPhoto
                {
                    FileName = request.HrPhoto.FileName,
                    GuidId = Guid.NewGuid(),
                    UniqueId = request.HrPhoto.UniqueId,
                    FileUrl = request.HrPhoto.FileUrl,
                    Active = true,
                    UploadDate = DateTime.Now,
                    UserId = request.UserId
                });
            }

            return staff;
        }

        public void UpdateEnity(HrStaff entity, StaffDetails request, int entityId)
        {
            entity.AnnualLeave = request.AnnualLeave?.Trim();
            entity.BankAccountNo = request.BankAccount?.Trim();
            entity.BankName = request.BankName?.Trim();
            entity.BirthDate = request.DateBirth?.ToDateTime();
            entity.CompanyEmail = request.CompanyEmail?.Trim();
            entity.CompanyMobileNo = request.CompanyMobile?.Trim();
            entity.DepartmentId = request.SubDepartmentId == null || request.SubDepartmentId == 0 ? SetIntDefaultValue(request.DepartmentId) : request.SubDepartmentId;
            entity.EmaiLaddress = request.Email?.Trim();
            entity.EmergencyCall = request.Phone?.Trim();
            entity.EmergencyContactName = request.EmergencyContact?.Trim();
            entity.EmergencyContactPhone = request.EmergencyContactPhone?.Trim();
            entity.EmployeeTypeId = request.EmployeeTypeId;
            entity.EmpNo = request.EmployeeNo?.Trim();
            entity.GraduateDate = request.GraduateDate?.ToDateTime();
            entity.GraduateSchool = request.Graduate;
            entity.Gender = request.Gender;
            entity.HousingFuncard = request.HousingFundCard?.Trim();
            entity.JoinDate = request.JoinDate?.ToDateTime();
            entity.LaborContractExpiredDate = request.ProbExpDate?.ToDateTime();
            entity.LocationId = SetIntDefaultValue(request.OfficeId);
            entity.LocalLanguage = request.LocalLanguage?.Trim();
            entity.ManagerId = SetIntDefaultValue(request.ManagerId);
            entity.MaritalStatus = request.Martial;
            entity.NationalityCountryId = SetIntDefaultValue(request.CountryId);
            entity.PayrollCurrencyId = SetIntDefaultValue(request.PayrollCurrencyId);
            entity.PersonName = request.StaffName?.Trim();
            entity.PassportNo = request.PassportNo?.Trim();
            entity.ParentStaffId = request.ReportHeadId;
            entity.PositionId = SetIntDefaultValue(request.PositionId);
            entity.PlacePurchasingSihf = request.PlacePurchSiHf?.Trim();
            entity.ProbationExpiredDate = request.ProbExpDate?.ToDateTime();
            entity.ProbationPeriod = request.ProbatonPeriod;
            entity.QualificationId = SetIntDefaultValue(request.QualificationId);
            entity.SkypeId = request.SkypeId;
            entity.SocialInsuranceCardNo = request.AssCardNo?.Trim();
            entity.StartWorkingDate = request.StartWkDate?.ToDateTime();
            entity.TotalWorkingYears = request.WorkingYears;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = request.UserId;
            entity.HomeCityId = SetIntDefaultValue(request.HomeCityId);
            entity.HomeAddress = request.HomeAddress?.Trim();
            entity.CurrentCityId = request.HomeIsCurrent ? request.HomeCityId : request.CurrentCityId;
            entity.CurrentCountyId = request.CurrentCountyId;
            entity.CurrentAddress = request.HomeIsCurrent ? request.HomeAddress : request.CurrentAddress;
            entity.IsForecastApplicable = request.IsForecastApplicable;

            entity.HukoLocationId = request.HukoLocationId;
            entity.MajorSubject = request.MajorSubject;
            entity.BandId = request.BandId;
            entity.SocialInsuranceTypeId = request.SocialInsuranceTypeId;
            entity.NoticePeriod = request.NoticePeriod;
            entity.GlobalGrading = request.GlobalGrading;
            entity.EmergencyContactRelationship = request.EmergencyContactRelationship;
            // entity.Photo = request.Picture;
            entity.PrimaryEntity = request.PrimaryEntity;
            entity.StartPortId = request.StartPortId;
            entity.HroutSourceCompanyId = request.HrOutSourceCompanyId;
            entity.CompanyId = request.CompanyId;
            entity.PayrollCompany = request.PayrollCompany;
            if (entity.HrStaffMarketSegments == null)
                entity.HrStaffMarketSegments = new HashSet<HrStaffMarketSegment>();

            entity.HrStaffMarketSegments.Clear();

            foreach (var item in request.MarketSegmentValues)
            {
                entity.HrStaffMarketSegments.Add(new HrStaffMarketSegment
                {
                    MarketSegmentId = item
                });
            }

            if (entity.HrStaffProductCategories == null)
                entity.HrStaffProductCategories = new HashSet<HrStaffProductCategory>();

            entity.HrStaffProductCategories.Clear();

            foreach (var item in request.ProductCategoryValues)
            {
                entity.HrStaffProductCategories.Add(new HrStaffProductCategory
                {
                    ProductCategoryId = item
                });
            }

            if (entity.HrStaffExpertises == null)
                entity.HrStaffExpertises = new HashSet<HrStaffExpertise>();

            entity.HrStaffExpertises.Clear();

            foreach (var item in request.ExpertiseValues)
            {
                entity.HrStaffExpertises.Add(new HrStaffExpertise
                {
                    ExpertiseId = item
                });
            }

            if (entity.HrStaffProfiles == null)
                entity.HrStaffProfiles = new HashSet<HrStaffProfile>();

            entity.HrStaffProfiles.Clear();

            foreach (var item in request.ProfileValues)
            {
                entity.HrStaffProfiles.Add(new HrStaffProfile
                {
                    ProfileId = item
                });
            }

            entity.HrStaffServices.Clear();

            foreach (var item in request.ApiServiceIds)
            {
                entity.HrStaffServices.Add(new HrStaffService
                {
                    ServiceId = item,
                    Active = true,
                    CreatedOn = DateTime.Now
                });
            }


            if (entity.HrStaffOpCountries == null)
                entity.HrStaffOpCountries = new HashSet<HrStaffOpCountry>();

            entity.HrStaffOpCountries.Clear();

            foreach (var item in request.OpCountryValues)
            {
                entity.HrStaffOpCountries.Add(new HrStaffOpCountry
                {
                    CountryId = item
                });
            }
            if (entity.HrStaffEntityServiceMaps == null)
                entity.HrStaffEntityServiceMaps = new HashSet<HrStaffEntityServiceMap>();

            if (request.EntityServiceIds != null)
            {
                foreach (var item in request.EntityServiceIds)
                {
                    entity.HrStaffEntityServiceMaps.Add(new HrStaffEntityServiceMap
                    {
                        EntityId = item.EntityId,
                        ServiceId = item.ServiceId,
                    });
                }
            }
            else if (request.ApiEntityIds != null && request.ApiServiceIds != null)
            {
                foreach (var apiEntityId in request.ApiEntityIds)
                {
                    foreach (var serviceId in request.ApiServiceIds)
                    {
                        entity.HrStaffEntityServiceMaps.Add(new HrStaffEntityServiceMap
                        {
                            EntityId = entityId,
                            ServiceId = serviceId
                        });
                    }
                }
            }

            int i = 0;
            if (entity.HrRenews == null)
                entity.HrRenews = new HashSet<HrRenew>();

            // entity.HrRenews.Clear();

            foreach (var item in request.RenewList.Where(x => x.StartDate != null))
            {
                i++;
                entity.HrRenews.Add(new HrRenew
                {
                    StartDate = item.StartDate.ToDateTime(),
                    EndDate = item.EndDate.ToDateTime(),
                    Number = i
                });
            }

            if (entity.HrStaffTrainings == null)
                entity.HrStaffTrainings = new HashSet<HrStaffTraining>();

            //entity.HrStaffTrainings.Clear();

            foreach (var item in request.TrainingList.Where(x => !string.IsNullOrEmpty(x.TrainingTopic)))
            {
                entity.HrStaffTrainings.Add(new HrStaffTraining
                {
                    TrainingTopic = item.TrainingTopic,
                    Comment = item.Comments?.Trim(),
                    DateStart = item.StartDate?.ToDateTime(),
                    DateEnd = item.EndDate?.ToDateTime(),
                    Trainer = item.Trainer
                });
            }

            if (entity.HrStaffHistories == null)
                entity.HrStaffHistories = new HashSet<HrStaffHistory>();

            //entity.HrStaffHistories.Clear();

            foreach (var item in request.JobList.Where(x => !string.IsNullOrEmpty(x.Company)))
            {
                entity.HrStaffHistories.Add(new HrStaffHistory
                {
                    Company = item.Company?.Trim(),
                    CurrencyId = item.CurrencyId,
                    Datebegin = item.StartDate?.ToDateTime(),
                    DateEnd = item.EndDate?.ToDateTime(),
                    Position = item.Position?.Trim(),
                    Salary = item.Salary
                });

            }

            if (entity.HrAttachments == null)
                entity.HrAttachments = new HashSet<HrAttachment>();

            // Hr Attachments
            if (request.AttachedList != null)
            {
                foreach (var item in request.AttachedList.Where(x => !entity.HrAttachments.Any(y => y.UniqueId == x.UniqueId) && x.FileTypeId > 0 && !string.IsNullOrEmpty(x.UniqueId)))
                {
                    entity.HrAttachments.Add(new HrAttachment
                    {
                        FileTypeId = item.FileTypeId,
                        FileName = item.FileName,
                        GuidId = Guid.NewGuid(),
                        UniqueId = item.UniqueId,
                        FileUrl = item.FileUrl,
                        Active = true,
                        UploadDate = DateTime.Now,
                        UserId = request.UserId
                    });
                }
            }

            // Hr Photo
            if (request.HrPhoto != null && !string.IsNullOrEmpty(request.HrPhoto.UniqueId))
            {
                var hrPhoto = entity.HrPhotos.FirstOrDefault(x => x.UniqueId == request.HrPhoto.UniqueId);
                if (hrPhoto == null)
                {
                    entity.HrPhotos?.Add(new HrPhoto
                    {
                        FileName = request.HrPhoto.FileName,
                        GuidId = Guid.NewGuid(),
                        UniqueId = request.HrPhoto.UniqueId,
                        FileUrl = request.HrPhoto.FileUrl,
                        Active = true,
                        UploadDate = DateTime.Now,
                        UserId = request.UserId
                    });
                }
            }
        }

        private int? SetIntDefaultValue(int? number)
        {
            return number == 0 ? null : number;
        }

        public Entity GetEntity(ApEntity entity)
        {
            if (entity == null)
                return null;

            return new Entity
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public HolidayDayType GetHolidayDayType(HrHolidayDayType entity)
        {
            if (entity == null)
                return null;

            return new HolidayDayType
            {
                Id = entity.Id,
                Label = entity.TypeTransId.GetTranslation(entity.Label)
            };
        }

        public LeaveRequest GetLeave(HrLeave entity)
        {
            if (entity == null)
                return null;

            return new LeaveRequest
            {
                Id = entity.Id,
                StartDate = entity.DateBegin.GetCustomDate(),
                EndDate = entity.DateEnd.GetCustomDate(),
                LeaveDays = entity.NumberOfDays,
                LeaveTypeId = entity.LeaveTypeId,
                Reason = entity.Comments?.Trim(),
                StartDayType = entity.IdTypeStartDate,
                EndDayType = entity.IdTypeEndDate,
                StaffId = entity.StaffId,
                StaffName = entity.Staff?.PersonName?.Trim(),
                StatusId = entity.Status == null ? (int)Entities.Enums.LeaveStatus.Request : entity.Status.Value,
                StatusName = entity.StatusNavigation == null ? "Request" : entity.StatusNavigation.IdTran.GetTranslation(entity.StatusNavigation.Label)
            };
        }

        public LeaveItem GetLeaveItem(HrLeave entity, int staffId, IEnumerable<int> roleList, IEnumerable<int> subempid)
        {
            if (entity == null)
                return null;

            return new LeaveItem
            {
                Id = entity.Id,
                ApplicationDate = entity.LeaveApplicationDate == null ? string.Empty : entity.LeaveApplicationDate.Value.ToString(StandardDateFormat),
                Days = entity.NumberOfDays,
                EndDate = entity.DateEnd.ToString(StandardDateFormat),
                LeaveType = GetLeaveType(entity.LeaveType),
                OfficeName = entity.Staff?.Location?.LocationName,
                PositionName = entity.Staff?.Position?.PositionName,
                Reason = entity.Comments,
                staffName = entity.Staff?.PersonName,
                StartDate = entity.DateBegin.ToString(StandardDateFormat),
                Status = GetLeaveStatus(entity.StatusNavigation),
                Comment = entity.Comments1?.Trim(),
                CanApprove = staffId != entity.StaffId && roleList.Contains((int)RoleEnum.Management) && subempid.Contains(entity.StaffId)

            };

        }

        public LeaveType GetLeaveType(HrLeaveType entity)
        {
            if (entity == null)
                return null;

            return new LeaveType
            {
                Id = entity.Id,
                Label = entity.IdTran.GetTranslation(entity.Description)
            };
        }

        public DTO.HumanResource.LeaveStatus GetLeaveStatus(HrLeaveStatus entity)
        {
            if (entity == null)
                return null;

            return new DTO.HumanResource.LeaveStatus
            {
                Id = entity.Id,
                Label = entity.IdTran.GetTranslation(entity.Label)
            };

        }

        public HrOutSourceCompany MapHROutSourceCompany(SaveHROutSourceRequest request, int userId, int entityId)
        {
            var hrOutSourceCompanyEntity = new HrOutSourceCompany()
            {
                Id = request.Id,
                Name = request.Name,
                EntityId = entityId,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            };
            return hrOutSourceCompanyEntity;
        }

        public HrOutSourceCompany MapEditHROutSourceCompany(HrOutSourceCompany hrOutSourceCompanyEntity, SaveHROutSourceRequest request, int userId)
        {
            hrOutSourceCompanyEntity.Name = request.Name;
            hrOutSourceCompanyEntity.UpdatedBy = userId;
            hrOutSourceCompanyEntity.UpdatedOn = DateTime.Now;

            return hrOutSourceCompanyEntity;
        }
    }
}
