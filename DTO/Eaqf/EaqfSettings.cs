namespace DTO.Eaqf
{
    public class EAQFSettings
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string GrantType { get; set; }
        public string OAuthRequestUrl { get; set; }
        public string BookingEventRequestUrl { get; set; }

    }

    public class EAQFOauthTokenRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
    }
}
