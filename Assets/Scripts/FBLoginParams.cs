using System;

public class FBLoginParams
{
	public Action<FBLoginResponse> onComplete;

	public bool isPublishPermissionLogin;

	public string scope;
}
