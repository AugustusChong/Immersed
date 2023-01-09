USE [Immersed]
GO
/****** Object:  StoredProcedure [dbo].[DemoAccounts_Select_Active]    Script Date: 01/03/2023 12:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Augustus Chong>
-- Create date: <11/26/2022>
-- Description: <Select Active Demo Accounts per Week for 8 Weeks starting from Today>
-- Code Reviewer: R****l L*****o

-- MODIFIED BY: author
-- MODIFIED DATE: mm/dd/yyyy
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[DemoAccounts_Select_Active]

AS

/*

    EXECUTE dbo.DemoAccounts_Select_Active

*/

BEGIN

    DECLARE  @GeneratingDateFrom DATE = DATEADD(WEEK, -7, GETUTCDATE())
            ,@GeneratingDateTo DATE = GETUTCDATE();
    WITH    GeneratedDates AS   (
                                SELECT	GeneratedDate = @GeneratingDateFrom
                                UNION	ALL
                                SELECT	GeneratedDate = DATEADD(DAY, 1, gd.[GeneratedDate])
                                FROM	GeneratedDates AS gd
                                WHERE	DATEADD(DAY, 1, gd.[GeneratedDate]) < @GeneratingDateTo
                                )
	
    SELECT  CONVERT(DATE, DATEADD(dd, 0 - (@@DATEFIRST + 6 + DATEPART(dw, gd.[GeneratedDate])) % 7, gd.[GeneratedDate])) AS [Week]
            ,ActiveAccounts =   (
                                SELECT COUNT(*)
                                FROM dbo.DemoAccounts AS da
                                WHERE CONVERT(DATE, DATEADD(dd, 0 - (@@DATEFIRST + 6 + DATEPART(dw, da.[StartDate])) % 7, da.[StartDate])) <= CONVERT(DATE, DATEADD(dd, 0 - (@@DATEFIRST + 6 + DATEPART(dw, gd.[GeneratedDate])) % 7, gd.[GeneratedDate])) 
                                AND da.[ExpirationDate] > GETUTCDATE()
                                )
    FROM        GeneratedDates AS gd
    LEFT JOIN	dbo.[DemoAccounts] AS da
    ON          gd.[GeneratedDate] = CONVERT(date, da.[StartDate])
    GROUP BY	CONVERT(DATE, DATEADD(dd, 0 - (@@DATEFIRST + 6 + DATEPART(dw, gd.[GeneratedDate])) % 7, gd.[GeneratedDate]))
    ORDER BY	[Week] DESC

END

GO
