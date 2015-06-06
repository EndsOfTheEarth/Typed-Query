
0.91 Changes
Added truncate query
Force transaction on thread
More parameters on query performed event
Refactored query interface classes
Implemented ExecuteReturnGeneratedKeys() on insert query for sql server
Definition checker working for postgreSql
IQueryResult is now called IResult

0.92 Changes
Added database class code generator
Added JoinIf, LeftJoinIf and RightJoinIf query syntax
Fixed default value issue on new rows
Table generator now includes schema name in code

0.93 Changes
Added Float, NFloat, Double and NDouble columns
Added table generator support for DateTime2 and DateTimeOffset fields
Bug fix - Cast exception on BigIntSum and BigIntAvg running on PostgreSql
Added include schema option to table generator
Added % (Mod) operator to some fields
Added Query Executing event that fires when a query first begins
Fixed issue where Left Joins threw an exception

0.94 Changes
Added new stored procedure support
Added RETURNING syntax to insert, update and delete queries
Added DateTimeOffset column support
Added aggregate date functions for - Year, Month, DayOfMonth, Hour, Minute, Second

0.95
Added window function syntax support including new Row_Number(), Rank() and Dense_Rank() functions
Removed ExecuteReturnGeneratedKeys(...) Syntax. Noew replaced by Returning(...) syntax.

0.96
Added read only connection security feature so that select queries that run outside of a trasaction are run in a read only query if the database class implements this feature. See documentation.
Changed QueryExecuting and QueryPerformed events to pass the database class being used in the executing query.
Added Key Column Support
Added support for table and column comments
Added static And(...) and Or(...) methods to Condition class
Fixed a number of issues with the table definition generator.
Fixed bug where calling Update(...) on a row more than once causes an exception.
Fixed row update rules to stop rows getting into an invalid state after roll backs and multiple updates within a single transaction.

0.97
Added feature to output enum values when generating column schema meta data. 
Added support for user permissions meta data.
Added a set of settings to automatically stop on a breakpoint when a query is being executed in debug mode.
Change to make table aliasing more scalable.