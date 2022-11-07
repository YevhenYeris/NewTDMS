using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTDBMS.RelationalAdapter.Models;

public class RowModel
{
	public int Id { get; set; }

	public string DBName { get; set; }
	
	public string TableName { get; set; }

	public List<RowValueModel> Values { get; set; }
	}
