using System;

public class IFacebookProvider
{
	public virtual void Login(FBLoginParams loginParams)
	{
	}

	public virtual bool IsInitialized()
	{
		return false;
	}
}
