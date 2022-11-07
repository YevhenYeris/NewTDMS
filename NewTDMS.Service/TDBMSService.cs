using NewTDBMS.Domain.Entities;
using NewTDBMS.Service.Validation;

namespace NewTDBMS.Service;

public class TDBMSService : ITDBMSService
{
	private IDBContext _dbContext;

	public TDBMSService(IDBContext dbContext)
	{
		_dbContext = dbContext;
	}

	public IEnumerable<string> GetDBNames()
	{
		return _dbContext.GetDBNames();
	}

	public DB GetDB(string dBName)
	{
		var dB = _dbContext.DBs.Where(dB => dB.Name == dBName).FirstOrDefault();

		if (dB is null) dB = _dbContext.LoadDB(dBName);

		return dB;
	}

	public void CreateDB(string dBName)
	{
		var dB = new DB() { Name = dBName };

		_dbContext.DBs.Add(dB);
		
		_dbContext.SaveDB(dBName);
	}

	public void DeleteDB(string dBName)
	{
		_dbContext.DeleteDB(dBName);
		_dbContext.SaveDBs();
	}

	public bool DBExists(string dBName)
	{
		return _dbContext.DBExists(dBName);
	}

	public IEnumerable<string> GetTableNames(string dBName)
	{
		var dB = GetDB(dBName);
		
		var tables = dB?.Tables.Select(table => table.Name);

		return tables ?? new List<string>();
	}

	public IEnumerable<Table> GetTables(string dBName)
	{
		var dB = GetDB(dBName);

		return dB?.Tables;
	}

	public Table GetTable(string dBName, string tableName)
	{
		var dB = GetDB(dBName);

		return dB?.Tables.Where(table => table.Name == tableName).FirstOrDefault();
	}

	public void CreateTable(string dBName, Table table)
	{
		if (!Validator.ValidateTable(table)) throw new ValidationException();

		var dB = GetDB(dBName);

		if (dB is null) return;

		dB.Tables.Add(table);
		_dbContext.SaveDB(dBName);
	}

	public void DeleteTable(string dBName, string tableName)
	{
		var dB = GetDB(dBName);

		dB.Tables.Remove(dB?.Tables.Where(table => table.Name == tableName).FirstOrDefault());

		_dbContext.SaveDB(dBName);
	}

	public bool TableExists(string dBName, string tableName)
	{
		if (!DBExists(dBName)) return false;

		return GetTable(dBName, tableName) is not null;
	}

	public Column GetColumn(string dBName, string tableName, string columnName)
	{
		var table = GetTable(dBName, tableName);

		return table?.Columns.Where(column => column.Name == columnName).FirstOrDefault();
	}

	public void RenameColumn(string dBName, string tableName, string oldName, string newName)
	{
		var table = GetTable(dBName, tableName);
		var column = table.Columns.Where(column => column.Name == oldName).FirstOrDefault();

		if (table is null || column is null) return;

		column.Name = newName;
		
		_dbContext.SaveDB(dBName);
	}

	public void CreateColumn(string dBName, string tableName, Column column)
	{
		if (ColumnExists(dBName, tableName, column.Name)) return;

		var table = GetTable(dBName, tableName);
		table.Columns.Add(column);
		table.Rows.ForEach(r => r.Values.Add(string.Empty));

		_dbContext.SaveDB(dBName);
	}

	public bool ColumnExists(string dBName, string tableName, string columnName)
	{
		if (!TableExists(dBName, tableName)) return false;

		return GetColumn(dBName, tableName, columnName) is not null;
	}

	public IEnumerable<Row> GetRows(string dBName, string tableName)
	{
		var table = GetTable(dBName, tableName);

		return table.Rows;
	}

	public void AddRow(string dBName, string tableName, Row row)
	{		
		var table = GetTable(dBName, tableName);

		if (!Validator.ValidateRow(table, row)) throw new ValidationException();

		if (table is null) return;

		table.Rows.Add(row);
		_dbContext.SaveDB(dBName);
	}

	public void UpdateRow(string dBName, string tableName, Row row)
	{
		if (!RowExists(dBName, tableName, row.Id)) return;

		var table = GetTable(dBName, tableName);

		if (!Validator.ValidateRow(table, row)) throw new ValidationException();;

		var oldRow = table.Rows.Where(r => r.Id == row.Id).FirstOrDefault();

		row.Values.ForEach(v => oldRow.Values.Add(v));
	}

	public void DeleteRow(string dBName, string tableName, int rowId)
	{
		var table = GetTable(dBName, tableName);

		if (table is null || !table.Rows.Where(row => row.Id == rowId).Any())
			return;

		table.Rows.Remove(table.Rows.Where(row => row.Id == rowId).FirstOrDefault());

		_dbContext.SaveDB(dBName);
	}

	public bool RowExists(string dBName, string tableName, int rowId)
	{
		if (!TableExists(dBName, tableName)) return false;

		var table = GetTable(dBName, tableName);

		return table.Rows.Where(row => row.Id == rowId).Any();
	}
}
