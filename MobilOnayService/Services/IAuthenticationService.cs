using MobilOnayService.Models;
using System.Threading.Tasks;

namespace MobilOnayService.Services
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAsync(LoginModel loginModel);
    }
}
