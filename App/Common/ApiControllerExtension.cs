using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

namespace App.Common
{
    public static class ApiControllerExtension
    {
        public static string AsString(this ModelStateDictionary state)
        {
            var sb = new StringBuilder();
            foreach (var error in state.Keys.SelectMany(key => state[key].Errors))
            {
                sb.AppendLine(error.ErrorMessage);
            }
            return sb.ToString();
        }

        public static IHttpActionResult BadRequestError(this ApiController controller, ModelStateDictionary modelState)
        {
            return new BadRequestErrorMessageResult(modelState.AsString(), controller);
        }
    }
}