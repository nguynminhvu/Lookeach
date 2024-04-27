USE [Lookeach]
GO

INSERT INTO [dbo].[Roles]
           ([Id]
           ,[Name])
     VALUES
           (newid()
           ,'Customer')
GO

INSERT INTO [dbo].[Roles]
           ([Id]
           ,[Name])
     VALUES
           (newid()
           ,'Admin')
GO

INSERT INTO [dbo].[Roles]
           ([Id]
           ,[Name])
     VALUES
           (newid()
           ,'Merchant')
GO


