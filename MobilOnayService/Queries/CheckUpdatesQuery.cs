using MediatR;
using MobilOnayService.Models;
using MobilOnayService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MobilOnayService.Queries
{
    public class CheckUpdatesQuery : IRequest<UpdateDetails>
    {
        public string PackageName { get; set; }

        public string Version { get; set; }
    }

    public class CheckUpdatesQueryHandler : IRequestHandler<CheckUpdatesQuery, UpdateDetails>
    {
        private readonly IUpdateManagerRepository _updateManagerRepository;

        public CheckUpdatesQueryHandler(IUpdateManagerRepository updateManagerRepository)
        {
            _updateManagerRepository = updateManagerRepository;
        }
        public async Task<UpdateDetails> Handle(CheckUpdatesQuery request, CancellationToken cancellationToken)
        {
            return await _updateManagerRepository.CheckUpdatesAsync(new PackageModel() { PackageName = request.PackageName, Version = request.Version });
        }
    }
}
