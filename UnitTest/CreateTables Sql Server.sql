﻿
CREATE TABLE GuidTable (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
);

CREATE TABLE StringTable(
	Str NVARCHAR(10) NOT NULL PRIMARY KEY
);

CREATE TABLE DateTimeTable (
	dtId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	dtDt DATETIME NOT NULL
);

CREATE TABLE NDateTimeTable (
	dtId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	dtDt DATETIME NULL
);

CREATE TABLE DateTimeTable2 (
	dtId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	dtDt DATETIME2 NOT NULL
);

CREATE TABLE NDateTimeTable2 (
	dtId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	dtDt DATETIME2 NULL
);

CREATE TABLE DateTimeTableOffset (
	dtId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	dtDt DATETIMEOFFSET NOT NULL,
	dtNullDt DATETIMEOFFSET NULL
);

CREATE TABLE EnumTable (
	EnumValue TINYINT NOT NULL
);

CREATE TABLE AutoTable (
	Id INTEGER IDENTITY NOT NULL PRIMARY Key,
	Text NVARCHAR(255) NOT NULL
);

CREATE TABLE BooleanTable (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Bool BIT NOT NULL
);

CREATE TABLE DecimalTable (
	Id INTEGER IDENTITY NOT NULL PRIMARY KEY,
	DecimalValue DECIMAL(19,8) NOT NULL
);

CREATE TABLE NDecimalTable (
	Id INTEGER IDENTITY NOT NULL PRIMARY KEY,
	DecimalValue DECIMAL(19,8) NULL
);

CREATE TABLE IntTable (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	IntValue INTEGER NOT NULL	
);

CREATE TABLE NIntTable (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	IntValue INTEGER NULL
);

CREATE TABLE BigIntTable (
	Id BIGINT IDENTITY NOT NULL PRIMARY KEY,
	IntValue BIGINT NULL
);

CREATE TABLE SmallIntTable (
	Id SMALLINT IDENTITY NOT NULL PRIMARY KEY,
	IntValue SMALLINT NULL
);

CREATE TABLE BinaryTable (
	BinaryValue VARBINARY(255) NOT NULL,
	NBinaryValue VARBINARY(255) NULL
);

CREATE TABLE Earthquake (
	earCUSP_ID INTEGER NOT NULL,
	earLAT DECIMAL(19,8) NOT NULL,
	earLONG DECIMAL(19,8) NOT NULL,
	earNZMGE INTEGER NOT NULL,
	earNZMGN INTEGER NOT NULL,
	earDateTime DATETIME NOT NULL,
	earMAG DECIMAL(19,8) NOT NULL,
	earDEPTH DECIMAL(19,8) NOT NULL,	
	CONSTRAINT pk_Earthquake PRIMARY KEY(earCUSP_ID)
);

CREATE TABLE FloatTable (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Value REAL NOT NULL,
	NValue REAL NULL
);

CREATE TABLE DoubleTable (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Value FLOAT NOT NULL,
	NValue FLOAT NULL
);
GO

CREATE PROCEDURE SP_Test_In_Out (@In_param INTEGER, @Out_param INTEGER OUT)AS
	SET @Out_param = @In_param;
	
	DELETE FROM dbo.IntTable;
	
	INSERT INTO dbo.IntTable (Id, IntValue) VALUES(NEWID(), 12345);
	INSERT INTO dbo.IntTable (Id, IntValue) VALUES(NEWID(), 123456);
	INSERT INTO dbo.IntTable (Id, IntValue) VALUES(NEWID(), 1234567);
	INSERT INTO dbo.IntTable (Id, IntValue) VALUES(NEWID(), 12345678);
	
	SELECT Id, IntValue FROM dbo.IntTable ORDER BY IntValue ASC;
GO

CREATE TABLE Person (
	perKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	perFirstName NVARCHAR(100) NOT NULL,
	perSurname NVARCHAR(100) NOT NULL
);

CREATE TABLE OrderLog (
	ordKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	ordPersonKey UNIQUEIDENTIFIER NOT NULL,
	ordItem NVARCHAR(100) NOT NULL,
	CONSTRAINT fk_Order_Person FOREIGN KEY(ordPersonKey) REFERENCES Person(perKey)
);
GO

CREATE TABLE PersonId (
	perId INTEGER NOT NULL IDENTITY PRIMARY KEY,
	perFirstName NVARCHAR(100) NOT NULL,
	perSurname NVARCHAR(100) NOT NULL
);

CREATE TABLE OrderLogId (
	ordId INTEGER NOT NULL IDENTITY PRIMARY KEY,
	ordPersonId INTEGER NOT NULL,
	ordItem NVARCHAR(100) NOT NULL,
	CONSTRAINT fk_OrderId_PersonId FOREIGN KEY(ordPersonId) REFERENCES PersonId(perId)
);
GO