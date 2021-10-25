using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class ScrollableSelectRoomScreen : MonoBehaviour
{
	public void Show(ScrollableSelectRoomScreen.ChangeRoomArguments changeRoomArguments)
	{
		this.changeRoomArguments = changeRoomArguments;
		NavigationManager.instance.Push(base.gameObject, false);
	}

	private void Init(ScrollableSelectRoomScreen.ChangeRoomArguments changeRoomArguments)
	{
		GGUtil.Hide(this.widgetsToHide);
		this.roomsPool.Clear();
		RoomsDB instance = ScriptableObjectSingleton<RoomsDB>.instance;
		List<RoomsDB.Room> rooms = instance.rooms;
		RectTransform component = this.roomsPool.parent.GetComponent<RectTransform>();
		float x = this.roomsPool.prefabSizeDelta.x;
		float num = x * (float)rooms.Count + this.spacing * (float)(rooms.Count - 1);
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.x = num;
		sizeDelta.y = this.roomsPool.prefabSizeDelta.y;
		component.sizeDelta = sizeDelta;
		float num2 = -num * 0.5f;
		for (int i = 0; i < rooms.Count; i++)
		{
			RoomsDB.Room room = rooms[i];
			ScrollableSelectRoomScreenButton scrollableSelectRoomScreenButton = this.roomsPool.Next<ScrollableSelectRoomScreenButton>(true);
			RectTransform component2 = scrollableSelectRoomScreenButton.GetComponent<RectTransform>();
			Vector3 localPosition = component2.localPosition;
			localPosition.x = num2 + (float)i * (this.spacing + x) + x * 0.5f;
			localPosition.y = 0f;
			component2.localPosition = localPosition;
			scrollableSelectRoomScreenButton.Init(room);
		}
		this.roomsPool.HideNotUsed();
		this.animationEnumerator = null;
		if (changeRoomArguments == null)
		{
			ScrollableSelectRoomScreenButton button = this.GetButton(instance.ActiveRoom);
			this.CenterContainerToCenterChild(button.GetComponent<RectTransform>());
			return;
		}
		this.animationEnumerator = this.DoChangeRoomsAnimation(changeRoomArguments);
		this.animationEnumerator.MoveNext();
	}

	private IEnumerator DoChangeRoomsAnimation(ScrollableSelectRoomScreen.ChangeRoomArguments changeRoom)
	{
		return new ScrollableSelectRoomScreen._003CDoChangeRoomsAnimation_003Ed__9(0)
		{
			_003C_003E4__this = this,
			changeRoom = changeRoom
		};
	}

	private ScrollableSelectRoomScreenButton GetButton(RoomsDB.Room room)
	{
		for (int i = 0; i < this.roomsPool.usedObjects.Count; i++)
		{
			ScrollableSelectRoomScreenButton component = this.roomsPool.usedObjects[i].GetComponent<ScrollableSelectRoomScreenButton>();
			if (component.room == room)
			{
				return component;
			}
		}
		return null;
	}

	private void CenterContainerToCenterChild(RectTransform item)
	{
		RectTransform component = item.parent.GetComponent<RectTransform>();
		Vector3 localPosition = component.localPosition;
		Vector3 localPosition2 = item.localPosition;
		localPosition.x = -localPosition2.x;
		component.localPosition = localPosition;
	}

	private IEnumerator MoveCenterContainerToCenterChild(RectTransform item, float duration)
	{
		return new ScrollableSelectRoomScreen._003CMoveCenterContainerToCenterChild_003Ed__12(0)
		{
			item = item,
			duration = duration
		};
	}

	private void OnEnable()
	{
		this.Init(this.changeRoomArguments);
		this.changeRoomArguments = null;
	}

	private void Update()
	{
		if (this.animationEnumerator != null)
		{
			this.animationEnumerator.MoveNext();
		}
	}

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private Transform stopInteractionWidget;

	[SerializeField]
	private ComponentPool roomsPool = new ComponentPool();

	[SerializeField]
	private float spacing = 80f;

	private ScrollableSelectRoomScreen.ChangeRoomArguments changeRoomArguments;

	private IEnumerator animationEnumerator;

	public class ChangeRoomArguments
	{
		public RoomsDB.Room passedRoom;

		public RoomsDB.Room unlockedRoom;
	}

	private sealed class _003CDoChangeRoomsAnimation_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoChangeRoomsAnimation_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this._003C_003E1__state;
			ScrollableSelectRoomScreen scrollableSelectRoomScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGUtil.Show(scrollableSelectRoomScreen.stopInteractionWidget);
				this._003CpassedRoom_003E5__2 = scrollableSelectRoomScreen.GetButton(this.changeRoom.passedRoom);
				this._003CunlockedRoom_003E5__3 = scrollableSelectRoomScreen.GetButton(this.changeRoom.unlockedRoom);
				if (this._003CunlockedRoom_003E5__3 != null)
				{
					this._003CunlockedRoom_003E5__3.ShowLocked();
				}
				this._003Cdelay_003E5__4 = 0f;
				this._003Ctime_003E5__5 = 0f;
				if (this._003CpassedRoom_003E5__2 != null)
				{
					this._003CpassedRoom_003E5__2.ShowOpenNotPassed();
					scrollableSelectRoomScreen.CenterContainerToCenterChild(this._003CpassedRoom_003E5__2.GetComponent<RectTransform>());
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				goto IL_13D;
			case 1:
				this._003C_003E1__state = -1;
				this._003CpassedRoom_003E5__2.ShowPassedAnimation();
				this._003Cdelay_003E5__4 = this._003CpassedRoom_003E5__2.passAnimationDuration;
				this._003Ctime_003E5__5 = 0f;
				break;
			case 2:
				this._003C_003E1__state = -1;
				break;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_183;
			case 4:
				this._003C_003E1__state = -1;
				goto IL_1E2;
			default:
				return false;
			}
			if (this._003Ctime_003E5__5 <= this._003Cdelay_003E5__4)
			{
				this._003Ctime_003E5__5 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			IL_13D:
			if (!(this._003CunlockedRoom_003E5__3 != null))
			{
				goto IL_1F7;
			}
			this._003Cenumerator_003E5__6 = scrollableSelectRoomScreen.MoveCenterContainerToCenterChild(this._003CunlockedRoom_003E5__3.GetComponent<RectTransform>(), 0.5f);
			IL_183:
			if (this._003Cenumerator_003E5__6.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 3;
				return true;
			}
			this._003CunlockedRoom_003E5__3.ShowUnlockAnimation();
			this._003Cdelay_003E5__4 = this._003CunlockedRoom_003E5__3.unlockAnimationDuration;
			this._003Ctime_003E5__5 = 0f;
			IL_1E2:
			if (this._003Ctime_003E5__5 < this._003Cdelay_003E5__4)
			{
				this._003Ctime_003E5__5 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 4;
				return true;
			}
			this._003Cenumerator_003E5__6 = null;
			IL_1F7:
			GGUtil.Hide(scrollableSelectRoomScreen.stopInteractionWidget);
			return false;
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScrollableSelectRoomScreen _003C_003E4__this;

		public ScrollableSelectRoomScreen.ChangeRoomArguments changeRoom;

		private ScrollableSelectRoomScreenButton _003CpassedRoom_003E5__2;

		private ScrollableSelectRoomScreenButton _003CunlockedRoom_003E5__3;

		private float _003Cdelay_003E5__4;

		private float _003Ctime_003E5__5;

		private IEnumerator _003Cenumerator_003E5__6;
	}

	private sealed class _003CMoveCenterContainerToCenterChild_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CMoveCenterContainerToCenterChild_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this._003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
			}
			else
			{
				this._003C_003E1__state = -1;
				this._003Ccontainer_003E5__2 = this.item.parent.GetComponent<RectTransform>();
				Vector3 localPosition = this._003Ccontainer_003E5__2.localPosition;
				Vector3 localPosition2 = this.item.localPosition;
				this._003CstartPosition_003E5__3 = localPosition;
				this._003CendPosition_003E5__4 = this._003CstartPosition_003E5__3;
				this._003CendPosition_003E5__4.x = -localPosition2.x;
				this._003Ctime_003E5__5 = 0f;
			}
			if (this._003Ctime_003E5__5 > this.duration)
			{
				return false;
			}
			this._003Ctime_003E5__5 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, this.duration, this._003Ctime_003E5__5);
			Vector3 localPosition3 = Vector3.Lerp(this._003CstartPosition_003E5__3, this._003CendPosition_003E5__4, t);
			this._003Ccontainer_003E5__2.localPosition = localPosition3;
			this._003C_003E2__current = null;
			this._003C_003E1__state = 1;
			return true;
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RectTransform item;

		public float duration;

		private RectTransform _003Ccontainer_003E5__2;

		private Vector3 _003CstartPosition_003E5__3;

		private Vector3 _003CendPosition_003E5__4;

		private float _003Ctime_003E5__5;
	}
}
