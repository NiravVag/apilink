using System;
using System.Collections.Generic;
using DTO.User;

namespace DTO.CombineOrders
{
    public class SaveCombineOrdersResponse
    {
        public int Id { get; set; }
        public IEnumerable<User.User> ToRecipients { get; set; }
        public IEnumerable<User.User> CcRecipients { get; set; }
        public bool isEmailRequired { get; set; }
        public SaveCombineOrdersResult Result { get; set; }
    }



    //public enum InternalUserCombineAccess
    //{
    //    InspectionRequest = 23,
    //    InspectionConfirmed = 24,
    //    InspectionVerified = 25
    //}

    //public enum InternalUserCombineAccess
    //{
    //    InspectionRequest = 23,
    //    InspectionConfirmed = 24,
    //    InspectionVerified = 25
    //}

    public enum SaveCombineOrdersResult
    {
        Success = 1,
        CombineOrdersIsNotSaved = 2,
        CombineOrdersIsNotFound = 3,
        CombineOrdersExists = 4,
        CombineProductIdCannotBeNull = 5,
        CombineAqlQuantityGreaterThanZero = 6,
        InternalUserRoleNotMatched = 7,
        InternalUserStatusNotMatched = 8,
        ExternalUserStatusNotMatched = 9
    }
}
