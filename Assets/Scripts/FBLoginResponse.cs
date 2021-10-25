using System;

[Serializable]
public class FBLoginResponse
{
	public bool isError
	{
		get
		{
			return GGUtil.HasText(this.error);
		}
	}

	public string error;

	public string user_id;
}
