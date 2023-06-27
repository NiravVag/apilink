using Microsoft.Extensions.Configuration;
using Contracts.Managers;
using DTO.Common;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace LINK_UI.Filters
{
    public class RightFilter : IActionFilter
    {

        static IUserRightsManager _manager = null;
        static IHostingEnvironment _env = null;
        static IConfiguration _configuration = null;


        public RightFilter(IUserRightsManager manager, IHostingEnvironment env, IConfiguration configuration)
        {
            _manager = manager;

            _env = env;
            _configuration = configuration;

            // var task = _manager.HasRight("edit-supplier", new List<string> { "1"});
            // task.Wait();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                var currentLang = context.HttpContext.Request.Query["lang"].ToString();
                if (string.IsNullOrEmpty(currentLang))
                    currentLang = "en";
                //var errors = context.ModelState.Select(x => x.Value.Errors)
                //          .Where(y => y.Count > 0)
                //          .ToList();
                context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState, 
                    _env, _configuration, currentLang));
            }
            //var currentLang = context.HttpContext.Request.Query["lang"].ToString();
            //var currentEntity = context.HttpContext.Request.Query["EntityName"].ToString();

            //try
            //{

            //    if (string.IsNullOrEmpty(currentLang))
            //        CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en");
            //    else
            //        CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo(currentLang);

            //}
            //catch (Exception ex)
            //{
            //    /// TODO to log
            //}

            //// if right attribute does not exists then stop
            //if (!((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes.Any(x => x.AttributeType == typeof(RightAttribute)))
            //    return;

            //var currentUserEntity = GetClaim<string>(context, "EntityName");

            //if (string.IsNullOrEmpty(currentEntity) || string.IsNullOrEmpty(currentUserEntity))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            //if (currentEntity.Trim().ToUpper() != currentUserEntity.Trim().ToUpper())
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            //var attribute = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttribute(typeof(RightAttribute)) as RightAttribute;

            //var roles = GetClaimList<string>(context, ClaimTypes.Role);

            //bool hasRight = true;

            ////try
            ////{
            ////    foreach (var path in attribute.Paths)
            ////    {
            ////        var task = _manager.HasRight(path, roles);
            ////        task.Wait();

            ////        if (task.Result)
            ////        {
            ////            hasRight = true;
            ////            break;
            ////        }
            ////    }
            ////}
            ////catch (Exception ex)
            ////{

            ////}

            //if (!hasRight)
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

        }

        private T GetClaim<T>(ActionExecutingContext context, string key)
        {
            if (context != null && context.HttpContext != null)
            {
                var elements = context.HttpContext.User.Claims.Where(x => x.Type == key)
                                .Select(x => x.Value);

                if (elements == null || !elements.Any())
                    return default(T);

                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), elements.First(), true);

                //if (typeof(T).GetTypeInfo().IsEnum)
                //    return (T)Enum.Parse(typeof(T), elements.First());
                return (T)Convert.ChangeType(elements.First(), typeof(T));
            }
            else
            {
                return default(T);
            }
            

        }

        private IEnumerable<T> GetClaimList<T>(ActionExecutingContext context, string key)
        {
            if (context != null && context.HttpContext != null)
            {
                var elements = context.HttpContext.User.Claims.Where(x => x.Type == key)
                .Select(x => x.Value);

                if (elements == null || !elements.Any())
                    return null;

                return elements.Select(x => (T)Convert.ChangeType(x, typeof(T)));
            }
            else
            {
                return null;
            }
        }
    }
}
