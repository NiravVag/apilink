using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.RoleRight;
using Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class RoleRightManager : IRoleRightManager
    {
        #region Declaration 
        private ICacheManager _cache = null;
        private readonly ILogger _logger = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IUserRepository _userRepository = null;
        private readonly RoleRightMap RoleRightMap = null;
        private readonly UserMap UserMap = null;
        #endregion Declaration

        #region Constructor
        public RoleRightManager(IUserAccountRepository repository, ICacheManager cache, ILogger<RoleRightManager> logger,
            IAPIUserContext applicationContext, IUserRepository userRepository)
        {
            _cache = cache;
            _logger = logger;
            _ApplicationContext = applicationContext;
            _userRepository = userRepository;
            RoleRightMap = new RoleRightMap();
            UserMap = new UserMap();
        }
        #endregion Constructor


        public RoleRightResponse GetRoleRightSummary()
        {
            var response = new RoleRightResponse();
            try
            {
                response.RoleList = _userRepository.GetRoleList().Where(x => x.PrimaryRole.HasValue && x.PrimaryRole.Value).Select(UserMap.GetRoleModel).ToArray();
                if (response.RoleList == null)
                    return new RoleRightResponse { Result = RoleRightResult.CannotGetListRole };

                response.Result = RoleRightResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get role right summary");
            }
            return response;
        }

        public RoleRightSearchResponse GetRoleRightSearch(int roleId)
        {
            var response = new RoleRightSearchResponse();
            try
            {
                response.RoleId = roleId;
                response.RightList = _userRepository.GetRightList().Where(x => !x.ParentId.HasValue).Select(x => RoleRightMap.GetRightTreeView(x, roleId)).ToArray();
                if (response.RightList == null)
                    return new RoleRightSearchResponse { Result = RoleRightSearchResult.CannotGetListRight };
                response.Result = RoleRightSearchResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get role right by role id");
            }
            return response;
        }

        public async Task<SaveRoleRightResponse> SaveRoleRight(RoleRightRequest request)
        {
            try
            {
                var entity = _userRepository.GetRoleList().Where(x => x.Id == request.RoleId).FirstOrDefault();
                var rightList = entity.ItRoleRights.ToList();
                foreach (var item in rightList)
                    entity.ItRoleRights.Remove(item);

                if (rightList.Count > 0)
                    _userRepository.RemoveEntities(rightList);

                foreach (var element in request.RightIdList)
                {
                    ItRoleRight newRoleRight = new ItRoleRight();
                    newRoleRight.RoleId = request.RoleId;
                    newRoleRight.RightId = element;
                    entity.ItRoleRights.Add(newRoleRight);
                }

                int result = await _userRepository.SaveRoleDetail(entity);
                if (result > 0)
                    return new SaveRoleRightResponse { Id = entity.Id, Result = SaveRoleRightResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save role right");
            }
            return new SaveRoleRightResponse() { Result = SaveRoleRightResult.CannotSaveRoleRight };
        }

        //get Role list
        public async Task<DataSourceResponse> GetRoleList(int userId)
        {
            var data = await _userRepository.GetRolesList(userId);
            if (data == null)
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            return new DataSourceResponse()
            {
                DataSourceList = data.Select(UserMap.RoleMap).ToArray(),
                Result = DataSourceResult.Success
            };
        }
    }
}
