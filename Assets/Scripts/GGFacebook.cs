using System;

public class GGFacebook : BehaviourSingletonInit<GGFacebook>
{
	public override void Init()
	{
		this.facebook = new FacebookAndroidPlugin();
	}

	public void Login(FBLoginParams loginParams)
	{
		this.facebook.Login(loginParams);
	}

	public bool IsInitialized()
	{
		return this.facebook.IsInitialized();
	}

	private IFacebookProvider facebook;
}
