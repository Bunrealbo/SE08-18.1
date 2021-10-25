using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public abstract class LightningBoltPathScriptBase : LightningBoltPrefabScriptBase
	{
		protected List<GameObject> GetCurrentPathObjects()
		{
			this.currentPathObjects.Clear();
			if (this.LightningPath != null)
			{
				foreach (GameObject gameObject in this.LightningPath)
				{
					if (gameObject != null && gameObject.activeInHierarchy)
					{
						this.currentPathObjects.Add(gameObject);
					}
				}
			}
			return this.currentPathObjects;
		}

		protected override LightningBoltParameters OnCreateParameters()
		{
			LightningBoltParameters lightningBoltParameters = base.OnCreateParameters();
			lightningBoltParameters.Generator = LightningGenerator.GeneratorInstance;
			return lightningBoltParameters;
		}

		public List<GameObject> LightningPath;

		private readonly List<GameObject> currentPathObjects = new List<GameObject>();
	}
}
