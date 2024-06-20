CREATE PROCEDURE [dbo].[spTransaction_AddTransaction]
	@AccountCode INT,
    @TransactionDate DATETIME,
    @CapturedDate DATETIME,
    @Amount MONEY,
    @Description NVARCHAR(255)
AS
BEGIN
    DECLARE @IsSuccess BIT = 0, @Message VARCHAR(250) = '', @Status VARCHAR(250) = '';  
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Transactions(account_code, transaction_date, capture_date, amount, [description])
        VALUES (@AccountCode, @TransactionDate, @CapturedDate, @Amount, @Description);

        UPDATE Accounts
        SET outstanding_balance = outstanding_balance + @Amount
        WHERE code = @AccountCode;

        COMMIT TRANSACTION;
        SET @IsSuccess = 1;
        SET @Message = 'Transaction successfully added.';
        SET @Status = 'Success';

    END TRY
    BEGIN CATCH
       IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

       SELECT @IsSuccess = 0, @Message = ERROR_MESSAGE(), @Status = 'Failure';

    END CATCH
    SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
END;