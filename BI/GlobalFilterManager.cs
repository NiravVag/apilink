using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = null;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// get company id by company name from header values
        /// </summary>
        /// <returns></returns>
        public int GetCompanyId()
        {
            var headers = _httpContextAccessor?.HttpContext?.Request?.Headers;

            string companyName = string.Empty;

            // default is api
            int entityId = (int)Company.api;

            if (headers != null && headers["entityId"].ToString() != "")
            {
                companyName = EncryptionDecryption.DecryptStringAES(headers["entityId"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                entityId = Int32.Parse(companyName);
            }

            return entityId;
        }

        public enum Company
        {
            api = 1,
            sgt = 2,
            aqf = 3
        }
    }
}
