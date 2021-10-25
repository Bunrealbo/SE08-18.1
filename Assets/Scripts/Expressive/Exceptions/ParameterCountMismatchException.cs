using System;

namespace Expressive.Exceptions
{
	[Serializable]
	public sealed class ParameterCountMismatchException : Exception
	{
		internal ParameterCountMismatchException(string message) : base(message)
		{
		}
	}
}
