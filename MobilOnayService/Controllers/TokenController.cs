using MobilOnayService.Helpers;
using MobilOnayService.Models;
using MobilOnayService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MobilOnayService.Commands;
using Microsoft.AspNetCore.Authorization;

namespace MobilOnayService.Controllers
{
    public class TokenController : ApiControllerBase
    {
        protected readonly IAuthenticationService _authenticationService;

        public TokenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<User> Login([FromForm] GetTokenCommand command)
            => await Mediator.Send(command);
    }
}
