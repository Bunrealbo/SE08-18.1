using System;
using UnityEngine;
using UnityEngine.Audio;

public class ConfigBase : ScriptableObject
{
	public bool debug
	{
		get
		{
			return this.isDebug && (Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || this.isDebugOnDevice);
		}
	}

	public bool isFakePlayerIdOn
	{
		get
		{
			return Application.isEditor && this.fakePlayerId && !string.IsNullOrEmpty(this.playerId);
		}
	}

	public string printAppName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.appNameOverrideForPrint))
			{
				return this.appNameOverrideForPrint;
			}
			return this.appName;
		}
	}

	public string platformRateProvider
	{
		get
		{
			return this.rateProvider;
		}
	}

	public bool IsSyncEnabledInCurrentScene()
	{
		return string.IsNullOrEmpty(this.menuSceneName) || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == ConfigBase.instance.menuSceneName;
	}

	public string GetSuggestionUrl(string playerName, string appName, string pid = "")
	{
		string text = this.suggestionUrl;
		text = string.Concat(new string[]
		{
			text,
			"?player_name=",
			playerName,
			"&game=",
			appName
		});
		if (!string.IsNullOrEmpty(pid))
		{
			text = text + "&player_id=" + pid;
		}
		return text;
	}

	public string GetBugReportUrl(string playerName, string appName, string pid = "")
	{
		string text = this.bugReportUrl;
		text = string.Concat(new string[]
		{
			text,
			"?player_name=",
			playerName,
			"&game=",
			appName
		});
		if (!string.IsNullOrEmpty(pid))
		{
			text = text + "&player_id=" + pid;
		}
		return text;
	}

	public bool shouldShowAmazonAds
	{
		get
		{
			return !string.IsNullOrEmpty(this.amazonAppKey);
		}
	}

	public bool isProVersionEnabled
	{
		get
		{
			return this.isProVersion;
		}
	}

	public bool isProVersionAvailable
	{
		get
		{
			return this.proVersionPackage != null;
		}
	}

	public static ConfigBase instance
	{
		get
		{
			if (ConfigBase._instance == null)
			{
				ConfigBase._instance = (Resources.Load("Config", typeof(ConfigBase)) as ConfigBase);
				if (ConfigBase._instance.activeConfig != null)
				{
					ConfigBase configBase = Resources.Load(ConfigBase._instance.activeConfig, typeof(ConfigBase)) as ConfigBase;
					if (configBase != null)
					{
						ConfigBase._instance = configBase;
					}
				}
			}
			return ConfigBase._instance;
		}
	}

	public void SetAudioMixerValues(GGPlayerSettings playerSettings)
	{
		if (this.masterMixer == null)
		{
			return;
		}
		this.masterMixer.SetFloat("MusicVolume", (float)(playerSettings.isMusicOff ? -80 : 0));
		this.masterMixer.SetFloat("SfxVolume", (float)(playerSettings.isSoundFXOff ? -80 : 0));
	}

	public string GetMatchServerAppName()
	{
		if (this.returnSameMatchServerAppNameWhenDebug)
		{
			return this.matchServerApp;
		}
		return this.matchServerApp + (this.debug ? "testing" : "");
	}

	public bool GGOfficialUserEnabled
	{
		get
		{
			return this.GGOfficialUserEnabled_ && Application.isEditor;
		}
	}

	public GGGameName gameName;

	[SerializeField]
	private bool isDebug;

	public bool changeChipOnDevice;

	public bool isTestingSettingsScreen;

	public bool allowStockpiling;

	public bool overrideEnergyDurationOnlyIfBigger;

	[SerializeField]
	private bool isDebugOnDevice;

	[SerializeField]
	public bool useSinglePlayerSettings;

	public bool returnSameMatchServerAppNameWhenDebug;

	public bool showTotalEarningsOnWinningScreen;

	public bool useProfilePictureInMultiplayer;

	public bool useDeck;

	public bool useAddaptiveAIOnMultiplayer;

	public bool fakePlayerId;

	public string playerId;

	public bool useFakeFacebookPlayerId;

	public string fakeFacebookPlayerId;

	public bool useFakeApplePlayerId;

	public string fakeApplePlayerId;

	public string facebookAppId = "752983338151813";

	public string facebookAppPlayerSuffix = "";

	public string facebookLoginPermissions = "public_profile, user_friends";

	public string facebookMockResponse;

	public string facebookDisplayName = "";

	public string facebookTestAccessToken = "CAAKs1b0mBXIBAL30WYTgiDUD4Ok9fbZBGPxz76FWxcHZBGs1XqkWOgm9uOKxjtvuk3byE9WSNtwwXJy5rY7C3k7qZBLSClZAPRHkyilRmc0nWgRirGMFZBRkmJhzKt0YdOFpZAJmrJvtXRD22YUYiw0rWPj4up8h61MRVLTjXTrzuIdLVZBoLrxF8fKYmybFskoLdtalTn0TX3fAqPxZByd5";

	public string experimentsResourceName;

	public float cloudSyncTimeDelay = 30f;

	public float cloudSyncTimeDelayWhenRequestFails = 120f;

	public int maxSyncFrequency = 10;

	public Material adsMaterial;

	public bool secureCurrency;

	public string styleName;

	public string appNameOverrideForPrint;

	public string appName;

	public string inAppAdsName;

	public string matchServerUrl;

	public string matchServerApp;

	public int maxDisconnects = 2;

	public string rankingsServerUrl;

	public string rankingsApp;

	public string iosAppId;

	public string proVersionPackage;

	public bool usingProfileData;

	public string activeConfig;

	public bool usingNewPhoton;

	public string activeConfigIOS;

	public string activeConfigWinRT;

	public bool tournamentsOnlyAvailableInPro;

	public bool noWaitingInPro;

	public string leaderboardId;

	public bool isProVersion;

	public bool canUseRate;

	public bool gameCenterAvailable;

	[SerializeField]
	private string rateProvider;

	public bool verifyPlayInApp;

	public bool testingInAppPurchases;

	public bool useGiftiz;

	public bool allDifficultiesInPro;

	public bool canUseFacebook;

	public string menuSceneName = "Assets/Scenes/MenuSceneDemo.unity";

	public bool useGuestForNonLoggedInUsers;

	public bool showUpdatedPrivacyPolicyNotice;

	public int minVersionThatHasUpdatedPrivacyPolicy;

	public bool onlyShowNoticeIfUserLoggedIntoFacebook;

	public bool canChangeCuesMidGame;

	public bool updateSpinControlFromCueSpin;

	public ConfigBase.GGFileIOCloudSyncTypes cloudSyncType;

	public string gameCenterCategory;

	public string mopubId;

	public string suggestionUrl;

	public string bugReportUrl;

	public ConfigBase.SocialProvider socialProvider;

	public ConfigBase.InAppProvider inAppProvider;

	public string interstitialAdId;

	public string amazonAppKey;

	public bool noAdsOnPromotionDay;

	public int initialCoins = 100;

	public int initialStars = 3;

	public int promotionCoins;

	public string promotionStart;

	public string promotionEnd;

	public string promotionMessage;

	private static ConfigBase _instance;

	public int notificationLatestTime = 22;

	public int notificationEarliestTime = 9;

	public AudioMixer masterMixer;

	public int minAudioVal = -80;

	public int maxAudioVal;

	public int initialVolumeLevel = 50;

	public int initialPlayerVersion = 3;

	public string facebookError = "100";

	public float multiplayerSkillPointsScale = 1f;

	public float multiplayerSkillPointsOffset;

	public int coinsCap = 100000000;

	public int tokensCap = 100000;

	public int ggDollarsCap = 100000000;

	public int eloCap = 9999;

	public int freeCoins = 50;

	public int coinsForLike = 50;

	public bool hasLootBoxes;

	public CurrencyType freeCoinsCurrencyType = CurrencyType.diamonds;

	public bool overrideTimeForFreeCoins;

	public float freeCoinsTimeHours;

	[SerializeField]
	private bool GGOfficialUserEnabled_;

	public string GGofficialUserName = "Giraffe Games";

	public string GGOfficialImageURL = "";

	public enum GGFileIOCloudSyncTypes
	{
		WhisperSync,
		GGCloudSync,
		GGSaveLocalOnly,
		GGSnapshotCloudSync
	}

	public enum SocialProvider
	{
		GooglePlayServices,
		AmazonGameCircle
	}

	public enum InAppProvider
	{
		GooglePlayServices,
		AmazonInApp
	}
}
