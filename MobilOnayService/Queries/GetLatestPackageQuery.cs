using MediatR;
using MobilOnayService.Models;
using MobilOnayService.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MobilOnayService.Queries
{
    public class GetLatestPackageQuery : IRequest<List<PackageModel>>
    {
        [Required]
        public string PackageName { get; set; }
    }

    public class GetLatestPackageQueryHandler : IRequestHandler<GetLatestPackageQuery, List<PackageModel>>
    {
        private readonly IUpdateManagerRepository _updateManagerRepository;

        public GetLatestPackageQueryHandler(IUpdateManagerRepository updateManagerRepository)
        {
            _updateManagerRepository = updateManagerRepository;
        }

        public async Task<List<PackageModel>> Handle(GetLatestPackageQuery request, CancellationToken cancellationToken)
        {
            return await _updateManagerRepository.GetLatestPackageAsync(request.PackageName);
        }
    }
}
