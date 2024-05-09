USE [Lookeach]
GO

INSERT INTO [dbo].[Actions]
           ([Id]
           ,[Name]
           ,[Value]
           ,[Index]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (newid()
           ,'Read'
           ,1
           ,0
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate()
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate())
GO


USE [Lookeach]
GO

INSERT INTO [dbo].[Actions]
           ([Id]
           ,[Name]
           ,[Value]
           ,[Index]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (newid()
           ,'Write'
           ,2
           ,1
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate()
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate())
GO


USE [Lookeach]
GO

INSERT INTO [dbo].[Actions]
           ([Id]
           ,[Name]
           ,[Value]
           ,[Index]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (newid()
           ,'Overwrite'
           ,3
           ,2
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate()
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate())
GO


USE [Lookeach]
GO

INSERT INTO [dbo].[Actions]
           ([Id]
           ,[Name]
           ,[Value]
           ,[Index]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (newid()
           ,'Delete'
           ,4
           ,3
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate()
           ,'5D1179AA-7385-4126-B5B6-EB6E1F06694C'
           ,getdate())
GO