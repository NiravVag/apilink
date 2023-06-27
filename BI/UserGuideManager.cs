using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.File;
using DTO.UserGuide;
using Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class UserGuideManager : IUserGuideManager
    {
        private readonly IUserGuideRepository _userGuideRepository = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly IFileManager _fileManager = null;
        private readonly IHostingEnvironment _env;

        public UserGuideManager(IUserGuideRepository userGuideRepository, IAPIUserContext applicationContext, IFileManager fileManager, IHostingEnvironment env)
        {
            _userGuideRepository = userGuideRepository;
            _applicationContext = applicationContext;
            _fileManager = fileManager;
            _env = env;
        }

        /// <summary>
        /// Get the user guide details data
        /// </summary>
        /// <returns></returns>
        public async Task<UserGuideDetailResponse> GetUserGuideDetails()
        {
            //get the user guide details
            var userGuideDetailsRepo = _userGuideRepository.GetUserGuideDetails();

            //apply customer filter if usertype is customer
            if (_applicationContext.UserType == UserTypeEnum.Customer)
            {
                userGuideDetailsRepo = userGuideDetailsRepo.Where(x => x.IsCustomer.HasValue && x.IsCustomer.Value);
            }
            //apply supplier filter if usertype is supplier
            else if (_applicationContext.UserType == UserTypeEnum.Supplier)
            {
                userGuideDetailsRepo = userGuideDetailsRepo.Where(x => x.IsSupplier.HasValue && x.IsSupplier.Value);
            }
            //apply factory filter if usertype is factory
            else if (_applicationContext.UserType == UserTypeEnum.Factory)
            {
                userGuideDetailsRepo = userGuideDetailsRepo.Where(x => x.IsFactory.HasValue && x.IsFactory.Value);
            }
            //apply internaluser filter if usertype is internal user
            else if (_applicationContext.UserType == UserTypeEnum.InternalUser)
            {
                userGuideDetailsRepo = userGuideDetailsRepo.Where(x => x.IsInternal.HasValue && x.IsInternal.Value);
            }

            //execute the user guide list
            var userGuideDetails = await userGuideDetailsRepo.AsNoTracking().Select(x => new UserGuideDetail()
            {
                Id = x.Id,
                Name = x.Name,
                TotalPage = x.TotalPage,
                FileUrl = x.FileUrl,
                VideoUrl = x.VideoUrl,
                UGRoleCount = x.UgRoles.Count
            }).OrderBy(x => x.Name).ToListAsync();

            //get the user guide ids which mapped to role list
            var userGuideIds = userGuideDetails.Where(x => x.UGRoleCount > 0).Select(x => x.Id).ToList();

            //assign the userguide details to userguide list
            var userGuideList = userGuideDetails;

            ////apply rolelist filter and if any role assigned to userguides
            if (userGuideIds != null && userGuideIds.Any() && _applicationContext.RoleList != null && _applicationContext.RoleList.Any())
            {
                //take the role user guide ids
                var roleUserGuideIds = await _userGuideRepository.GetRoleUserGuideIds(_applicationContext.RoleList.ToList(), userGuideIds);
                //take the user guide list which is not mapped to any roles
                var commonUserGuideList = userGuideDetails.Where(x => x.UGRoleCount == 0).ToList();
                userGuideList = commonUserGuideList;
                //filter the role mapped user guide ids
                var roleUserGuideList = userGuideDetails.Where(x => roleUserGuideIds.Contains(x.Id)).ToList();
                userGuideList.AddRange(roleUserGuideList);
            }
            //sort userguide by name
            userGuideList = userGuideList.OrderBy(x => x.Name).ToList();

            if (userGuideDetails != null && userGuideDetails.Any())
            {
                return new UserGuideDetailResponse() { UserGuideDetails = userGuideList, Result = UserGuideDetailResult.Success };
            }

            return new UserGuideDetailResponse() { Result = UserGuideDetailResult.NotFound };

        }

        /// <summary>
        /// Get the file data
        /// </summary>
        /// <param name="userGuideId"></param>
        /// <returns></returns>
        public async Task<FileResponse> GetFileData(int userGuideId)
        {
            //get the root path
            var rootPath = _env.WebRootPath;
            //get the file url
            var fileUrl = await _userGuideRepository.GetUserGuideFile(userGuideId);
            //concat the filename and url
            var fileName = string.Concat(rootPath, fileUrl);

            if (string.IsNullOrEmpty(fileName))
                return null;
            //read the file content
            var filecontent = FileParser.ReadFiletoByteArray(fileName);

            if (filecontent == null)
                return null;

            return new FileResponse
            {
                Content = filecontent,
                MimeType = _fileManager.GetMimeType(Path.GetExtension(fileName)),
                Result = FileResult.Success
            };

        }
    }
}
