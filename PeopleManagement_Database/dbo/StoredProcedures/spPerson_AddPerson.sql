CREATE PROCEDURE [dbo].[spPerson_AddPerson]
	@Name VARCHAR(50),
    @Surname VARCHAR(50),
    @IdNumber VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DECLARE @IsSuccess BIT = 0, @Message VARCHAR(250) = '', @Status VARCHAR(250) = '';

    BEGIN TRANSACTION;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Persons WHERE id_number = @IdNumber)
        BEGIN
            SET @Message = 'Person with this ID number already exists.';
            SET @Status = 'Failure';
            ROLLBACK TRANSACTION;
            SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
            RETURN;
        END

        INSERT INTO Persons ([name], surname, id_number)
        VALUES (@Name, @Surname, @IdNumber);

        SET @IsSuccess = 1;
        SET @Message = 'Person successfully created.';
        SET @Status = 'Success';

        COMMIT TRANSACTION;
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
