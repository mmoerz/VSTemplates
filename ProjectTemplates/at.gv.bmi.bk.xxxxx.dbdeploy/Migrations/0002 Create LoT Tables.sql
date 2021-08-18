GO
CREATE TABLE [LoT].[StatusCategory] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE TABLE [LoT].[StatisticalCategory] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE TABLE [LoT].[Reporter] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



GO
CREATE TABLE [LoT].[MonitoredSubstance] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



GO
CREATE TABLE [LoT].[Measurement] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (20) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



GO
CREATE TABLE [LoT].[Explosive] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE TABLE [LoT].[DrugPrecursor] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ValidFrom] DATETIME      NOT NULL,
    [ValidTo]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



