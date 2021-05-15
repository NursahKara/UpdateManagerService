using MobilOnayService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilOnayService.Repositories
{
    public interface IUpdateManagerRepository
    {
        Task<List<PackageModel>> GetLatestPackageAsync(string packageName);
        Task<bool> AddPackageAsync(PackageModel model);
        Task<UpdateDetails> CheckUpdatesAsync(PackageModel model);

    }
}
