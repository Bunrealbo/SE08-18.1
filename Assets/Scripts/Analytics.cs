using System;
using GGMatch3;
using ProtoModels;

public class Analytics : BehaviourSingletonInit<Analytics>
{
	public override void Init()
	{
		if (!ProtoIO.LoadFromFile<ProtoSerializer, AnalyticsDAO>("ans.bytes", GGFileIO.instance, out this.model))
		{
			this.model = new AnalyticsDAO();
			this.model.version = GGPlayerSettings.instance.Model.version;
		}
	}

	public void Save()
	{
		ProtoIO.SaveToFile<AnalyticsDAO>("ans.bytes", this.model);
	}

	public void SetExited()
	{
		this.model.lastTimeWhenExited = DateTime.UtcNow.Ticks;
		this.Save();
	}

	public float secondsTillExited
	{
		get
		{
			return (float)TimeSpan.FromTicks(DateTime.UtcNow.Ticks - this.model.lastTimeWhenExited).TotalSeconds;
		}
	}

	public void IncSessionNum()
	{
		AnalyticsDAO analyticsDAO = this.model;
		int sessionNum = analyticsDAO.sessionNum;
		analyticsDAO.sessionNum = sessionNum + 1;
		this.Save();
	}

	public const string Filename = "ans.bytes";

	public AnalyticsDAO model;

	public class EventBase
	{
		public virtual void Send()
		{
			BehaviourSingletonInit<GGNotificationCenter>.instance.BroadcastEvent(this);
		}
	}

	public class StageStartedEvent : Analytics.EventBase
	{
		public GameScreen.StageState stageState;
	}

	public class StageCompletedEvent : Analytics.EventBase
	{
		public GameScreen.StageState stageState;
	}

	public class StageFailedEvent : Analytics.EventBase
	{
		public GameScreen.StageState stageState;
	}

	public class IAPEvent : Analytics.EventBase
	{
		public InAppBackend.PurchaseEventArguments purchaseArguments;

		public OffersDB.ProductDefinition inAppObject;

		public string purchaseToken;
	}

	public class MovesBoughtEvent : Analytics.EventBase
	{
		public GameScreen.StageState stageState;

		public BuyMovesPricesConfig.OfferConfig offer;
	}

	public class LivesRefillBoughtEvent : Analytics.EventBase
	{
		public int livesBeforeRefill;

		public int livesAfterRefill;

		public LivesPriceConfig.PriceConfig config;
	}

	public class RoomItemBoughtEvent : Analytics.EventBase
	{
		public SingleCurrencyPrice price;

		public DecorateRoomScreen screen;

		public GraphicsSceneConfig.VisualObject visualObject;

		public GraphicsSceneConfig.Variation variation;

		public int numberOfItemsOwned;
	}

	public class RoomItemChangedEvent : Analytics.EventBase
	{
		public DecorateRoomScreen screen;

		public GraphicsSceneConfig.VisualObject visualObject;

		public GraphicsSceneConfig.Variation variation;
	}

	public class BoosterUsedEvent : Analytics.EventBase
	{
		public BoosterConfig booster;

		public GameScreen.StageState stageState;
	}

	public class RateDialog : Analytics.EventBase
	{
		public int timesShown;

		public bool isLike;

		public bool isGoingToRate;
	}
}
