using System;
using System.Collections.Generic;
using UnityEngine;

public class GGNotificationCenter : BehaviourSingletonInit<GGNotificationCenter>
{
	public event GGNotificationCenter.GGNotificationCenterDelegate onMessage;

	public GGNotificationCenter.EventDispatcher defaultEventDispatcher
	{
		get
		{
			return this._003CdefaultEventDispatcher_003Ek__BackingField;
		}
		protected set
		{
			this._003CdefaultEventDispatcher_003Ek__BackingField = value;
		}
	}

	public override void Init()
	{
		base.Init();
		this.defaultEventDispatcher = new GGNotificationCenter.EventDispatcher();
		this.eventDispatchers.Add(this.defaultEventDispatcher);
	}

	public void AddEventDispatcher(GGNotificationCenter.EventDispatcher ed)
	{
		this.eventDispatchers.Add(ed);
	}

	public void RemoveEventDispatcher(GGNotificationCenter.EventDispatcher ed)
	{
		this.eventDispatchers.Remove(ed);
	}

	protected void NotifyEventDispatchers(Type type, object data)
	{
		for (int i = 0; i < this.eventDispatchers.Count; i++)
		{
			GGNotificationCenter.EventDispatcher eventDispatcher = this.eventDispatchers[i];
			if (eventDispatcher != null)
			{
				eventDispatcher.NotifyListeners(type, data);
			}
		}
	}

	public void Broadcast(string message)
	{
		UnityEngine.Debug.Log("GGNotificationCenter.Broadcast('" + message + "')");
		this.onMessage(message);
	}

	public void BroadcastEvent(object e)
	{
		this.NotifyEventDispatchers(e.GetType(), e);
	}

	public const string PurchaseIAPSuccess = "Purchase.IAP.Success";

	protected List<GGNotificationCenter.EventDispatcher> eventDispatchers = new List<GGNotificationCenter.EventDispatcher>();

	private GGNotificationCenter.EventDispatcher _003CdefaultEventDispatcher_003Ek__BackingField;

	public delegate void GGNotificationCenterDelegate(string message);

	public class EventDispatcher
	{
		public void AssignListener(Type type, GGNotificationCenter.EventDispatcher.EventDelegateListener listener)
		{
			GGNotificationCenter.EventDispatcher.EventDelegateList eventDelegateList;
			if (this.eventMap.ContainsKey(type))
			{
				eventDelegateList = this.eventMap[type];
			}
			else
			{
				eventDelegateList = new GGNotificationCenter.EventDispatcher.EventDelegateList();
				this.eventMap.Add(type, eventDelegateList);
			}
			bool flag = false;
			for (int i = 0; i < eventDelegateList.eventList.Count; i++)
			{
				if (eventDelegateList.eventList[i].onMessageCall == listener)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return;
			}
			GGNotificationCenter.EventDispatcher.EventDelegate eventDelegate = new GGNotificationCenter.EventDispatcher.EventDelegate();
			eventDelegate.onMessageCall = listener;
			eventDelegateList.eventList.Add(eventDelegate);
		}

		public void NotifyListeners(Type type, object data)
		{
			GGNotificationCenter.EventDispatcher.EventDelegateList eventDelegateList = null;
			if (this.eventMap.ContainsKey(type))
			{
				eventDelegateList = this.eventMap[type];
			}
			if (eventDelegateList == null)
			{
				return;
			}
			eventDelegateList.NotifyListeners(data);
		}

		protected Dictionary<Type, GGNotificationCenter.EventDispatcher.EventDelegateList> eventMap = new Dictionary<Type, GGNotificationCenter.EventDispatcher.EventDelegateList>();

		public delegate void EventDelegateListener(object data);

		public class EventDelegate
		{
			public GGNotificationCenter.EventDispatcher.EventDelegateListener onMessageCall;
		}

		public class EventDelegateList
		{
			public void NotifyListeners(object data)
			{
				for (int i = 0; i < this.eventList.Count; i++)
				{
					GGNotificationCenter.EventDispatcher.EventDelegate eventDelegate = this.eventList[i];
					if (eventDelegate.onMessageCall != null)
					{
						try
						{
							eventDelegate.onMessageCall(data);
						}
						catch
						{
							UnityEngine.Debug.Log("ERROR IN DELEGATE");
						}
					}
				}
			}

			public List<GGNotificationCenter.EventDispatcher.EventDelegate> eventList = new List<GGNotificationCenter.EventDispatcher.EventDelegate>();
		}
	}
}
