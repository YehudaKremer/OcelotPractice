using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace OcelotPractice.Authentication.UserIdHeader
{
    public class UserIdHeaderAuthenticationSchemeHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public UserIdHeaderAuthenticationSchemeHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get user id from user-id header
            var userId = Request.Headers["user-id"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authentication failed: empty user-id header"));
            }

            // If the user id is valid, return success
            var claims = new List<Claim>
            {
                 new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", userId)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
