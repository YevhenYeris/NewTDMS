using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
				"$INVL" => (string a) => ValidateMoneyInvl(a),
				_ => (string a) => false
			};
		}

		private static bool ValidateMoneyInvl(string invl)
		{
			try
			{
				var values = invl.Split('-').Select(v => v.Trim()).ToList();
				var validator = CreateValidator("$");

				if (!validator(values[0]) || !validator(values[1])) return false;

				var firstPart = values[0].Split('.');
				var secondPart = values[1].Split('.');
				var firstInt = BigInteger.Parse(firstPart[0]);
				var secondInt = BigInteger.Parse(secondPart[0]);
				var firstDecimal = Decimal.Parse(firstPart.ElementAtOrDefault(1) ?? "0");
				var secondDecimal = Decimal.Parse(secondPart.ElementAtOrDefault(1) ?? "0");

				return firstInt < secondInt ||
					   firstInt == secondInt && firstDecimal < secondDecimal;
			}
			catch
			{
				return false;
			}
		}
	}
}
