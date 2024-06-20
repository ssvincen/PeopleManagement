CREATE PROCEDURE [dbo].[spAccount_AddAccount]
	@PersonCode INT,
    @AccountNumber VARCHAR(50),
    @OutstandingBalance MONEY
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

    -- Check for duplicate account number
    IF EXISTS (SELECT 1 FROM Accounts WHERE account_number = @AccountNumber)
    BEGIN
        SET @Message = 'Account number already exists.';
        SET @Status = 'Failure';
        SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
        RETURN;
    END

    -- Insert the new account
    BEGIN TRY
        INSERT INTO Accounts(person_code, account_number, outstanding_balance, statusCode)
        VALUES (@PersonCode, @AccountNumber, @OutstandingBalance, 1);

        SET @Message = 'Account successfully created.';
        SET @Status = 'Success';
    END TRY

    BEGIN CATCH
         IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
         
        SET @Message = ERROR_MESSAGE();
        SET @Status = 'Failure';

    END CATCH
    SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
END