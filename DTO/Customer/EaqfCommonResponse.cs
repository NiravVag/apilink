using System.Collections.Generic;
using System.Net;

namespace DTO.Customer
{
    public class EaqfCommonResponse
    {
    }
    public class EaqfErrorResponse
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public List<string> errors { get; set; }
        public EaqfErrorResponse()
        {
            this.errors = new List<string>();
        }
    }

    public class EaqfSaveSuccessResponse
    {
        public int Id { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
    }

    public class EaqfCustomerContactSaveSuccessResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
    }

    public class EaqfGetSuccessResponse
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public object data { get; set; }
    }

    public class LinkGetSuccessResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
    }

    public class LinkErrorResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; }
        public LinkErrorResponse()
        {
            this.Errors = new List<string>();
        }
    }

    public class LinkSaveSuccessResponse
    {
        public int Id { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }

    public class ServiceQueryParam
    {
        public int CustomerId { get; set; }
        public string ServiceCategoryName { get; set; }
        public int ServiceCategoryId { get; set; }
    }

    public class ProductCategoryQueryParam
    {
        public string ProductLineName { get; set; }
        public int ProductLineId { get; set; }
    }
}
