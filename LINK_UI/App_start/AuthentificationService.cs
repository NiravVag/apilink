using BI.Utilities;
using DTO;
using DTO.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static LINK_UI.Controllers.IdentityServerController;

namespace LINK_UI.App_start
{
    public static class AuthentificationService
    {
        private static IConfiguration _Configuration = null;

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            _Configuration = configuration;

            services.AddMvcCore()
            .AddAuthorization(options => options.AddPolicy("ApiUserPolicy", policy => policy.RequireClaim("role", "ApiUser")))
            .AddAuthorization(options => options.AddPolicy("FbUserPolicy", policy => policy.RequireClaim("role", "FbUser")))
            .AddAuthorization(options => options.AddPolicy("ZohoUserPolicy", policy => policy.RequireClaim("role", "ZohoUser")))
            .AddAuthorization(options => options.AddPolicy("InvoicePdfUserPolicy", policy => policy.RequireClaim("role", "InvoicePdfUser")))
            .AddAuthorization(options => options.AddPolicy("MobileUserFbPolicy", policy => policy.RequireClaim("role", "MobileAppUser")))
            .AddAuthorization(options => options.AddPolicy("EAQFUserPolicy", policy => policy.RequireClaim("role", "EAQFUser")))
              .AddAuthorization(options => options.AddPolicy("CflUserPolicy", policy => policy.RequireClaim("role", "CflUser")))
            .AddNewtonsoftJson();
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = _Configuration["IdentityServer"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "role"
                    };
                });
        }

        public static void SetToken(SignInResponse response)
        {

            Uri authorizationServerTokenIssuerUri = new Uri(_Configuration["IdentityServer"] + "/connect/token");

            var claims = new List<KeyValuePair<string, string>>()
                 {
                                new KeyValuePair<string, string>("client_id", _Configuration["APIClientId"]),
                                new KeyValuePair<string, string>("client_secret",  _Configuration["APISecretId"]),
                                new KeyValuePair<string, string>("scope", _Configuration["APIScope"]),
                                new KeyValuePair<string, string>("grant_type", _Configuration["APIGrantType"]),

                                new KeyValuePair<string, string>(JwtRegisteredClaimNames.Sub, response.User.LoginName),
                                new KeyValuePair<string, string>(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new KeyValuePair<string, string>("PersonName", response.User.FullName),
                                new KeyValuePair<string, string>("EntityName", response.User.EntityName),
                                new KeyValuePair<string, string>("EntityId", response.User.EntityId.ToString()),
                                new KeyValuePair<string, string>("CustomerId", response.User.CustomerId.ToString()),
                                new KeyValuePair<string, string>("LocationId", response.User.LocationId.ToString()),
                                new KeyValuePair<string, string>("FactoryId", response.User.FactoryId.ToString()),
                                new KeyValuePair<string, string>("UserTypeId", ((int)response.User.UserType).ToString()),
                                new KeyValuePair<string, string>("ProfileList", string.Join("|",(response.User.UserProfileList == null ? new List<int>() : response.User.UserProfileList))),
                                new KeyValuePair<string, string>("StaffId", response.User.StaffId.ToString()),
                                new KeyValuePair<string, string>("EmailId", (response.User?.EmailAddress==null)? "": response.User?.EmailAddress?.ToString()),
                                new KeyValuePair<string, string>("RoleList", string.Join("|",response.User.Roles.Select(x => x.Id))),
                                new KeyValuePair<string, string>("LocationList", string.Join("|",(response.User.LocationList == null ? new List<int>() : response.User.LocationList))),
                                new KeyValuePair<string,string>("SupplierId", response.User.SupplierId.ToString()),
                                new KeyValuePair<string, string>(ClaimTypes.Name, response.User?.LoginName),
                                new KeyValuePair<string, string>(ClaimTypes.NameIdentifier, response.User.Id.ToString())
                  };
            if (response.User.Roles != null && response.User.Roles.Any())
                foreach (var role in response.User.Roles)
                    claims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, role.RoleName));

            //access token request
            HttpContent httpContent = new FormUrlEncodedContent(claims);

            string rawJwtToken = RequestTokenToAuthorizationServer(
                 authorizationServerTokenIssuerUri,
                 httpContent
                )
                .GetAwaiter()
                .GetResult();



            AuthorizationServerAnswer authorizationServerToken;
            authorizationServerToken = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthorizationServerAnswer>(rawJwtToken);

            response.Token = authorizationServerToken.access_token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriAuthorizationServer"></param>
        /// <param name="clientId"></param>
        /// <param name="scope"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        private static async Task<string> RequestTokenToAuthorizationServer(Uri uriAuthorizationServer, HttpContent httpContent)
        {
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                tokenRequest.Content = httpContent;
                responseMessage = await client.SendAsync(tokenRequest);
            }
            return await responseMessage.Content.ReadAsStringAsync();
        }

        private class AuthorizationServerAnswer
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string token_type { get; set; }

        }

        /// <summary>
        /// Create token for Full bridge with our private key.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="privateRsaKey"></param>
        /// <returns></returns>
        public static string CreateFBToken(List<Claim> claims, string privateRsaKey)
        {
            // string privateKey = File.ReadAllText(@"C:\Sabari\Projects\API\LINK_UI\Pages\api");            

            RSAParameters rsaParams;
            using (var tr = new StringReader(privateRsaKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object> payload = claims.ToDictionary(k => k.Type, v => (object)v.Value);
                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS512);
            }
        }

        public static string GenerateAPIToken(IConfiguration configuration)
        {
            var client = new HttpClient();
            IdentityServerModel identityServerModel = new IdentityServerModel
            {
                ClientId = Convert.ToString(configuration["APIClientID"]),
                ClientSecret = Convert.ToString(configuration["APISecretId"])
            };
            Log.Information("Get the token using API credentials starts");
            var tokenResponse = client.PostAsync(Convert.ToString(configuration["ServerUrl"])+"api/identityServer", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(identityServerModel), Encoding.UTF8, "application/json")).Result;
            var token = tokenResponse.Content.ReadAsStringAsync().Result;
            Log.Information("Get the token using API credentials ends " + token);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthorizationServerAnswer>(token);
            return result.access_token;
        }
        public static void SignOut()
        {
        }

    }
}
