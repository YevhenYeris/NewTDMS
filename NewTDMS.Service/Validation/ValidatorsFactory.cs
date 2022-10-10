using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewTDBMS.Service.Validation
{
	public class ValidatorsFactory
	{
		public static Predicate<string> CreateValidator(string type)
		{
			var moneyRegex = @"^[0-9]{1,13}(\.[0-9]{1,2})*|(10000000000000)(\.00)*$";

			return type.ToUpper() switch
			{
				"STRING" => (string a) => true,
				"INTEGER" => (string a) => Int32.TryParse(a, out _),
				"REAL" => (string a) => Double.TryParse(a, out _),
				"CHAR" => (string a) => Char.TryParse(a, out _),
				"$" => (string a) => Regex.Match(a, moneyRegex).Value.Equals(a),
				"$Invl" => (string a) => Regex.Matches(a, $"{moneyRegex}-{moneyRegex}").Equals(a),
				_ => (string a) => false
			};
		}
	}
}
