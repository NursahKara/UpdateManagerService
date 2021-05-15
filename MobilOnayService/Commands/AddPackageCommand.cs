using MediatR;
using MobilOnayService.Models;
using MobilOnayService.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MobilOnayService.Commands
{
    public class AddPackageCommand : IRequest
    {
        [Required]
        public string PackageName { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public string Url { get; set; }
    }

    public class AddPackageCommandHandler : IRequestHandler<AddPackageCommand, Unit>
    {
        private readonly IUpdateManagerRepository _updateManagerRepository;

        public AddPackageCommandHandler(IUpdateManagerRepository updateManagerRepository)
        {
            _updateManagerRepository = updateManagerRepository;
        }

        public async Task<Unit> Handle(AddPackageCommand request, CancellationToken cancellationToken)
        {
            await _updateManagerRepository.AddPackageAsync(new PackageModel()
            { 
                PackageName = request.PackageName,
                Url = request.Url, 
                Version = request.Version
            });

            return Unit.Value;
        }
    }
}
