using PeopleManagement_BO;

namespace PeopleManagement_BI;

public interface IPersonRepository
{
    Task<ReturnResponse> AddPersonAsync(PersonRequest personRequest);
    Task<PagedResults<PersonResponse>> GetPersonListAsync(PageFilterModel pageFilterModel);
    Task<PersonResponse> GetPersonByCodeAsync(int code);
    Task<PersonResponse> GetPersonByIdNumberAsync(string idNumber);
    Task<ReturnResponse> UpdatePersonAsync(PersonResponse personResponse);
    Task<ReturnResponse> DeletePersonAsync(int code);

}
