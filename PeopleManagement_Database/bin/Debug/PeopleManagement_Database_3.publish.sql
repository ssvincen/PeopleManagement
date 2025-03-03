﻿/*
Deployment script for PeopleManagementDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "PeopleManagementDB"
:setvar DefaultFilePrefix "PeopleManagementDB"
:setvar DefaultDataPath "C:\Users\ssvin\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\ssvin\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
CREATE TABLE #aspNetRoles ([Id] [INT] IDENTITY(1,1) NOT NULL, [Name] [NVARCHAR](200) NOT NULL)
INSERT INTO #aspNetRoles([Name])
VALUES ('Admin'),
	   ('General')
	   --Insert into physical table
INSERT INTO [dbo].[Roles] ([Name])
SELECT [Name] FROM #aspNetRoles
WHERE [Name] NOT IN (SELECT [Name] FROM [dbo].[Roles] WITH (NOLOCK))
DROP TABLE #aspNetRoles
GO

GO
PRINT N'Update complete.';


GO
