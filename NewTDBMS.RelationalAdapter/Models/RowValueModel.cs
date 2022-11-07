namespace NewTDBMS.RelationalAdapter.Models;

public class RowValueModel
{
	public string DBName { get; set; }

	public string TableName { get; set; }

	public int RowId { get; set; }

	public string Value { get; set; }
}
