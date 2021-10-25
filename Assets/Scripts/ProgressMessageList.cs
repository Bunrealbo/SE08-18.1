using System;
using System.Collections.Generic;

public class ProgressMessageList
{
	public ProgressMessageList.ProgressChange progressChange;

	public List<string> messageList;

	public class ProgressChange
	{
		public float fromProgress;

		public float toProgress;
	}
}
