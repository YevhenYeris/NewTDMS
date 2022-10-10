using NewTDBMS.Domain.Entities;
using NewTDBMS.Service;
using System.Diagnostics.Metrics;
using System.Xml;
using System.Xml.Linq;

namespace NewTDBMS.LocalAdapter;

public class LocalDBContext : IDBContext
{
	private readonly string _dbPathsFile = "./databases/allpaths.txt";

	private int _dBsCount = 0;

	private Dictionary<string, string> _databases = new Dictionary<string, string>();

	public LocalDBContext()
	{
		ReadDBPaths();
	}

	public List<DB> DBs { get; set; } = new List<DB>();

	private void ReadDBPaths()
	{
		if (!File.Exists(_dbPathsFile))
		{
			File.CreateText(_dbPathsFile);
			//File.AppendAllText(_dbPathsFile, "0");
			return;
		}

		using(StreamReader file = new StreamReader(_dbPathsFile))
		{
			//var count = int.Parse(file.ReadLine() ?? "0");
			//_dBsCount = count;

			string ln = string.Empty;
			while ((ln = file.ReadLine()) is not null)
			{  
				var dBName = ln.Substring(0, ln.IndexOf(" "));
				var dBPath = ln.Substring(ln.IndexOf(" ") + 1);
				_databases[dBName] = dBPath;
				++_dBsCount;
			}  
		}
	}

	private void SavePaths()
	{
		if (!File.Exists(_dbPathsFile))
		{
			File.CreateText(_dbPathsFile);
			//File.AppendAllText(_dbPathsFile, "0");
			return;
		}

		using (StreamWriter file = new StreamWriter(_dbPathsFile))
		{
			//file.WriteLine($"{_dBsCount}");

			foreach(var item in _databases)
			{
				file.WriteLine($"{item.Key} {item.Value}");
			}
		}
	}

	public DB LoadDB(string dBName)
	{
		if (!_databases.ContainsKey(dBName)) throw new ArgumentException(dBName);

		using (TextReader file = new StreamReader(_databases[dBName]))
		{
			XElement dBEl = XElement.Load(file);

			var dB = new DB();
			dB.Name = (string)dBEl.Element("Name");

			dB.Tables = new List<Table>();
			var tables = from table in dBEl.Descendants("Table")
						 select new Table() { Name = (string)table.Element("Name"),
											  Columns = (from column in table.Descendants("Column")
														select new Column() { Name = (string)column.Element("Name"),
																			  Type = (string)column.Element("Type")}).ToList(),
											  Rows = (from row in table.Descendants("Row")
													  select new Row() { Id = (int)row.Element("Id"),
																		 Values = (from val in row.Descendants("Item")
																				   select (string)val).ToList()}).ToList()};
			
			dB.Tables = tables.ToList();
			DBs.Add(dB);

			return dB;
		}
	}

	public void SaveDBs()
	{
		//XElement dBs = new XElement("DBs");

		foreach (var dB in DBs)
		{
			SaveDB(dB.Name);
		}

		SavePaths();
	}

	public void SaveDB(string dBName)
	{
		var dB = DBs.Where(db => db.Name == dBName).FirstOrDefault();
		if (dB is null) return;

		var path = _databases.ContainsKey(dB.Name) ? _databases[dB.Name] : String.Empty;

			if (!path.Any())
			{
				path = $"./databases/{_dBsCount}.xml";
				_databases.Add(dB.Name, path);
			}

			var dBEl = new XElement("DB",
						new XElement("Name", dB.Name)/*,
						new XElement("Tables")*/);

			foreach(var table in dB.Tables)
			{
				var tableEl = new XElement("Table",
											new XElement("Name", table.Name)/*,
											new XElement("Columns"),
											new XElement("Rows")*/);

				foreach (var column in table.Columns)
				{
					tableEl/*.Element("Columns")?*/.Add(new XElement("Column",
															new XElement("Name", column.Name),
															new XElement("Type", column.Type)));
				}

				foreach (var row in table.Rows)
				{
					var rowEl = new XElement("Row", new XElement("Id", row.Id)/*,
											 new XElement("Items")*/);

					foreach (var item in row.Values)
					{
						rowEl/*.Element("Items")?*/.Add(new XElement("Item", item));
					}

					tableEl/*.Element("Rows")?*/.Add(rowEl);
				}

				dBEl/*.Element("Tables")?*/.Add(tableEl);
			}

			//dBs.Element("DBs")?.Add(dBEl);
			//File.Create(path);
			dBEl.Save(path);

			SavePaths();
	}

	public bool IsDBLoaded(string dBName)
	{
		return DBs.Where(dB => dB.Name == dBName).Any();
	}

	public bool DBExists(string dBName)
	{
		return _databases.ContainsKey(dBName);
	}

	public IEnumerable<string> GetDBNames()
	{
		return _databases.Keys;
	}

	public void DeleteDB(string dBName)
	{
		if (!_databases.ContainsKey(dBName)) return;

		//DBs.Remove(DBs.Where(dB => dB.Name == dBName).FirstOrDefault());

		_databases.Remove(dBName);
	}
}
