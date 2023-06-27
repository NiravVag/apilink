using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.UserConfig;
using DTO.UserProfile;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class UserProfileManager: IUserProfileManager
    {
        private readonly IUserProfileRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;

        public UserProfileManager(IUserProfileRepository repo, IAPIUserContext applicationContext)
        {
            _repo = repo;
            _applicationContext = applicationContext;
        }

        //get the user profile details
        public async Task<UserProfileResponse> GetUserProfileSummary(int userId)
        {
            var data = await _repo.GetUserProfileData(userId, (int)_applicationContext.UserType);

                return new UserProfileResponse
                {
                    Data = data,
                    Result = data != null ? UserProfileResult.Success : UserProfileResult.NoData
                };
        }

        //Update the user profile details
        public async Task<UserProfileSaveResponse> Save(UserProfileRequest request)
        {
            //fetch the it user master data
            var entity = await _repo.GetUserProfileEntity(request.UserId);

            if(entity == null)
            {
                return new UserProfileSaveResponse { Result = UserProfileResponseResult.NotFound };
            }

            entity.FileUrl = request.ProfileImageUrl;
            entity.FileName = request.ProfileImageName;

            //compare the full name and update if not equal
            if (!string.Equals(entity.FullName, request.DisplayName, StringComparison.InvariantCulture))
            {
                entity.FullName = request.DisplayName;
            }

            //fetch the emails to check if it already exists
            var emailList = await _repo.GetContactsByEmail(request.EmailId, (int)_applicationContext.UserType);

            //update the customer contact
            if ((int)_applicationContext.UserType == (int)UserTypeEnum.Customer)
            {
                //check if email exists 
                var emailAlreadyExists = emailList.Any(x => x.UserId != entity.CustomerContact.Id);

                if(emailAlreadyExists)
                {
                    return new UserProfileSaveResponse { Id = entity.Id, Result = UserProfileResponseResult.AlreadyExists };
                }

                //compare the email Id and update if not equal
                if (!string.Equals(entity.CustomerContact.Email, request.EmailId, StringComparison.InvariantCulture))
                    entity.CustomerContact.Email = request.EmailId;

                //compare the phone and update if not equal
                if (!string.Equals(entity.CustomerContact.Phone, request.Phone, StringComparison.InvariantCulture))
                    entity.CustomerContact.Phone = request.Phone;

                _repo.EditEntity(entity.CustomerContact);
            }

            //update the supplier contact
            else if ((int)_applicationContext.UserType == (int)UserTypeEnum.Supplier)
            { 
                //check if email exists 
                var emailAlreadyExists = emailList.Any(x => x.UserId != entity.SupplierContact.Id);

                if (emailAlreadyExists)
                {
                    return new UserProfileSaveResponse { Id = entity.Id, Result = UserProfileResponseResult.AlreadyExists };
                }
                //compare the email Id and update if not equal
                if (!string.Equals(entity.SupplierContact.Mail, request.EmailId, StringComparison.InvariantCulture))
                    entity.SupplierContact.Mail = request.EmailId;

                //compare the phone and update if not equal
                if (!string.Equals(entity.SupplierContact.Phone, request.Phone, StringComparison.InvariantCulture))
                    entity.SupplierContact.Phone = request.Phone;

                _repo.EditEntity(entity.SupplierContact);
            }

            //update the factory contact
            else if ((int)_applicationContext.UserType == (int)UserTypeEnum.Factory)
            {
                //check if email exists 
                var emailAlreadyExists = emailList.Any(x => x.UserId != entity.FactoryContact.Id);

                if (emailAlreadyExists)
                {
                    return new UserProfileSaveResponse { Id = entity.Id, Result = UserProfileResponseResult.AlreadyExists };
                }
                //compare the email Id and update if not equal
                if (!string.Equals(entity.FactoryContact.Mail, request.EmailId, StringComparison.InvariantCulture))
                    entity.FactoryContact.Mail = request.EmailId;

                if (!string.Equals(entity.FactoryContact.Phone, request.Phone, StringComparison.InvariantCulture))
                    entity.FactoryContact.Phone = request.Phone;

                _repo.EditEntity(entity.FactoryContact);
            }

            _repo.EditEntity(entity);
            await _repo.Save();

            return new UserProfileSaveResponse { Id = entity.Id, Result = UserProfileResponseResult.Success };
        }
    }
}
