using System;
using System.Collections.Generic;
using GGOptimize;

public class GGAB : SingletonInit<GGAB>
{
	public override void Init()
	{
		base.Init();
		this.optimize = new Optimize();
		this.optimize.Init(ExperimentDefinitionAssetStore.instance.experimentsDefinition);
	}

	public static int GetInt(string propertyName, int defaultValue)
	{
		return SingletonInit<GGAB>.instance.optimize.GetInt(propertyName, defaultValue);
	}

	public static bool GetBool(string propertyName, bool defaultValue)
	{
		return SingletonInit<GGAB>.instance.optimize.GetBool(propertyName, defaultValue);
	}

	public static string GetString(string propertyName, string defaultValue)
	{
		return SingletonInit<GGAB>.instance.optimize.GetString(propertyName, defaultValue);
	}

	public static List<Experiment> GetActiveExperiments()
	{
		return SingletonInit<GGAB>.instance.optimize.GetActiveExperiments();
	}

	public Optimize optimize;
}
