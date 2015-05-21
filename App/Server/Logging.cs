using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace App.Plugins
{
    public class Logging : OwinMiddleware
    {
        public Logging(OwinMiddleware next) 
            : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("Processing request: {0}:[{1}], from: [{2}]",
                context.Request.Method,
                context.Request.Uri.PathAndQuery,
                context.Request.RemoteIpAddress));

            if (context.Request.User != null &&
                context.Request.User.Identity.IsAuthenticated)
            {
                sb.Append(string.Format(", user name: [{0}]", context.Request.User.Identity.Name));
            }            

            return Next.Invoke(context);
        }
        
    }
}