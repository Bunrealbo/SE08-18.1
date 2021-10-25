using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsDB : ScriptableObject
{
	public static SceneObjectsDB instance
	{
		get
		{
			if (SceneObjectsDB._instance == null)
			{
				SceneObjectsDB._instance = Resources.Load<SceneObjectsDB>("SceneObjectsDB");
			}
			return SceneObjectsDB._instance;
		}
	}

	public List<SceneObjectsDB.SceneDependencies> scenes = new List<SceneObjectsDB.SceneDependencies>();

	public SceneObjectsDB.MarginsInfo maxMargins = new SceneObjectsDB.MarginsInfo();

	public SceneObjectsDB.MarginsInfo minMargins = new SceneObjectsDB.MarginsInfo();

	private static SceneObjectsDB _instance;

	[Serializable]
	public class SceneDependencies
	{
		public string name;

		public string assetsPath;

		public List<SceneObjectsDB.SceneObjectInfo> objectInfos = new List<SceneObjectsDB.SceneObjectInfo>();
	}

	[Serializable]
	public class MarginsInfo
	{
		public Vector2 visibleScenePercent
		{
			get
			{
				return new Vector2(1f - (float)(this.leftMargin + this.rightMargin) / (float)this.totalWidth, 1f - (float)(this.topMargin + this.bottomMargin) / (float)this.totalHeight);
			}
		}

		public Vector2 marginsOffset
		{
			get
			{
				return new Vector2((float)(-(float)this.leftMargin + this.rightMargin), (float)(-(float)this.topMargin + this.bottomMargin));
			}
		}

		public int totalWidth;

		public int leftMargin;

		public int rightMargin;

		public int totalHeight;

		public int bottomMargin;

		public int topMargin;
	}

	[Serializable]
	public class SceneObjectInfo
	{
		public string objectName;

		public string displayName;

		public List<string> backwardDependencies = new List<string>();

		public SingleCurrencyPrice price = new SingleCurrencyPrice(1, CurrencyType.diamonds);

		public bool autoSelect;

		public List<string> toSayAfterOpen = new List<string>();

		public int groupIndex = -1;

		public bool isMarkersAbove;

		public string thumbnailNamePrefix;

		public string iconSpriteName;

		public SceneObjectsDB.SceneObjectInfo.AnimationType animationType;

		public string animationSettingsName;

		public bool usedDashedLineForIconHandlePosition;

		public bool hideCharacterWhenSelectingVariations;

		public bool isVisualObjectOverriden;

		public DecoratingSceneConfig.VisualObjectOverride objectOverride = new DecoratingSceneConfig.VisualObjectOverride();

		public string sceneName;

		public enum AnimationType
		{
			ScaleAnimation
		}
	}
}
