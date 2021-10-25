using System;
using System.Collections.Generic;
using ProtoModels;
using UnityEngine;

public class GGMessageHandlerConfig : ScriptableObject
{
	public static GGMessageHandlerConfig instance
	{
		get
		{
			if (GGMessageHandlerConfig.instance_ == null)
			{
				GGMessageHandlerConfig.instance_ = (Resources.Load("GGServerAssets/GGMessageHandlerConfig", typeof(GGMessageHandlerConfig)) as GGMessageHandlerConfig);
			}
			if (GGMessageHandlerConfig.instance_ == null)
			{
				UnityEngine.Debug.LogError("No message config defined");
				GGMessageHandlerConfig.instance_ = new GGMessageHandlerConfig();
			}
			return GGMessageHandlerConfig.instance_;
		}
	}

	public virtual Dictionary<string, GGMessageHandlerConfig.GGServerMessageHandlerBase> GetHandlers()
	{
		return new Dictionary<string, GGMessageHandlerConfig.GGServerMessageHandlerBase>();
	}

	private static GGMessageHandlerConfig instance_;

	public class GGServerMessageHandlerBase
	{
		public virtual void Execute(ServerMessages.GGServerMessage.Attachment attachment)
		{
			GGDebug.DebugLog("Default message");
		}
	}
}
