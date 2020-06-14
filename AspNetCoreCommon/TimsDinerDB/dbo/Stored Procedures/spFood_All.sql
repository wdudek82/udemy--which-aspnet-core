CREATE PROCEDURE [dbo].[spFood_All]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [Id], [Title], [Description], [Price]
    FROM dbo.Food;
END
