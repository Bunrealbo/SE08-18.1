using System;
using GGOptimize;
using UnityEngine;

public class ExperimentDefinitionAssetStore : ScriptableObjectSingleton<ExperimentDefinitionAssetStore>
{
	public new void OnDestroy()
	{
		ExperimentDefinitionAssetStore.applicationIsQuitting = true;
	}

	public new static ExperimentDefinitionAssetStore instance
	{
		get
		{
			if (ExperimentDefinitionAssetStore.instance_ == null)
			{
				if (ExperimentDefinitionAssetStore.applicationIsQuitting)
				{
					return null;
				}
				ExperimentDefinitionAssetStore.instance_ = Resources.Load<ExperimentDefinitionAssetStore>(ConfigBase.instance.experimentsResourceName);
				if (ExperimentDefinitionAssetStore.instance_ == null)
				{
					ExperimentDefinitionAssetStore.instance_ = Resources.Load<ExperimentDefinitionAssetStore>(typeof(ExperimentDefinitionAssetStore).ToString());
				}
			}
			return ExperimentDefinitionAssetStore.instance_;
		}
	}

	private static bool applicationIsQuitting;

	protected new static ExperimentDefinitionAssetStore instance_;

	public ExperimentsDefinition experimentsDefinition = new ExperimentsDefinition();
}
