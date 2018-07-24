
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/23/2018 13:59:54
-- Generated from EDMX file: F:\Fiesta\Emus\DragonFiesta\Source\Emulator\DragonFiesta\DragonFiesta.Database\Models\World.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DF_World];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__GroupMemb__Group__6E01572D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupMembers] DROP CONSTRAINT [FK__GroupMemb__Group__6E01572D];
GO
IF OBJECT_ID(N'[dbo].[FK__ItemOptio__ItemI__6EF57B66]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemOptions] DROP CONSTRAINT [FK__ItemOptio__ItemI__6EF57B66];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BlockList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlockList];
GO
IF OBJECT_ID(N'[dbo].[Buffs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Buffs];
GO
IF OBJECT_ID(N'[dbo].[Character_Options]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Character_Options];
GO
IF OBJECT_ID(N'[dbo].[Characters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Characters];
GO
IF OBJECT_ID(N'[dbo].[Friends]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Friends];
GO
IF OBJECT_ID(N'[dbo].[GoldSpammer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoldSpammer];
GO
IF OBJECT_ID(N'[dbo].[GroupMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupMembers];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[GuildMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GuildMembers];
GO
IF OBJECT_ID(N'[dbo].[Guilds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Guilds];
GO
IF OBJECT_ID(N'[dbo].[ItemOptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemOptions];
GO
IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[Skills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Skills];
GO
IF OBJECT_ID(N'[dbo].[Titles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Titles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BlockLists'
CREATE TABLE [dbo].[BlockLists] (
    [OwnerID] int  NOT NULL,
    [BlockedID] int  NOT NULL
);
GO

-- Creating table 'Buffs'
CREATE TABLE [dbo].[Buffs] (
    [ID] bigint  NOT NULL,
    [OwnerID] int  NOT NULL,
    [AbStateIndex] int  NOT NULL,
    [Strength] int  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [ExpireDate] datetime  NOT NULL,
    [test] varchar(max)  NULL
);
GO

-- Creating table 'Character_Options'
CREATE TABLE [dbo].[Character_Options] (
    [ID] int  NOT NULL,
    [ShortCutData] varbinary(1024)  NOT NULL,
    [ShortCutSize] varbinary(24)  NOT NULL,
    [Video] varbinary(60)  NOT NULL,
    [Sound] varbinary(1)  NOT NULL,
    [Game] varbinary(64)  NOT NULL,
    [WindowsPos] varbinary(392)  NOT NULL,
    [KeyMapping] varbinary(308)  NOT NULL
);
GO

-- Creating table 'Characters'
CREATE TABLE [dbo].[Characters] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AccountID] int  NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Slot] tinyint  NOT NULL,
    [Map] smallint  NULL,
    [PositionX] int  NOT NULL,
    [PositionY] int  NOT NULL,
    [Rotation] tinyint  NOT NULL,
    [Class] tinyint  NOT NULL,
    [Level] tinyint  NOT NULL,
    [HP] int  NOT NULL,
    [SP] int  NOT NULL,
    [HPStones] smallint  NOT NULL,
    [SPStones] smallint  NOT NULL,
    [EXP] bigint  NOT NULL,
    [Money] bigint  NOT NULL,
    [Fame] int  NOT NULL,
    [KillPoints] int  NOT NULL,
    [IsMale] bit  NOT NULL,
    [Hair] tinyint  NOT NULL,
    [HairColor] tinyint  NOT NULL,
    [Face] tinyint  NOT NULL,
    [FreeStat_Points] tinyint  NOT NULL,
    [FreeStat_Str] tinyint  NOT NULL,
    [FreeStat_End] tinyint  NOT NULL,
    [FreeStat_Dex] tinyint  NOT NULL,
    [FreeStat_Int] tinyint  NOT NULL,
    [FreeStat_Spr] tinyint  NOT NULL,
    [SkillPoints] tinyint  NOT NULL,
    [LastLogin] datetime  NOT NULL,
    [IsOnline] bit  NOT NULL,
    [IsFirstLogin] bit  NOT NULL,
    [LP] int  NOT NULL,
    [FriendPoints] int  NOT NULL
);
GO

-- Creating table 'GoldSpammers'
CREATE TABLE [dbo].[GoldSpammers] (
    [ReporterID] int  NOT NULL,
    [ReporterIP] nvarchar(15)  NOT NULL,
    [SpammerID] int  NOT NULL,
    [SpammerIP] nvarchar(15)  NOT NULL,
    [ReportDate] datetime  NOT NULL
);
GO

-- Creating table 'GroupMembers'
CREATE TABLE [dbo].[GroupMembers] (
    [MemberID] int  NOT NULL,
    [Rank] tinyint  NOT NULL,
    [Group_ID] smallint  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [ID] smallint  NOT NULL,
    [Flags] tinyint  NOT NULL
);
GO

-- Creating table 'GuildMembers'
CREATE TABLE [dbo].[GuildMembers] (
    [GuildID] int  NOT NULL,
    [MemberID] int  NOT NULL,
    [Rank] tinyint  NOT NULL,
    [Corp] smallint  NOT NULL
);
GO

-- Creating table 'Guilds'
CREATE TABLE [dbo].[Guilds] (
    [ID] int  NOT NULL,
    [Name] nvarchar(16)  NOT NULL,
    [Password] varbinary(max)  NOT NULL,
    [AllowGuildWar] bit  NOT NULL,
    [Message] nvarchar(512)  NOT NULL,
    [MessageCreateDate] datetime  NOT NULL,
    [MessageCreaterID] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Money] bigint  NOT NULL
);
GO

-- Creating table 'ItemOptions'
CREATE TABLE [dbo].[ItemOptions] (
    [ItemKey] bigint  NOT NULL,
    [OptionType] smallint  NOT NULL,
    [OptionData] bigint  NOT NULL,
    [Item_ItemKey] bigint  NOT NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [ItemKey] bigint IDENTITY(1,1) NOT NULL,
    [StorageType] tinyint  NOT NULL,
    [Owner] int  NOT NULL,
    [Storage] smallint  NOT NULL,
    [ItemID] int  NOT NULL,
    [Flags] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Character_ID] int  NOT NULL
);
GO

-- Creating table 'Skills'
CREATE TABLE [dbo].[Skills] (
    [ID] bigint  NOT NULL,
    [OwnerID] int  NOT NULL,
    [SkillID] smallint  NOT NULL,
    [IsPassive] bit  NOT NULL,
    [Upgrades] varbinary(4)  NOT NULL
);
GO

-- Creating table 'Titles'
CREATE TABLE [dbo].[Titles] (
    [ID] bigint  NOT NULL,
    [OwnerID] int  NOT NULL,
    [TitleType] int  NOT NULL,
    [TitleLevel] tinyint  NOT NULL,
    [RegisterDate] datetime  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Data] bigint  NOT NULL
);
GO

-- Creating table 'Friends'
CREATE TABLE [dbo].[Friends] (
    [OwnerID] int IDENTITY(1,1) NOT NULL,
    [FriendID] int  NOT NULL,
    [RegisterDate] datetime  NOT NULL,
    [Character_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [OwnerID] in table 'BlockLists'
ALTER TABLE [dbo].[BlockLists]
ADD CONSTRAINT [PK_BlockLists]
    PRIMARY KEY CLUSTERED ([OwnerID] ASC);
GO

-- Creating primary key on [ID] in table 'Buffs'
ALTER TABLE [dbo].[Buffs]
ADD CONSTRAINT [PK_Buffs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Character_Options'
ALTER TABLE [dbo].[Character_Options]
ADD CONSTRAINT [PK_Character_Options]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [PK_Characters]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ReporterID] in table 'GoldSpammers'
ALTER TABLE [dbo].[GoldSpammers]
ADD CONSTRAINT [PK_GoldSpammers]
    PRIMARY KEY CLUSTERED ([ReporterID] ASC);
GO

-- Creating primary key on [MemberID] in table 'GroupMembers'
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [PK_GroupMembers]
    PRIMARY KEY CLUSTERED ([MemberID] ASC);
GO

-- Creating primary key on [ID] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [MemberID] in table 'GuildMembers'
ALTER TABLE [dbo].[GuildMembers]
ADD CONSTRAINT [PK_GuildMembers]
    PRIMARY KEY CLUSTERED ([MemberID] ASC);
GO

-- Creating primary key on [ID] in table 'Guilds'
ALTER TABLE [dbo].[Guilds]
ADD CONSTRAINT [PK_Guilds]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ItemKey] in table 'ItemOptions'
ALTER TABLE [dbo].[ItemOptions]
ADD CONSTRAINT [PK_ItemOptions]
    PRIMARY KEY CLUSTERED ([ItemKey] ASC);
GO

-- Creating primary key on [ItemKey] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([ItemKey] ASC);
GO

-- Creating primary key on [ID] in table 'Skills'
ALTER TABLE [dbo].[Skills]
ADD CONSTRAINT [PK_Skills]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Titles'
ALTER TABLE [dbo].[Titles]
ADD CONSTRAINT [PK_Titles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [OwnerID] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [PK_Friends]
    PRIMARY KEY CLUSTERED ([OwnerID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Group_ID] in table 'GroupMembers'
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [FK_Group_GroupMembers]
    FOREIGN KEY ([Group_ID])
    REFERENCES [dbo].[Groups]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_GroupMembers'
CREATE INDEX [IX_FK_Group_GroupMembers]
ON [dbo].[GroupMembers]
    ([Group_ID]);
GO

-- Creating foreign key on [Item_ItemKey] in table 'ItemOptions'
ALTER TABLE [dbo].[ItemOptions]
ADD CONSTRAINT [FK_Item_ItemOptions]
    FOREIGN KEY ([Item_ItemKey])
    REFERENCES [dbo].[Items]
        ([ItemKey])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_ItemOptions'
CREATE INDEX [IX_FK_Item_ItemOptions]
ON [dbo].[ItemOptions]
    ([Item_ItemKey]);
GO

-- Creating foreign key on [Character_ID] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_Character_Items]
    FOREIGN KEY ([Character_ID])
    REFERENCES [dbo].[Characters]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Character_Items'
CREATE INDEX [IX_FK_Character_Items]
ON [dbo].[Items]
    ([Character_ID]);
GO

-- Creating foreign key on [Character_ID] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_Character_Friends]
    FOREIGN KEY ([Character_ID])
    REFERENCES [dbo].[Characters]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Character_Friends'
CREATE INDEX [IX_FK_Character_Friends]
ON [dbo].[Friends]
    ([Character_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------