using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTDBMS.RelationalAdapter.Models;

	public class TableModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string DBName { get; set; }

		public List<ColumnModel> Columns { get; set; }

		public List<RowModel> Rows { get; set; }
	}
