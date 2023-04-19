using Microsoft.AspNetCore.Authentication.Certificate;
using OcelotPractice.Authentication.UserIdHeader;

namespace OcelotPractice.Services
{
    public static class AuthenticationServices
    {
        public static void AddAuthenticationServices(
            this IServiceCollection services, WebHostBuilderContext context)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Dynamic";
            })
            .AddPolicyScheme("Dynamic", "Certificate or UserIdHeader", options =>
            {
                options.ForwardDefaultSelector = httpContext =>
                {
                    var allowedUserIdAuthIps = context.Configuration["AllowUserIdHeaderAuthenticationToIpAddressed"]?.Trim();

                    if (!string.IsNullOrEmpty(allowedUserIdAuthIps))
                    {
                        if (allowedUserIdAuthIps == "*" || httpContext.Connection.RemoteIpAddress != null &&
                            allowedUserIdAuthIps.Contains(httpContext.Connection.RemoteIpAddress.ToString()))
                        {
                            var userId = httpContext.Request.Headers["user-id"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(userId))
                            {
                                return UserIdHeaderAuthenticationAppBuilderExtensions.AuthenticationScheme;
                            }
                        }
                    }

                    return CertificateAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddCertificate()
            .AddUserIdHeader();
        }
    }
}
