using Microsoft.Owin.Security.OAuth;
using Owin;

namespace App
{
    public partial class Startup
    {
        static Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions();
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}