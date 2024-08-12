using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ProductAPI.Auth
{
    

     public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
                                           ILoggerFactory logger,
                                           UrlEncoder encoder,
                                           ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
           
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("ApiKey"))
            {
                return AuthenticateResult.Fail("API key is missing");
            }

            var apiKey = Request.Headers["ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return AuthenticateResult.Fail("API key is missing");
            }

            if (!IsApiKeyValid(apiKey))
            {
                return AuthenticateResult.Fail("Invalid API key");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "ApiKeyUser")
                
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private bool IsApiKeyValid(string apiKey)
        {
            if (apiKey != "Produtos 123456789")
            {
                return false;
            }

            return true;
        }
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKeyScheme";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}