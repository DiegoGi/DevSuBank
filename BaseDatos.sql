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

    EXEC sys.sp_addextendedproperty
        @name = N'MS_Description', @value = N'1 = Male, 2 = Female, 3 = Other',
        @level0type = N'SCHEMA', @level0name = N'dbo',
        @level1type = N'TABLE',  @level1name = N'Clients',
        @level2type = N'COLUMN', @level2name = N'Gender';
END
GO

IF DB_ID('DevSuBankAccounts') IS NULL
    CREATE DATABASE DevSuBankAccounts;
GO

USE DevSuBankAccounts;
GO

IF OBJECT_ID('dbo.Clients', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clients
    (
        Id      INT           NOT NULL,
        Name    NVARCHAR(150) NOT NULL,
        Status  BIT           NOT NULL,

        CONSTRAINT PK_Clients PRIMARY KEY CLUSTERED (Id)
    );
END
GO

IF OBJECT_ID('dbo.Accounts', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Accounts
    (
        Id             INT            IDENTITY(1,1) NOT NULL,
        AccountNumber  NVARCHAR(20)   NOT NULL,
        AccountType    TINYINT        NOT NULL,
        InitialBalance DECIMAL(18, 2) NOT NULL,
        CurrentBalance DECIMAL(18, 2) NOT NULL,
        Status         BIT            NOT NULL CONSTRAINT DF_Accounts_Status DEFAULT (1),
        ClientId       INT            NOT NULL,

        CONSTRAINT PK_Accounts                PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Accounts_AccountNumber  UNIQUE (AccountNumber),
        CONSTRAINT FK_Accounts_Clients        FOREIGN KEY (ClientId) REFERENCES dbo.Clients (Id),
        CONSTRAINT CK_Accounts_AccountType    CHECK (AccountType IN (1, 2)),
        CONSTRAINT CK_Accounts_InitialBalance CHECK (InitialBalance >= 0)
    );

    CREATE INDEX IX_Accounts_ClientId ON dbo.Accounts (ClientId);

    EXEC sys.sp_addextendedproperty
        @name = N'MS_Description', @value = N'1 = Savings, 2 = Checking',
        @level0type = N'SCHEMA', @level0name = N'dbo',
        @level1type = N'TABLE',  @level1name = N'Accounts',
        @level2type = N'COLUMN', @level2name = N'AccountType';
END
GO

IF OBJECT_ID('dbo.Transactions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Transactions
    (
        Id              BIGINT         IDENTITY(1,1) NOT NULL,
        TransactionDate DATETIME2(3)   NOT NULL CONSTRAINT DF_Transactions_Date DEFAULT (SYSUTCDATETIME()),
        TransactionType TINYINT        NOT NULL,
        Amount          DECIMAL(18, 2) NOT NULL,
        Balance         DECIMAL(18, 2) NOT NULL,
        AccountId       INT            NOT NULL,

        CONSTRAINT PK_Transactions                 PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Transactions_Accounts        FOREIGN KEY (AccountId) REFERENCES dbo.Accounts (Id),
        CONSTRAINT CK_Transactions_TransactionType CHECK (TransactionType IN (1, 2)),
        CONSTRAINT CK_Transactions_Amount          CHECK (Amount <> 0)
    );

    CREATE INDEX IX_Transactions_Account_Date ON dbo.Transactions (AccountId, TransactionDate);

    EXEC sys.sp_addextendedproperty
        @name = N'MS_Description', @value = N'1 = Deposit, 2 = Withdrawal',
        @level0type = N'SCHEMA', @level0name = N'dbo',
        @level1type = N'TABLE',  @level1name = N'Transactions',
        @level2type = N'COLUMN', @level2name = N'TransactionType';
END
GO
