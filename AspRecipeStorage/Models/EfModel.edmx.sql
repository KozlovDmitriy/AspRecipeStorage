
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/15/2015 14:38:58
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
IF OBJECT_ID(N'[dbo].[FK_DishTypeRecipe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeSet] DROP CONSTRAINT [FK_DishTypeRecipe];
GO
IF OBJECT_ID(N'[dbo].[FK_RecipeRecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeStepSet] DROP CONSTRAINT [FK_RecipeRecipeStep];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRecipe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeSet] DROP CONSTRAINT [FK_UserRecipe];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientRecipeStep_Ingredient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientRecipeStep] DROP CONSTRAINT [FK_IngredientRecipeStep_Ingredient];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientRecipeStep_RecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientRecipeStep] DROP CONSTRAINT [FK_IngredientRecipeStep_RecipeStep];
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
IF OBJECT_ID(N'[dbo].[RecipeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecipeSet];
GO
IF OBJECT_ID(N'[dbo].[DishTypeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DishTypeSet];
GO
IF OBJECT_ID(N'[dbo].[RecipeStepSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecipeStepSet];
GO
IF OBJECT_ID(N'[dbo].[IngredientSet1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientSet1];
GO
IF OBJECT_ID(N'[dbo].[UserUserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserUserRole];
GO
IF OBJECT_ID(N'[dbo].[IngredientRecipeStep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientRecipeStep];
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

-- Creating table 'Recipe'
CREATE TABLE [dbo].[Recipe] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Picture] varbinary(max)  NOT NULL,
    [DishTypeId] int  NOT NULL,
    [AuthorId] int  NOT NULL
);
GO

-- Creating table 'DishType'
CREATE TABLE [dbo].[DishType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RecipeStep'
CREATE TABLE [dbo].[RecipeStep] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Discription] nvarchar(max)  NOT NULL,
    [Time] nvarchar(max)  NOT NULL,
    [StepNumber] int  NOT NULL,
    [RecipeId] int  NOT NULL
);
GO

-- Creating table 'Ingredient'
CREATE TABLE [dbo].[Ingredient] (
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
    [Ingredients_Id] int  NOT NULL,
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

-- Creating primary key on [Id] in table 'Recipe'
ALTER TABLE [dbo].[Recipe]
ADD CONSTRAINT [PK_Recipe]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DishType'
ALTER TABLE [dbo].[DishType]
ADD CONSTRAINT [PK_DishType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecipeStep'
ALTER TABLE [dbo].[RecipeStep]
ADD CONSTRAINT [PK_RecipeStep]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ingredient'
ALTER TABLE [dbo].[Ingredient]
ADD CONSTRAINT [PK_Ingredient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [Roles_Id] in table 'UserUserRole'
ALTER TABLE [dbo].[UserUserRole]
ADD CONSTRAINT [PK_UserUserRole]
    PRIMARY KEY CLUSTERED ([Users_Id], [Roles_Id] ASC);
GO

-- Creating primary key on [Ingredients_Id], [RecipeSteps_Id] in table 'IngredientRecipeStep'
ALTER TABLE [dbo].[IngredientRecipeStep]
ADD CONSTRAINT [PK_IngredientRecipeStep]
    PRIMARY KEY CLUSTERED ([Ingredients_Id], [RecipeSteps_Id] ASC);
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

-- Creating foreign key on [DishTypeId] in table 'Recipe'
ALTER TABLE [dbo].[Recipe]
ADD CONSTRAINT [FK_DishTypeRecipe]
    FOREIGN KEY ([DishTypeId])
    REFERENCES [dbo].[DishType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DishTypeRecipe'
CREATE INDEX [IX_FK_DishTypeRecipe]
ON [dbo].[Recipe]
    ([DishTypeId]);
GO

-- Creating foreign key on [RecipeId] in table 'RecipeStep'
ALTER TABLE [dbo].[RecipeStep]
ADD CONSTRAINT [FK_RecipeRecipeStep]
    FOREIGN KEY ([RecipeId])
    REFERENCES [dbo].[Recipe]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeRecipeStep'
CREATE INDEX [IX_FK_RecipeRecipeStep]
ON [dbo].[RecipeStep]
    ([RecipeId]);
GO

-- Creating foreign key on [AuthorId] in table 'Recipe'
ALTER TABLE [dbo].[Recipe]
ADD CONSTRAINT [FK_UserRecipe]
    FOREIGN KEY ([AuthorId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRecipe'
CREATE INDEX [IX_FK_UserRecipe]
ON [dbo].[Recipe]
    ([AuthorId]);
GO

-- Creating foreign key on [Ingredients_Id] in table 'IngredientRecipeStep'
ALTER TABLE [dbo].[IngredientRecipeStep]
ADD CONSTRAINT [FK_IngredientRecipeStep_Ingredient]
    FOREIGN KEY ([Ingredients_Id])
    REFERENCES [dbo].[Ingredient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RecipeSteps_Id] in table 'IngredientRecipeStep'
ALTER TABLE [dbo].[IngredientRecipeStep]
ADD CONSTRAINT [FK_IngredientRecipeStep_RecipeStep]
    FOREIGN KEY ([RecipeSteps_Id])
    REFERENCES [dbo].[RecipeStep]
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