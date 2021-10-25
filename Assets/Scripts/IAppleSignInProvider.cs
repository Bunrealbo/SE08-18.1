using System;

public class IAppleSignInProvider
{
	public virtual bool isAvailable
	{
		get
		{
			return false;
		}
	}

	public virtual void SignIn(Action<IAppleSignInProvider.SignInResponse> onComplete)
	{
	}

	[Serializable]
	public class SignInResponse
	{
		public bool isError
		{
			get
			{
				return GGUtil.HasText(this.error);
			}
		}

		public bool cancelled;

		public string error;

		public string user_id;
	}
}
