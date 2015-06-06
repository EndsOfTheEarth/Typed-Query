# Typed Query
Typed Query (TQ) is a C# API for querying databases in a type safe manor. The aim of TQ is to closely follow standard sql, be fully in code (i.e. No configuration files) and have helpful debugging features.

##Features
* Type safe queries - (Select, Insert, Insert Select, Update, Update Select, Delete, Truncate);
* Sql queries in code - Syntax 'select', 'top', 'distinct', 'from', 'join', 'where', 'group by', 'having', 'order by', 'union', 'nested queries'
* Sql functions, auto id's, parameters, set timeouts for indvidual queries
* Work with jpa style row objects to insert/update/delete or use normal queries
* Stored procedures, Temporary tables
* Database independent - (SqlServer, PostgreSql currently supported). Easy to support more
* No configuration files. Everything is compiled in code.
* Table definition creation and validation tool (UI)
* Transactions - Commit, Rollback, resource blocks so transactions don't accidentally get left open
* TQ can output query details as they are executed. These details include - sql, query duration, result size, exception, isolation level, transaction id
* Set transaction to be only one used on current thread. (Great for finding potential dead locks in code)

##Examples
###Select Query
```C#
User.Table userTable = User.Table.INSTANCE;
Order.Table orderTable = Order.Table.INSTANCE;
 
Sql.Function.CountAll count = new Sql.Function.CountAll();
 
Sql.IResult result = Sql.Query
    .Select(userTable.Id, count)
    .From(userTable)
    .Join(orderTable, userTable.Id == orderTable.UserId)
    .GroupBy(userTable.Id)
    .Having(count > 1)
    .Execute();
 
for (int index = 0; index < result.Count; index++) {
    int userId = userTable.Id[index, result];
    int? countValue = count[index, result];
}
```
###Insert Query
```C#
Table table = Table.INSTANCE;
 
using (Sql.Transaction transaction = new Sql.Transaction()) {
 
    Sql.Query.InsertInto(table)
        .Set(table.FirstName, "jo")
        .Set(table.LasName, "smith")
        .Execute(transaction);
 
    transaction.Commit();
}
```
###Nested Query
```C#
User.Table userTable = User.Table.INSTANCE;
Order.Table orderTable = Order.Table.INSTANCE;
 
Sql.Query.Select(userTable.Id)
    .From(userTable)
    .Where(
        userTable.Id.In(
            Sql.Query.Select(orderTable.UserId)
            .From(orderTable))
     ).Execute();
```

##Licensing
TQ is released under the GNU Lesser General Public License (LGLP) version 3 license. This means the library can be used by both open and closed source applications.
