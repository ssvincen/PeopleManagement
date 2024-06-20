CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[FirstName] NVARCHAR(150) NOT NULL, 
	[Surname] NVARCHAR(150) NOT NULL, 
	[MobileNumber] NVARCHAR(20) NOT NULL, 
	[MobileNumberConfirmed] BIT NOT NULL DEFAULT 0,
	[EmailAddress] NVARCHAR(200) NOT NULL, 
	[PasswordHash] NVARCHAR(MAX) NOT NULL, 
	[EmailConfirmed] BIT NOT NULL DEFAULT 0, 
	[OTP] NVARCHAR(50) NULL, 
	[EmailOTP] NVARCHAR(50) NULL, 
	[DateCreated] DATETIME DEFAULT GETDATE(),
	[LastLogin] DATETIME,
	[Active] BIT NOT NULL DEFAULT 1,
	[PasswordLastUpdate] DATETIME DEFAULT GETDATE(),
	[RefreshToken] NVARCHAR(200) NULL,
	[RefreshTokenExpiryTime] DATETIME NULL,
	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED (Id ASC)
)

go;
CREATE NONCLUSTERED INDEX IX_Users_EmailAddress
ON [dbo].[Users] ([EmailAddress]);