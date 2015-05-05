using System.Security.Claims;

namespace App.Identity
{
    public interface ILoginProvider
    {
        bool ValidateCredentials(string userName, string password, out ClaimsIdentity identity);
    }
}
