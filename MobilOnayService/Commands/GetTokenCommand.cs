using MediatR;
using MobilOnayService.Helpers;
using MobilOnayService.Models;
using MobilOnayService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MobilOnayService.Commands
{
    public class GetTokenCommand : IRequest<User>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class GetTokenCommandHandler : IRequestHandler<GetTokenCommand, User>
    {
        private readonly IAuthenticationService _authenticationService;

        public GetTokenCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<User> Handle(GetTokenCommand request, CancellationToken cancellationToken)
        {

            var isAuthenticated = await _authenticationService.AuthenticateAsync(new LoginModel() { Username = request.Username, Password = request.Password });
            if (!isAuthenticated)
                throw new Exception("Kullanıcı adı veya şifre hatalı!");

            var userData = new UserData()
            {
                Username = request.Username,
                EncryptedPassword = PasswordHelper.Encrypt(request.Password),
            };
            var claims = TokenHelper.GenerateClaims(request.Username, userData);
            var token = TokenHelper.GenerateToken(claims, out DateTime expires);

            return new User()
            {
                Username = userData.Username,
                Token = token,
                Expires = expires.ToString("yyyy-MM-ddTHH:mm")
            };
        }
    }
}
