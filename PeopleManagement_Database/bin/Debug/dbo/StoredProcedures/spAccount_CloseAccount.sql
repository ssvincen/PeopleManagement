CREATE PROCEDURE [dbo].[spAccount_CloseAccount]
	@AccountCode INT
AS
BEGIN
    DECLARE @IsSuccess BIT = 0, @Message VARCHAR(250) = '', @Status VARCHAR(250) = ''; 
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE code = @AccountCode)
    BEGIN
        SET @Message = 'Account does not exist.';
        SET @Status = 'Failure';
        SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Accounts WHERE code = @AccountCode AND outstanding_balance <> 0)
    BEGIN
        SET @Message = 'Account cannot be closed because the outstanding balance is not zero.';
        SET @Status = 'Failure';
        SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE Accounts
        SET statusCode = (SELECT Code FROM [Status] WHERE [Name] = 'Closed')
        WHERE code = @AccountCode;

        COMMIT TRANSACTION;

        SET @IsSuccess = 1;
        SET @Message = 'Account successfully closed.';
        SET @Status = 'Success';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @IsSuccess = 0;
        SET @Message = ERROR_MESSAGE();
        SET @Status = 'Failure';
    END CATCH

    SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
END

