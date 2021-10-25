using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGOptimize
{
	[Serializable]
	public class Experiment
	{
		public bool IsActive(Optimize optimize)
		{
			return !this.isArchived && (!this.onlyNewUsers || optimize.IsNewUserOnExperiment(this)) && this.bucketRange.IsAcceptable(optimize.GetUserBucket(this));
		}

		public Variation GetActiveVariation(int userBucket)
		{
			if (!this.bucketRange.IsAcceptable(userBucket))
			{
				return null;
			}
			if (this.variations.Count == 0)
			{
				return null;
			}
			int num = this.bucketRange.count / this.variations.Count;
			int index = Mathf.Clamp((userBucket - this.bucketRange.min) / num, 0, this.variations.Count - 1);
			return this.variations[index];
		}

		public string name;

		public string guid;

		public BucketRange bucketRange = new BucketRange();

		public string customDimensionToMark;

		public bool onlyNewUsers;

		public bool acceptsNewUsers = true;

		public bool useLocalBucket;

		public bool isArchived;

		public List<Variation> variations = new List<Variation>();
	}
}
