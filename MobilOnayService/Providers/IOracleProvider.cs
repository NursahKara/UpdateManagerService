using MobilOnayService.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MobilOnayService.Providers
{
    public interface IOracleProvider
    {
        Task<DataTable> QueryAsync(UserData userData, string sql, List<DatabaseParameter> parameters = null);

        Task<int> ExecuteAsync(UserData userData, string commandText, CommandType commandType = CommandType.Text, List<DatabaseParameter> parameters = null);
    }
}
