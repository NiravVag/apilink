using Components.Core.contracts;
using Components.Core.entities;
using Components.Core.entities.Emails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Components.Web
{
    public static class ControlllerExtensions
    {
        private static ICompositeViewEngine _viewEngine;
        private static IServiceProvider _serviceProvider;
        private static ITempDataProvider _tempDataProvider;
        private static IFileManager _fileManager = null;
        private static IEmailManager _emailManager = null;
        private static IHostingEnvironment _hostEnvironment = null;
        public static void InitControlllerExtensions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewEngine = serviceProvider.GetService<ICompositeViewEngine>();
            _tempDataProvider = serviceProvider.GetService<ITempDataProvider>();
            _fileManager = serviceProvider.GetService<IFileManager>();
            _emailManager = serviceProvider.GetService<IEmailManager>();
            _hostEnvironment = serviceProvider.GetService<IHostingEnvironment>();
        }

        /// <summary>
        /// Render to string Async
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<string> RenderToStringAsync(this ControllerBase controller, string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} does not match any available view");


                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        public static IActionResult File(this ControllerBase controller, string viewName, object model, FileType filetype)
        {
            var task = RenderToStringAsync(controller, viewName, model);

            task.Wait();

            string html = task.Result;

            var file = _fileManager.BuildFile(html, filetype, SourceEnum.Html);

            return controller.File(file.Content, file.MimeType);
        }

        public static IActionResult File(this ControllerBase controller, string viewName, FileType filetype)
        {
            var task = RenderToStringAsync(controller, viewName, null);

            task.Wait();

            string html = task.Result;

            var file = _fileManager.BuildFile(html, filetype, SourceEnum.Html);

            return controller.File(file.Content, file.MimeType);
        }

        public static async Task<IActionResult> FileAsync(this ControllerBase controller, string viewName, object model, FileType filetype)
        {
            string html = await RenderToStringAsync(controller, viewName, model);

            var file = _fileManager.BuildFile(html, filetype, SourceEnum.Html);

            return controller.File(file.Content, file.MimeType);
        }

        public static async Task<IActionResult> FileAsync(this ControllerBase controller, string viewName, FileType filetype)
        {
            string html = await RenderToStringAsync(controller, viewName, null);

            var file = _fileManager.BuildFile(html, filetype, SourceEnum.Html);

            return controller.File(file.Content, file.MimeType);
        }

        public static async Task<byte[]> GetFileBytesAsync(this ControllerBase controller, string viewName, object model, FileType filetype)
        {
            string html = await RenderToStringAsync(controller, viewName, model);

            var file = _fileManager.BuildFile(html, filetype, SourceEnum.Html);

            return file.Content;
        }

        public static IActionResult FileFromJson(this ControllerBase controller, string fileName,  object model,   FileType filetype)
        {
            return FileFromJson(controller, fileName, model, filetype, null);
        }

        public static IActionResult FileFromJson(this ControllerBase controller, string fileName, object model, FileType filetype, Action<int, string> onLog)
        {

            string jsonPath = $"{_hostEnvironment.ContentRootPath}\\Views\\Shared\\{fileName.Replace("/", "\\")}";

            var request = new JsonRequestModel
            {
                JsonPath = jsonPath,
                Model = model
            };
            var file = _fileManager.BuildFile(request, filetype, SourceEnum.Json, onLog);

            return controller.File(file.Content, file.MimeType, file.FileModelName);
        }

        public static void SendEmail(this ControllerBase controller, string viewName, object model, EmailRequest request)
        {
            var task = RenderToStringAsync(controller, viewName, model);

            task.Wait();

            request.Body = task.Result;

            _emailManager.SendEmail(request, 0);
        }

        public static string GetEmailBody(this ControllerBase controller, string viewName, object model)
        {
            var task = RenderToStringAsync(controller, viewName, model);

            task.Wait();

            return task.Result;
        }

        public static void SendEmailList(this ControllerBase controller, IEnumerable<(string, object, EmailRequest)> emailList)
        {
            var emailRequests = new List<EmailRequest>();

            foreach (var email in emailList)
            {
                var task = RenderToStringAsync(controller, email.Item1, email.Item2);
                task.Wait();

                email.Item3.Body = task.Result;

                emailRequests.Add(email.Item3);
            }

            _emailManager.SendEmails(emailRequests);
        }
    }
}
