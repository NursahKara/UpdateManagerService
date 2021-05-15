using MobilOnayService.Helpers;
using MobilOnayService.Models;
using MobilOnayService.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MobilOnayService.Repositories
{
    public class UpdateManagerRepository : IUpdateManagerRepository
    {
        private readonly UserData _userData;
        private readonly IOracleProvider _oracleProvider;

        public UpdateManagerRepository(IPrincipal principal, IOracleProvider oracleProvider)
        {
            _userData = TokenHelper.GetUserData(principal as ClaimsPrincipal);
            _oracleProvider = oracleProvider;
        }

        public async Task<List<PackageModel>> GetLatestPackageAsync(string packageName)
        {
            try
            {
                var result = new List<PackageModel>();
                var sql = "SELECT PACKAGE_NAME, VERSION, APK_URL, CREATION_DATE" +
                          "FROM IFSAPP.UPDATE_MANAGER_TAB  " +
                          "WHERE  PACKAGE_NAME='" + packageName + "' " +
                          "ORDER BY CREATION_DATE";

                var dt = await _oracleProvider.QueryAsync(_userData, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new PackageModel()
                        {
                            PackageName = row["PACKAGE_NAME"].ToString(),
                            Version = row["VERSION"].ToString(),
                            Url = row["APK_URL"].ToString(),
                            CreationDate = DateTime.Parse(row["CREATION_DATE"].ToString())
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, typeof(UpdateManagerRepository).Name, "GetLatestPackageAsync");
            }
        }

        public async Task<bool> AddPackageAsync(PackageModel model)
        {
            try
            {
                var result = new List<PackageModel>();
                var sql = "BEGIN " +
                            " UPDATE_MANAGER_UTILITY_API.ADD_NEW_VERSION('" +
                                                                                  model.PackageName +
                                                                                  "','" +
                                                                                  model.Version +
                                                                                  "','" +
                                                                                  model.Url +
                                                                                  "'); " +
                                                                                  "COMMIT; " +
                           "END; " ;
                         

                return await _oracleProvider.ExecuteAsync(_userData, sql) > 0;

            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, typeof(UpdateManagerRepository).Name, "GetLatestPackageAsync");
            }
        }

        public async Task<UpdateDetails> CheckUpdatesAsync(PackageModel model)
        {
            try
            {
                var result = new UpdateDetails();
                var sql = "SELECT PACKAGE_NAME, VERSION, APK_URL, CREATION_DATE " +
                          "FROM IFSAPP.UPDATE_MANAGER_TAB  " +
                          "WHERE  PACKAGE_NAME='" + model.PackageName + "' " +
                          "ORDER BY CREATION_DATE DESC " +
                          "OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

                var dt = await _oracleProvider.QueryAsync(_userData, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                     result = new UpdateDetails()
                    {
                        PackageName = row["PACKAGE_NAME"].ToString(),
                        Version = row["VERSION"].ToString(),
                        Url = row["APK_URL"].ToString(),
                        CreationDate = DateTime.Parse(row["CREATION_DATE"].ToString()),
                        UpToDate = model.Version.Equals(row["VERSION"])
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, typeof(UpdateManagerRepository).Name, "CheckUpdatesAsync");
            }
        }
    }
}
