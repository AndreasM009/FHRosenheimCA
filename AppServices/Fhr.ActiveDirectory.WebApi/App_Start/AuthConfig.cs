using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Fhr.ActiveDirectory.WebApi
{
    public class AuthConfig
    {
        private static readonly string mAadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        private static readonly string mTenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static readonly string mClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static readonly string mSignUpPolicy = ConfigurationManager.AppSettings["ida:SignUpPolicyId"];
        private static readonly string mSignInPolicy = ConfigurationManager.AppSettings["ida:SignInPolicyId"];
        private static readonly string mEditProfilePolicy = ConfigurationManager.AppSettings["ida:UserProfilePolicyId"];

        public static void Configure(IAppBuilder aAppBuilder)
        {
            aAppBuilder.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(mSignInPolicy));
            aAppBuilder.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(mSignUpPolicy));            
        }

        private static OAuthBearerAuthenticationOptions CreateBearerOptionsFromPolicy(string aPolicy)
        {
            TokenValidationParameters tvps = new TokenValidationParameters
                                             {
                                                 ValidAudience = mClientId,
                                                 AuthenticationType = aPolicy
                                             };
            

            return new OAuthBearerAuthenticationOptions
                   {
                       AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(String.Format(mAadInstance, mTenant, aPolicy)))
                   };
        }
    }
}