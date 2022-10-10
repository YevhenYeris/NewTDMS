﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTDBMS.Domain.Entities;

	public class Table
	{
		public string Name { get; set; }

		public List<Column> Columns { get; set; }

		public List<Row> Rows { get; set; }
	}
