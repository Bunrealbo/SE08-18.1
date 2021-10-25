using System;
using System.Collections.Generic;
using ProtoModels;
using UnityEngine;

public class GGPlayerSettings
{
	public PlayerModel Model
	{
		get
		{
			return this.model;
		}
	}

	public PlayerModel.GivenGiftsData givenGifts
	{
		get
		{
			if (this.Model.givenGiftsData == null)
			{
				this.Model.givenGiftsData = new PlayerModel.GivenGiftsData();
			}
			return this.Model.givenGiftsData;
		}
	}

	public bool isMusicOff
	{
		get
		{
			return this.Model.musicOff;
		}
		set
		{
			this.Model.musicOff = value;
			this.Save();
			ConfigBase.instance.SetAudioMixerValues(this);
		}
	}

	public bool isSoundFXOff
	{
		get
		{
			return this.Model.sfxOff;
		}
		set
		{
			this.Model.sfxOff = value;
			this.Save();
			ConfigBase.instance.SetAudioMixerValues(this);
		}
	}

	public void ResetEverything()
	{
		this.model = new PlayerModel();
		this.model.creationTime = DateTime.UtcNow.Ticks;
		ConfigBase instance = ConfigBase.instance;
		this.model.mood = 0.5f;
		this.model.lastTimeMoodBoost = DateTime.Now.Ticks;
		this.model.version = ConfigBase.instance.initialPlayerVersion;
		this.model.musicVolume = (float)ConfigBase.instance.initialVolumeLevel;
		this.model.sfxVolume = (float)ConfigBase.instance.initialVolumeLevel;
		this.model.ambientVolume = (float)ConfigBase.instance.initialVolumeLevel;
		new DateTime(this.model.creationTime);
		this.Save();
		this.ReloadModel();
		this.walletManager.SetCurrency(CurrencyType.coins, instance.initialCoins);
		this.walletManager.SetCurrency(CurrencyType.diamonds, instance.initialStars);
	}

	public static GGPlayerSettings instance
	{
		get
		{
			if (GGPlayerSettings.instance_ == null)
			{
				GGPlayerSettings.instance_ = new GGPlayerSettings();
				GGPlayerSettings.instance_.walletManager = new WalletManager(GGPlayerSettings.instance_.model, new WalletManager.OnSave(GGPlayerSettings.instance_.Save));
				GGPlayerSettings.instance_.Init();
			}
			return GGPlayerSettings.instance_;
		}
	}

	public bool canCloudSync
	{
		get
		{
			return this.Model.canCloudSync || ConfigBase.instance.isFakePlayerIdOn;
		}
		set
		{
			this.Model.canCloudSync = value;
			this.Save();
		}
	}

	public WalletManager walletManager
	{
		get
		{
			return this._003CwalletManager_003Ek__BackingField;
		}
		private set
		{
			this._003CwalletManager_003Ek__BackingField = value;
		}
	}

	public List<PlayerModel.UsageData> usageDataList
	{
		get
		{
			if (this.model.usageDataList == null)
			{
				this.model.usageDataList = new List<PlayerModel.UsageData>();
			}
			return this.model.usageDataList;
		}
	}

	public void IncreaseSessionCoins(int coinsAmount)
	{
		if (this.usageDataList.Count == 0)
		{
			return;
		}
		this.usageDataList[this.usageDataList.Count - 1].coinsUsed += coinsAmount;
		this.Save();
	}

	private GGPlayerSettings()
	{
	}

	private GGPlayerSettings(PlayerModel model)
	{
		this.isSavingSuspended = true;
		this.model = model;
		this.walletManager = new WalletManager(model, new WalletManager.OnSave(this.Save));
	}

	public GGPlayerSettings CreateFromData(CloudSyncData fileSystemData)
	{
		PlayerModel playerModel = ProtoIO.Clone<PlayerModel>(this.model);
		if (fileSystemData == null)
		{
			return new GGPlayerSettings(playerModel);
		}
		CloudSyncData.CloudSyncFile file = ProtoModelExtensions.GetFile(fileSystemData, "player.bytes");
		if (file == null)
		{
			return new GGPlayerSettings(playerModel);
		}
		PlayerModel playerModel2 = null;
		if (!ProtoIO.LoadFromBase64String<PlayerModel>(file.data, out playerModel2))
		{
			return new GGPlayerSettings(playerModel);
		}
		if (playerModel2 == null)
		{
			return new GGPlayerSettings(playerModel);
		}
		return new GGPlayerSettings(playerModel2);
	}

	public int MultiplayerGamesPlayed()
	{
		return this.model.multiplayerWins + this.model.multiplayerLoses;
	}

	private void Init()
	{
		this.ReloadModel();
		bool isPlaying = Application.isPlaying;
		if (!this.isSavingSuspended)
		{
			SingletonInit<FileIOChanges>.instance.OnChange(new FileIOChanges.OnDataChangedDelegate(this.ReloadModel));
		}
	}

	public void ReloadModel()
	{
		bool flag = false;
		if (!ProtoIO.LoadFromFileLocal<ProtoSerializer, PlayerModel>("player.bytes", out this.model))
		{
			this.model = new PlayerModel();
			this.model.creationTime = DateTime.UtcNow.Ticks;
			flag = true;
			this.model.mood = 0.5f;
			this.model.lastTimeMoodBoost = DateTime.Now.Ticks;
			this.model.version = ConfigBase.instance.initialPlayerVersion;
			this.model.musicVolume = (float)ConfigBase.instance.initialVolumeLevel;
			this.model.sfxVolume = (float)ConfigBase.instance.initialVolumeLevel;
			this.model.ambientVolume = (float)ConfigBase.instance.initialVolumeLevel;
			new DateTime(this.model.creationTime);
			ProtoIO.SaveToFile<ProtoSerializer, PlayerModel>("player.bytes", GGFileIO.instance, this.model);
		}
		if (this.model.version <= 7)
		{
			this.model.version = ConfigBase.instance.initialPlayerVersion;
			this.model.ambientVolume = this.model.sfxVolume;
			ProtoIO.SaveToFile<ProtoSerializer, PlayerModel>("player.bytes", GGFileIO.instance, this.model);
		}
		if (this.walletManager != null)
		{
			this.walletManager.ReloadModel(this.model);
		}
		if (flag)
		{
			ConfigBase instance = ConfigBase.instance;
			this.walletManager.SetCurrency(CurrencyType.coins, instance.initialCoins);
			this.walletManager.SetCurrency(CurrencyType.diamonds, instance.initialStars);
		}
		this.CheckShouldGiveExperience(false);
	}

	public void AddPurchase(InAppPurchaseDAO inAppPurchase)
	{
		if (inAppPurchase == null)
		{
			return;
		}
		if (this.model.purchases == null)
		{
			this.model.purchases = new List<InAppPurchaseDAO>();
		}
		this.model.purchases.Add(inAppPurchase);
		this.Save();
	}

	public bool IsPurchaseConsumed(string token)
	{
		if (this.model.purchases == null)
		{
			return false;
		}
		List<InAppPurchaseDAO> purchases = this.model.purchases;
		for (int i = 0; i < purchases.Count; i++)
		{
			InAppPurchaseDAO inAppPurchaseDAO = purchases[i];
			if (!string.IsNullOrEmpty(inAppPurchaseDAO.receipt) && inAppPurchaseDAO.receipt == token)
			{
				return true;
			}
		}
		return false;
	}

	public List<InAppPurchaseDAO> GetPurchases()
	{
		if (this.model.purchases == null)
		{
			this.model.purchases = new List<InAppPurchaseDAO>();
		}
		return this.model.purchases;
	}

	public string GetName()
	{
		if (string.IsNullOrEmpty(this.Model.name))
		{
			return "You";
		}
		return this.Model.name;
	}

	public bool shouldGiveExperience
	{
		get
		{
			return this.model.rankExperienceLong == 0L && this.model.rankLevelDouble == 0.0 && this.MultiplayerGamesPlayed() > 0;
		}
	}

	public static long GetExperienceToGive(int multiplayerWins, int multiplayerLoses)
	{
		long num = 0L;
		for (int i = 0; i < multiplayerWins; i++)
		{
			if (i < 5)
			{
				num += 10L;
			}
			else if (i < 15)
			{
				num += 15L;
			}
			else if (i < 25)
			{
				num += 20L;
			}
			else if (i < 50)
			{
				num += 25L;
			}
			else if (i < 150)
			{
				num += 30L;
			}
			else
			{
				num += 35L;
			}
		}
		for (int j = 0; j < multiplayerLoses; j++)
		{
			if (j < 15)
			{
				num += 1L;
			}
			else if (j < 25)
			{
				num += 2L;
			}
			else if (j < 50)
			{
				num += 3L;
			}
			else if (j < 150)
			{
				num += 4L;
			}
			else
			{
				num += 5L;
			}
		}
		num = MathEx.Max(num, 0L);
		UnityEngine.Debug.Log("GIVING EXPERINENCE " + num);
		return num;
	}

	public void CheckShouldGiveExperience(bool save)
	{
		if (!this.shouldGiveExperience)
		{
			return;
		}
		long experienceToGive = GGPlayerSettings.GetExperienceToGive(this.model.multiplayerWins, this.model.multiplayerLoses);
		UnityEngine.Debug.Log("GIVING EXPERINENCE " + experienceToGive);
		double rankLevelDouble = this.model.rankLevelDouble;
		this.model.rankExperienceLong = experienceToGive;
		if (save)
		{
			this.Save();
		}
	}

	public void SetName(string name)
	{
		this.model.name = name;
		this.Save();
	}

	public void Save()
	{
		if (this.isSavingSuspended)
		{
			return;
		}
		ProtoIO.SaveToFileCS<PlayerModel>("player.bytes", this.model);
	}

	public void SetEnergy(float energy, DateTime lastTimeTookEnergy)
	{
		this.Model.energy = energy;
		this.Model.lastTimeTookEnergy = lastTimeTookEnergy.Ticks;
		this.Save();
	}

	private PlayerModel model;

	public float overPocketNominationScale = 1.5f;

	private static GGPlayerSettings instance_;

	private WalletManager _003CwalletManager_003Ek__BackingField;

	private bool isSavingSuspended;
}
