using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityServerController : ControllerBase
    {

        private static IConfiguration _configuration = null;

        public IdentityServerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST: api/IdentityServer
        [HttpPost]
        public AuthorizationServerAnswer Post([FromBody] IdentityServerModel request)
        {

            Uri authorizationServerTokenIssuerUri = new Uri(_configuration["IdentityServer"] + "/connect/token");

            //access token request          
                    string rawJwtToken = RequestTokenToAuthorizationServer(
                 authorizationServerTokenIssuerUri,
                 request.ClientId,
                 _configuration["APIScope"],
                 request.ClientSecret)
                .GetAwaiter()
                .GetResult();

            AuthorizationServerAnswer authorizationServerToken;
            authorizationServerToken = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthorizationServerAnswer>(rawJwtToken);

            return authorizationServerToken;
        }

        private static async Task<string> RequestTokenToAuthorizationServer(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {

            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                HttpContent httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                                new KeyValuePair<string, string>("grant_type",_configuration["APIGrantType"]),
                                new KeyValuePair<string, string>("client_id", clientId),
                                new KeyValuePair<string, string>("scope", scope),
                                new KeyValuePair<string, string>("client_secret", clientSecret)
                    });
                tokenRequest.Content = httpContent;
                responseMessage = await client.SendAsync(tokenRequest);
            }
            return await responseMessage.Content.ReadAsStringAsync();
        }

        public class AuthorizationServerAnswer
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string token_type { get; set; }
        }

        public class IdentityServerModel
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }
    }
}
