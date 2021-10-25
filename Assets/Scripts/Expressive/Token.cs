using System;

namespace Expressive
{
	internal sealed class Token
	{
		internal string CurrentToken
		{
			get
			{
				return this._003CCurrentToken_003Ek__BackingField;
			}
			private set
			{
				this._003CCurrentToken_003Ek__BackingField = value;
			}
		}

		internal int Length
		{
			get
			{
				return this._003CLength_003Ek__BackingField;
			}
			private set
			{
				this._003CLength_003Ek__BackingField = value;
			}
		}

		internal int StartIndex
		{
			get
			{
				return this._003CStartIndex_003Ek__BackingField;
			}
			private set
			{
				this._003CStartIndex_003Ek__BackingField = value;
			}
		}

		public Token(string currentToken, int startIndex)
		{
			this.CurrentToken = currentToken;
			this.StartIndex = startIndex;
			string currentToken2 = this.CurrentToken;
			this.Length = ((currentToken2 != null) ? currentToken2.Length : 0);
		}

		private string _003CCurrentToken_003Ek__BackingField;

		private int _003CLength_003Ek__BackingField;

		private int _003CStartIndex_003Ek__BackingField;
	}
}
