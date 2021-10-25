using System;

namespace Expressive.Exceptions
{
	[Serializable]
	public sealed class ExpressiveException : Exception
	{
		internal ExpressiveException(string message) : base(message)
		{
		}

		internal ExpressiveException(Exception innerException) : base(innerException.Message, innerException)
		{
		}
	}
}
