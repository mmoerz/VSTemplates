CREATE TABLE [dbo].[Address]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [ZipCode] INT NULL, 
    [Country] NVARCHAR(50) NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [StreetName] NVARCHAR(150) NOT NULL
)