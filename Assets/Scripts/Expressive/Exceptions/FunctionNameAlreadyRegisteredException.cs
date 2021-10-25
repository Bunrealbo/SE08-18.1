using System;
using System.Runtime.Serialization;

namespace Expressive.Exceptions
{
	[Serializable]
	public sealed class FunctionNameAlreadyRegisteredException : Exception
	{
		public string Name
		{
			get
			{
				return this._003CName_003Ek__BackingField;
			}
			private set
			{
				this._003CName_003Ek__BackingField = value;
			}
		}

		internal FunctionNameAlreadyRegisteredException(string name) : base("A function has already been registered '" + name + "'")
		{
			this.Name = name;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Name", this.Name);
		}

		private string _003CName_003Ek__BackingField;
	}
}
