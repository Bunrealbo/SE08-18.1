using System;
using System.Collections.Generic;
using ProtoModels;
using UnityEngine;

namespace GGOptimize
{
	public class Optimize
	{
		protected ExperimentsData.ExperimentData GetExperimentData(Experiment e)
		{
			if (this.dataModel == null)
			{
				this.dataModel = new ExperimentsData();
			}
			if (this.dataModel.experiments == null)
			{
				this.dataModel.experiments = new List<ExperimentsData.ExperimentData>();
			}
			for (int i = 0; i < this.dataModel.experiments.Count; i++)
			{
				ExperimentsData.ExperimentData experimentData = this.dataModel.experiments[i];
				if (experimentData.guid == e.guid)
				{
					return experimentData;
				}
			}
			return null;
		}

		public bool IsNewUserOnExperiment(Experiment e)
		{
			ExperimentsData.ExperimentData experimentData = this.GetExperimentData(e);
			return experimentData != null && experimentData.isNewUserOnExperiment;
		}

		protected int userBucket
		{
			get
			{
				if (this.dataModel == null)
				{
					this.dataModel = new ExperimentsData();
				}
				return this.dataModel.userBucket;
			}
		}

		public int GetUserBucket(Experiment experiment)
		{
			if (!experiment.useLocalBucket)
			{
				return this.userBucket;
			}
			ExperimentsData.ExperimentData experimentData = this.GetExperimentData(experiment);
			GGPlayerSettings instance = GGPlayerSettings.instance;
			if ((experimentData == null || !experimentData.isBucketSet) && instance.Model.experiments != null)
			{
				this.dataModel = instance.Model.experiments;
			}
			experimentData = this.GetExperimentData(experiment);
			if (experimentData == null || !experimentData.isBucketSet)
			{
				if (experimentData == null)
				{
					experimentData = new ExperimentsData.ExperimentData();
					experimentData.guid = experiment.guid;
					this.dataModel.experiments.Add(experimentData);
				}
				experimentData.isBucketSet = true;
				experimentData.userBucket = UnityEngine.Random.Range(0, 100);
				instance.Model.experiments = this.dataModel;
				instance.Save();
			}
			return experimentData.userBucket;
		}

		protected void SetUserIsNewOnExperiment(Experiment experiment)
		{
			if (this.dataModel == null)
			{
				this.dataModel = new ExperimentsData();
			}
			if (this.dataModel.experiments == null)
			{
				this.dataModel.experiments = new List<ExperimentsData.ExperimentData>();
			}
			ExperimentsData.ExperimentData experimentData = this.GetExperimentData(experiment);
			GGPlayerSettings instance = GGPlayerSettings.instance;
			if (experimentData == null)
			{
				experimentData = new ExperimentsData.ExperimentData();
				experimentData.guid = experiment.guid;
				this.dataModel.experiments.Add(experimentData);
			}
			experimentData.isNewUserOnExperiment = true;
		}

		public List<Experiment> GetActiveExperiments()
		{
			this.activeExperiments_.Clear();
			List<Experiment> experiments = this.experimentsDefinition.experiments;
			for (int i = 0; i < experiments.Count; i++)
			{
				Experiment experiment = experiments[i];
				if (experiment.IsActive(this))
				{
					this.activeExperiments_.Add(experiment);
				}
			}
			return this.activeExperiments_;
		}

		public NamedProperty GetNamedProperty(string name)
		{
			List<Experiment> activeExperiments = this.GetActiveExperiments();
			NamedProperty namedProperty = null;
			for (int i = activeExperiments.Count - 1; i >= 0; i--)
			{
				Experiment experiment = activeExperiments[i];
				namedProperty = experiment.GetActiveVariation(this.GetUserBucket(experiment)).GetProperty(name);
				if (namedProperty != null)
				{
					break;
				}
			}
			if (namedProperty == null)
			{
				namedProperty = this.experimentsDefinition.defaultProperties.GetProperty(name);
			}
			return namedProperty;
		}

		public int GetInt(string propertyName, int defaultValue)
		{
			NamedProperty namedProperty = this.GetNamedProperty(propertyName);
			if (namedProperty != null)
			{
				return namedProperty.GetInt();
			}
			return defaultValue;
		}

		public bool GetBool(string propertyName, bool defaultValue)
		{
			NamedProperty namedProperty = this.GetNamedProperty(propertyName);
			if (namedProperty != null)
			{
				return namedProperty.GetBool();
			}
			return defaultValue;
		}

		public string GetString(string propertyName, string defaultValue)
		{
			NamedProperty namedProperty = this.GetNamedProperty(propertyName);
			if (namedProperty != null)
			{
				return namedProperty.GetString();
			}
			return defaultValue;
		}

		public void Init(ExperimentsDefinition experimentsDefinition)
		{
			this.experimentsDefinition = experimentsDefinition;
			GGPlayerSettings instance = GGPlayerSettings.instance;
			this.dataModel = instance.Model.experiments;
			if (this.dataModel == null || !this.dataModel.isUserBucketSet)
			{
				this.dataModel = new ExperimentsData();
				List<Experiment> experiments = experimentsDefinition.experiments;
				if (instance.Model.version == ConfigBase.instance.initialPlayerVersion)
				{
					for (int i = 0; i < experiments.Count; i++)
					{
						Experiment experiment = experiments[i];
						if (experiment.acceptsNewUsers)
						{
							this.SetUserIsNewOnExperiment(experiment);
						}
					}
				}
				this.dataModel.userBucket = UnityEngine.Random.Range(0, 100);
				this.dataModel.isUserBucketSet = true;
				instance.Model.experiments = this.dataModel;
				instance.Save();
			}
		}

		public ExperimentsDefinition experimentsDefinition = new ExperimentsDefinition();

		protected ExperimentsData dataModel;

		protected List<Experiment> activeExperiments_ = new List<Experiment>();
	}
}
