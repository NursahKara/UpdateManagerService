using MobilOnayService.Helpers;
using MobilOnayService.Models;
using MobilOnayService.Providers;
using System;
using System.Threading.Tasks;

namespace MobilOnayService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        readonly static string ClassName = "MobilOnayService.Services.AuthenticationService";
        readonly IOracleProvider _oracleProvider;

        public AuthenticationService(IOracleProvider oracleProvider)
        {
            _oracleProvider = oracleProvider;
        }

        public async Task<bool> AuthenticateAsync(LoginModel loginModel)
        {
            try
            {
                var userData = new UserData()
                {
                    Username = loginModel.Username,
                    EncryptedPassword = PasswordHelper.Encrypt(loginModel.Password)
                };
                var dt = await _oracleProvider.QueryAsync(userData, "SELECT 1 FROM DUAL");
                return true;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "AuthenticateAsync");
            }
        }
    }
}
