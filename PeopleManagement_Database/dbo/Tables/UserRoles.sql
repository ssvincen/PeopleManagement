﻿CREATE TABLE [dbo].[UserRoles]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[FK_UserId] INT NOT NULL, 
	[FK_RoleId] INT NOT NULL, 
	CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT [FK_UserId] FOREIGN KEY ([FK_UserId]) REFERENCES [dbo].[Users](Id),
	CONSTRAINT [FK_UserRoleId] FOREIGN KEY ([FK_RoleId]) REFERENCES [dbo].[Roles](Id)
)
