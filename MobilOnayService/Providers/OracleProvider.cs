using MobilOnayService.Helpers;
using MobilOnayService.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MobilOnayService.Providers
{
    public class OracleProvider : IOracleProvider
    {
        readonly static string ClassName = "MobilOnayService.Providers.OracleProvider";

        public OracleProvider()
        {

        }

        public async Task<int> ExecuteAsync(UserData userData, string commandText, CommandType commandType = CommandType.Text, List<DatabaseParameter> parameters = null)
        {
            try
            {
                var connString =
                    BuildConnectionString(
                        Properties.AppSettings.ConnectionString,
                        userData.Username,
                        PasswordHelper.Decrypt(userData.EncryptedPassword));

                using var conn = new OracleConnection(connString);
                try
                {
                    await conn.OpenAsync();
                    var tx = (OracleTransaction)await conn.BeginTransactionAsync();

                    commandText = commandText.Replace("&AO", Properties.AppSettings.ApplicationOwner);
                    using var cmd = new OracleCommand(commandText, conn)
                    {
                        Transaction = tx,
                        CommandType = commandType
                    };

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var paremeter in parameters)
                        {
                            if (commandText.IndexOf(":" + paremeter.Name) > -1)
                            {
                                cmd.Parameters.Add(GetParameter(paremeter.Name, paremeter.Value, paremeter.DataType, paremeter.Direction, paremeter.Size));
                            }
                        }
                    }

                    return await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        await conn.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "ExecuteAsync");
            }
        }

        public async Task<DataTable> QueryAsync(UserData userData, string sql, List<DatabaseParameter> parameters = null)
        {
            try
            {
                var connString = 
                    BuildConnectionString(
                        Properties.AppSettings.ConnectionString,
                        userData.Username, 
                        PasswordHelper.Decrypt(userData.EncryptedPassword));

                using var conn = new OracleConnection(connString);
                try
                {
                    await conn.OpenAsync();
                    var tx = (OracleTransaction)await conn.BeginTransactionAsync();

                    DataTable result = new DataTable("DATA");
                    sql = sql.Replace("&AO", Properties.AppSettings.ApplicationOwner);
                    using var cmd = new OracleCommand(sql, conn);
                    cmd.Transaction = tx;

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var paremeter in parameters)
                        {
                            if (sql.IndexOf(":" + paremeter.Name) > -1)
                            {
                                cmd.Parameters.Add(GetParameter(paremeter.Name, paremeter.Value, paremeter.DataType, paremeter.Direction, paremeter.Size));
                            }
                        }
                    }

                    var reader = await cmd.ExecuteReaderAsync();
                    result.Load(reader);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        await conn.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "QueryAsync");
            }
        }


        #region private

        private string BuildConnectionString(string dataSource, string username, string password)
        {
            var builder = new OracleConnectionStringBuilder()
            {
                DataSource = dataSource,
                UserID = username,
                Password = password,
            };
            return builder.ConnectionString;
        }

        private DbParameter GetParameter(string parameterName, object value, DbType dataType, ParameterDirection direction, int size = 0)
        {
            var result = new OracleParameter
            {
                ParameterName = parameterName,
                Value = value ?? DBNull.Value,
                Direction = direction,
            };

            switch (dataType)
            {
                case DbType.Binary:
                    result.OracleDbType = OracleDbType.Blob;
                    break;

                case DbType.Date:
                case DbType.DateTime:
                    result.OracleDbType = OracleDbType.Date;
                    break;

                case DbType.Decimal:
                case DbType.Int32:
                    result.OracleDbType = OracleDbType.Decimal;
                    break;

                case DbType.String:
                    result.OracleDbType = OracleDbType.Varchar2;
                    break;
            }


            if (size > 0)
            {
                result.Size = size;
            }
            return result;
        }

        #endregion
    }
}
