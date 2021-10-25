using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;
using Amazon.Runtime;
using GGMatch3;
using GGOptimize;
using ProtoModels;
using UnityEngine;

public class AWSFirehoseAnalytics : MonoBehaviour
{
	public static void AttachAmazonToGameObject(GameObject gameObject)
	{
		if (AWSFirehoseAnalytics.isAttached)
		{
			return;
		}
		UnityInitializer.AttachToGameObject(gameObject);
		AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
	}

	public int sessionNum
	{
		get
		{
			return this.model.sessionNum;
		}
		set
		{
			this.model.sessionNum = value;
			this.Save();
		}
	}

	public bool isTester
	{
		get
		{
			return ConfigBase.instance.debug || GGPlayerSettings.instance.Model.isTestUser;
		}
	}

	public string sessionID
	{
		get
		{
			return this._sessionID;
		}
		set
		{
			this._sessionID = value;
			int sessionNum = this.sessionNum;
			this.sessionNum = sessionNum + 1;
			AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("session_start");
			this.RecordEvent(customEvent);
		}
	}

	public int maxRecordsInLocalStack
	{
		get
		{
			return this.maxBytes / this.bytesPerRecordEstimation;
		}
	}

	protected string S3Name
	{
		get
		{
			return ScriptableObjectSingleton<AWSFirehoseAnalyticsConfig>.instance.kinesisFirehoseStreamName;
		}
	}

	private RegionEndpoint _CognitoIdentityRegion
	{
		get
		{
			return RegionEndpoint.GetBySystemName(this.CognitoIdentityRegion);
		}
	}

	private RegionEndpoint _KinsesisRegion
	{
		get
		{
			return RegionEndpoint.GetBySystemName(this.KinsesisRegion);
		}
	}

	public bool isSupportedPlatform
	{
		get
		{
			return !Application.isEditor && (this.supportIOS || Application.platform != RuntimePlatform.IPhonePlayer);
		}
	}

	private void Start()
	{
		if (!this.isSupportedPlatform && !Application.isEditor)
		{
			return;
		}
		AWSFirehoseAnalytics.AttachAmazonToGameObject(base.gameObject);
		this._credentials = new CognitoAWSCredentials(this.IdentityPoolId, this._CognitoIdentityRegion);
		this.analyticsManager = new AWSFirehoseAnalytics.AnalyticsProducerConsumerManager(this, this._credentials, this._KinsesisRegion, this.S3Name, this.timeInterval, this.maxRowsBeforeSend, this.maxRowsInBatch, this.maxRecordsInLocalStack, this.localStackTrashingPercentage, new AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse>(this.AsynchPutRecordBatchCallback));
		if (string.IsNullOrEmpty(this.sessionID))
		{
			this.sessionID = GGUID.NewGuid();
		}
	}

	private void AsynchPutRecordCallback(AmazonServiceResult<PutRecordRequest, PutRecordResponse> res)
	{
		HttpStatusCode httpStatusCode = res.Response.HttpStatusCode;
	}

	private void AsynchPutRecordBatchCallback(AmazonServiceResult<PutRecordBatchRequest, PutRecordBatchResponse> res)
	{
		HttpStatusCode httpStatusCode = res.Response.HttpStatusCode;
	}

	private void OnDisable()
	{
		if (this.analyticsManager != null)
		{
			this.analyticsManager.OnDisable();
		}
	}

	private void Awake()
	{
		if (!this.isSupportedPlatform && !Application.isEditor)
		{
			return;
		}
		this.eventDispatcher = new GGNotificationCenter.EventDispatcher();
		this.eventDispatcher.AssignListener(typeof(Analytics.StageStartedEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordStageStartedEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.StageCompletedEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordStageCompletedEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.StageFailedEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordStageFailedEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.IAPEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordIAPEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.MovesBoughtEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordMovesBoughtEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.LivesRefillBoughtEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordLivesRefilledEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.RoomItemBoughtEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordRoomItemBoughtEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.RoomItemChangedEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordRoomItemChangedEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.BoosterUsedEvent), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RecordBoosterUsedEvent));
		this.eventDispatcher.AssignListener(typeof(Analytics.RateDialog), new GGNotificationCenter.EventDispatcher.EventDelegateListener(this.RateDialogEvent));
		BehaviourSingletonInit<GGNotificationCenter>.instance.AddEventDispatcher(this.eventDispatcher);
	}

	private void Update()
	{
		if (this.analyticsManager == null)
		{
			return;
		}
		this.analyticsManager.OnUpdate();
	}

	private void OnApplicationFocus(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			this.CreateEvent("application_focus_false");
			this.lastTimestamp = DateTime.UtcNow;
			return;
		}
		if (string.IsNullOrEmpty(this.sessionID))
		{
			return;
		}
		if ((DateTime.UtcNow - this.lastTimestamp).TotalSeconds > (double)this.maxPauseBetweenSameSessionSeconds)
		{
			this.sessionID = GGUID.NewGuid();
			return;
		}
		this.CreateEvent("application_focus_true");
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("application_pause_true");
			this.lastTimestamp = DateTime.UtcNow;
			this.RecordEvent(customEvent);
			return;
		}
		if (string.IsNullOrEmpty(this.sessionID))
		{
			return;
		}
		if ((DateTime.UtcNow - this.lastTimestamp).TotalSeconds > (double)this.maxPauseBetweenSameSessionSeconds)
		{
			this.sessionID = GGUID.NewGuid();
			return;
		}
		AWSFirehoseAnalytics.CustomEvent customEvent2 = this.CreateEvent("application_pause_false");
		this.RecordEvent(customEvent2);
	}

	private void OnApplicationQuit()
	{
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("application_quit");
		this.RecordEvent(customEvent);
	}

	protected void AddBaseAttributesToEvent(AWSFirehoseAnalytics.CustomEvent customEvent)
	{
		customEvent.AddAttribute("session_id", this.sessionID);
		customEvent.AddAttribute("session_num", this.sessionNum.ToString());
		if (this.isTester)
		{
			string attribute = string.Format("Tester.{0}", GGPlayerSettings.instance.GetName());
			customEvent.AddAttribute("client_type", attribute);
		}
		else
		{
			customEvent.AddAttribute("client_type", "User");
		}
		List<Experiment> activeExperiments = GGAB.GetActiveExperiments();
		for (int i = 0; i < activeExperiments.Count; i++)
		{
			Experiment experiment = activeExperiments[i];
			string customDimensionToMark = experiment.customDimensionToMark;
			if (!string.IsNullOrEmpty(customDimensionToMark))
			{
				string attribute2 = experiment.name + "-" + experiment.GetActiveVariation(SingletonInit<GGAB>.instance.optimize.GetUserBucket(experiment)).name;
				customEvent.AddAttribute(customDimensionToMark, attribute2);
			}
		}
		GGPlayerSettings instance = GGPlayerSettings.instance;
		customEvent.AddAttribute("player_version", instance.Model.version.ToString());
		customEvent.AddMetric("stages_passed", (double)Match3StagesDB.instance.stagesPassed);
		List<InAppPurchaseDAO> purchases = instance.GetPurchases();
		customEvent.AddMetric("total_iap_num", (double)purchases.Count);
	}

	protected AWSFirehoseAnalytics.CustomEvent CreateEvent(string eventName)
	{
		AWSFirehoseAnalytics.CustomEvent customEvent = new AWSFirehoseAnalytics.CustomEvent(eventName);
		this.AddBaseAttributesToEvent(customEvent);
		return customEvent;
	}

	public void RecordStageStartedEvent(object data)
	{
		GameScreen.StageState stageState = (data as Analytics.StageStartedEvent).stageState;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("stage_started");
		this.AddStageDataToEvent(customEvent, stageState);
		this.RecordEvent(customEvent);
	}

	public void RecordStageCompletedEvent(object data)
	{
		GameScreen.StageState stageState = (data as Analytics.StageCompletedEvent).stageState;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("stage_completed");
		this.AddStageDataToEvent(customEvent, stageState);
		this.RecordEvent(customEvent);
	}

	public void RecordStageFailedEvent(object data)
	{
		GameScreen.StageState stageState = (data as Analytics.StageFailedEvent).stageState;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("stage_failed");
		this.AddStageDataToEvent(customEvent, stageState);
		this.RecordEvent(customEvent);
	}

	public void RecordMovesBoughtEvent(object data)
	{
		Analytics.MovesBoughtEvent movesBoughtEvent = data as Analytics.MovesBoughtEvent;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("moves_bought_event");
		customEvent.AddMetric("spent_amount", (double)movesBoughtEvent.offer.price.cost);
		customEvent.AddAttribute("currency_type", movesBoughtEvent.offer.price.currency.ToString());
		customEvent.AddAttribute("context", movesBoughtEvent.offer.movesCount.ToString());
		GameScreen.StageState stageState = movesBoughtEvent.stageState;
		this.AddStageDataToEvent(customEvent, stageState);
		this.RecordEvent(customEvent);
	}

	public void RecordLivesRefilledEvent(object data)
	{
		Analytics.LivesRefillBoughtEvent livesRefillBoughtEvent = data as Analytics.LivesRefillBoughtEvent;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("lives_refill_bought_event");
		int lives = livesRefillBoughtEvent.livesAfterRefill - livesRefillBoughtEvent.livesBeforeRefill;
		SingleCurrencyPrice priceForLives = livesRefillBoughtEvent.config.GetPriceForLives(lives);
		customEvent.AddMetric("spent_amount", (double)priceForLives.cost);
		customEvent.AddAttribute("currency_type", priceForLives.currency.ToString());
		customEvent.AddAttribute("context", lives.ToString());
		this.RecordEvent(customEvent);
	}

	protected AWSFirehoseAnalytics.MonetizationEvent CreateMonetizationEvent(OffersDB.ProductDefinition inApp)
	{
		AWSFirehoseAnalytics.MonetizationEvent monetizationEvent = new AWSFirehoseAnalytics.MonetizationEvent();
		string formatedPrice = GGInAppPurchase.instance.GetFormatedPrice(inApp.productID);
		string priceAmountMicros = GGInAppPurchase.instance.GetPriceAmountMicros(inApp.productID);
		string text = GGInAppPurchase.instance.GetPriceCurrencyCode(inApp.productID);
		float num = 0f;
		if (string.IsNullOrEmpty(text))
		{
			text = "XXX";
		}
		if (!string.IsNullOrEmpty(priceAmountMicros))
		{
			int num2 = 0;
			if (int.TryParse(priceAmountMicros, out num2))
			{
				num = (float)num2 / 1000000f;
			}
		}
		monetizationEvent.ProductId = inApp.productID;
		monetizationEvent.ItemPrice = new double?((double)num);
		monetizationEvent.ItemPriceFormatted = formatedPrice;
		monetizationEvent.Currency = text;
		monetizationEvent.Store = GGInAppPurchase.instance.GetType().ToString();
		this.AddBaseAttributesToEvent(monetizationEvent);
		return monetizationEvent;
	}

	public void RecordIAPEvent(object data)
	{
		Analytics.IAPEvent iapevent = data as Analytics.IAPEvent;
		if (iapevent.inAppObject == null)
		{
			return;
		}
		AWSFirehoseAnalytics.MonetizationEvent monetizationEvent = this.CreateMonetizationEvent(iapevent.inAppObject);
		string attribute = "empty";
		if (!string.IsNullOrEmpty(iapevent.purchaseToken))
		{
			attribute = iapevent.purchaseToken;
		}
		monetizationEvent.AddAttribute("purchase_token", attribute);
		monetizationEvent.AddAttribute("context", "no_check");
		if (monetizationEvent.ItemPrice != null)
		{
			double value = monetizationEvent.ItemPrice.Value;
		}
		this.RecordEvent(monetizationEvent);
	}

	public void RecordRoomItemBoughtEvent(object data)
	{
		Analytics.RoomItemBoughtEvent roomItemBoughtEvent = data as Analytics.RoomItemBoughtEvent;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("room_item_bought_event");
		customEvent.AddMetric("spent_amount", (double)roomItemBoughtEvent.price.cost);
		customEvent.AddAttribute("currency_type", roomItemBoughtEvent.price.currency.ToString());
		customEvent.AddAttribute("context", roomItemBoughtEvent.variation.name);
		customEvent.AddAttribute("item_id", roomItemBoughtEvent.visualObject.displayName);
		customEvent.AddAttribute("event_place", roomItemBoughtEvent.screen.activeRoom.name);
		customEvent.AddMetric("moves_played", (double)roomItemBoughtEvent.numberOfItemsOwned);
		this.RecordEvent(customEvent);
	}

	public void RecordRoomItemChangedEvent(object data)
	{
		Analytics.RoomItemChangedEvent roomItemChangedEvent = data as Analytics.RoomItemChangedEvent;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("room_item_changed_event");
		customEvent.AddAttribute("context", roomItemChangedEvent.variation.name);
		customEvent.AddAttribute("item_id", roomItemChangedEvent.visualObject.displayName);
		customEvent.AddAttribute("event_place", roomItemChangedEvent.screen.activeRoom.name);
		this.RecordEvent(customEvent);
	}

	public void RecordBoosterUsedEvent(object data)
	{
		Analytics.BoosterUsedEvent boosterUsedEvent = data as Analytics.BoosterUsedEvent;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("booster_used_event");
		customEvent.AddAttribute("context", boosterUsedEvent.booster.boosterType.ToString());
		GameScreen.StageState stageState = boosterUsedEvent.stageState;
		this.AddStageDataToEvent(customEvent, stageState);
		this.RecordEvent(customEvent);
	}

	public void RateDialogEvent(object data)
	{
		Analytics.RateDialog rateDialog = data as Analytics.RateDialog;
		AWSFirehoseAnalytics.CustomEvent customEvent = this.CreateEvent("rate_dialog");
		if (rateDialog.isGoingToRate)
		{
			customEvent.AddAttribute("context", "rate");
		}
		else if (rateDialog.isLike)
		{
			customEvent.AddAttribute("context", "like");
		}
		else
		{
			customEvent.AddAttribute("context", "notlike");
		}
		customEvent.AddMetric("moves_played", (double)rateDialog.timesShown);
		customEvent.AddMetric("level_times_played", (double)Match3StagesDB.instance.passedStages);
		this.RecordEvent(customEvent);
	}

	private void RecordEvent(AWSFirehoseAnalytics.CustomEvent customEvent)
	{
		if (customEvent == null)
		{
			return;
		}
		if (this.analyticsManager == null)
		{
			return;
		}
		this.analyticsManager.RecordEvent(customEvent);
	}

	private void AddStageDataToEvent(AWSFirehoseAnalytics.CustomEvent customEvent, GameScreen.StageState stageState)
	{
		int userMovesCount = stageState.userMovesCount;
		Match3StagesDB.Stage stage = stageState.currentGameProgress.game.initParams.stage;
		Match3GameParams initParams = stageState.currentGameProgress.game.initParams;
		int index = stage.index;
		int timesPlayed = stage.timesPlayed;
		int num = 0;
		long num2 = 0L;
		List<GameScreen.GameProgress> gameProgressList = stageState.gameProgressList;
		for (int i = 0; i < gameProgressList.Count; i++)
		{
			GameScreen.GameProgress gameProgress = gameProgressList[i];
			num += gameProgress.game.totalCoinsSpent;
			num2 += gameProgress.game.timePlayed;
		}
		string attribute = string.Format("{0}/{1}", Match3StagesDB.instance.name, index);
		customEvent.AddAttribute("event_place", attribute);
		customEvent.AddMetric("moves_played", (double)userMovesCount);
		customEvent.AddMetric("level_times_played", (double)timesPlayed);
		customEvent.AddMetric("play_duration_sec", TimeSpan.FromTicks(num2).TotalSeconds);
		customEvent.AddMetric("level_total_cash_spent", (double)num);
		customEvent.AddMetric("booster_rank_level", (double)initParams.giftBoosterLevel);
		customEvent.AddMetric("b_bomb", (double)initParams.BoughtBoosterCount(GGMatch3.BoosterType.BombBooster));
		customEvent.AddMetric("b_disco", (double)initParams.BoughtBoosterCount(GGMatch3.BoosterType.DiscoBooster));
		customEvent.AddMetric("b_rocket", (double)initParams.BoughtBoosterCount(GGMatch3.BoosterType.VerticalRocketBooster));
		customEvent.AddMetric("u_hammer", (double)stageState.hammersUsed);
		customEvent.AddMetric("u_phammer", (double)stageState.powerHammersUsed);
		if (stage.multiLevelReference.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Match3StagesDB.LevelReference> multiLevelReference = stage.multiLevelReference;
			for (int j = 0; j < multiLevelReference.Count; j++)
			{
				Match3StagesDB.LevelReference levelReference = multiLevelReference[j];
				if (levelReference != null)
				{
					if (!string.IsNullOrEmpty(levelReference.levelDBName))
					{
						stringBuilder.AppendFormat("{0}/{1}, ", levelReference.levelDBName, levelReference.levelName);
					}
					else
					{
						stringBuilder.AppendFormat("{0}, ", levelReference.levelName);
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			string attribute2 = stringBuilder.ToString();
			customEvent.AddAttribute("level_name", attribute2);
			Match3StagesDB.LevelReference levelReference2 = stage.multiLevelReference[stageState.currentGameProgressIndex];
			string attribute3 = levelReference2.levelName;
			if (!string.IsNullOrEmpty(levelReference2.levelDBName))
			{
				attribute3 = string.Format("{0}/{1}", levelReference2.levelDBName, levelReference2.levelName);
			}
			customEvent.AddAttribute("context", attribute3);
		}
		else
		{
			string attribute4 = stage.levelReference.levelName;
			if (!string.IsNullOrEmpty(stage.levelReference.levelDBName))
			{
				attribute4 = string.Format("{0}/{1}", stage.levelReference.levelDBName, stage.levelReference.levelName);
			}
			customEvent.AddAttribute("level_name", attribute4);
			customEvent.AddAttribute("context", attribute4);
		}
		List<MultiLevelGoals.Goal> activeGoals = stageState.goals.GetActiveGoals();
		int num3 = 0;
		for (int k = 0; k < activeGoals.Count; k++)
		{
			MultiLevelGoals.Goal goal = activeGoals[k];
			num3 += goal.RemainingCount;
		}
		customEvent.AddMetric("goals_left_to_pass_stage", (double)num3);
	}

	private void OnDestroy()
	{
		if (!this.isSupportedPlatform && !Application.isEditor)
		{
			return;
		}
		try
		{
			BehaviourSingletonInit<GGNotificationCenter>.instance.RemoveEventDispatcher(this.eventDispatcher);
		}
		catch
		{
		}
	}

	private EventLog model
	{
		get
		{
			if (this._model == null && !ProtoIO.LoadFromFile<ProtoSerializer, EventLog>(AWSFirehoseAnalytics.FileName, GGFileIO.instance, out this._model))
			{
				this._model = new EventLog();
				this._model.events = new List<EventLog.Event>();
				ProtoIO.SaveToFile<ProtoSerializer, EventLog>(AWSFirehoseAnalytics.FileName, GGFileIO.instance, this._model);
			}
			if (this._model.events == null)
			{
				this._model.events = new List<EventLog.Event>();
			}
			return this._model;
		}
	}

	public void ResetModel()
	{
		this.model.events.Clear();
		this.model.sessionNum = 0;
		this.Save();
	}

	private void Save()
	{
		ProtoIO.SaveToFile<ProtoSerializer, EventLog>(AWSFirehoseAnalytics.FileName, GGFileIO.instance, this._model);
	}

	public static int UnixTimestamp(DateTime time)
	{
		return (int)time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static byte[] ToByteArray(string str)
	{
		return Encoding.ASCII.GetBytes(str);
	}

	private static bool isAttached;

	protected GGNotificationCenter.EventDispatcher eventDispatcher;

	public static string FileName = "analytics.bytes";

	public DateTime lastTimestamp;

	public string _sessionID;

	public float timeInterval = 5f;

	public int maxRowsInBatch = 100;

	public int maxRowsBeforeSend = 10;

	public int bytesPerRecordEstimation = 500;

	public int maxPauseBetweenSameSessionSeconds = 30;

	public float localStackTrashingPercentage = 0.1f;

	public int maxBytes = 500000;

	public string IdentityPoolId = "us-east-1:5a6b2a1f-fe0c-49fa-82a7-f812fb9eae7e";

	private CognitoAWSCredentials _credentials;

	public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

	public string KinsesisRegion = RegionEndpoint.EUWest1.SystemName;

	public AWSFirehoseAnalytics.AnalyticsManager analyticsManager;

	public bool supportIOS;

	private EventLog _model;

	public class CustomEvent
	{
		public CustomEvent(string eventType)
		{
			this.AddAttribute("event_type", eventType);
			this.AddBasicAttributes();
		}

		public CustomEvent()
		{
			this.AddBasicAttributes();
		}

		private void AddBasicAttributes()
		{
			string version = Application.version;
			this.AddAttribute("application_version_code", version);
			string attribute = GGUIDPrivate.InstallId();
			this.AddAttribute("client_id", attribute);
			string attribute2 = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
			this.AddAttribute("event_timestamp", attribute2);
			this.AddAttribute("platform", Application.platform.ToString());
			double value = Convert.ToDouble(AWSFirehoseAnalytics.UnixTimestamp(new DateTime(GGPlayerSettings.instance.Model.creationTime)));
			this.AddMetric("install_timestamp", value);
		}

		public void AddMetric(string name, double value)
		{
			if (this.metricDictionary.ContainsKey(name))
			{
				this.metricDictionary[name] = new double?(value);
				return;
			}
			this.metricDictionary.Add(name, new double?(value));
		}

		public void AddAttribute(string name, string attribute)
		{
			if (string.IsNullOrEmpty(attribute))
			{
				return;
			}
			if (this.attributeDictionary.ContainsKey(name))
			{
				this.attributeDictionary[name] = attribute;
				return;
			}
			this.attributeDictionary.Add(name, attribute);
		}

		private string Escape(string str)
		{
			string result;
			try
			{
				result = GGFormat.JavaScriptStringEncode(str, false);
			}
			catch
			{
				result = "";
			}
			return result;
		}

		private string DictionaryToJSON(Dictionary<string, object> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, object> keyValuePair in dict)
			{
				if (keyValuePair.Value is string)
				{
					stringBuilder.Append("\"").Append(this.Escape(keyValuePair.Key)).Append("\":\"").Append(this.Escape(keyValuePair.Value.ToString())).Append("\"");
				}
				else if (keyValuePair.Value is long || keyValuePair.Value is int)
				{
					stringBuilder.Append("\"").Append(this.Escape(keyValuePair.Key)).Append("\":").Append(keyValuePair.Value.ToString());
				}
				else if (keyValuePair.Value is Dictionary<string, object>)
				{
					stringBuilder.Append("\"").Append(this.Escape(keyValuePair.Key)).Append("\":").Append(this.DictionaryToJSON(keyValuePair.Value as Dictionary<string, object>));
				}
				else
				{
					UnityEngine.Debug.LogError("AWS: Trying to serialize unknown type");
				}
				if (num < dict.Count - 1)
				{
					stringBuilder.Append(",");
				}
				num++;
			}
			return stringBuilder.ToString();
		}

		private string DictionaryToJSON(Dictionary<string, string> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				if (keyValuePair.Value == null)
				{
					UnityEngine.Debug.LogError("attribute key is null " + keyValuePair.Key);
				}
				else
				{
					stringBuilder.Append("\"").Append(this.Escape(keyValuePair.Key)).Append("\":\"").Append(this.Escape(keyValuePair.Value.ToString())).Append("\"");
					if (num < dict.Count - 1)
					{
						stringBuilder.Append(",");
					}
					num++;
				}
			}
			return stringBuilder.ToString();
		}

		private string DictionaryToJSON(Dictionary<string, double?> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, double?> keyValuePair in dict)
			{
				if (keyValuePair.Value == null)
				{
					UnityEngine.Debug.LogError("metric key is null " + keyValuePair.Key);
				}
				else
				{
					stringBuilder.Append("\"").Append(this.Escape(keyValuePair.Key)).Append("\":").Append(keyValuePair.Value.ToString());
					if (num < dict.Count - 1)
					{
						stringBuilder.Append(",");
					}
					num++;
				}
			}
			return stringBuilder.ToString();
		}

		public virtual string SerializeToJSON()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (this.baseDictionary.Count > 0)
			{
				stringBuilder.Append(this.DictionaryToJSON(this.baseDictionary));
				if (this.attributeDictionary.Count > 0 || this.metricDictionary.Count > 0)
				{
					stringBuilder.Append(",");
				}
			}
			if (this.attributeDictionary.Count > 0)
			{
				stringBuilder.Append("\"attributes\":{").Append(this.DictionaryToJSON(this.attributeDictionary)).Append("}");
				if (this.metricDictionary.Count > 0)
				{
					stringBuilder.Append(",");
				}
			}
			if (this.metricDictionary.Count > 0)
			{
				stringBuilder.Append("\"metrics\":{").Append(this.DictionaryToJSON(this.metricDictionary)).Append("}");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		protected Dictionary<string, double?> metricDictionary = new Dictionary<string, double?>();

		protected Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();

		protected Dictionary<string, object> baseDictionary = new Dictionary<string, object>();
	}

	public class MonetizationEvent : AWSFirehoseAnalytics.CustomEvent
	{
		public string Currency
		{
			set
			{
				if (!this.attributeDictionary.ContainsKey("Currency"))
				{
					this.attributeDictionary.Add("Currency", value);
					return;
				}
				this.attributeDictionary["Currency"] = value;
			}
		}

		public double? ItemPrice
		{
			get
			{
				double? result;
				this.metricDictionary.TryGetValue("ItemPrice", out result);
				return result;
			}
			set
			{
				if (!this.metricDictionary.ContainsKey("ItemPrice"))
				{
					this.metricDictionary.Add("ItemPrice", value);
					return;
				}
				this.metricDictionary["ItemPrice"] = value;
			}
		}

		public string ItemPriceFormatted
		{
			set
			{
				if (!this.attributeDictionary.ContainsKey("ItemPriceFormatted"))
				{
					this.attributeDictionary.Add("ItemPriceFormatted", value);
					return;
				}
				this.attributeDictionary["ItemPriceFormatted"] = value;
			}
		}

		public string ProductId
		{
			set
			{
				if (!this.attributeDictionary.ContainsKey("ProductId"))
				{
					this.attributeDictionary.Add("ProductId", value);
					return;
				}
				this.attributeDictionary["ProductId"] = value;
			}
		}

		public string Store
		{
			set
			{
				if (!this.attributeDictionary.ContainsKey("Store"))
				{
					this.attributeDictionary.Add("Store", value);
					return;
				}
				this.attributeDictionary["Store"] = value;
			}
		}

		public MonetizationEvent()
		{
			this.attributeDictionary.Add("event_type", "_monetization.purchase");
		}
	}

	public class AnalyticsManager
	{
		public virtual void RecordEvent(AWSFirehoseAnalytics.CustomEvent ce)
		{
		}

		public virtual void OnDisable()
		{
		}

		public virtual void OnUpdate()
		{
		}
	}

	public class AnalyticsProducerConsumerManager : AWSFirehoseAnalytics.AnalyticsManager
	{
		private EventLog model
		{
			get
			{
				return this.analytics.model;
			}
		}

		private void Save()
		{
			this.analytics.Save();
		}

		public void OnDisable()
		{
			base.OnDisable();
		}

		public void OnUpdate()
		{
			if (Application.isEditor && !ScriptableObjectSingleton<AWSFirehoseAnalyticsConfig>.instance.sendEventsInEditor)
			{
				this.consumer.DebugConsume();
				return;
			}
			this.consumer.Consume();
		}

		public void RecordEvent(AWSFirehoseAnalytics.CustomEvent ce)
		{
			this.producer.RecordEvent(ce);
		}

		public AnalyticsProducerConsumerManager(AWSFirehoseAnalytics analytics, CognitoAWSCredentials credentials, RegionEndpoint kinesisRegion, string S3Name, float timeInterval, int maxBeforeSend, int maxPerBatch, int maxRecordsLocalStack, float trashPercentage, AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse> callback)
		{
			this.analytics = analytics;
			this.consumer = new AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Consumer(credentials, kinesisRegion, S3Name, this, timeInterval, maxBeforeSend, maxPerBatch, maxRecordsLocalStack, trashPercentage, callback);
			this.producer = new AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Producer(this);
		}

		private AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Consumer consumer;

		private AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Producer producer;

		private AWSFirehoseAnalytics analytics;

		private class Producer
		{
			public Producer(AWSFirehoseAnalytics.AnalyticsProducerConsumerManager manager)
			{
				this.manager = manager;
			}

			public void RecordEvent(AWSFirehoseAnalytics.CustomEvent ce)
			{
				if (ce == null)
				{
					return;
				}
				EventLog.Event @event = new EventLog.Event();
				@event.dataJSON = ce.SerializeToJSON();
				@event.time = DateTime.UtcNow.Ticks;
				this.manager.model.events.Add(@event);
				this.manager.Save();
			}

			private AWSFirehoseAnalytics.AnalyticsProducerConsumerManager manager;
		}

		private class Consumer
		{
			public Consumer(CognitoAWSCredentials credentials, RegionEndpoint kinesisRegion, string S3Name, AWSFirehoseAnalytics.AnalyticsProducerConsumerManager manager, float timeInterval, int maxBeforeSend, int maxPerBatch, int maxRecordsLocalStack, float trashPercentage, AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse> callback = null)
			{
				this.credentials = credentials;
				this.kinesisRegion = kinesisRegion;
				this.S3Name = S3Name;
				this.externalCallback = callback;
				this.manager = manager;
				this.timeInterval = timeInterval;
				this.maxBeforeSend = maxBeforeSend;
				this.maxPerBatch = maxPerBatch;
				this.maxRecordsLocalStack = maxRecordsLocalStack;
				this.trashPercentage = trashPercentage;
			}

			private AmazonKinesisFirehoseClient client
			{
				get
				{
					if (this.client_ == null)
					{
						this.client_ = new AmazonKinesisFirehoseClient(this.credentials, this.kinesisRegion);
					}
					return this.client_;
				}
			}

			public void Consume()
			{
				this.timer += Time.deltaTime;
				if (this.status == AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Consumer.Status.Busy)
				{
					return;
				}
				this.MaintainStack();
				if (Application.internetReachability <= NetworkReachability.NotReachable)
				{
					return;
				}
				int count = this.manager.model.events.Count;
				if (count == 0)
				{
					return;
				}
				if (this.timer < this.timeInterval && count < this.maxBeforeSend)
				{
					return;
				}
				this.timer = 0f;
				int maxRows = Mathf.Min(count, this.maxPerBatch);
				this.ConsumeRows(maxRows);
			}

			public void DebugConsume()
			{
				List<EventLog.Event> events = this.manager.model.events;
				for (int i = 0; i < events.Count; i++)
				{
					EventLog.Event @event = events[i];
					UnityEngine.Debug.LogFormat("AWS: Debug JSON:{0}", new object[]
					{
						@event.dataJSON
					});
				}
				this.manager.model.events.Clear();
				this.manager.Save();
			}

			private List<EventLog.Event> localStack
			{
				get
				{
					if (this.localStack_ == null)
					{
						this.localStack_ = new List<EventLog.Event>();
					}
					return this.localStack_;
				}
			}

			public void ConsumeRows(int maxRows)
			{
				EventLog model = this.manager.model;
				int count = Mathf.Min(maxRows, model.events.Count);
				this.localStack.Clear();
				this.localStack.AddRange(model.events.GetRange(0, count));
				model.events.RemoveRange(0, count);
				this.manager.Save();
				List<Record> list = new List<Record>(this.localStack.Count);
				for (int i = 0; i < this.localStack.Count; i++)
				{
					EventLog.Event @event = this.localStack[i];
					if (@event != null)
					{
						byte[] buffer = AWSFirehoseAnalytics.ToByteArray(@event.dataJSON);
						list.Add(new Record
						{
							Data = new MemoryStream(buffer)
						});
					}
				}
				this.status = AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Consumer.Status.Busy;
				PutRecordBatchRequest putRecordBatchRequest = new PutRecordBatchRequest();
				putRecordBatchRequest.DeliveryStreamName = this.S3Name;
				putRecordBatchRequest.Records = list;
				this.client.PutRecordBatchAsync(putRecordBatchRequest, new AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse>(this.AsynchPutRecordCallback), null);
			}

			private void MaintainStack()
			{
				List<EventLog.Event> events = this.manager.model.events;
				if (events.Count > this.maxRecordsLocalStack)
				{
					int count = Mathf.FloorToInt(Mathf.Min(this.trashPercentage * (float)events.Count, (float)events.Count));
					events.RemoveRange(0, count);
					this.manager.Save();
				}
			}

			private void AsynchPutRecordCallback(AmazonServiceResult<PutRecordBatchRequest, PutRecordBatchResponse> res)
			{
				this.status = AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Consumer.Status.Available;
				if (this.externalCallback != null)
				{
					this.externalCallback(res);
				}
				if (res.Response.HttpStatusCode != HttpStatusCode.OK)
				{
					EventLog model = this.manager.model;
					if (model == null)
					{
						return;
					}
					if (model.events == null)
					{
						return;
					}
					this.localStack.AddRange(model.events);
					model.events.Clear();
					model.events.AddRange(this.localStack);
					this.localStack.Clear();
					this.manager.Save();
				}
			}

			private AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse> externalCallback;

			private AWSFirehoseAnalytics.AnalyticsProducerConsumerManager manager;

			private float timeInterval;

			private int maxBeforeSend;

			private int maxPerBatch;

			private int maxRecordsLocalStack;

			private float trashPercentage;

			private CognitoAWSCredentials credentials;

			private RegionEndpoint kinesisRegion;

			private string S3Name;

			private AmazonKinesisFirehoseClient client_;

			private float timer;

			private List<EventLog.Event> localStack_;

			private AWSFirehoseAnalytics.AnalyticsProducerConsumerManager.Consumer.Status status;

			private enum Status
			{
				Available,
				Busy
			}
		}
	}
}
