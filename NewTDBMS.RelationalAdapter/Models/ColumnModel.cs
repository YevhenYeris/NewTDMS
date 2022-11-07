using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTDBMS.RelationalAdapter.Models;

public class ColumnModel
{
	public string Name { get; set; }

	public string DBName { get; set; }

	public string TableName { get; set; }

	public string Type { get; set; }
}
