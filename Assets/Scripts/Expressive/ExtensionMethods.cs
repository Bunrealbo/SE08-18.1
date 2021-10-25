using System;
using System.Collections.Generic;

namespace Expressive
{
	internal static class ExtensionMethods
	{
		internal static bool IsArithmeticOperator(string source)
		{
			return string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "-", StringComparison.Ordinal) || string.Equals(source, "−", StringComparison.Ordinal) || string.Equals(source, "/", StringComparison.Ordinal) || string.Equals(source, "÷", StringComparison.Ordinal) || string.Equals(source, "*", StringComparison.Ordinal) || string.Equals(source, "×", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal);
		}

		internal static bool IsNumeric(string source)
		{
			double num;
			return double.TryParse(source, out num);
		}

		internal static T PeekOrDefault<T>(Queue<T> queue)
		{
			if (queue.Count <= 0)
			{
				return default(T);
			}
			return queue.Peek();
		}

		internal static string SubstringUpTo(string source, int startIndex, char character)
		{
			if (startIndex != 0)
			{
				string text = source.Substring(startIndex);
				return text.Substring(0, text.IndexOf(character) + 1);
			}
			return source.Substring(startIndex, source.IndexOf(character) + 1);
		}
	}
}
