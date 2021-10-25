using System;
using System.Collections.Generic;
using GGMatch3;
using ProtoModels;
using UnityEngine;

public class RateCallerSettings : ScriptableObjectSingleton<RateCallerSettings>
{
	private RateCallerSettings.RateIntervalConfig checkInterval
	{
		get
		{
			if (this.checkIntervals.Count == 0)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < this.checkIntervals.Count; i++)
			{
				RateCallerSettings.RateIntervalConfig result = this.checkIntervals[i];
				num++;
				if (this.timesShown < num)
				{
					return result;
				}
			}
			return null;
		}
	}

	public void OnDialogShow()
	{
		this.model.timestampWhenAskedForRate = DateTime.Now.Ticks;
		AppRateDAO appRateDAO = this.model;
		int timesRateShown = appRateDAO.timesRateShown;
		appRateDAO.timesRateShown = timesRateShown + 1;
		this.model.stagesPassedSinceLastRate = Match3StagesDB.instance.passedStages;
		this.Save();
	}

	public void OnUserRated()
	{
		this.model.userRatedApp = true;
		this.Save();
	}

	public void OnUserNotLike()
	{
		this.model.userDoesNotLikeApp = true;
		this.Save();
	}

	public bool ShouldShow(int levelsPassed)
	{
		return !this.model.userRatedApp && levelsPassed - this.model.stagesPassedSinceLastRate >= this.minStagesBetweenRates && this.checkInterval != null && this.checkInterval.ShouldShow(levelsPassed);
	}

	public long lastShownTime
	{
		get
		{
			return this.model.timestampWhenAskedForRate;
		}
	}

	public int timesShown
	{
		get
		{
			return this.model.timesRateShown;
		}
	}

	protected override void UpdateData()
	{
		base.UpdateData();
		this.ReloadModel();
		SingletonInit<FileIOChanges>.instance.OnChange(new FileIOChanges.OnDataChangedDelegate(this.ReloadModel));
	}

	public void ReloadModel()
	{
		if (!ProtoIO.LoadFromFileLocal<AppRateDAO>(RateCallerSettings.RateFilename, out this.model))
		{
			this.model = new AppRateDAO();
		}
	}

	public void Save()
	{
		ProtoIO.SaveToFileCS<AppRateDAO>(RateCallerSettings.RateFilename, this.model);
	}

	public void Reset()
	{
		this.model.isUserEnjoying = false;
		this.model.playerGamesSinceLastRate = 0;
		this.model.stagesPassedSinceLastRate = 0;
		this.model.timesRateShown = 0;
		this.model.timestampWhenAskedForRate = 0L;
		this.model.userRatedApp = false;
		this.model.multiplayerGamesSinceLastRate = 0;
		this.Save();
	}

	private AppRateDAO model;

	private static string RateFilename = "internalRB.byte";

	[SerializeField]
	private int minStagesBetweenRates = 5;

	[SerializeField]
	private List<RateCallerSettings.RateIntervalConfig> checkIntervals;

	[Serializable]
	public class RateIntervalConfig
	{
		public int levelsPassedThreshold
		{
			get
			{
				return this._levelsPassedThreshold;
			}
		}

		public bool ShouldShow(int levelsPassed)
		{
			return levelsPassed >= this.levelsPassedThreshold;
		}

		[SerializeField]
		private int _levelsPassedThreshold;
	}
}
