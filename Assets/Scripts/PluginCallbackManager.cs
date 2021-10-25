using System;
using System.Collections.Generic;
using UnityEngine;

public class PluginCallbackManager : BehaviourSingletonInit<PluginCallbackManager>
{
	public string RegisterCallback(PluginCallbackManager.CallbackDelegate callback)
	{
		PluginCallbackManager.nextCallbackId++;
		string text = PluginCallbackManager.nextCallbackId.ToString();
		this.callbacks.Add(text, callback);
		return text;
	}

	public void OnCallCallback(string jsonMessage)
	{
		try
		{
			UnityEngine.Debug.Log("RECEIEVED: \"" + jsonMessage + "\"");
			PluginCallbackManager.Response.BaseParameters baseParameters = JsonUtility.FromJson<PluginCallbackManager.Response.BaseParameters>(jsonMessage);
			if (baseParameters != null)
			{
				string callback_id = baseParameters.callback_id;
				PluginCallbackManager.CallbackDelegate callbackDelegate = null;
				if (this.callbacks.TryGetValue(callback_id, out callbackDelegate))
				{
					this.callbacks.Remove(callback_id);
					if (callbackDelegate != null)
					{
						callbackDelegate(new PluginCallbackManager.Response(baseParameters, jsonMessage));
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private Dictionary<string, PluginCallbackManager.CallbackDelegate> callbacks = new Dictionary<string, PluginCallbackManager.CallbackDelegate>();

	private static int nextCallbackId;

	public class Response
	{
		public Response(PluginCallbackManager.Response.BaseParameters baseParameters, string jsonResponse)
		{
			this.baseParameters = baseParameters;
			this.jsonResponse = jsonResponse;
		}

		public string jsonResponse;

		public PluginCallbackManager.Response.BaseParameters baseParameters;

		[Serializable]
		public class BaseParameters
		{
			public string callback_id;
		}
	}

	public delegate void CallbackDelegate(PluginCallbackManager.Response msg);
}
