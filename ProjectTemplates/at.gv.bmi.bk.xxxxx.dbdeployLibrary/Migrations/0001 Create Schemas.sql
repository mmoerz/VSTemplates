﻿/*
Deployment script for DAEMeldestelleEmpty

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;

/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/

GO
PRINT N'Creating Schema [LoT]...';


GO
CREATE SCHEMA [LoT]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating Schema [test]...';


GO
CREATE SCHEMA [test]
    AUTHORIZATION [dbo];

