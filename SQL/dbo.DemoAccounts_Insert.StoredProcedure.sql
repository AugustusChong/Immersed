USE [Immersed]
GO
/****** Object:  StoredProcedure [dbo].[DemoAccounts_Insert]    Script Date: 12/17/2022 11:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Augustus Chong>
-- Create date: <11/23/2022>
-- Description: <Creates new Demo Account>
-- Code Reviewer: R****l L*****o

-- MODIFIED BY: author
-- MODIFIED DATE: mm/dd/yyyy
-- Code Reviewer: 
-- Note:
-- =============================================

CREATE PROC [dbo].[DemoAccounts_Insert]
             @Id int OUTPUT
            ,@CreatedBy int
            ,@OrgId int
            ,@StartDate datetime2(7)
            ,@ExpirationDate datetime2(7)

AS

/*

  DECLARE  @Id int = 0
          ,@CreatedBy int = 91
          ,@OrgId int = 73
          ,@StartDate datetime2(7) = GETUTCDATE()
          ,@ExpirationDate datetime2(7) = '12-30-2022'

  EXECUTE dbo.DemoAccounts_Insert
          @Id OUTPUT
         ,@CreatedBy
         ,@OrgId
         ,@StartDate
         ,@ExpirationDate
         
  EXECUTE dbo.DemoAccounts_SelectById @Id

*/

BEGIN

  INSERT INTO [dbo].[DemoAccounts]
              (
               [CreatedBy]
              ,[OrgId]
              ,[StartDate]
              ,[ExpirationDate]
              )
  VALUES      (
               @CreatedBy
              ,@OrgId
              ,@StartDate
              ,@ExpirationDate
              )
  SET @Id = SCOPE_IDENTITY()

END

GO
