using System;

namespace Expressive.Exceptions
{
	[Serializable]
	public sealed class MissingParticipantException : Exception
	{
		internal MissingParticipantException(string message) : base(message)
		{
		}
	}
}
