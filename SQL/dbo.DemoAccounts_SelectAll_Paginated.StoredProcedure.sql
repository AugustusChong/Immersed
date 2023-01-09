USE [Immersed]
GO
/****** Object:  StoredProcedure [dbo].[DemoAccounts_SelectAll_Paginated]    Script Date: 12/17/2022 11:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Augustus Chong>
-- Create date: <11/23/2022>
-- Description: <Selects all Demo Accounts in Paginated View>
-- Code Reviewer: R****l L*****o

-- MODIFIED BY: author
-- MODIFIED DATE: mm/dd/yyyy
-- Code Reviewer: 
-- Note:
-- =============================================

CREATE PROC [dbo].[DemoAccounts_SelectAll_Paginated]
             @PageIndex int
            ,@PageSize int

AS

/*

    DECLARE  @PageIndex int = 0
            ,@PageSize int = 5

    EXECUTE dbo.DemoAccounts_SelectAll_Paginated
             @PageIndex
            ,@PageSize

*/

BEGIN

    DECLARE     @offset int = @PageIndex * @PageSize

    SELECT       [Id]
                ,[CreatedBy]
                ,[OrgId]
                ,[StartDate]
                ,[ExpirationDate]
                ,[TotalCount] = COUNT(1) OVER()
    FROM        [dbo].[DemoAccounts]

    ORDER BY    [Id]

    OFFSET      @offset ROWS
    FETCH NEXT  @PageSize ROWS ONLY

END

GO
