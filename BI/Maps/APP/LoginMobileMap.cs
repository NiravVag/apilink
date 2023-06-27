using DTO.Common;
using DTO.MobileApp;
using DTO.User;

namespace BI.Maps.APP
{
    public static class LoginMobileMap
    {
        public static LoginMobileItem MapLoginResponse(SignInResponse request)
        {
            var response = new LoginMobileItem();

            var user = new MobileUser()
            {
                id = request.User.Id,
                fullName = request.User.FullName,
                officeLocationId = request.User.LocationId,
                customerId = request.User.CustomerId,
                changePassword = request.User.ChangePassword,
                emailAddress = request.User.EmailAddress,
                userType = request.User.UserType,
                roles = request.User.Roles,
                userImageUrl = request.User.ProfileImageUrl
            };

            response.Result = (signInResult)request.Result;
            response.Token = request.Token;
            response.User = user;

            return response;
        }
    }
}
