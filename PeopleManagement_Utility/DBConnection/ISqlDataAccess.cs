using PeopleManagement_BO;

namespace PeopleManagement_Utility;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task<T> LoadSingleData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task UpdataData<U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task<T> UpsertData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task<T> DeleteData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
    Task<PagedResults<T>> LoadDataWithPagination<T, U>(PageFilterModel pageFilter, string storedProcedure, U parameters, string connectionId = "Default");

}
