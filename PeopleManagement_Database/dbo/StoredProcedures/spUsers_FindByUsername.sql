CREATE PROCEDURE [dbo].[spUsers_FindByUsername]
	@EmailAddress NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id
          ,FirstName
		  ,Surname
		  ,MobileNumber
		  ,MobileNumberConfirmed
		  ,OTP
		  ,EmailAddress
		  ,PasswordHash
		  ,EmailConfirmed
		  ,EmailOTP
		  ,DateCreated
		  ,LastLogin
		  ,PasswordLastUpdate
		  ,Active
		  ,RefreshToken
		  ,RefreshTokenExpiryTime
	FROM [dbo].[Users] 
	WHERE EmailAddress = @EmailAddress
	
END
