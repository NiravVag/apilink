using DTO.Customer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace LINK_UI.Controllers.EXTERNAL
{

    public class ExternalBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult BuildCommonResponse(HttpStatusCode statusCode, object responseData)
        {
            switch (statusCode)
            {

                case HttpStatusCode.OK:

                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), StatusCode = (int)HttpStatusCode.OK };

                case HttpStatusCode.BadRequest:

                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), ContentType = "application/json", StatusCode = (int)HttpStatusCode.BadRequest };

                case HttpStatusCode.NotFound:

                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), ContentType = "application/json", StatusCode = (int)HttpStatusCode.NotFound };

                case HttpStatusCode.InternalServerError:

                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), ContentType = "application/json", StatusCode = (int)HttpStatusCode.InternalServerError };

                case HttpStatusCode.Created:

                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), ContentType = "application/json", StatusCode = (int)HttpStatusCode.Created };

                case HttpStatusCode.Accepted:

                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), ContentType = "application/json", StatusCode = (int)HttpStatusCode.Accepted };

                default:
                    return new ContentResult { Content = JsonConvert.SerializeObject(responseData), ContentType = "application/json", StatusCode = (int)HttpStatusCode.OK };
            }
        }

        [NonAction]
        public IActionResult BuildCommonEaqfResponse(object response)
        {
            System.Reflection.PropertyInfo pi = response.GetType().GetProperty("statusCode");
            if (pi != null)
            {
                var statusCode = (HttpStatusCode)(pi.GetValue(response, null));

                switch (statusCode)
                {

                    case HttpStatusCode.OK:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.OK };

                    case HttpStatusCode.BadRequest:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.BadRequest };

                    case HttpStatusCode.NotFound:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.NotFound };

                    case HttpStatusCode.InternalServerError:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.InternalServerError };

                    case HttpStatusCode.Created:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.Created };

                    case HttpStatusCode.Accepted:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.Accepted };

                    default:
                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.OK };
                }
            }
            else
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new EaqfErrorResponse()
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Internal server error",
                        errors = new List<string>() { "Statuscode not found"}

                    }),
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

        }

        [NonAction]
        public IActionResult BuildCommonLinkAPIResponse(object response)
        {
            System.Reflection.PropertyInfo pi = response.GetType().GetProperty("StatusCode");
            if (pi != null)
            {
                var statusCode = (HttpStatusCode)(pi.GetValue(response, null));

                switch (statusCode)
                {

                    case HttpStatusCode.OK:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), StatusCode = (int)HttpStatusCode.OK };

                    case HttpStatusCode.BadRequest:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.BadRequest };

                    case HttpStatusCode.NotFound:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.NotFound };

                    case HttpStatusCode.InternalServerError:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.InternalServerError };

                    case HttpStatusCode.Created:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.Created };

                    case HttpStatusCode.Accepted:

                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.Accepted };

                    default:
                        return new ContentResult { Content = JsonConvert.SerializeObject(response), ContentType = "application/json", StatusCode = (int)HttpStatusCode.OK };
                }
            }
            else
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new EaqfErrorResponse()
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Internal server error",
                        errors = new List<string>() { "Statuscode not found" }

                    }),
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

        }
    }
}
