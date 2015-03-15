
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/15/2015 13:08:20
-- Generated from EDMX file: C:\Users\fifa\Documents\Visual Studio 2013\Projects\AspRecipeStorage\AspRecipeStorage\Models\EfModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-AspRecipeStorage-20150314101023];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserClaim_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserClaims] DROP CONSTRAINT [FK_UserClaim_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLogin_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserLogins] DROP CONSTRAINT [FK_UserLogin_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserRole_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserUserRole] DROP CONSTRAINT [FK_UserUserRole_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserRole_UserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserUserRole] DROP CONSTRAINT [FK_UserUserRole_UserRole];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserClaims];
GO
IF OBJECT_ID(N'[dbo].[UserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserLogins];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[UserUserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserUserRole];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [Email] nvarchar(100)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(100)  NULL,
    [SecurityStamp] nvarchar(100)  NULL,
    [PhoneNumber] nvarchar(25)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL
);
GO

-- Creating table 'UserClaims'
CREATE TABLE [dbo].[UserClaims] (
    [UserId] int  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'UserLogins'
CREATE TABLE [dbo].[UserLogins] (
    [UserId] int  NOT NULL,
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Id] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'RecipeSet'
CREATE TABLE [dbo].[RecipeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Picture] varbinary(max)  NOT NULL,
    [DishTypeId] int  NOT NULL,
    [AuthorId] int  NOT NULL
);
GO

-- Creating table 'DishTypeSet'
CREATE TABLE [dbo].[DishTypeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RecipeStepSet'
CREATE TABLE [dbo].[RecipeStepSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Discription] nvarchar(max)  NOT NULL,
    [Time] nvarchar(max)  NOT NULL,
    [StepNumber] nvarchar(max)  NOT NULL,
    [RecipeId] int  NOT NULL
);
GO

-- Creating table 'IngredientSet1'
CREATE TABLE [dbo].[IngredientSet1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserUserRole'
CREATE TABLE [dbo].[UserUserRole] (
    [Users_Id] int  NOT NULL,
    [Roles_Id] int  NOT NULL
);
GO

-- Creating table 'IngredientRecipeStep'
CREATE TABLE [dbo].[IngredientRecipeStep] (
    [Ingredient_Id] int  NOT NULL,
    [RecipeSteps_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserClaims'
ALTER TABLE [dbo].[UserClaims]
ADD CONSTRAINT [PK_UserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [LoginProvider], [ProviderKey] in table 'UserLogins'
ALTER TABLE [dbo].[UserLogins]
ADD CONSTRAINT [PK_UserLogins]
    PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [ProviderKey] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecipeSet'
ALTER TABLE [dbo].[RecipeSet]
ADD CONSTRAINT [PK_RecipeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DishTypeSet'
ALTER TABLE [dbo].[DishTypeSet]
ADD CONSTRAINT [PK_DishTypeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecipeStepSet'
ALTER TABLE [dbo].[RecipeStepSet]
ADD CONSTRAINT [PK_RecipeStepSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IngredientSet1'
ALTER TABLE [dbo].[IngredientSet1]
ADD CONSTRAINT [PK_IngredientSet1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [Roles_Id] in table 'UserUserRole'
ALTER TABLE [dbo].[UserUserRole]
ADD CONSTRAINT [PK_UserUserRole]
    PRIMARY KEY CLUSTERED ([Users_Id], [Roles_Id] ASC);
GO

-- Creating primary key on [Ingredient_Id], [RecipeSteps_Id] in table 'IngredientRecipeStep'
ALTER TABLE [dbo].[IngredientRecipeStep]
ADD CONSTRAINT [PK_IngredientRecipeStep]
    PRIMARY KEY CLUSTERED ([Ingredient_Id], [RecipeSteps_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'UserClaims'
ALTER TABLE [dbo].[UserClaims]
ADD CONSTRAINT [FK_UserClaim_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserClaim_User'
CREATE INDEX [IX_FK_UserClaim_User]
ON [dbo].[UserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserLogins'
ALTER TABLE [dbo].[UserLogins]
ADD CONSTRAINT [FK_UserLogin_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_Id] in table 'UserUserRole'
ALTER TABLE [dbo].[UserUserRole]
ADD CONSTRAINT [FK_UserUserRole_User]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Roles_Id] in table 'UserUserRole'
ALTER TABLE [dbo].[UserUserRole]
ADD CONSTRAINT [FK_UserUserRole_UserRole]
    FOREIGN KEY ([Roles_Id])
    REFERENCES [dbo].[UserRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserRole_UserRole'
CREATE INDEX [IX_FK_UserUserRole_UserRole]
ON [dbo].[UserUserRole]
    ([Roles_Id]);
GO

-- Creating foreign key on [DishTypeId] in table 'RecipeSet'
ALTER TABLE [dbo].[RecipeSet]
ADD CONSTRAINT [FK_DishTypeRecipe]
    FOREIGN KEY ([DishTypeId])
    REFERENCES [dbo].[DishTypeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DishTypeRecipe'
CREATE INDEX [IX_FK_DishTypeRecipe]
ON [dbo].[RecipeSet]
    ([DishTypeId]);
GO

-- Creating foreign key on [RecipeId] in table 'RecipeStepSet'
ALTER TABLE [dbo].[RecipeStepSet]
ADD CONSTRAINT [FK_RecipeRecipeStep]
    FOREIGN KEY ([RecipeId])
    REFERENCES [dbo].[RecipeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeRecipeStep'
CREATE INDEX [IX_FK_RecipeRecipeStep]
ON [dbo].[RecipeStepSet]
    ([RecipeId]);
GO

-- Creating foreign key on [AuthorId] in table 'RecipeSet'
ALTER TABLE [dbo].[RecipeSet]
ADD CONSTRAINT [FK_UserRecipe]
    FOREIGN KEY ([AuthorId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRecipe'
CREATE INDEX [IX_FK_UserRecipe]
ON [dbo].[RecipeSet]
    ([AuthorId]);
GO

-- Creating foreign key on [Ingredient_Id] in table 'IngredientRecipeStep'
ALTER TABLE [dbo].[IngredientRecipeStep]
ADD CONSTRAINT [FK_IngredientRecipeStep_Ingredient]
    FOREIGN KEY ([Ingredient_Id])
    REFERENCES [dbo].[IngredientSet1]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RecipeSteps_Id] in table 'IngredientRecipeStep'
ALTER TABLE [dbo].[IngredientRecipeStep]
ADD CONSTRAINT [FK_IngredientRecipeStep_RecipeStep]
    FOREIGN KEY ([RecipeSteps_Id])
    REFERENCES [dbo].[RecipeStepSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientRecipeStep_RecipeStep'
CREATE INDEX [IX_FK_IngredientRecipeStep_RecipeStep]
ON [dbo].[IngredientRecipeStep]
    ([RecipeSteps_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------