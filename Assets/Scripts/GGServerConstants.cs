using System;
using UnityEngine;

public class GGServerConstants : ScriptableObject
{
	public static GGServerConstants instance
	{
		get
		{
			if (GGServerConstants.instance_ == null && GGServerConstants.instance_ == null)
			{
				GGServerConstants.instance_ = (Resources.Load("GGServerAssets/GGServerConstants", typeof(GGServerConstants)) as GGServerConstants);
			}
			return GGServerConstants.instance_;
		}
	}

	public string verifyInAppPurchasesUrl
	{
		get
		{
			return this.urlBase + this.verifyInAppPurchases;
		}
	}

	private static GGServerConstants instance_;

	[SerializeField]
	private string constantsName;

	public string privateKey;

	public string publicKey;

	public string appName;

	public string urlBase;

	public int leadVersion;

	public string nonceUrlPath = "client/nonce";

	public string timeUrlPath = "client/time";

	public string leadUrlPath = "comp/lead";

	public string updateUrlPath = "comp/update";

	public string getIdUrlPath = "users/getId";

	public string getPrizesUrlPath = "comp/getPrizes";

	public string ackPrizesUrlPath = "comp/ackPrizes";

	public string getActiveCompetitionUrlPath = "comp/active";

	public string getCloudSyncUrlPath = "cs/get";

	public string updateCloudSyncUrlPath = "cs/update";

	public string FBLoginUrlPath = "login/fb";

	public string FBInvitableFriendsPath = "facebook/getInvitableFriends";

	public string PlayerProfilePath = "facebook/getPlayerProfile";

	public string FBPlayingFriendsPath = "facebook/getPlayingFriends";

	public string PlayerGetMessagesPath = "messages/getMessages";

	public string MarkMessageReadPath = "messages/markAsRead";

	public string getFriendProfilesPath = "cs/getProfiles";

	public string getSegmentedLeaderboards = "comp/segmentedLead";

	public string verifyInAppPurchases = "ia/play/verify";

	public string exchangeCurrencyPath = "ia/exchange";

	public string getOnlineEventsPath = "getonlineevents";

	public string getOnlineEventsLeaderboardsPath = "geteventslead";

	public string updateOnlineEventsPath = "updatechallengescore";

	public string likeReplayPath = "likereplay";

	public string getPositionPath = "getposition";

	public string getPlayerPositionList = "comp/getPositions";

	public string uploadLeadDataPath = "savedata";

	public string getAppMessagesPath = "getMessage";

	public string getEventMessagesPath = "messageforevent";

	public string updateAppMessagesPath = "updateMessage";

	public string createUserChallengePath = "createuserchallenge";

	public string getUserChallengePath = "getuserchallenge";

	public string getChallengesForPlayerPath = "getchallengesforplayer";

	public string userChallengeReportScorePath = "uploadleaderboard";

	public string likeUserChallengesPath = "setlikelevel";

	public string rateUserChallengesPath = "setratinglevel";

	public string getSingleChallengePath = "getchallengefromid";

	public string userChallengeIncrementDownloadCountPath = "incDownloadCount";

	public string createChallengeReportPath = "createchallengereport";

	public int fbInvitePagesToFetch = 2;

	public int maxEntriesPerLead = 5;
}
