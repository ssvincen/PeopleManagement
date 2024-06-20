namespace PeopleManagement_BO;

public class PagedResults<T>
{
    public PagedResults(int totalItems, int pageNumber = 1, int pageSize = 10, int maxNavigationPages = 5)
    {
        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }
        else if (pageNumber > totalPages)
        {
            pageNumber = totalPages;
        }

        int startPage;
        int endPage;
        if (totalPages <= maxNavigationPages)
        {
            startPage = 1;
            endPage = totalPages;
        }
        else
        {
            var maxPagesBeforeActualPage = (int)Math.Floor(maxNavigationPages / (decimal)2);
            var maxPagesAfterActualPage = (int)Math.Ceiling(maxNavigationPages / (decimal)2) - 1;
            if (pageNumber <= maxPagesBeforeActualPage)
            {
                // Page at the start
                startPage = 1;
                endPage = maxNavigationPages;
            }
            else if (pageNumber + maxPagesAfterActualPage >= totalPages)
            {
                // Page at the end
                startPage = totalPages - maxNavigationPages + 1;
                endPage = totalPages;
            }
            else
            {
                // Page in the middle
                startPage = pageNumber - maxPagesBeforeActualPage;
                endPage = pageNumber + maxPagesAfterActualPage;
            }
        }

        // Create list of Page numbers
        var pageNumbers = Enumerable.Range(startPage, (endPage + 1) - startPage);

        StartPage = startPage;
        EndPage = endPage;
        PageNumber = pageNumber;
        PageNumbers = pageNumbers;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }

    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int MaxNavigationPages { get; private set; } = 5;
    public int PageNumber { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public int TotalPages { get; private set; }
    public int StartPage { get; private set; }
    public int EndPage { get; private set; }
    public IEnumerable<int> PageNumbers { get; private set; }
}