CREATE FUNCTION [dbo].[TicksToDateTime]
(@Ticks BIGINT) 
RETURNS DATETIME AS
BEGIN 
         -- First, we will convert the ticks into a datetime value with UTC time 
         DECLARE @BaseDate DATETIME = '01/01/1900'; 
         -- The numeric constant is the number of .Net Ticks between the System.DateTime.MinValue (01/01/0001) and the base date (01/01/1900) 
		 DECLARE @NetFxTicksFromBaseDate BIGINT = @Ticks - 599266080000000000; 
		 -- The numeric constant is the number of .Net Ticks in a single day. 
         DECLARE @DaysFromBaseDate INT = @NetFxTicksFromBaseDate / 864000000000; 
		 -- Remaining leftover Ticks
         DECLARE @TimeOfDayInTicks BIGINT = @NetFxTicksFromBaseDate - @DaysFromBaseDate * 864000000000; 
		 -- A Tick equals to 100 nanoseconds which is 0.0001 milliseconds 
         DECLARE @TimeOfDayInMilliseconds INT = @TimeOfDayInTicks / 10000; 
         DECLARE @UtcDate DATETIME = DATEADD(ms, @TimeOfDayInMilliseconds, DATEADD(d,@DaysFromBaseDate, @BaseDate)); 
		 -- To get the local time  
		 RETURN @UtcDate --+ GETDATE() - GETUTCDATE();
END