using NewTDBMS.LocalAdapter;
using NewTDBMS.Domain.Entities;

Console.WriteLine("Hello, World!");

LocalDBContext localDBContext = new LocalDBContext();

localDBContext.DBs = new List<DB>();
DB dB = new DB() { Name = "NewDB" };
localDBContext.DBs.Add(dB);

dB.Tables = new List<Table>();
Table table = new Table() { Name = "NewTable" };
dB.Tables.Add(table);

table.Columns = new List<Column>();
Column column = new Column() { Name = "NewColumn", Type = "NewType" };
table.Columns.Add(column);

/*table.Rows = new List<Row>();
Row row = new Row() { Id = 0 };
row.Values = new List<string>() { "NewValue" };
Row row1 = new Row() { Id = 1 };
row1.Values = new List<string>() { "NewValue1" };
table.Rows.Add(row);
table.Rows.Add(row1);*/

localDBContext.SaveDBs();

var a = localDBContext.LoadDB("NewDB");