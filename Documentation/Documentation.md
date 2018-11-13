# C# Typed Query Documentation
---
## Index
- [Introduction](#introduction)
- [Getting Started](#getting-started)
- [Defining a Table](#defining-a-table)
- [Defining a Row](#defining-a-row)
- [Insert Rows](#insert-rows)
- [Update Rows](#update-rows)
- [Delete Rows](#delete-rows)
- [Select Queries](#select-queries)
- [Conditions](#conditions)
- [Numeric Conditions](#numeric-conditions)
- [Dynamic Conditions](#dynamic-conditions)
- [Select Fields](#select-fields)
- [Select Options](#select-options)
- [Joins](#joins)
- [Group By And Having](#group-by-and-having)
- [Order By](#order-by)
- [Query Hints (Append)](#query-hints-append)
- [Aggregate Functions](#aggregate-functions)
- [Date Functions](#date-functions)
- [Query Time Out And Parameters](#query-time-out-and-parameters)
- [Execute Uncommitted Query](#execute-uncommitted-query)
- [Query Within a Transaction](#query-within-a-transaction)
- [Transaction Isolation Levels](#transaction-isolation-levels)
- [Commit and RollBack](#commit-and-rollback)
- [Row Rollback](#row-rollback)
- [Field Types](#field-types)
- [Key Columns](#key-columns)
- [Enum Fields](#enum-fields)
- [Database Compatibility](#database-compatibility)
- [Global Settings](#global-settings)
- [Temp Tables](#temp-tables)
- [Custom TSql](#custom-tsql)
- [Plain Text Queries](#plain-text-queries)
- [Sql Helper Class](#sql-helper-class)
- [Insert Select](#insert-select)
- [Bulk Insert](#bulk-insert)
- [Join Update](#join-update)
- [Nested Queries](#nested-queries)
- [Union, Except](#union-except)
- [Stored Procedures](#stored-procedures)
- [Thread Safety](#thread-safety)
- [Force Transaction on Current Thread](#force-transaction-on-current-thread)
- [Window Functions](#window-functions)
- [Read Only Connection Security Feature](#read-only-connection-security-feature)
- [Table and Column Comments](#table-and-column-comments)

## Introduction
Typed Query (TQ) is a type safe (i.e. No sql strings in code) database query api that sits on top of Ado.net. Written in C# it is designed to have a simple api that closely maps to standard sql.
TQ is designed to be database independent. Currently SqlServer and PostgreSql are supported. (Note: There are some features that are database dependent like string comparison and query hints).

Pros
- Table and Row mappings are in code not xml or config file - Mappings can be validated against database at runtime
- TypeSafe queries and data access
- API gives lower level query control compared to other APIs like Linq and ORMs i.e. You know what your query really looks like
- Complex query features supported like unions, nested queries, query hints, temp tables, functions, window functions, stored procedures.
- Transactions with standard isolation levels
- Parameterised Queries (Configurable for debugging ease)
- API can notify you of each query performed before and after they happen
- Good for debugging and logging
- No strings or sql injection attacks
- Because tables scehmas are defined in code any field changes are verified by the compiler
- UI tools to generate Table and Row definitions and validate existing definitions
- Open source with a small code base

## Getting Started
One database class must be defined for each database your code is connecting to. The database class must inherit from Sql.ADatabase. Table objects will reference this class to create connections to the database.
Example:
```C#
public class MainDatabase : Sql.ADatabase {

    public readonly static Sql.ADatabase INSTANCE = new MainDatabase();

    private MainDatabase() : base("db_name", Sql.DatabaseType.Mssql) { }

    protected override string ConnectionString {
        get {
            return "user id=sa;password=pwd;server=localhost\\SQLEXPRESS;" +
            "Trusted_Connection=no;database=db_name;" +
            "connection timeout=30";
        }
    }
    public override System.Data.Common.DbConnection GetConnection() {
        SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}
```

## Defining a Table
Table classes should be generated using the Table Generator UI tool (To make life easy). Here is example code for the UserTable. Table classes inherit from Sql.ATable.
Sql (For Sql Server):
```SQL
CREATE TABLE Sec_User (

    userId INTEGER IDENTITY NOT NULL PRIMARY KEY,
    userCode NVARCHAR(100) NOT NULL,
    userPassword NVARCHAR(100) NOT NULL,
    userFirstName NVARCHAR(100) NOT NULL,
    userLastName NVARCHAR(100) NOT NULL
);
```
Code definition:
```C#
public sealed class Table : Sql.ATable {

    public static readonly Table INSTANCE = new Table();    //Single table instance

    public readonly Sql.Column.IntegerColumn Id;
    public readonly Sql.Column.StringColumn Code;
    public readonly Sql.Column.StringColumn Password;
    public readonly Sql.Column.StringColumn FirstName;
    public readonly Sql.Column.StringColumn LastName;

    internal Table() : base(MainDatabase.INSTANCE, "Sec_User", typeof(Row)) {

        Id = new Sql.Column.IntegerColumn(this, "userId", true, true);    //Auto Id
        Code = new Sql.Column.StringColumn(this, "userCode", false);
        Password = new Sql.Column.StringColumn(this, "userPassword", false);
        FirstName = new Sql.Column.StringColumn(this, "userFirstName", false);
        LastName = new Sql.Column.StringColumn(this, "userLastName", false);

        AddColumns(Id, Code, Password, FirstName, LastName);
    }

    public Row this[int pIndex, Sql.IResult pQueryResult] {
        get { return (Row)pQueryResult.GetRow(this, pIndex); }
    }
}
```

## Defining a Row
Row classes should be generated using the Table Generator UI tool (To make life easy). Here is example code for the UserTable Row. Row classes inherit from Sql.ARow.
```C#
public sealed class Row : Sql.ARow {

    private new Table Tbl {
        get { return (Table)base.Tbl; }
    }
    public Row() : base(Table.INSTANCE) {
    }

    public int Id {    //Auto id has no setter
        get { return Tbl.Id.ValueOf(this); }
    }
    public string Code {
        get { return Tbl.Code.ValueOf(this); }
        set { Tbl.Code.SetValue(this, value); }
    }
    public string Password {
        get { return Tbl.Password.ValueOf(this); }
        set { Tbl.Password.SetValue(this, value); }
    }
    public string FirstName {
        get { return Tbl.FirstName.ValueOf(this); }
        set { Tbl.FirstName.SetValue(this, value); }
    }
    public string LastName {
        get { return Tbl.LastName.ValueOf(this); }
        set { Tbl.LastName.SetValue(this, value); }
    }
}
```

## Insert Rows
With TQ there are two ways to insert rows. The first is by using the Row object and the second is using an insert query.
### Row object insert
First create an instance of the user Row class. Set fields with data. Calling Update(...) on the row writes the row to the database. When the row is written to the database it loads any auto id fields.
```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Row userRow = new Row();

    userRow.Code = "user_code";
    userRow.Password = "password";
    userRow.FirstName = "first_name";
    userRow.LastName = "last_name";

    userRow.Update(transaction);

    int autoId = userRow.Id;    //Retrieve auto generated id

    transaction.Commit();
}
```
### Query Insert
Rows can be inserted using a normal sql insert query.

```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    Sql.Query.Insert(userTable)
        .Set(userTable.Code, "user_code")
        .Set(userTable.Password, "password")
        .Set(userTable.FirstName, "first_name")
        .Set(userTable.LastName, "last_name")
        .Execute(transaction);

    transaction.Commit();
}
```
Returning insert syntax is also supported
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    Sql.IResult result =
        Sql.Query.Insert(userTable)
        .Set(userTable.Code, "user_code")
        .Set(userTable.Password, "password")
        .Set(userTable.FirstName, "first_name")
        .Set(userTable.LastName, "last_name")
        .Returning(userTable.Id, userTable.Code)    //Returns the provided fields in the result
        .Execute(transaction);

    transaction.Commit();

    int autoId = userTable[0, result].Id;    //Get the returned auto id
    string code = userTable[0, result].Code;    //Get code field to show that multiple fields can be returned
}
```

## Update Rows
With TQ there are two ways to update rows. The first is by using the Row object and the second is using an update query.
### Row object update
First the row must be loaded from the database. In order for the row to be update-able every field in the row must be selected in the load query.
```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    //Load row
    Sql.IResult result =
        Sql.Query.Select(userTable)
            .From(userTable)
            .Where(userTable.Id == 2)
            .Execute(MainDatabase.INSTANCE);

    Row userRow = userTable[0, result];    //Get first row

    userRow.Code = "user_code";
    userRow.Password = "password";
    userRow.FirstName = "first_name";
    userRow.LastName = "last_name";

    userRow.Update(transaction);
    transaction.Commit();
}
```
### Update query
Rows can be updated using a normal sql update query.
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    Sql.IResult result = Sql.Query.Update(userTable)
        .Set(userTable.Code, "user_code")
        .Set(userTable.Password, "password")
        .Set(userTable.FirstName, "first_name")
        .Set(userTable.LastName, "last_name")
        .Where(userTable.Id == 2)
        .Execute(transaction);

    transaction.Commit();

    if(result.RowsEffected > 0) {
        //Do something
    }
}
```
Returning update syntax:
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    Sql.IResult result = Sql.Query.Update(userTable)
        .Set(userTable.Code, "user_code")
        .Set(userTable.Password, "password")
        .Set(userTable.FirstName, "first_name")
        .Set(userTable.LastName, "last_name")
        .Where(userTable.Id == 2)
        .Returning(userTable.Id)    //Returns Id field of rows updated in result
        .Execute(transaction);

    transaction.Commit();

    for(int index = 0; index < result.Count; index++) { //Get returned fields from result

        Row userRow = userTable[index, result];
        int autoIdsUpdated = userRow.Id;
    }
}
```

## Delete Rows
With TQ there are two ways to delete rows. The first is by using the Row object and the second is using a delete query.
### Row object delete
First load row with all fields selected. Then flag row to be deleted. Call Update(...) to write the change to the database.
```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    //Load row
    Sql.IResult result =
        Sql.Query.Select(userTable)
            .From(userTable)
            .Where(userTable.Id == 2)
            .Execute(MainDatabase.INSTANCE);

    Row userRow = userTable[0, result];

    userRow.Delete();    //Flag to be deleted

    userRow.Update(transaction);
    transaction.Commit();
}
```
### Delete query
Rows can be deleted using a normal sql delete query.
```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    int rows = Sql.Query.Delete(userTable)
        .Where(userTable.Id == 2)
        .Execute(transaction);

    transaction.Commit();
}
```
Returning delete syntax:
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {

    Table userTable = Table.INSTANCE;

    Sql.IResult result =
        Sql.Query.Delete(userTable)
        .Where(userTable.Id == 2)
        .Returning(userTable.Id, userTable.Code)    //Return id and code of deleted row(s)
        .Execute(transaction);

    for(int index = 0; index < result.Count; index++) {    //Get id and code of deleted row(s)

        Row userRow = userTable[index, result];

        int autoId = userRow.Id;
        string code = userRow.Code;
    }

    transaction.Commit();
}
```

## Select Queries
Select queries in TQ are very similar to standard sql.
### Simiple Select (3 Fields)
```C#
Table userTable = Table.INSTANCE;

Sql.IResult result = Sql.Query
    .Select(userTable.Id, userTable.FirstName, userTable.LastName)
    .From(userTable)
    .Where(userTable.FirstName == "james")
    .Execute(MainDatabase.INSTANCE);

for (int index = 0; index < result.Count; index++) {
    Row userRow = userTable[index, result];

    int? id = userRow.Id;
    string firstName = userRow.FirstName;
    string lastName = userRow.LastName;
}
```

## Conditions
In TQ the SQL condition operators 'AND' and 'OR' map to the C# operators '&' and '|'. (Note: the operators && and || are reserved in C# so these cannot be used).
As a rule of thumb always surround an 'OR' operator with brackets.
Example:
```C#
Sql.IResult result = Sql.Query
    .Select(userTable.Id, userTable.FirstName, userTable.LastName)
    .From(userTable)
    .Where(
        userTable.LastName == "smith" &
        (userTable.FirstName.Like("j%") | userTable.FirstName.Like("s%"))
    )
    .Execute(MainDatabase.INSTANCE);
```

## Numeric Conditions
TQ supports numeric operators in conditions using numeric fields.
```C#
Sql.IResult result = Sql.Query
    .Select(userTable.Id)
    .From(userTable)
    .Where(userTable.Id % 2 != 0)
    .Execute(MainDatabase.INSTANCE);

Sql.IResult result = Sql.Query
    .Select(userTable.Id)
    .From(userTable)
    .Where(userTable.Id + 5 > 0 & (userTable.Id * userTable.Id) > userTable.Id)
    .Execute(MainDatabase.INSTANCE);

Sql.IResult result = Sql.Query
    .Select(userTable.Id)
    .From(userTable)
    .GroupBy(userTable.Id)
    .Having((userTable.Id * userTable.Id) - 2 > 0)
    .Execute(MainDatabase.INSTANCE);
```

## Dynamic Conditions
There are cases where the condition needs to differ depending on certain inputs. This can be achieved by using the Condition class to build up the condition dynamically.
```C#
Table userTable = Table.INSTANCE;

Sql.Condition condition = userTable.LastName == "smith" &
            (userTable.FirstName.Like("j%") | userTable.FirstName.Like("s%"));

if (pId != null)
    condition = condition & userTable.Id == pId;

Sql.IResult result = Sql.Query
    .Select(userTable.Id, userTable.FirstName, userTable.LastName)
    .From(userTable)
    .Where(condition)
    .Execute(MainDatabase.INSTANCE);
```
Example 2: Using 'AndIf' condition. (Also "AndOr(...)"). This will only include the condition if the first parameter is true.
```C#
Table userTable = Table.INSTANCE;

Sql.Condition condition = userTable.LastName == "smith" &
            (userTable.FirstName.Like("j%") | userTable.FirstName.Like("s%"));

condition = condition.AndIf(pId != null, userTable.Id == pId);   //Condition is only included if Pid != null

Sql.IResult result = Sql.Query
    .Select(userTable.Id, userTable.FirstName, userTable.LastName)
    .From(userTable)
    .Where(condition)
    .Execute(MainDatabase.INSTANCE);
```

## Select Fields
The TQ select query can take a number of different field inputs. It can take whole tables, individual fields and functions all within the same method.
Example: This selected every column in the userTable and the id field (A second time).
```C#
Sql.IResult result = Sql.Query
    .Select(userTable, userTable.Id)
    .From(userTable)
    .Execute(MainDatabase.INSTANCE);
```

## Select Options
TQ supports 3 select options. Distinct, TOP (Translates to LIMIT) and INTO.
Example:
```C#
Sql.Query.Select(userTable).Distinct
    .From(userTable)
    .Execute(MainDatabase.INSTANCE);

Sql.Query.Select(userTable).Top(1)
    .From(userTable)
    .Execute(MainDatabase.INSTANCE);

Sql.Query.Select(userTable)
    .Into(tempTable)
    .From(userTable)
    .Execute(MainDatabase.INSTANCE);
```

## Joins
TQ supports three joins types. Join, Left Join and Right Join.
Example: Self join
(Please note with self joins (i.e. Joining to the same table) different table object instances are required. This is because each instance has its own unique alias. If the query uses the same table object for a self join the sql aliasing will clash and cause an error.
```C#
Table userTableA = Table.INSTANCE;    //Instance one
Table userTableB = new Table();    //Instance two

Sql.IResult result = Sql.Query
    .Select(userTableA)
    .From(userTableA)
    .Join(userTableB, userTableA.Id == userTableB.Id)
    .Execute(MainDatabase.INSTANCE);

result = Sql.Query
    .Select(userTableA)
    .From(userTableA)
    .LeftJoin(userTableB, userTableA.Id == userTableB.Id)
    .Execute(MainDatabase.INSTANCE);

result = Sql.Query
    .Select(userTableA)
    .From(userTableA)
    .RightJoin(userTableB, userTableA.Id == userTableB.Id)
    .Execute(MainDatabase.INSTANCE);
```

## Group By And Having
```C#
Sql.Function.CountAll count = new Sql.Function.CountAll();

Sql.IResult result = Sql.Query
    .Select(userTable.FirstName, count)
    .From(userTable)
    .GroupBy(userTable.FirstName)
    .Having(count > 1)
    .Execute(MainDatabase.INSTANCE);
```

## Order By
TQ sorts fields either by the default sort of the database or by the defined value e.g. ASC or DESC.
```C#
Sql.IResult result = Sql.Query
    .Select(userTable)
    .From(userTable)
    .OrderBy(userTable.FirstName, userTable.LastName.ASC, userTable.Code.DESC)
    .Execute(MainDatabase.INSTANCE);
```

## Query Hints (Append)
Query hints can be added using the Append(...) method.
```C#
Sql.IResult result = Sql.Query
    .Select(userTable)
    .From(userTable)
    .Append("OPTION(MAXDOP 1)")
    .Execute(MainDatabase.INSTANCE);
```

## Aggregate Functions
Example: SUM(usrId) grouped by first name.
```C#
Table userTable = Table.INSTANCE;
Sql.Function.SumInt sum = new Sql.Function.SumInt(userTable.Id);

Sql.IResult result = Sql.Query
    .Select(userTable.FirstName, sum)
    .From(userTable)
    .GroupBy(userTable.FirstName)
    .ExecuteUncommitted();

int sumValue = sum[0, result].Value;    //Get first value for example
```
TQ supports a number of standard functions. Custom function classes can be written by implementing the IFunction interface.
Supported Functions
AVG, MIN, MAX, SUM
CURRENT_DATE
COUNT
Example: Count(*) from userTable
```C#
Table userTable = Table.INSTANCE;
Sql.Function.CountAll count = new Sql.Function.CountAll();

Sql.IResult result = Sql.Query
    .Select(count)
    .From(userTable)
    .ExecuteUncommitted();

int countValue = count[0, result].Value;
```

## Date Functions
TQ supports date functions Year, Month, DayOfMonth, Hour, Minute, Second.
Example:
```C#
Tables.DateTimeTable.Table table = Tables.DateTimeTable.Table.INSTANCE;

Sql.Function.DateFunction year = new Sql.Function.DateFunction(table.Dt, DatePart.Year);
Sql.Function.DateFunction month = new Sql.Function.DateFunction(table.Dt, DatePart.Month);
Sql.Function.DateFunction day = new Sql.Function.DateFunction(table.Dt, DatePart.DayOfMonth);
Sql.Function.DateFunction hour = new Sql.Function.DateFunction(table.Dt, DatePart.Hour);
Sql.Function.DateFunction minute = new Sql.Function.DateFunction(table.Dt, DatePart.Minute);
Sql.Function.DateFunction second = new Sql.Function.DateFunction(table.Dt, DatePart.Second);

IResult result = Query.Select(year, month, day, hour, minute, second)
    .From(table)
    .Where(year > 10)
    .GroupBy(year, month, day, hour, minute, second)
    .Having(year > 20 & year < 3000)
    .OrderBy(year, month, day, hour, minute, second)
    .Execute(MainDatabase.INSTANCE);
    
int? yearValue = year[0, result];
int? monthValue = month[0, result];
int? dayValue = day[0, result];
int? hourValue = hour[0, result];
int? minuteValue = minute[0, result];
int? secondValue = second[0, result];
```

## Query Time Out And Parameters
The default time out can be overridden for a particular query. This overrides the global setting in Sql.Settings.DefaultTimeout. In this example the query will timeout after 60 seconds:
```C#
Sql.Query
    .Select(userTable)
    .From(userTable)
    .Timeout(60)    //60 seconds
    .Execute(MainDatabase.INSTANCE);
```
Parameters can be turned on or off on a query. This overrides the global setting in Sql.Settings.UseParameters. This can be useful when using parameters produces an undesirable query plan.
```C#
Sql.Query
    .Select(userTable)
    .From(userTable)
    .UseParameters(false)
    .Execute(MainDatabase.INSTANCE);
```

## Execute Uncommitted Query
Execute a query as read uncommitted without having to use a Sql.Transaction object.
```C#
Sql.Query
    .Select(userTable)
    .From(userTable)
    .ExecuteUncommitted();
```

## Query Within a Transaction
```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
                
    for (int index = 0; index < 100; index++) {    //Delete row one at a time

        Table userTable = Table.INSTANCE;

        Sql.IResult result =
            Sql.Query.Select(userTable).Top(1)
                .From(userTable)
                .Execute(transaction);

        Row userRow = userTable[0, result];

        userRow.Delete();
        userRow.Update(transaction);
    }                
    transaction.Commit();
}
```

## Transaction Isolation Levels
TQ supports the standard ado.net database isolation levels. Not all of these are supported by non sqlserver databases.
```C#
new Sql.Transaction(MainDatabase.INSTANCE, System.Data.IsolationLevel.ReadUncommitted);
new Sql.Transaction(MainDatabase.INSTANCE, System.Data.IsolationLevel.ReadCommitted);
new Sql.Transaction(MainDatabase.INSTANCE, System.Data.IsolationLevel.RepeatableRead);
new Sql.Transaction(MainDatabase.INSTANCE, System.Data.IsolationLevel.Serializable);
```

## Commit and RollBack
If a transaction is used in a resource block it is automatically rolled back if it is not committed when the code exits the block.
Example:
```C#
using (Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
    //Do something
}//Will rollback here because transaction has not been committed
```
Transactions can be committed by calling the Commit(...) method on the transaction. They can be rolled back by calling Rollback(...) on the transaction.
Example:
```C#
transaction.Commit();
transaction.Rollback();
```

## Row Rollback
When a transaction is rolled back rows saved to the database within that transaction will retain their most recent column values and stay in the same state. So for example if a row is set to be deleted and the transaction is rolled back it will stay in the to be deleted state.

## Field Types
These are currently supported field types:

Column | Type
------- | --------
BigIntegerColumn | Int64 Not Null
BinaryColumn | Binary Field Not Null
BoolColumn | Bool or TinyInt Not Null
ByteColumn | Byte or Bool Not Null
DateTimeColumn | DateTime Not Null
DateTime2Column | DateTime Not Null
DateTimeOffsetColumn | NDateTimeOffsetColumn DateTimeOffset. Note the behaviour of this field differs between sql and postgreSql. PostgreSql doesn't store an offset so retrieved values are converted into local time. Sql server returns retrieved values in the offset time they were stored.
DecimalColumn | Decimal Not Null
DoubleColumn | Double Not Null
FloatColumn | Float Not Null
EnumColumn | Integer Not Null. Maps to C# enum
GuidColumn | Guid Not Null
IntegerColumn | Int Not Null
NBigIntegerColumn | Int64? Null
NBinaryColumn | Binary Field Null
NBoolColumn | Bool? or TinyInt Null
NByteColumn | Byte? or Bool? Null
NDateTimeColumn | DateTime? Null
NDateTime2Column | DateTime? Null
NDecimalColumn | Decimal? Null
NDoubleColumn | Double? Null
NfloatColumn | Float? Null
NGuidColumn | Guid? Null
NIntegerColumn | Int? Null
NSmallIntColumn | Int16 Null
SmallIntColumn | Int16 Not Null
StringColumn | String Null and Null

## Key Columns
'Key Columns' are columns that are used to map the primary and foreign key relationship between tables. These columns should be used for fields that are either a primary key field or a forgein key field. Key columns allow the compiler to check that join conditions on those fields are only between the primary and foreign key columns. Joins to non key columns of the same type are disallowed by the compiler.

Currently only a subset of Column types are supported 

Column | Type
------- | --------
GuidKeyColumn | Guid Not Null
NGuidKeyColumn | Guid? Null
BigIntegerKeyColumn | Int64 Not Null
NBigIntegerKeyColumn | Int64? Null
IntegerKeyColumn | Int Not Null
NIntegerKeyColumn | Int? Null
SmallIntKeyColumn | Int16 Not Null
NSmallIntKeyColumn | Int16 Null

Code Example:
Here we have two tables, the PersonTable and the OrderLogTable. The OrderLogTable has a foreign key to the PersonTable. This is mapped using a 'GuidKeyColumn' to the Person table using a generic argument.

Sql:
```SQL
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
```
Code Definition:
```C#
namespace Sql.Tables.Person {

    //
    //    PersonTable
    //
    public sealed class Table : Sql.ATable {

        public static readonly Table INSTANCE = new Table();

        public Sql.Column.GuidKeyColumn<Table> Key { get; private set; }    //Primary Key Column
        public Sql.Column.StringColumn FirstName { get; private set; }
        public Sql.Column.StringColumn Surname { get; private set; }

        public Table() : base(Database, "Person", typeof(Row)) {

            Key = new Sql.Column.GuidKeyColumn<Table>(this, "perKey", true);
            FirstName = new Sql.Column.StringColumn(this, "perFirstName", false);
            Surname = new Sql.Column.StringColumn(this, "perSurname", false);

            AddColumns(Key,FirstName,Surname);
        }

        public Row this[int pIndex, Sql.IResult pResult]{
            get { return (Row)pResult.GetRow(this, pIndex); }
        }
    }
}
namespace Sql.Tables.OrderLog {

    //
    //    OrderLogTable
    //
    public sealed class Table : Sql.ATable {

        public static readonly Table INSTANCE = new Table();

        public Sql.Column.GuidColumn Key { get; private set; }
        public Sql.Column.GuidKeyColumn<Sql.Tables.Person.Table> PersonKey { get; private set; }    //Foreign Key Column To Person Table
        public Sql.Column.StringColumn Item_ { get; private set; }

        public Table() : base(Database, "OrderLog", typeof(Row)) {

            Key = new Sql.Column.GuidColumn(this, "ordKey", true);
            PersonKey = new Sql.Column.GuidKeyColumn<Sql.Tables.Person.Table>(this, "ordPersonKey", false);
            Item_ = new Sql.Column.StringColumn(this, "ordItem", false);

            AddColumns(Key,PersonKey,Item_);
        }

        public Row this[int pIndex, Sql.IResult pResult]{
            get { return (Row)pResult.GetRow(this, pIndex); }
        }
    }
}
```

## Enum Fields
An enum field is an interger field that is mapped to an integer enum in C#. There is a special AColumn field to map this in a type safe way.
Full Example:
```C#
public void Example() {

    Table enumTable = Table.INSTANCE;

    Sql.IResult result =
        Sql.Query.Select(enumTable)
        .From(enumTable)
        .Where(enumTable.EnumValue == EnumTypes.A)
        .Execute(MainDatabase.INSTANCE);

    for(int index = 0; index < result.Count; index++) {

        EnumTypes value = enumTable[index, result].EnumValue;
    }
}

public enum EnumTypes {
    A = 1,
    B = 2,
    C = 3
}

public sealed class Table : Sql.ATable {

    public static readonly Table INSTANCE = new Table();

    public readonly Sql.Column.EnumColumn<EnumTypes> EnumValue;

    public Table()
        : base(MyDatabase.INSTANCE, "EnumTable", typeof(Row)) {

        EnumValue = new Sql.Column.EnumColumn<EnumTypes>(this, "EnumValue", false);

        AddColumns(EnumValue);
    }

    public Row this[int pIndex, Sql.IResult pQueryResult] {
        get { return (Row)pQueryResult.GetRow(this, pIndex); }
    }
}

public sealed class Row : Sql.ARow {

    private new Table Tbl {
        get { return (Table)base.Tbl; }
    }

    public Row() : base(Table.INSTANCE) {

    }

    public EnumTypes EnumValue {
        get { return Tbl.EnumValue.ValueOf(this); }
        set { Tbl.EnumValue.SetValue(this, value); }
    }
}
```

## Database Compatibility
Most of TQ is database independent. The main areas where functionality may differ are:
- String compare and LIKE operator - Some databases are case sensitive
- Query hints are generally database specific
- DateTimeOffset and NdateTimeOffset have slightly different behaviour. This is due to the way each database stores time zones datetime values.
- PostgreSql stores 'TIMESTAMPE WITH TIME ZONE' type as UTC without any offset information. SqlServer stores 'DateTimeOffset' as UTC with offset. This means that when retrieving a date postgreSql will return the value converted into local time where as sql server will return it as the original offset time.
- Aggregate functions like AVG(...) output different values between Sql Server and PostgreSql. Sql server rounds integer averages where as postgreSql does not.
- Window function syntax differs between Sql Server and PostgreSql. Generally sql server has tighter rules around the use of the 'Order By' clause.

## Global Settings
There is a global settings class to set defaults on the following:
- UseParameters - Turn query parameters on or off
- UseConcurrenyChecking - This is used for row object updates and deletes. If set true then every field in the row is used to identify the row during an update or delete. If it is set to false then only the primary key fields are used to find the row. This can be overridden on the Table definition if needed.
- DefaultTimeout - Sets the default query time out on the client in seconds. This can be overridden on particular queries if needed.
- QueryExecuting event - This fires before a query is executed. Can be used for debugging.
- QueryPerformed event - This event fires every time a query is executed. Can be used for debugging.

```C#
static void Main(string[] args) {

    Sql.Settings.DefaultTimeout = 30;    //30 Second default timeout
    Sql.Settings.QueryExecuting += new Sql.Settings.QueryExecutingDelegate(QueryExecuting);
    Sql.Settings.QueryPerformed += new Sql.Settings.QueryPerformedDelegate(QueryPerformed);
    Sql.Settings.ReturnResultSize = true;    //Setting to populate the result size field on query results
    Sql.Settings.UseConcurrenyChecking = true;
    Sql.Settings.UseParameters = true;
}

private static void QueryPerformed(string pSql, int pRows, Sql.QueryType pQueryType, DateTime? pStart, DateTime? pEnd,
        Exception pException, System.Data.IsolationLevel pIsolationLevel, int? pResultSize, ulong? pTransactionId) {
}

private static void QueryExecuting(string pSql, Sql.QueryType pQueryType, DateTime? pStart,
        System.Data.IsolationLevel pIsolationLevel, ulong? pTransactionId) {
}
```

## Temp Tables
To use a temp table you first need to define a table and row object for the table. By not providing a table name in the table class TQ treats it as a temp table.
```C#
public sealed class Table : Sql.ATable {

    public static readonly Table INSTANCE = new Table();

    public readonly Sql.Column.StringColumn Str;

    //Notice no table name is provided for a temp table definition
    public Table() : base(MainDatabase.INSTANCE, typeof(Row)) {
        Str = new Sql.Column.StringColumn(this, "Str", true);
        AddColumns(Str);
    }

    public Row this[int pIndex, Sql.IResult pQueryResult] {
        get { return (Row)pQueryResult.GetRow(this, pIndex); }
    }
}
public sealed class Row : Sql.ARow {

    private new Table Tbl {
        get { return (Table)base.Tbl; }
    }

    public Row() : base(Table.INSTANCE) {
    }
    public string Str {
        get { return Tbl.Str.ValueOf(this); }
        set { Tbl.Str.SetValue(this, value); }
    }
}
```
Query example: Insert into temp table
```C#
Table userTable = Table.INSTANCE;
TempTable.Table tempTable = TempTable.Table.INSTANCE;

Sql.Query.Select(userTable.FirstName)
    .Into(tempTable)
    .From(userTable)
    .Execute(transaction);

//Query temp table
Sql.IResult result =
    Sql.Query.Select(tempTable)
    .From(tempTable)
    .Execute(transaction);
```

## Custom TSql
Custom sql can be used by using the class Sql.Function.CustomSql(...). This can be used when TQ doesn't support database functions features that you want to use.
Warning: Remember to validate against sql injection attacks when using custom sql.
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)){
                
    Sql.Query.Insert(table)
        .Set(table.Id, Guid.NewGuid())
        .Set(table.Dt, new Sql.Function.CustomSql("GETDATE()"))
        .Execute(transaction);
                
    transaction.Commit();
}
```
Example: Send custom TSql query.
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
            
    string sql = "DECLARE @dt DATETIME;SET @dt=GETDATE();";
                
    sql += Sql.Query.Insert(table)
        .Set(table.Id, Guid.NewGuid())
        .Set(table.Dt, new Sql.Function.CustomSql("@dt"))
        .GetSql();
                
    Sql.Query.ExecuteNonQuery(sql, table.Database, transaction);                
    transaction.Commit();
}
```

## Plain Text Queries
TQ allows you to send non select plain text queries.
Warning: Remember to validate against sql injection attacks when using plain text queries.
```C#
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
            
    string sql = "UPDATE userTable SET password = NULL;";
                
    Sql.Query.ExecuteNonQuery(sql, MainDatabase.INSTANCE, transaction);                
    transaction.Commit();
}
```

## Sql Helper Class
The class Sql.SqlHelper contains static methods that help load query results into lists, dictionaries and to format sql to be more human readable.

## Insert Select
TQ allows insert select queries.
```C#
Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;
            
using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
    Sql.Query.InsertSelect(table)
            .Columns(table.IntValue)
            .Query(Sql.Query
                    .Select(table.IntValue)
                    .From(table))
            .Execute(transaction);
    transaction.Commit();
}
```

## Bulk Insert
TQ has a bulk insert class that can but used when performing a large number of insert queries against a table. This functionality takes advantage of the insert 'values' syntax to reduce the number of network round trips between the client and the database.
```C#
using(Transaction transaction = new Transaction(MainDatabase.INSTANCE)) {
    
    Tables.User.Table userTable = Tables.User.Table.INSTANCE;
    
    BulkInsert bulkInsert = new BulkInsert();                
    
    foreach(User user in userList) {
        
        bulkInsert.AddValues(
            Sql.Query.Insert(userTable)
                .Set(userTable.Code, user.Code)
                .Set(userTable.FirstName, user.FirstName)
                .Set(userTable.LastName, user.LastName)
                .Set(userTable.Password, user.Password)
        );
    }                
    
    bulkInsert.Execute(transaction);                
    transaction.Commit();
}
```

## Join Update
TQ supports update join syntax
```C#
Table tableA = new Table();
Table tableB = new Table();

using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
                
    Sql.Query.Update(tableA)
        .Set(tableA.Code, tableB.Code)    //Set tableB code to tableA code
        .Set(tableA.FirstName, string.Empty)
        .Join(tableB, tableA.FirstName == tableB.FirstName)
        .Where(tableB.Id > 25)
        .Execute(transaction);
}
```

## Nested Queries
When using nested queries it is important to use different instances of tables. In this example we must have two different instances of userTable. This is so the sql table aliases are correct when executing the query.
```C#
Table userTable = Table.INSTANCE;
Table userTableB = new Table();

Sql.Query.Select(userTable)
    .From(userTable)
    .Where(userTable.Id.In(
            Sql.Query
            .Select(userTableB.Id)
            .From(userTableB)
            .Where(userTableB.FirstName == "james"))
    ).Execute(MainDatabase.INSTANCE);
```

## Union, Except
When using union queries it is important to use different instances of table classes so the sql aliasing works correctly.
Also due to implementation limitations the union result is only accessible from tables defined in the last union query (e.g. UserTableB in this example).
Union, Union All, Interset and Except syntax is supported.
```C#
Table userTable = Table.INSTANCE;
Table userTableB = new Table();

Sql.IResult result = Sql.Query
    .Select(userTable.FirstName, userTable.LastName)
    .From(userTable)
    .Union(userTableB.FirstName, userTableB.LastName)
    .From(userTableB)
    .OrderBy(userTableB.FirstName)
    .Execute(MainDatabase.INSTANCE);

for (int index = 0; index < result.Count; index++) {
    //Note result is only accessable from the last query tables
    Row userRow = userTableB[index, result];
}
```

## Stored Procedures
To execute stored procedures you must create a stored procedure class definition by implementing the abstract class Sql.AStoredProc. This can be generated using the code generator.
In and out parameters are supported as well as returned row results

(Note: The code generater tool can not generate return column fields for stored procedures so you will need to add them manually like on a table definition).

Full Example:
```C#
using System;
using System.Data;
using System.Data.SqlClient;
 
namespace SP_Test_In_Out {
 
    public class Test {
 
        static void Main(string[] args) {
 
            #region Turn on global debugging features
 
            //Set up to QueryPerformed event to show queries executing in real time. Debugging feature.
            Sql.Settings.QueryPerformed += new Sql.Settings.QueryPerformedDelegate(QueryPerformed);
            Sql.Settings.UseParameters = false; //Turn off parameters so queries are easier to read.
            Sql.Settings.ReturnResultSize = true;
 
            #endregion
 
            using(Sql.Transaction transaction = new Sql.Transaction(MainDatabase.INSTANCE)) {
 
                Proc proc = Proc.INSTANCE;  //Get instance of stored procedure object
 
                int inParam = 25;
                int outParam = 0;   //Note: Out parameter
 
                Sql.IResult result = proc.Execute(inParam, ref outParam, transaction);  //Execute stored procedure and get result back.
 
                for(int index = 0; index < result.Count; index++) {
 
                    Row row = proc[index, result];
 
                    Guid id = row.Id;
                    int intValue = row.IntValue;
                }
 
                transaction.Commit();
            }
        }
 
        private static void QueryPerformed(string pSql, int pRows, Sql.QueryType pQueryType, DateTime? pStart, DateTime? pEnd, Exception pException, 
         IsolationLevel pIsolationLevel, int? pResultSize, ulong? pTransactionId) {
            System.Console.WriteLine(pSql);
        }
    }
/*
--
--  Sql to create stored procedure
--
CREATE TABLE IntTable (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IntValue INTEGER NOT NULL   
);
GO
CREATE PROCEDURE SP_Test_In_Out (@In_param INTEGER, @Out_param INTEGER OUT)AS
    SET @Out_param = @In_param;
     
    DELETE FROM IntTable;
     
    INSERT INTO IntTable (Id, IntValue) VALUES(NEWID(), 12345);
    INSERT INTO IntTable (Id, IntValue) VALUES(NEWID(), 123456);
    INSERT INTO IntTable (Id, IntValue) VALUES(NEWID(), 1234567);
    INSERT INTO IntTable (Id, IntValue) VALUES(NEWID(), 12345678);
     
    SELECT Id, IntValue FROM dbo.IntTable ORDER BY IntValue ASC;
 
GO
*/
    //
    //  Generated stored procedure definitions
    //
    public sealed class Proc : Sql.AStoredProc {
 
        public static readonly Proc INSTANCE = new Proc();
 
        public readonly Sql.Column.GuidColumn Id;
        public readonly Sql.Column.IntegerColumn IntValue;
 
        public Proc()
            : base(MyDatabase.INSTANCE, "SP_Test_In_Out", typeof(Row)) {
 
            Id = new Sql.Column.GuidColumn(this, "Id", false);
            IntValue = new Sql.Column.IntegerColumn(this, "IntValue", false);
 
            AddColumns(Id, IntValue); //-->Note: Columns can be added to proc in the same way as a table definition.
        }
 
        public Sql.IResult Execute(int @In_param, ref int @Out_param, Sql.Transaction pTransaction) {
 
            SqlParameter p0 = new SqlParameter("@In_param", SqlDbType.Int);
            p0.Direction = ParameterDirection.Input;
            p0.Value = @In_param;
 
            SqlParameter p1 = new SqlParameter("@Out_param", SqlDbType.Int);
            p1.Direction = ParameterDirection.InputOutput;
            p1.Value = @Out_param;
 
            Sql.IResult result = ExecuteProcedure(pTransaction, p0, p1);
 
            @Out_param = (int)p1.Value;
            return result;
        }
 
        public Row this[int pIndex, Sql.IResult pResult] {
            get { return (Row)pResult.GetRow(this, pIndex); }
        }
    }
 
    public sealed class Row : Sql.ARow {
 
        private new Proc Tbl {
            get { return (Proc)base.Tbl; }
        }
 
        public Guid Id {
            get { return Tbl.Id.ValueOf(this); }
            set { Tbl.Id.SetValue(this, value); }
        }
 
        public int IntValue {
            get { return Tbl.IntValue.ValueOf(this); }
            set { Tbl.IntValue.SetValue(this, value); }
        }
 
        public Row()
            : base(Proc.INSTANCE) {
        }
    }
 
    //
    //Database definition class. Note only one instance is required in your application
    //
    public class MyDatabase : Sql.ADatabase {
 
        public readonly static Sql.ADatabase INSTANCE = new MyDatabase();
 
        private MyDatabase() : base("<<db>>", Sql.DatabaseType.Mssql) {
        }
 
        protected override string ConnectionString {
            get {   //*** Note: Connection details here ***//
                return "Data Source=localhost\\SQLEXPRESS;User Id=login;Password=password;database=db_name;";
            }
        }
 
        public override System.Data.Common.DbConnection GetConnection() {
            lock(this) {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
        }
    }
}
```

## Thread Safety
You can execute queries that don't have a transaction from multiple threads without a problem.
Transactions are not thread safe because the underlying ado connection is not thread safe. You can not execute queries in parallel on a single transaction instance. You can execute queries on separate transaction instances in parallel though.
ATable classes are immutable so they are thread safe. IResult class is not thread safe (But reading should work multi threaded).

## Force Transaction on Current Thread
When creating a new transaction you can set a parameter to check that every query running on the current thread executes using that transaction. If there is a query that doesn't execute on that transaction an exception is thrown. This feature is used to help identify dead lock bugs. A good example would be a service thread that is doing a particular task and you want to catch any executing queries on that thread and make sure they are running in the transaction.

## Window Functions
TQ has syntax support for window functions. This includes 'OVER', 'PARTITION BY' and 'ORDER BY' syntax.

Note: Sql Server and PostgreSql have slightly different syntax rules for window functions. Mostly around the use of the order by clause.
```C#
Table userTable = Table.INSTANCE;

Sql.Function.AvgInteger avg = new Sql.Function.AvgInteger(userTable.Id);
Sql.Function.MinInteger min = new Sql.Function.MinInteger(userTable.Id);
Sql.Function.MaxInt max = new Sql.Function.MaxInt(userTable.Id);
Sql.Function.SumInteger sum = new Sql.Function.SumInteger(userTable.Id);
Sql.Function.RowNumber rowNumber = new Sql.Function.RowNumber();
Sql.Function.Rank rank = new Sql.Function.Rank();
Sql.Function.DenseRank denseRank = new Sql.Function.DenseRank();

Sql.IResult result = Sql.Query.Select(
        userTable.Id,
        avg.OverPartitionBy(userTable.Id).OrderBy(userTable.Id),
        min.OverPartitionBy(userTable.Id),
        max.OverPartitionBy(userTable.Id),
        sum.OverPartitionBy(userTable.Id),
        rowNumber.Over(),
        rank.Over().OrderBy(userTable.Id),
        denseRank.Over().OrderBy(userTable.Id)
    )
    .From(userTable)
    .OrderBy(userTable.Id.ASC)
    .Execute(MainDatabase.INSTANCE);

for(int index = 0; index < result.Count; index++) {

    int id = userTable[index, result].Id;
    decimal? avgValue = avg[0, result];
    int? rankValue = rank[index, result];
}
```

## Read Only Connection Security Feature
TQ has a security feature that allows queries that run outside of a transaction to be run on a read only connection. For example: Select queries that are run without having a transaction object passed in the Execute(...) method are run with a readonly connection if the database class provides one.

To enable this feature the method GetConnection(...) in the database class needs to be altered to return a readonly connection when the parameter pCanBeReadonly is true.
```C#
public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly) {

    SqlConnection connection;
    
    //If the connection can be readonly then return a readonly connection
    if(!pCanBeReadonly)
        connection = new SqlConnection(ConnectionString);
    else
        connection = new SqlConnection(ReadonlyConnectionString);
        
    connection.Open();
    return connection;
}
```

## Table and Column Comments
TQ supports meta data comments against tables and columns. These are added to the table definition in code as attributes. TQ can generate sql to create and update these comments in the database.

Once the comments are assigned to the database a third party tool can be used to generate schema documentation.

Attribute Example:
```C#
[Sql.TableAttribute("Table description does here")]
public sealed class Table : Sql.ATable {

    public static readonly Table INSTANCE = new Table();

    [Sql.ColumnAttribute("Column description goes here")]
    public readonly Sql.Column.GuidColumn Id;

    public Table() : base(DB.TestDB, "GuidTable", typeof(Row)) {
        Id = new Sql.Column.GuidColumn(this, "Id", true);
        AddColumns(Id);
    }

    public Row this[int pIndex, Sql.IResult pQueryResult]{
        get { return (Row)pQueryResult.GetRow(this, pIndex); }
    }
}
```
To generate comment sql call the following method and pass the table definition class.
```C#
Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
            
Sql.Database.GenerateMetaDataSql metaData = new Sql.Database.GenerateMetaDataSql();
            
string commentSql = metaData.GenerateSql(table);
```
