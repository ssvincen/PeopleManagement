using PeopleManagement_BO;

namespace PeopleManagementAPI;

public static class DataFilter
{
    public static PageFilterModel PageFilterModel(string searchString, int pageNumber, int pageSize)
    {
        return new PageFilterModel()
        {
            SearchString = searchString,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public static PagedResults<T> GetPagedResults<T>(List<T> cachedItems, PageFilterModel pageFilter)
    {
        return new PagedResults<T>(cachedItems.Count, pageFilter.PageNumber, pageFilter.PageSize)
        {
            Items = cachedItems.Skip(pageFilter.PageNumber).Take(pageFilter.PageSize).ToList()
        };
    }
}
