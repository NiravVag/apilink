//using Contracts.Managers;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace LINK_UI.Filters
//{
//    public class GlobalFilter : IActionFilter
//    {
//        static IUserRightsManager _manager = null;

//        public static void SetUserRightManager(IUserRightsManager manager)
//        {
//            _manager = manager;
//        }
        

//        public void OnActionExecuted(ActionExecutedContext context)
//        {

//        }

//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            // if rightFIlter exists then stop
//            if (context.ActionDescriptor.FilterDescriptors.Any(x => x.Filter == typeof(RightFilter)))
//                return;

//            // if right attribute does not exists then stop
//            if (!((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes.Any(x => x.AttributeType == typeof(RightAttribute)))
//                return;

//            var attribute = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttribute(typeof(RightAttribute)) as RightAttribute;

//            var roles = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

//            if(!_manager.HasRight(attribute.Path, roles))
//                throw new Exception("401");

//        }
//    }
//}
