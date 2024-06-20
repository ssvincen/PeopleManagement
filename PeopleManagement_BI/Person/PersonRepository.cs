using PeopleManagement_BO;
using PeopleManagement_Utility;

namespace PeopleManagement_BI;

public class PersonRepository(ISqlDataAccess sqlDataAccess) : IPersonRepository
{
    private readonly ISqlDataAccess _dataAccess = sqlDataAccess;

    public async Task<ReturnResponse> AddPersonAsync(PersonRequest personRequest)
    {
        var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spPerson_AddPerson",
            new { personRequest.Name, personRequest.Surname, personRequest.IdNumber });
        return results;
    }

    public async Task<PagedResults<PersonResponse>> GetPersonListAsync(PageFilterModel pageFilterModel)
    {
        var results = await _dataAccess.LoadDataWithPagination<PersonResponse, dynamic>(pageFilterModel, "dbo.spPerson_GetAll",
           new { pageFilterModel.SearchString, pageFilterModel.PageNumber, pageFilterModel.PageSize });
        return results;
    }

    public async Task<ReturnResponse> UpdatePersonAsync(PersonResponse personResponse)
    {
        var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spPerson_UpdatePerson",
           new { PersonCode = personResponse.Code, personResponse.Name, personResponse.Surname, personResponse.IdNumber });
        return results;
    }

    public async Task<PersonResponse> GetPersonByCodeAsync(int code)
    {
        var results = await _dataAccess.LoadSingleData<PersonResponse, dynamic>("dbo.spPerson_GetPersonByCode",
            new { Code = code });
        return results;
    }

    public async Task<ReturnResponse> DeletePersonAsync(int code)
    {
        var results = await _dataAccess.UpsertData<ReturnResponse, dynamic>("dbo.spPerson_DeletePersonByCode",
            new { PersonCode = code });
        return results;
    }

    public Task<PersonResponse> GetPersonByIdNumberAsync(string idNumber)
    {
        throw new NotImplementedException();
    }



}
