using System;

public class GGAppleSignIn : BehaviourSingletonInit<GGAppleSignIn>
{
	public override void Init()
	{
		this.signInProvider = new IAppleSignInProvider();
	}

	public bool isAvailable
	{
		get
		{
			return this.signInProvider.isAvailable;
		}
	}

	public void SignIn(Action<IAppleSignInProvider.SignInResponse> onComplete)
	{
		this.signInProvider.SignIn(onComplete);
	}

	private IAppleSignInProvider signInProvider;
}
