CREATE PROCEDURE [dbo].[spPerson_DeletePersonByCode]
	@PersonCode INT
AS
BEGIN
	DECLARE @IsSuccess BIT = 0, @Message VARCHAR(250) = '', @Status VARCHAR(250) = '';  
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Persons WHERE code = @PersonCode)
    BEGIN
        SET @Message = 'Person does not exist.';
        SET @Status = 'Failure';
        SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
        RETURN;
    END

	IF EXISTS (SELECT 1 FROM Accounts WHERE person_code = @PersonCode 
       AND (statusCode IS NULL OR statusCode <> (SELECT Code FROM [Status] WHERE [Name] = 'Closed'))
    )
    BEGIN
        SET @Message = 'Cannot delete person with open accounts.';
        SET @Status = 'Failure';
        SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
        RETURN;
    END

    BEGIN TRANSACTION;
    BEGIN TRY

        DELETE FROM Accounts
        WHERE person_code = @PersonCode;


        DELETE FROM Persons
        WHERE code = @PersonCode;

        COMMIT TRANSACTION;

        SET @IsSuccess = 1;
        SET @Status = 'Success';
        SET @Message = 'Person successfully deleted.';
    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Message = ERROR_MESSAGE();
        SET @Status = 'Failure';
    END CATCH
    SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
END
