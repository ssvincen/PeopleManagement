CREATE PROCEDURE [dbo].[spAccount_GetAccountByCode]
	@Code INT
AS
BEGIN
	SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT a.code AS 'Code'
	      ,a.account_number AS 'AccountNumber'
		  ,a.outstanding_balance AS 'OutstandingBalance'
		  ,s.[Name] AS 'AccountStatus'
		  ,a.person_code AS 'PersonCode'
	FROM dbo.Accounts a LEFT JOIN dbo.[Status] s
	ON a.statusCode = s.Code
	WHERE a.Code = @Code

END
