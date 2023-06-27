using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Common
{
    public interface IAPIUserContext
    {
        int CustomerId { get; }
        int SupplierId { get; }
        int LocationId { get; }
        int FactoryId { get; }
        UserTypeEnum UserType { get; }
        int UserId { get; }
        int StaffId { get; }
        string EmailId { get; }
        string UserName { get; }
        //int EntityId { get; }
        IEnumerable<int> RoleList { get; }
        IEnumerable<int> LocationList { get; }
        IEnumerable<int> UserProfileList { get; }
        string AppBaseUrl { get; }
    }
}
