CREATE PROCEDURE [dbo].[spUsers_AddUser]
	@FirstName NVARCHAR(150), 
	@Surname NVARCHAR(150), 
	@MobileNumber NVARCHAR(20), 
	@EmailAddress NVARCHAR(200), 
	@PasswordHash NVARCHAR(MAX), 
	@OTP NVARCHAR(50), 
	@EmailOTP NVARCHAR(50)
AS 
BEGIN
	 DECLARE @IsSuccess BIT = 0, @Message VARCHAR(250) = '', @Status VARCHAR(250) = '';  
	 DECLARE @NewUserId INT;
	 SET NOCOUNT ON;
	 BEGIN TRY
		BEGIN TRANSACTION;

        INSERT INTO Users (FirstName, Surname, MobileNumber, EmailAddress, PasswordHash, OTP, EmailOTP)
        VALUES (@FirstName, @Surname, @MobileNumber, @EmailAddress, @PasswordHash, @OTP, @EmailOTP);

		SET @NewUserId = SCOPE_IDENTITY();

		INSERT INTO dbo.UserRoles(FK_UserId, FK_RoleId)
		VALUES(@NewUserId , 1)

        COMMIT TRANSACTION;

        SET @IsSuccess = 1;
        SET @Message = 'User created successfully with ID: ' + CAST(@NewUserId AS VARCHAR(10));
        SET @Status = 'Success';

	 END TRY
	 BEGIN CATCH
		IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

		SELECT @IsSuccess = 0, @Message = ERROR_MESSAGE(), @Status = 'Failure';
	 END CATCH

	 SELECT @IsSuccess AS [IsSuccess], @Message AS [Message], @Status AS [Status];
END
