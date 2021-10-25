using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class LevelDB : ScriptableObjectSingleton<LevelDB>
	{
		public static LevelDB NamedInstance(string levelDBName)
		{
			LevelDB levelDB = null;
			if (LevelDB.levelDBDictionary.TryGetValue(levelDBName, out levelDB))
			{
				return levelDB;
			}
			levelDB = Resources.Load<LevelDB>(levelDBName);
			if (levelDB == null)
			{
				levelDB = ScriptableObjectSingleton<LevelDB>.instance;
			}
			levelDB.UpdateData();
			LevelDB.levelDBDictionary.Add(levelDBName, levelDB);
			return levelDB;
		}

		public string currentLevelName
		{
			get
			{
				return this._currentLevelName;
			}
			set
			{
				this._currentLevelName = value;
			}
		}

		public int currentLevelIndex
		{
			get
			{
				for (int i = 0; i < this.levels.Count; i++)
				{
					if (this.levels[i].name == this.currentLevelName)
					{
						return i;
					}
				}
				return 0;
			}
		}

		public LevelDefinition Get(string levelName)
		{
			for (int i = 0; i < this.levels.Count; i++)
			{
				LevelDefinition levelDefinition = this.levels[i];
				if (levelDefinition.name == levelName)
				{
					return levelDefinition;
				}
			}
			return null;
		}

		private static Dictionary<string, LevelDB> levelDBDictionary = new Dictionary<string, LevelDB>();

		[SerializeField]
		private string _currentLevelName = "";

		public List<LevelDefinition> levels = new List<LevelDefinition>();
	}
}
