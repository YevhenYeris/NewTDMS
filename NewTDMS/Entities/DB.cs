using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTDBMS.Domain.Entities;

	public class DB
	{
		public string Name { get; set; }

		public List<Table> Tables { get; set; } = new List<Table>();
	}
