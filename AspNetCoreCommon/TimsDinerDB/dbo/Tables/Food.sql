CREATE TABLE [dbo].[Food]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Title] NVARCHAR(250) NOT NULL,
    [Description] NVARCHAR(250) NOT NULL,
    [Price] MONEY NOT NULL
)
