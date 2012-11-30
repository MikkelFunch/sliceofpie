
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/30/2012 13:41:26
-- Generated from EDMX file: D:\Visual Studio Workspace\sliceofpie\code\Slice of Pie\Slice of Pie\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PieFactory];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserFolder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet] DROP CONSTRAINT [FK_UserFolder];
GO
IF OBJECT_ID(N'[dbo].[FK_FolderFolder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FolderSet] DROP CONSTRAINT [FK_FolderFolder];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDocument_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDocument] DROP CONSTRAINT [FK_UserDocument_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDocument_Document]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDocument] DROP CONSTRAINT [FK_UserDocument_Document];
GO
IF OBJECT_ID(N'[dbo].[FK_FolderDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentSet] DROP CONSTRAINT [FK_FolderDocument];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[FolderSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FolderSet];
GO
IF OBJECT_ID(N'[dbo].[DocumentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentSet];
GO
IF OBJECT_ID(N'[dbo].[UserDocument]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDocument];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] longtext  NOT NULL,
    [Password] longtext  NOT NULL,
    [RootFolder_Id] int  NOT NULL
);
GO

-- Creating table 'FolderSet'
CREATE TABLE [dbo].[FolderSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] longtext  NOT NULL,
    [FolderId] int  NOT NULL
);
GO

-- Creating table 'DocumentSet'
CREATE TABLE [dbo].[DocumentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] longtext  NOT NULL,
    [Path] longtext  NOT NULL,
    [CreationTime] datetime  NOT NULL,
    [DocumentId] int  NOT NULL,
    [FolderId] int  NOT NULL
);
GO

-- Creating table 'UserDocument'
CREATE TABLE [dbo].[UserDocument] (
    [User_Id] int  NOT NULL,
    [Document_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FolderSet'
ALTER TABLE [dbo].[FolderSet]
ADD CONSTRAINT [PK_FolderSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentSet'
ALTER TABLE [dbo].[DocumentSet]
ADD CONSTRAINT [PK_DocumentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [User_Id], [Document_Id] in table 'UserDocument'
ALTER TABLE [dbo].[UserDocument]
ADD CONSTRAINT [PK_UserDocument]
    PRIMARY KEY NONCLUSTERED ([User_Id], [Document_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RootFolder_Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_UserFolder]
    FOREIGN KEY ([RootFolder_Id])
    REFERENCES [dbo].[FolderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserFolder'
CREATE INDEX [IX_FK_UserFolder]
ON [dbo].[UserSet]
    ([RootFolder_Id]);
GO

-- Creating foreign key on [FolderId] in table 'FolderSet'
ALTER TABLE [dbo].[FolderSet]
ADD CONSTRAINT [FK_FolderFolder]
    FOREIGN KEY ([FolderId])
    REFERENCES [dbo].[FolderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FolderFolder'
CREATE INDEX [IX_FK_FolderFolder]
ON [dbo].[FolderSet]
    ([FolderId]);
GO

-- Creating foreign key on [User_Id] in table 'UserDocument'
ALTER TABLE [dbo].[UserDocument]
ADD CONSTRAINT [FK_UserDocument_User]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Document_Id] in table 'UserDocument'
ALTER TABLE [dbo].[UserDocument]
ADD CONSTRAINT [FK_UserDocument_Document]
    FOREIGN KEY ([Document_Id])
    REFERENCES [dbo].[DocumentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDocument_Document'
CREATE INDEX [IX_FK_UserDocument_Document]
ON [dbo].[UserDocument]
    ([Document_Id]);
GO

-- Creating foreign key on [FolderId] in table 'DocumentSet'
ALTER TABLE [dbo].[DocumentSet]
ADD CONSTRAINT [FK_FolderDocument]
    FOREIGN KEY ([FolderId])
    REFERENCES [dbo].[FolderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FolderDocument'
CREATE INDEX [IX_FK_FolderDocument]
ON [dbo].[DocumentSet]
    ([FolderId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------