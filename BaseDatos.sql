IF DB_ID('DevSuBankCustomers') IS NULL
    CREATE DATABASE DevSuBankCustomers;
GO

USE DevSuBankCustomers;
GO

IF OBJECT_ID('dbo.Clients', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clients
    (
        Id             INT            IDENTITY(1,1) NOT NULL,
        Name           NVARCHAR(150)  NOT NULL,
        Gender         TINYINT        NOT NULL,
        Age            INT            NOT NULL,
        Identification NVARCHAR(30)   NOT NULL,
        Address        NVARCHAR(250)  NULL,
        Phone          NVARCHAR(30)   NULL,
        ClientId       NVARCHAR(20)   NOT NULL,
        PasswordHash   NVARCHAR(200)  NOT NULL,
        Status         BIT            NOT NULL CONSTRAINT DF_Clients_Status DEFAULT (1),

        CONSTRAINT PK_Clients                 PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Clients_ClientId        UNIQUE (ClientId),
        CONSTRAINT UQ_Clients_Identification  UNIQUE (Identification),
        CONSTRAINT CK_Clients_Gender          CHECK (Gender IN (1, 2, 3)),
        CONSTRAINT CK_Clients_Age             CHECK (Age >= 0 AND Age <= 120)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.extended_properties
               WHERE major_id = OBJECT_ID('dbo.Clients')
                 AND minor_id = COLUMNPROPERTY(OBJECT_ID('dbo.Clients'), 'Gender', 'ColumnId')
                 AND name = 'MS_Description')
    EXEC sys.sp_addextendedproperty
        @name = N'MS_Description', @value = N'1 = Male, 2 = Female, 3 = Other',
        @level0type = N'SCHEMA', @level0name = N'dbo',
        @level1type = N'TABLE',  @level1name = N'Clients',
        @level2type = N'COLUMN', @level2name = N'Gender';
GO
