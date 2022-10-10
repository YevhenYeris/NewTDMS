using NewTDBMS.Domain.Entities;

namespace NewTDBMS.Service;

public interface IDBContext
{
	public List<DB> DBs { get; set; }

	public DB LoadDB(string dBName);

	public void SaveDBs();

	public void SaveDB(string dBName);

	public bool IsDBLoaded(string dBName);

	public bool DBExists(string dBName);

	public IEnumerable<string> GetDBNames();

	public void DeleteDB(string dBName);
}
