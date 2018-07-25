
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/25/2018 17:17:43
-- Generated from EDMX file: D:\Gitlab\DragonFiesta\DragonFiesta\DragonFiesta.Database\Models\Account.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DF_Auth];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[IPBlock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IPBlock];
GO
IF OBJECT_ID(N'[dbo].[RegionServers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RegionServers];
GO
IF OBJECT_ID(N'[dbo].[Versions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Versions];
GO
IF OBJECT_ID(N'[AccountModelStoreContainer].[WorldList]', 'U') IS NOT NULL
    DROP TABLE [AccountModelStoreContainer].[WorldList];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(16)  NOT NULL,
    [EMail] nvarchar(255)  NOT NULL,
    [Password] nvarchar(32)  NULL,
    [CreationDate] datetime  NOT NULL,
    [CreationIP] nvarchar(15)  NOT NULL,
    [IsActivated] bit  NOT NULL,
    [IsBanned] bit  NOT NULL,
    [IsOnline] bit  NOT NULL,
    [LastLogin] datetime  NOT NULL,
    [LastIP] nvarchar(15)  NOT NULL,
    [RoleID] tinyint  NOT NULL,
    [RegionID] tinyint  NOT NULL,
    [BanDate] datetime  NOT NULL,
    [BanTime] bigint  NOT NULL,
    [BanReason] varchar(max)  NULL
);
GO

-- Creating table 'IPBlocks'
CREATE TABLE [dbo].[IPBlocks] (
    [BlockedIP] nvarchar(15)  NOT NULL,
    [BlockDate] datetime  NOT NULL,
    [BlockReason] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RegionServers'
CREATE TABLE [dbo].[RegionServers] (
    [Region] tinyint  NOT NULL,
    [ListenerIP] varchar(16)  NOT NULL,
    [Port] int  NOT NULL,
    [WorkThreadCount] int  NOT NULL
);
GO

-- Creating table 'Versions'
CREATE TABLE [dbo].[Versions] (
    [ClientDate] datetime  NOT NULL,
    [ClientHash] varchar(32)  NOT NULL
);
GO

-- Creating table 'WorldLists'
CREATE TABLE [dbo].[WorldLists] (
    [ID] int  NOT NULL,
    [WorldName] varchar(255)  NOT NULL,
    [TestServer] bit  NOT NULL,
    [AllowIP] varchar(16)  NOT NULL,
    [Region] tinyint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [BlockedIP] in table 'IPBlocks'
ALTER TABLE [dbo].[IPBlocks]
ADD CONSTRAINT [PK_IPBlocks]
    PRIMARY KEY CLUSTERED ([BlockedIP] ASC);
GO

-- Creating primary key on [Region], [Port] in table 'RegionServers'
ALTER TABLE [dbo].[RegionServers]
ADD CONSTRAINT [PK_RegionServers]
    PRIMARY KEY CLUSTERED ([Region], [Port] ASC);
GO

-- Creating primary key on [ClientDate], [ClientHash] in table 'Versions'
ALTER TABLE [dbo].[Versions]
ADD CONSTRAINT [PK_Versions]
    PRIMARY KEY CLUSTERED ([ClientDate], [ClientHash] ASC);
GO

-- Creating primary key on [ID], [WorldName], [TestServer], [AllowIP], [Region] in table 'WorldLists'
ALTER TABLE [dbo].[WorldLists]
ADD CONSTRAINT [PK_WorldLists]
    PRIMARY KEY CLUSTERED ([ID], [WorldName], [TestServer], [AllowIP], [Region] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------