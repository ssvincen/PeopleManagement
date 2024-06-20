using Dapper;
using Microsoft.Extensions.Configuration;
using PeopleManagement_BO;
using System.Data;
using System.Data.SqlClient;

namespace PeopleManagement_Utility;

public class SqlDataAccess(IConfiguration config) : ISqlDataAccess
{
    private readonly IConfiguration _config = config;

    public async Task<T> UpsertData<T, U>(string storedProcedure, U parameters, string connectionId)
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<T> LoadSingleData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdataData<U>(string storedProcedure, U parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedResults<T>> LoadDataWithPagination<T, U>(PageFilterModel pageFilter, string storedProcedure, U parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        using var multi = await connection.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        var data = multi.ReadAsync<T>();
        var totalData = await multi.ReadFirstOrDefaultAsync<int>();
        var result = new PagedResults<T>(totalData, pageFilter.PageNumber, pageFilter.PageSize)
        {
            Items = data.Result
        };
        return result;
    }

    public async Task<T> DeleteData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
