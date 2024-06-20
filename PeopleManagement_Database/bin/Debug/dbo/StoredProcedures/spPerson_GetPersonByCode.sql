CREATE PROCEDURE [dbo].[spPerson_GetPersonByCode]
	@Code INT
AS
BEGIN
	SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT code AS 'Code'
		  ,[name] AS 'Name'
		  ,surname AS 'Surname'
		  ,id_number AS 'IdNumber'
	FROM dbo.Persons 
	WHERE Code = @Code
END
