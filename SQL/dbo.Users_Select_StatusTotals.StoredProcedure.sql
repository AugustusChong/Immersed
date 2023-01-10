USE [Immersed]
GO
/****** Object:  StoredProcedure [dbo].[Users_Select_StatusTotals]    Script Date: 12/17/2022 11:12:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Augustus Chong>
-- Create date: <11/17/2022>
-- Description: <Selects all Active, Inactive, Pending, Flagged, and Removed Users with Total Users>
-- Code Reviewer: H*i S***e Jr.

-- MODIFIED BY: author
-- MODIFIED DATE: mm/dd/yyyy
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[Users_Select_StatusTotals]
            @Id int

AS

/*

    DECLARE @Id int = 90
    EXECUTE [dbo].[Users_Select_StatusTotals] 
    @Id

*/

BEGIN

    SELECT   [Id] 
            ,[ActiveUsers] =    (
                                SELECT  COUNT ([StatusTypeId])
                                FROM    [dbo].[Users]
                                WHERE   [StatusTypeId] = 1
                                )
            ,[InactiveUsers] =  (
                                SELECT  COUNT ([StatusTypeId])
                                FROM    [dbo].[Users]
                                WHERE   [StatusTypeId] = 2
                                )
            ,[PendingUsers] =   (
                                SELECT  COUNT(*)
                                FROM    [dbo].[InviteMembers]
                                )
            ,[FlaggedUsers]	=   (
                                SELECT  COUNT ([StatusTypeId])
                                FROM    [dbo].[Users]
                                WHERE   [StatusTypeId] = 4
                                )
            ,[RemovedUsers]	=   (
                                SELECT  COUNT ([StatusTypeId])
                                FROM    [dbo].[Users]
                                WHERE   [StatusTypeId] = 5
                                )
            ,[TotalUsers] =     (
                                SELECT COUNT ([Id])
                                FROM [dbo].[Users]
                                )

    FROM    [dbo].[Users]
    WHERE   [Id] = @Id

END

GO
