using NewTDBMS.Domain.Entities;

namespace NewTDBMS.Service.Validation;

public class Validator
{
	public static bool ValidateRow(Table table, Row row)
	{
		if (table.Columns.Count != row.Values.Count) return false;

		bool isValid = true;

		for(int i = 0; i < table.Columns.Count; ++i)
		{
			var validate = ValidatorsFactory.CreateValidator(
				table.Columns.ElementAt(i).Type);

			if (!(isValid &= validate(row.Values[i]))) return false;
		}

		return true;
	}

	public static bool ValidateTable(Table table)
	{
		foreach (Row row in table.Rows)
		{
			if (!Validator.ValidateRow(table, row)) return false;
		}

		return true;
	}
}
