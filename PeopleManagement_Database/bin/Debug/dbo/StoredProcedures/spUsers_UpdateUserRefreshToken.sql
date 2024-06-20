CREATE PROCEDURE [dbo].[spUsers_UpdateUserRefreshToken]
	@EmailAddress NVARCHAR(200),
	@RefreshToken NVARCHAR(200),
	@RefreshTokenExpiryTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Users]
	SET RefreshToken = @RefreshToken
	   ,RefreshTokenExpiryTime = @RefreshTokenExpiryTime
	   ,LastLogin = GETDATE()
	WHERE EmailAddress = @EmailAddress
END