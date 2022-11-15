using NewTDBMS.Domain.Entities;
using NewTDBMS.RelationalAdapter.Models;
using NewTDBMS.Service;

namespace NewTDBMS.RelationalAdapter;

public class RelationalDBContext : IDBContext
{
	private TDBMSContext _context;

	public RelationalDBContext(TDBMSContext context)
	{
		_context = context;
		_dBs = new List<DB>();
	}

	//TODO: Use the same logic as at LocalContext (maintain the collection)
	private List<DB> _dBs;
	public List<DB> DBs { get => _dBs; set => throw new NotImplementedException(); }

	public bool DBExists(string dBName)
	{
		return _context.DBs.Any(db => db.Name == dBName);
	}

	public void DeleteDB(string dBName)
	{
		//var dB = _context.DBs.Where(db => db.Name == dBName).FirstOrDefault();
		//_context.DBs.Remove(dB);
		DBs.Remove(DBs.Where(dB => dB.Name == dBName).FirstOrDefault());
	}

	public IEnumerable<string> GetDBNames()
	{
		return _context.DBs.Select(dB => dB.Name);
	}

	public bool IsDBLoaded(string dBName)
	{
		return DBs.Where(dB => dB.Name == dBName).Any();
	}

	public DB LoadDB(string dBName)
	{
		var dBModel = _context.DBs.Where(dB => dB.Name == dBName).FirstOrDefault();
		var tables = _context.Tables.Where(t => t.DBName == dBModel.Name);
		var dB = new DB { Name = dBModel.Name, Tables = dBModel.Tables.Select(t => new Table { Name = t.Name }).ToList() };
		dB.Tables = tables.Select(t => new Table() { Name = t.Name }).ToList();

		foreach (var table in dB.Tables)
		{
			var columns = _context.Columns.Where(c => c.DBName == dB.Name && c.TableName == table.Name);
			var rows = _context.Rows.Where(r => r.DBName == dB.Name && r.TableName == table.Name);

			table.Columns = columns.Select(c => new Column() { Name = c.Name, Type = c.Type, Id = c.Id }).ToList();
			table.Rows = rows.Select(r => new Row() { Id = r.Id }).ToList();

			foreach (var row in table.Rows)
			{
				var values = _context.RowValues.Where(v => v.RowId == row.Id && v.DBName == dB.Name && v.TableName == table.Name);
				row.Values = values.Select(v => v.Value).ToList();
			}
		}

		DBs.Add(dB);
		return dB;
	}

	public void SaveDB(string dBName)
	{
		if (!DBs.Where(dB => dB.Name == dBName).Any()) return;

		if (_context.DBs.Where(dB => dB.Name == dBName).Any())
		{
			UpdateDB(dBName);
		}
		else
		{
			AddDB(dBName);
		}

		_context.SaveChanges();
		//SaveDBs();
	}

	private void AddDB(string dBName)
	{

	}

	private void UpdateDB(string dBName)
	{
		var dB = DBs.Where(dB => dB.Name == dBName).FirstOrDefault();
		var dBModel = _context.DBs.Where(d => d.Name == dBName).FirstOrDefault();

		dBModel.Name = dB.Name;
		foreach (var table in dB.Tables)
		{
			if (dBModel.Tables.Where(t => table.Name == table.Name).Any())
			{
				var tableModel = _context.Tables.Where(c => c.DBName == dB.Name && c.Name == table.Name).FirstOrDefault();
				tableModel.Name = table.Name;

				foreach(var column in table.Columns)
				{
					if (_context.Columns.Where(c => c.Id == column.Id && c.DBName == dB.Name && c.TableName == table.Name && c.Name == column.Name).Any())
					{
						var columnModel = _context.Columns.Where(c => c.Id == column.Id && c.DBName == dB.Name && c.TableName == table.Name && c.Name == column.Name).FirstOrDefault();
						columnModel.Name = column.Name;
						columnModel.Type = column.Type;
					}
					else
					{
						var columnModel = new ColumnModel { DBName = dB.Name, TableName = table.Name, Name = column.Name, Type = column.Type };
						tableModel.Columns.Add(columnModel);
					}
				}

				foreach(var row in table.Rows)
				{
					if (_context.Rows.Where(r => r.DBName == dB.Name && r.TableName == table.Name && r.Id == row.Id).Any())
					{
						var rowModel = _context.Rows.Where(r => r.DBName == dB.Name && r.TableName == table.Name && r.Id == row.Id).FirstOrDefault();
						rowModel.Values = row.Values.Select(v => new RowValueModel { DBName = dB.Name, TableName = table.Name, Value = v }).ToList();
					}
					else
					{
						var rowModel = new RowModel { DBName = dB.Name, TableName = table.Name };
						rowModel.Values = row.Values.Select(v => new RowValueModel { DBName = dB.Name, TableName = table.Name, Value = v }).ToList();
						tableModel.Rows.Add(rowModel);
					}
				}
			}
			else
			{
				var tableModel = new TableModel { DBName = dB.Name, Name = table.Name };
				tableModel.Columns = table.Columns.Select(c => new ColumnModel
				{
					DBName = dB.Name,
					TableName = table.Name,
					Name = c.Name, 
					Type = c.Type }).ToList();

				tableModel.Rows = table.Rows.Select(r => new RowModel
				{
					DBName = dB.Name,
					TableName = table.Name,
					Values = r.Values.Select(v => new RowValueModel
					{ 
						DBName = dB.Name,
						TableName = table.Name,
						Value = v
					}).ToList()
				}).ToList();

				dBModel.Tables.Add(tableModel);
			}
		}
		//remove tables that dont exist
		_context.SaveChanges();
	}

	public void SaveDBs()
	{
		DBs.ForEach(dB => SaveDB(dB.Name));
		//TODO: Check for add or update
		/*foreach (var dB in DBs)
		{
			_context.DBs.Add(new DBModel { Name = dB.Name });
			foreach (var table in dB.Tables)
			{
				_context.Tables.Add(new TableModel { Name = table.Name, DBName = dB.Name });
				foreach (var column in table.Columns)
				{
					_context.Columns.Add(new ColumnModel { Name = column.Name, DBName = dB.Name, Type = column.Type });
				}
				foreach (var row in table.Rows)
				{
					_context.Rows.Add(new RowModel { DBName = dB.Name, TableName = table.Name });
					foreach (var value in row.Values)
					{
						_context.RowValues.Add(new RowValueModel { DBName = dB.Name, TableName = table.Name, Value = value });
					}
				}
			}
		}
		_context.SaveChanges();*/
	}
}
