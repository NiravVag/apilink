using Contracts.Managers;
using DTO.Common;
using DTO.EventBookingLog;
using LoggerComponent;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINK_UI.App_start
{
    public class ApiLoggingMiddleware
    {
        private IAPILogger _apiLogService;

        private IAPIUserContext _ApplicationContext = null;

        private readonly RequestDelegate _next;

        public ApiLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAPILogger apiLogService, IAPIUserContext ApplicationContext)
        {
            try
            {
                _apiLogService = apiLogService;
                _ApplicationContext = ApplicationContext;
                var request = httpContext.Request;
                // check api request and other than http options method
                if (request.Path.StartsWithSegments(new PathString("/api")) && !request.Method.Contains("OPTIONS"))
                {
                    var stopWatch = Stopwatch.StartNew();
                    var requestTime = DateTime.UtcNow;
                    var requestBodyContent = await ReadRequestBody(request);
                    await _next(httpContext);
                    stopWatch.Stop();

                    SafeLog(requestTime,
                       stopWatch.ElapsedMilliseconds,
                       httpContext.Response.StatusCode,
                       request.Method,
                       request.Path,
                       request.QueryString.ToString(),
                       requestBodyContent,
                       DateTime.UtcNow);
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                await _next(httpContext);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private void SafeLog(DateTime requestTime,
                    long responseMillis,
                    int statusCode,
                    string method,
                    string path,
                    string queryString,
                    string requestBody,
                    DateTime? responsTime)
        {
            if (path.ToLower().Contains("/api/user/signin"))
            {
                requestBody = "(Request logging disabled for /api/User/SignIn)";
            }

            if (requestBody.Length > 300)
            {
                requestBody = $"(Truncated to 300 chars) {requestBody.Substring(0, 300)}";
            }

            if (queryString.Length > 100)
            {
                queryString = $"(Truncated to 100 chars) {queryString.Substring(0, 100)}";
            }

            int? createdBy = null;

            if (_ApplicationContext.UserId > 0)
            {
                createdBy = _ApplicationContext.UserId;
            }
            _apiLogService.SaveRestAPILog(new RestApiLog
            {
                RequestBody = requestBody,
                RequestMethod = method,
                RequestPath = path,
                RequestQuery = queryString,
                RequestTime = requestTime,
                ResponseInMilliSeconds = (int?)responseMillis,
                ResponseStatus = statusCode.ToString(),
                ResponseTime = responsTime,
                CreatedBy = createdBy
            });
        }
    }
}
