using System;
using System.Collections.Generic;
using ProtoModels;

public class GGServerMessages : BehaviourSingletonInit<GGServerMessages>
{
	public event GGServerMessages.OnMessageExecuted onMessageExecuted;

	public override void Init()
	{
		this.messageHandlers = GGMessageHandlerConfig.instance.GetHandlers();
	}

	public void GetMessages(GGServerRequestsBackend.OnComplete onComplete)
	{
		GGServerRequestsBackend.GGServerPlayerMessages messagesRequest = new GGServerRequestsBackend.GGServerPlayerMessages();
		BehaviourSingletonInit<GGServerRequestsBackend>.instance.GetMessagesForPlayer(messagesRequest, onComplete);
	}

	public void ExecuteMessageAttachment(string key, ServerMessages.GGServerMessage.Attachment attachment)
	{
		GGDebug.DebugLog(key);
		GGMessageHandlerConfig.GGServerMessageHandlerBase ggserverMessageHandlerBase;
		this.messageHandlers.TryGetValue(key, out ggserverMessageHandlerBase);
		if (ggserverMessageHandlerBase != null)
		{
			ggserverMessageHandlerBase.Execute(attachment);
		}
		else
		{
			this.defaultHandler.Execute(attachment);
		}
		if (this.onMessageExecuted != null)
		{
			this.onMessageExecuted();
		}
	}

	private Dictionary<string, GGMessageHandlerConfig.GGServerMessageHandlerBase> messageHandlers;

	private GGMessageHandlerConfig.GGServerMessageHandlerBase defaultHandler = new GGMessageHandlerConfig.GGServerMessageHandlerBase();

	public delegate void OnMessageExecuted();
}
