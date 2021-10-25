using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class ScrollableSelectCarScreen : MonoBehaviour
{
	public void Show(ScrollableSelectCarScreen.ChangeCarArguments changeCarArguments)
	{
		this.changeCarArguments = changeCarArguments;
		NavigationManager.instance.Push(base.gameObject, false);
	}

	private void Init(ScrollableSelectCarScreen.ChangeCarArguments changeRoomArguments)
	{
		GGUtil.Hide(this.widgetsToHide);
		this.roomsPool.Clear();
		CarsDB instance = ScriptableObjectSingleton<CarsDB>.instance;
		List<CarsDB.Car> carsList = instance.carsList;
		RectTransform component = this.roomsPool.parent.GetComponent<RectTransform>();
		float x = this.roomsPool.prefabSizeDelta.x;
		float num = x * (float)carsList.Count + this.spacing * (float)(carsList.Count - 1);
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.x = num;
		sizeDelta.y = this.roomsPool.prefabSizeDelta.y;
		component.sizeDelta = sizeDelta;
		float num2 = -num * 0.5f;
		for (int i = 0; i < carsList.Count; i++)
		{
			CarsDB.Car car = carsList[i];
			ScrollableSelectCarScreenButton scrollableSelectCarScreenButton = this.roomsPool.Next<ScrollableSelectCarScreenButton>(true);
			RectTransform component2 = scrollableSelectCarScreenButton.GetComponent<RectTransform>();
			Vector3 localPosition = component2.localPosition;
			localPosition.x = num2 + (float)i * (this.spacing + x) + x * 0.5f;
			localPosition.y = 0f;
			component2.localPosition = localPosition;
			scrollableSelectCarScreenButton.Init(car);
		}
		this.roomsPool.HideNotUsed();
		this.animationEnumerator = null;
		if (changeRoomArguments == null)
		{
			ScrollableSelectCarScreenButton button = this.GetButton(instance.Active);
			this.CenterContainerToCenterChild(button.GetComponent<RectTransform>());
			return;
		}
		this.animationEnumerator = this.DoChangeRoomsAnimation(changeRoomArguments);
		this.animationEnumerator.MoveNext();
	}

	private IEnumerator DoChangeRoomsAnimation(ScrollableSelectCarScreen.ChangeCarArguments changeRoom)
	{
		return new ScrollableSelectCarScreen._003CDoChangeRoomsAnimation_003Ed__9(0)
		{
			_003C_003E4__this = this,
			changeRoom = changeRoom
		};
	}

	private ScrollableSelectCarScreenButton GetButton(CarsDB.Car car)
	{
		for (int i = 0; i < this.roomsPool.usedObjects.Count; i++)
		{
			ScrollableSelectCarScreenButton component = this.roomsPool.usedObjects[i].GetComponent<ScrollableSelectCarScreenButton>();
			if (component.car == car)
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
		return new ScrollableSelectCarScreen._003CMoveCenterContainerToCenterChild_003Ed__12(0)
		{
			item = item,
			duration = duration
		};
	}

	private void OnEnable()
	{
		this.Init(this.changeCarArguments);
		this.changeCarArguments = null;
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

	private ScrollableSelectCarScreen.ChangeCarArguments changeCarArguments;

	private IEnumerator animationEnumerator;

	public class ChangeCarArguments
	{
		public CarsDB.Car passedCar;

		public CarsDB.Car unlockedCar;
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
			ScrollableSelectCarScreen scrollableSelectCarScreen = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				GGUtil.Show(scrollableSelectCarScreen.stopInteractionWidget);
				this._003CpassedRoom_003E5__2 = scrollableSelectCarScreen.GetButton(this.changeRoom.passedCar);
				this._003CunlockedRoom_003E5__3 = scrollableSelectCarScreen.GetButton(this.changeRoom.unlockedCar);
				if (this._003CunlockedRoom_003E5__3 != null)
				{
					this._003CunlockedRoom_003E5__3.ShowLocked();
				}
				this._003Cdelay_003E5__4 = 0f;
				this._003Ctime_003E5__5 = 0f;
				if (this._003CpassedRoom_003E5__2 != null)
				{
					this._003CpassedRoom_003E5__2.ShowOpenNotPassed();
					scrollableSelectCarScreen.CenterContainerToCenterChild(this._003CpassedRoom_003E5__2.GetComponent<RectTransform>());
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
			this._003Cenumerator_003E5__6 = scrollableSelectCarScreen.MoveCenterContainerToCenterChild(this._003CunlockedRoom_003E5__3.GetComponent<RectTransform>(), 0.5f);
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
			GGUtil.Hide(scrollableSelectCarScreen.stopInteractionWidget);
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

		public ScrollableSelectCarScreen _003C_003E4__this;

		public ScrollableSelectCarScreen.ChangeCarArguments changeRoom;

		private ScrollableSelectCarScreenButton _003CpassedRoom_003E5__2;

		private ScrollableSelectCarScreenButton _003CunlockedRoom_003E5__3;

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
