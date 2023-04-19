using Microsoft.AspNetCore.Authentication;

namespace OcelotPractice.Authentication.UserIdHeader
{
    public static class UserIdHeaderAuthenticationAppBuilderExtensions
    {
        public const string AuthenticationScheme = "UserIdHeader";

        public static AuthenticationBuilder AddUserIdHeader(this AuthenticationBuilder builder, Action<AuthenticationSchemeOptions>? configureOptions = default)
            => builder.AddScheme<AuthenticationSchemeOptions, UserIdHeaderAuthenticationSchemeHandler>(
                AuthenticationScheme, configureOptions);
    }
}
