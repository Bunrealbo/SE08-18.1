using System;
using System.Collections;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using Facebook.Unity;

public class LoginToFBButton : MonoBehaviour
{
    public List<object> fbFriendList;
    public Dictionary<string, string> FriendsDict;
    public FacebookPlayer fbPlayer { get; set; }

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    Debug.LogError("Couldn't initialize");
                }
            },

            isGameShown =>
            {
                if (!isGameShown)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void FBLogin()
    {
        List<string> permissions = new List<string>()
        {
            "public_profile"
        };
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    public void GetFriends()
    {
        FB.API("/me/friends", HttpMethod.GET, GetFriendsPlayingThisGame);
    }

    public void GetFriendsPlayingThisGame(IResult result)
    {
        IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
        fbFriendList = (List<object>)dict["data"];
        FriendsDict = new Dictionary<string, string>();
        for (int i = 0; i < fbFriendList.Count; ++i)
        {
            string frId = ((Dictionary<string, object>)fbFriendList[i])["id"].ToString();
            string frName = ((Dictionary<string, object>)fbFriendList[i])["name"].ToString();
            FriendsDict.Add(frId, frName);
        }
        FB.API("/me?fields=id,name", HttpMethod.GET, SetFacebookPlayer);
    }

    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Facebook is Logged in");
            }
            else
            {
                Debug.Log("Facebook is not Logged in");
            }
            GetDataFromFacebook(FB.IsLoggedIn);
        }
    }

    void GetDataFromFacebook(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            GetFriends();
        }
    }

    void SetFacebookPlayer(IResult result)
    {
        if (result.Error == null)
        {
            IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
            try
            {
                string fbPlayerId= dict["id"].ToString();
                string fbPlayerName = dict["name"].ToString();
                this.fbPlayer = new FacebookPlayer(fbPlayerId, fbPlayerName, FriendsDict);
                // Test: print to Console
                Debug.Log(this.fbPlayer.ToString());
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    FacebookPlayer GetFacebookPlayer()
    {
        return this.fbPlayer;
    }

    //public void ButtonCallback_OnButtonPress()
    //{
    //	LoginToFBButton._003C_003Ec__DisplayClass0_0 _003C_003Ec__DisplayClass0_ = new LoginToFBButton._003C_003Ec__DisplayClass0_0();
    //	_003C_003Ec__DisplayClass0_._003C_003E4__this = this;
    //	_003C_003Ec__DisplayClass0_.nav = NavigationManager.instance;
    //	if (BehaviourSingletonInit<GGAppleSignIn>.instance.isAvailable)
    //	{
    //		_003C_003Ec__DisplayClass0_.nav.GetObject<LoginProviderDialog>().Show(new Action<LoginProviderDialog.LoginResponse>(_003C_003Ec__DisplayClass0_._003CButtonCallback_OnButtonPress_003Eb__0));
    //		return;
    //	}
    //	this.LoginToFacebook();
    //}

    //private void LoginToFacebook()
    //{
    //	GGFacebook instance = BehaviourSingletonInit<GGFacebook>.instance;
    //	if (!instance.IsInitialized())
    //	{
    //		UnityEngine.Debug.Log("FACEBOOK NOT INITIALIZED");
    //		return;
    //	}
    //	instance.Login(new FBLoginParams
    //	{
    //		scope = "public_profile",
    //		onComplete = new Action<FBLoginResponse>(this.OnLoginComplete)
    //	});
    //}

    //private void LoginToApple()
    //{
    //	GGAppleSignIn instance = BehaviourSingletonInit<GGAppleSignIn>.instance;
    //	if (!instance.isAvailable)
    //	{
    //		UnityEngine.Debug.Log("APPLE NOT AVAILABLE");
    //		return;
    //	}
    //	instance.SignIn(new Action<IAppleSignInProvider.SignInResponse>(this.OnAppleLoginComplete));
    //}

    //private void OnAppleLoginComplete(IAppleSignInProvider.SignInResponse response)
    //{
    //	UnityEngine.Debug.LogFormat("Cancelled: {0}, UserId: {1}, Error: {2}", new object[]
    //	{
    //		response.cancelled,
    //		response.user_id,
    //		response.error
    //	});
    //	bool flag = GGUtil.HasText(response.user_id);
    //	if (response.isError)
    //	{
    //		UnityEngine.Debug.Log("ERROR: " + response.error);
    //	}
    //	if (!flag)
    //	{
    //		return;
    //	}
    //	this.LoginWithAppleId(response.user_id);
    //}

    //private void LoginWithAppleId(string userId)
    //{
    //	NavigationManager.instance.GetObject<SyncGameScreen>().LoginToApple(userId);
    //}

    //private void OnLoginComplete(FBLoginResponse response)
    //{
    //	if (ConfigBase.instance.debug)
    //	{
    //		UnityEngine.Debug.Log("Login Complete " + response.user_id);
    //	}
    //	bool flag = GGUtil.HasText(response.user_id);
    //	if (response.isError)
    //	{
    //		UnityEngine.Debug.Log("FACEBOK ERROR: " + response.error);
    //	}
    //	if (!flag)
    //	{
    //		return;
    //	}
    //	this.LoginWithFacebookId(response.user_id);
    //}

    //private void LoginWithFacebookId(string facebookUserId)
    //{
    //	NavigationManager.instance.GetObject<SyncGameScreen>().LoginToFacebook(facebookUserId);
    //}

    //private sealed class _003C_003Ec__DisplayClass0_0
    //{
    //	internal void _003CButtonCallback_OnButtonPress_003Eb__0(LoginProviderDialog.LoginResponse response)
    //	{
    //		this.nav.Pop(true);
    //		if (response.isCancelled)
    //		{
    //			return;
    //		}
    //		if (response.loginProvider == LoginProvider.FacebookLogin)
    //		{
    //			this._003C_003E4__this.LoginToFacebook();
    //			return;
    //		}
    //		if (response.loginProvider == LoginProvider.AppleLogin)
    //		{
    //			this._003C_003E4__this.LoginToApple();
    //		}
    //	}

    //	public NavigationManager nav;

    //	public LoginToFBButton _003C_003E4__this;
    //}
}
