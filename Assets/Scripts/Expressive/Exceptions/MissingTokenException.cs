using System;
using System.Runtime.Serialization;

namespace Expressive.Exceptions
{
	[Serializable]
	public sealed class MissingTokenException : Exception
	{
		public char MissingToken
		{
			get
			{
				return this._003CMissingToken_003Ek__BackingField;
			}
			private set
			{
				this._003CMissingToken_003Ek__BackingField = value;
			}
		}

		internal MissingTokenException(string message, char missingToken) : base(message)
		{
			this.MissingToken = missingToken;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("MissingToken", this.MissingToken);
		}

		private char _003CMissingToken_003Ek__BackingField;
	}
}
