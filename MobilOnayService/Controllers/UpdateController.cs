using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobilOnayService.Commands;
using MobilOnayService.Models;
using MobilOnayService.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilOnayService.Controllers
{
    public class UpdateController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddPackage([FromForm] AddPackageCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpGet]
        public async Task<UpdateDetails> CheckUpdates([FromQuery] CheckUpdatesQuery query)
        => await Mediator.Send(query);
    }
}
