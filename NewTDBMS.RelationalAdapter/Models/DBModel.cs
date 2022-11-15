using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTDBMS.RelationalAdapter.Models;

public class DBModel
{
	public string Name { get; set; }

	public virtual ICollection<TableModel> Tables { get; set; } = new List<TableModel>();
}
