using System;
using System.Runtime.Serialization;

namespace Expressive.Exceptions
{
	[Serializable]
	public sealed class UnrecognisedTokenException : Exception
	{
		public string Token
		{
			get
			{
				return this._003CToken_003Ek__BackingField;
			}
			private set
			{
				this._003CToken_003Ek__BackingField = value;
			}
		}

		internal UnrecognisedTokenException(string token) : base("Unrecognised token '" + token + "'")
		{
			this.Token = token;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Token", this.Token);
		}

		private string _003CToken_003Ek__BackingField;
	}
}
