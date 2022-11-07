using NewTDBMS.Domain.Entities;

namespace NewTDBMS.Service;

public interface ITDBMSService
{
	public IEnumerable<string> GetDBNames();

	public DB GetDB(string dBName);

	public void CreateDB(string dBName);

	public void DeleteDB(string dBName);

	public bool DBExists(string dBName);

	public IEnumerable<string> GetTableNames(string dBName);

	public IEnumerable<Table> GetTables(string dBName);

	public Table GetTable(string dBName, string tableName);

	public void CreateTable(string dBName, Table table);

	public void DeleteTable(string dBName, string tableName);

	public bool TableExists(string dBName, string tableName);

	public IEnumerable<Row> GetRows(string dBName, string tableName);

	public void AddRow(string dBName, string tableName, Row row);

	public void UpdateRow(string dBName, string tableName, Row row);

	public void DeleteRow(string dBName, string tableName, int rowId);

	public bool RowExists(string dBName, string tableName, int rowId);

	public Column GetColumn(string dBName, string tableName, string columnName);

	public void RenameColumn(string dBName, string tableName, string oldName, string newName);

	public void CreateColumn(string dBName, string tableName, Column column);

	public bool ColumnExists(string dBName, string tableName, string columnName);
}
