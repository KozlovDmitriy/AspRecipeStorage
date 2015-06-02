
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/02/2015 18:45:55
-- Generated from EDMX file: C:\Users\fifa\Documents\Visual Studio 2013\Projects\AspRecipeStorage\AspRecipeStorage\Models\EfModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [db_recipes];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ChildRecipeRecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeStep] DROP CONSTRAINT [FK_ChildRecipeRecipeStep];
GO
IF OBJECT_ID(N'[dbo].[FK_DishTypeRecipe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_DishTypeRecipe];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientIngredientType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredient] DROP CONSTRAINT [FK_IngredientIngredientType];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientMeasureType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredient] DROP CONSTRAINT [FK_IngredientMeasureType];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientRecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredient] DROP CONSTRAINT [FK_IngredientRecipeStep];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientsSet_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientsSet] DROP CONSTRAINT [FK_IngredientsSet_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientsSetRow_IngredientsSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientsSetRow] DROP CONSTRAINT [FK_IngredientsSetRow_IngredientsSet];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientsSetRow_IngredientTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientsSetRow] DROP CONSTRAINT [FK_IngredientsSetRow_IngredientTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientsSetRow_MeasureTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientsSetRow] DROP CONSTRAINT [FK_IngredientsSetRow_MeasureTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientTypeMeasureConversion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeasureConversions] DROP CONSTRAINT [FK_IngredientTypeMeasureConversion];
GO
IF OBJECT_ID(N'[dbo].[FK_MeasureConversionMeasureTypeFirst]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeasureConversions] DROP CONSTRAINT [FK_MeasureConversionMeasureTypeFirst];
GO
IF OBJECT_ID(N'[dbo].[FK_MeasureConversionMeasureTypeSecond]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeasureConversions] DROP CONSTRAINT [FK_MeasureConversionMeasureTypeSecond];
GO
IF OBJECT_ID(N'[dbo].[FK_PictureRecipe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_PictureRecipe];
GO
IF OBJECT_ID(N'[dbo].[FK_PictureRecipeStep_Picture]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PictureRecipeStep] DROP CONSTRAINT [FK_PictureRecipeStep_Picture];
GO
IF OBJECT_ID(N'[dbo].[FK_PictureRecipeStep_RecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PictureRecipeStep] DROP CONSTRAINT [FK_PictureRecipeStep_RecipeStep];
GO
IF OBJECT_ID(N'[dbo].[FK_RecipeRecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecipeStep] DROP CONSTRAINT [FK_RecipeRecipeStep];
GO
IF OBJECT_ID(N'[dbo].[FK_StepInstrument_Instrument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StepInstrument] DROP CONSTRAINT [FK_StepInstrument_Instrument];
GO
IF OBJECT_ID(N'[dbo].[FK_StepInstrument_RecipeStep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StepInstrument] DROP CONSTRAINT [FK_StepInstrument_RecipeStep];
GO
IF OBJECT_ID(N'[dbo].[FK_UserClaim_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserClaims] DROP CONSTRAINT [FK_UserClaim_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLogin_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserLogins] DROP CONSTRAINT [FK_UserLogin_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRecipe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_UserRecipe];
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

IF OBJECT_ID(N'[dbo].[DishType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DishType];
GO
IF OBJECT_ID(N'[dbo].[Ingredient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingredient];
GO
IF OBJECT_ID(N'[dbo].[IngredientsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientsSet];
GO
IF OBJECT_ID(N'[dbo].[IngredientsSetRow]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientsSetRow];
GO
IF OBJECT_ID(N'[dbo].[IngredientTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientTypes];
GO
IF OBJECT_ID(N'[dbo].[Instrument]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Instrument];
GO
IF OBJECT_ID(N'[dbo].[MeasureConversions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasureConversions];
GO
IF OBJECT_ID(N'[dbo].[MeasureTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasureTypes];
GO
IF OBJECT_ID(N'[dbo].[PictureRecipeStep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PictureRecipeStep];
GO
IF OBJECT_ID(N'[dbo].[Pictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pictures];
GO
IF OBJECT_ID(N'[dbo].[Recipe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Recipe];
GO
IF OBJECT_ID(N'[dbo].[RecipeStep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecipeStep];
GO
IF OBJECT_ID(N'[dbo].[StepInstrument]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StepInstrument];
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
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
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
    [AccessFailedCount] int  NOT NULL,
    [RestorePwToket] nvarchar(max)  NULL
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
    [DishTypeId] int  NOT NULL,
    [AuthorId] int  NULL,
    [Time] int  NOT NULL,
    [PictureId] int  NULL
);
GO

-- Creating table 'DishType'
CREATE TABLE [dbo].[DishType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Ingredient'
CREATE TABLE [dbo].[Ingredient] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IngredientTypeId] int  NOT NULL,
    [MeasureTypeId] int  NOT NULL,
    [Amount] int  NOT NULL,
    [RecipeStepId] int  NOT NULL
);
GO

-- Creating table 'MeasureTypes'
CREATE TABLE [dbo].[MeasureTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'IngredientTypes'
CREATE TABLE [dbo].[IngredientTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pictures'
CREATE TABLE [dbo].[Pictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] varbinary(max)  NOT NULL
);
GO

-- Creating table 'MeasureConversions'
CREATE TABLE [dbo].[MeasureConversions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MeasureTypeFirstId] int  NOT NULL,
    [MeasureTypeSecondId] int  NOT NULL,
    [Ratio] float  NOT NULL,
    [IngredientTypeId] int  NOT NULL
);
GO

-- Creating table 'Instruments'
CREATE TABLE [dbo].[Instruments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ForAllUsers] bit  NOT NULL
);
GO

-- Creating table 'StepInstruments'
CREATE TABLE [dbo].[StepInstruments] (
    [Instrument_Id] int  NOT NULL,
    [RecipeStep_Id] int  NOT NULL,
    [InstrumentCount] int  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'RecipeSteps'
CREATE TABLE [dbo].[RecipeSteps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Discription] nvarchar(max)  NULL,
    [Time] int  NOT NULL,
    [StepNumber] int  NOT NULL,
    [RecipeId] int  NOT NULL,
    [ChildRecipeId] int  NULL
);
GO

-- Creating table 'IngredientsSets'
CREATE TABLE [dbo].[IngredientsSets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [DateCreate] datetimeoffset  NOT NULL
);
GO

-- Creating table 'IngredientsSetRows'
CREATE TABLE [dbo].[IngredientsSetRows] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IngredientTypeId] int  NOT NULL,
    [Amount] int  NULL,
    [MeasureTypeId] int  NOT NULL,
    [IngredientsSetId] int  NOT NULL
);
GO

-- Creating table 'UserUserRole'
CREATE TABLE [dbo].[UserUserRole] (
    [Users_Id] int  NOT NULL,
    [Roles_Id] int  NOT NULL
);
GO

-- Creating table 'PictureRecipeStep'
CREATE TABLE [dbo].[PictureRecipeStep] (
    [Pictures_Id] int  NOT NULL,
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

-- Creating primary key on [Id] in table 'Ingredient'
ALTER TABLE [dbo].[Ingredient]
ADD CONSTRAINT [PK_Ingredient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeasureTypes'
ALTER TABLE [dbo].[MeasureTypes]
ADD CONSTRAINT [PK_MeasureTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IngredientTypes'
ALTER TABLE [dbo].[IngredientTypes]
ADD CONSTRAINT [PK_IngredientTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [PK_Pictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeasureConversions'
ALTER TABLE [dbo].[MeasureConversions]
ADD CONSTRAINT [PK_MeasureConversions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Instruments'
ALTER TABLE [dbo].[Instruments]
ADD CONSTRAINT [PK_Instruments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StepInstruments'
ALTER TABLE [dbo].[StepInstruments]
ADD CONSTRAINT [PK_StepInstruments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecipeSteps'
ALTER TABLE [dbo].[RecipeSteps]
ADD CONSTRAINT [PK_RecipeSteps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IngredientsSets'
ALTER TABLE [dbo].[IngredientsSets]
ADD CONSTRAINT [PK_IngredientsSets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IngredientsSetRows'
ALTER TABLE [dbo].[IngredientsSetRows]
ADD CONSTRAINT [PK_IngredientsSetRows]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [Roles_Id] in table 'UserUserRole'
ALTER TABLE [dbo].[UserUserRole]
ADD CONSTRAINT [PK_UserUserRole]
    PRIMARY KEY CLUSTERED ([Users_Id], [Roles_Id] ASC);
GO

-- Creating primary key on [Pictures_Id], [RecipeSteps_Id] in table 'PictureRecipeStep'
ALTER TABLE [dbo].[PictureRecipeStep]
ADD CONSTRAINT [PK_PictureRecipeStep]
    PRIMARY KEY CLUSTERED ([Pictures_Id], [RecipeSteps_Id] ASC);
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
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DishTypeRecipe'
CREATE INDEX [IX_FK_DishTypeRecipe]
ON [dbo].[Recipe]
    ([DishTypeId]);
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

-- Creating foreign key on [MeasureTypeId] in table 'Ingredient'
ALTER TABLE [dbo].[Ingredient]
ADD CONSTRAINT [FK_IngredientMeasureType]
    FOREIGN KEY ([MeasureTypeId])
    REFERENCES [dbo].[MeasureTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientMeasureType'
CREATE INDEX [IX_FK_IngredientMeasureType]
ON [dbo].[Ingredient]
    ([MeasureTypeId]);
GO

-- Creating foreign key on [IngredientTypeId] in table 'Ingredient'
ALTER TABLE [dbo].[Ingredient]
ADD CONSTRAINT [FK_IngredientIngredientType]
    FOREIGN KEY ([IngredientTypeId])
    REFERENCES [dbo].[IngredientTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientIngredientType'
CREATE INDEX [IX_FK_IngredientIngredientType]
ON [dbo].[Ingredient]
    ([IngredientTypeId]);
GO

-- Creating foreign key on [PictureId] in table 'Recipe'
ALTER TABLE [dbo].[Recipe]
ADD CONSTRAINT [FK_PictureRecipe]
    FOREIGN KEY ([PictureId])
    REFERENCES [dbo].[Pictures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PictureRecipe'
CREATE INDEX [IX_FK_PictureRecipe]
ON [dbo].[Recipe]
    ([PictureId]);
GO

-- Creating foreign key on [MeasureTypeSecondId] in table 'MeasureConversions'
ALTER TABLE [dbo].[MeasureConversions]
ADD CONSTRAINT [FK_MeasureConversionMeasureTypeSecond]
    FOREIGN KEY ([MeasureTypeSecondId])
    REFERENCES [dbo].[MeasureTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeasureConversionMeasureTypeSecond'
CREATE INDEX [IX_FK_MeasureConversionMeasureTypeSecond]
ON [dbo].[MeasureConversions]
    ([MeasureTypeSecondId]);
GO

-- Creating foreign key on [MeasureTypeFirstId] in table 'MeasureConversions'
ALTER TABLE [dbo].[MeasureConversions]
ADD CONSTRAINT [FK_MeasureConversionMeasureTypeFirst]
    FOREIGN KEY ([MeasureTypeFirstId])
    REFERENCES [dbo].[MeasureTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MeasureConversionMeasureTypeFirst'
CREATE INDEX [IX_FK_MeasureConversionMeasureTypeFirst]
ON [dbo].[MeasureConversions]
    ([MeasureTypeFirstId]);
GO

-- Creating foreign key on [IngredientTypeId] in table 'MeasureConversions'
ALTER TABLE [dbo].[MeasureConversions]
ADD CONSTRAINT [FK_IngredientTypeMeasureConversion]
    FOREIGN KEY ([IngredientTypeId])
    REFERENCES [dbo].[IngredientTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientTypeMeasureConversion'
CREATE INDEX [IX_FK_IngredientTypeMeasureConversion]
ON [dbo].[MeasureConversions]
    ([IngredientTypeId]);
GO

-- Creating foreign key on [Instrument_Id] in table 'StepInstruments'
ALTER TABLE [dbo].[StepInstruments]
ADD CONSTRAINT [FK_StepInstrument_Instrument]
    FOREIGN KEY ([Instrument_Id])
    REFERENCES [dbo].[Instruments]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StepInstrument_Instrument'
CREATE INDEX [IX_FK_StepInstrument_Instrument]
ON [dbo].[StepInstruments]
    ([Instrument_Id]);
GO

-- Creating foreign key on [RecipeStepId] in table 'Ingredient'
ALTER TABLE [dbo].[Ingredient]
ADD CONSTRAINT [FK_IngredientRecipeStep]
    FOREIGN KEY ([RecipeStepId])
    REFERENCES [dbo].[RecipeSteps]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientRecipeStep'
CREATE INDEX [IX_FK_IngredientRecipeStep]
ON [dbo].[Ingredient]
    ([RecipeStepId]);
GO

-- Creating foreign key on [ChildRecipeId] in table 'RecipeSteps'
ALTER TABLE [dbo].[RecipeSteps]
ADD CONSTRAINT [FK_ChildRecipeRecipeStep]
    FOREIGN KEY ([ChildRecipeId])
    REFERENCES [dbo].[Recipe]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChildRecipeRecipeStep'
CREATE INDEX [IX_FK_ChildRecipeRecipeStep]
ON [dbo].[RecipeSteps]
    ([ChildRecipeId]);
GO

-- Creating foreign key on [RecipeId] in table 'RecipeSteps'
ALTER TABLE [dbo].[RecipeSteps]
ADD CONSTRAINT [FK_RecipeRecipeStep]
    FOREIGN KEY ([RecipeId])
    REFERENCES [dbo].[Recipe]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecipeRecipeStep'
CREATE INDEX [IX_FK_RecipeRecipeStep]
ON [dbo].[RecipeSteps]
    ([RecipeId]);
GO

-- Creating foreign key on [RecipeStep_Id] in table 'StepInstruments'
ALTER TABLE [dbo].[StepInstruments]
ADD CONSTRAINT [FK_StepInstrument_RecipeStep]
    FOREIGN KEY ([RecipeStep_Id])
    REFERENCES [dbo].[RecipeSteps]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StepInstrument_RecipeStep'
CREATE INDEX [IX_FK_StepInstrument_RecipeStep]
ON [dbo].[StepInstruments]
    ([RecipeStep_Id]);
GO

-- Creating foreign key on [Pictures_Id] in table 'PictureRecipeStep'
ALTER TABLE [dbo].[PictureRecipeStep]
ADD CONSTRAINT [FK_PictureRecipeStep_Picture]
    FOREIGN KEY ([Pictures_Id])
    REFERENCES [dbo].[Pictures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RecipeSteps_Id] in table 'PictureRecipeStep'
ALTER TABLE [dbo].[PictureRecipeStep]
ADD CONSTRAINT [FK_PictureRecipeStep_RecipeStep]
    FOREIGN KEY ([RecipeSteps_Id])
    REFERENCES [dbo].[RecipeSteps]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PictureRecipeStep_RecipeStep'
CREATE INDEX [IX_FK_PictureRecipeStep_RecipeStep]
ON [dbo].[PictureRecipeStep]
    ([RecipeSteps_Id]);
GO

-- Creating foreign key on [UserId] in table 'IngredientsSets'
ALTER TABLE [dbo].[IngredientsSets]
ADD CONSTRAINT [FK_IngredientsSet_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientsSet_Users'
CREATE INDEX [IX_FK_IngredientsSet_Users]
ON [dbo].[IngredientsSets]
    ([UserId]);
GO

-- Creating foreign key on [IngredientsSetId] in table 'IngredientsSetRows'
ALTER TABLE [dbo].[IngredientsSetRows]
ADD CONSTRAINT [FK_IngredientsSetRow_IngredientsSet]
    FOREIGN KEY ([IngredientsSetId])
    REFERENCES [dbo].[IngredientsSets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientsSetRow_IngredientsSet'
CREATE INDEX [IX_FK_IngredientsSetRow_IngredientsSet]
ON [dbo].[IngredientsSetRows]
    ([IngredientsSetId]);
GO

-- Creating foreign key on [IngredientTypeId] in table 'IngredientsSetRows'
ALTER TABLE [dbo].[IngredientsSetRows]
ADD CONSTRAINT [FK_IngredientsSetRow_IngredientTypes]
    FOREIGN KEY ([IngredientTypeId])
    REFERENCES [dbo].[IngredientTypes]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientsSetRow_IngredientTypes'
CREATE INDEX [IX_FK_IngredientsSetRow_IngredientTypes]
ON [dbo].[IngredientsSetRows]
    ([IngredientTypeId]);
GO

-- Creating foreign key on [MeasureTypeId] in table 'IngredientsSetRows'
ALTER TABLE [dbo].[IngredientsSetRows]
ADD CONSTRAINT [FK_IngredientsSetRow_MeasureTypes]
    FOREIGN KEY ([MeasureTypeId])
    REFERENCES [dbo].[MeasureTypes]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientsSetRow_MeasureTypes'
CREATE INDEX [IX_FK_IngredientsSetRow_MeasureTypes]
ON [dbo].[IngredientsSetRows]
    ([MeasureTypeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------