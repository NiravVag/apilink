using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class CustomClaimInjection : ICustomTokenRequestValidator
    {
        public CustomClaimInjection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IConfiguration _configuration { get; }
        public async Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var client = context.Result.ValidatedRequest.Client;

            var claims = new List<Claim>();

            // we want to add custom claims to our "api" client
            if (client.ClientId == "API")
            {
                // get the user id from the input.

                foreach (string key in context.Result.ValidatedRequest.Raw.Keys)
                {
                    if (key == "client_id" || key == "client_secret" || key == "scope" || key == "grant_type")
                    {

                    }
                    else
                    {
                        var values = context.Result.ValidatedRequest.Raw.GetValues(key);
                        foreach (string value in values)
                        {
                            context.Result.ValidatedRequest.ClientClaims.Add(new Claim(key, value));
                        }
                    }
                }
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "ApiUser"));
            }

            else if (client.ClientId == "FullBridge")
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "FbUser"));
            }

            else if (client.ClientId == "Zoho")
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "ZohoUser"));
            }
            else if (client.ClientId == "InvoicePdf")
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "InvoicePdfUser"));
            }

            else if (client.ClientId == "MobileApp")
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "MobileAppUser"));
            }
            else if (client.ClientId == "EAQF")
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "EAQFUser"));
            }
            else if (client.ClientId == "CFL")
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("role", "CflUser"));
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("CustomerId", _configuration["cflCustomerId"]));
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim(ClaimTypes.NameIdentifier, _configuration["cflUserId"]));
            }
            // don't want it to be prefixed with "client_" ? we change it here (or from global settings)
            context.Result.ValidatedRequest.Client.ClientClaimsPrefix = "";
        }
    }
}
