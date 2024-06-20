CREATE PROCEDURE [dbo].[spPerson_GetAll]
	@SearchString VARCHAR(100) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    -- Calculate the starting row number
    DECLARE @StartRow INT;
    SET @StartRow = (@PageNumber - 1) * @PageSize;

    -- Create temporary table for paginated results
    SELECT code AS Code,
        [name] AS [Name],
         surname AS [Surname],
        id_number AS [IdNumber],
        COUNT(code) OVER() AS TotalItems
    INTO #TempResult
    FROM Persons
    WHERE 
        (@SearchString IS NULL OR 
         id_number LIKE '%' + @SearchString + '%' OR
         name LIKE '%' + @SearchString + '%' OR
         surname LIKE '%' + @SearchString + '%')
    ORDER BY code
    OFFSET @StartRow ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT Code,
        [Name],
        [Surname],
        IdNumber
    FROM #TempResult;

    SELECT TOP 1 TotalItems FROM #TempResult;

    DROP TABLE #TempResult;
END
