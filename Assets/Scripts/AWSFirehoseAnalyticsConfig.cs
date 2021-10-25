using System;
using UnityEngine;

public class AWSFirehoseAnalyticsConfig : ScriptableObjectSingleton<AWSFirehoseAnalyticsConfig>
{
	[SerializeField]
	public string kinesisFirehoseStreamName;

	[SerializeField]
	public bool sendEventsInEditor;
}
