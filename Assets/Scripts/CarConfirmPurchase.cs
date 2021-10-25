using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class CarConfirmPurchase : MonoBehaviour
{
	public void Show(CarConfirmPurchase.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		this.screen = initArguments.screen;
		this.isDraging = false;
		GGUtil.SetActive(this, true);
		GGUtil.SetActive(this.backgroundClickArea, initArguments.showBackground);
		if (initArguments.inputHandler != null)
		{
			initArguments.inputHandler.onClick -= this.OnInputHandlerClick;
			initArguments.inputHandler.onClick += this.OnInputHandlerClick;
		}
		this.dragButton.Init(this);
		this.dragTarget.Init(this);
		GGUtil.ChangeText(this.dragSourceItemNameText, initArguments.displayName);
		this.dragTarget.transform.localPosition = this.screen.TransformWorldCarPointToLocalUIPosition(initArguments.buttonHandlePosition);
		Vector3 localPosition = this.dragTarget.transform.localPosition;
		Vector3 zero = Vector3.zero;
		zero.x = 1f;
		zero.y = 1f;
		Vector3 localPosition2 = zero.normalized * this.distanceOfSourceFromTarget;
		this.dragSourceRectTransform.localPosition = localPosition2;
		this.trailRenderer.enabled = false;
		GGUtil.SetActive(this.backgroundSelected, false);
		Vector3 normalized = (this.dragTarget.transform.position - this.dragButton.transform.position).normalized;
		this.arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized);
		GGUtil.SetActive(this.arrowTransform, true);
		this.UpdatePositionAndDirection();
		this.inAnimation = this.DoInAnimation();
		this.inAnimation.MoveNext();
		this.selectorAnimation = null;
	}

	private IEnumerator DoInAnimation()
	{
		return new CarConfirmPurchase._003CDoInAnimation_003Ed__20(0)
		{
			_003C_003E4__this = this
		};
	}

	public void OnPurchaseConfirmed()
	{
		GGUtil.SetActive(this, false);
		this.initArguments.CallOnSuccess();
	}

	private void OnInputHandlerClick(Vector2 position)
	{
		this.OnBackgroundClicked();
	}

	public void OnBackgroundClicked()
	{
		GGUtil.SetActive(this, false);
		this.initArguments.CallOnCancel();
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void OnDragStart()
	{
		this.trailRenderer.enabled = true;
		GGUtil.SetActive(this.arrowTransform, false);
		this.isDraging = true;
		this.initArguments.CallOnDrag();
	}

	public void OnDragEnd()
	{
		this.trailRenderer.enabled = false;
		GGUtil.SetActive(this.arrowTransform, true);
		GGUtil.SetActive(this.backgroundSelected, false);
		this.isDraging = false;
	}

	public bool IsTargetIn()
	{
		Vector3 position = this.dragButton.transform.position;
		return this.IsTargetIn(position);
	}

	public float DistancePercent()
	{
		Vector3 position = this.dragButton.transform.position;
		return this.DistancePercent(position);
	}

	public float DistancePercent(Vector3 dragButtonWorldPosition)
	{
		CarDragTarget carDragTarget = this.dragTarget;
		dragButtonWorldPosition.z = carDragTarget.transform.position.z;
		Vector3 position = carDragTarget.transform.position;
		Vector3 vector = this.dragSourceRectTransform.position - position;
		float magnitude = vector.magnitude;
		float value = Vector3.Dot(vector.normalized, dragButtonWorldPosition - position);
		return Mathf.InverseLerp(0f, magnitude, value);
	}

	public bool IsTargetIn(Vector3 dragButtonWorldPosition)
	{
		if (this.initArguments.useDistanceToFindIfInside && this.DistancePercent(dragButtonWorldPosition) <= 0f)
		{
			return true;
		}
		CarDragTarget carDragTarget = this.dragTarget;
		dragButtonWorldPosition.z = carDragTarget.transform.position.z;
		float magnitude = carDragTarget.transform.InverseTransformPoint(dragButtonWorldPosition).magnitude;
		float num = carDragTarget.GetComponent<RectTransform>().sizeDelta.x * 0.5f + this.dragButton.GetComponent<RectTransform>().sizeDelta.x * this.scaleParent.localScale.x * 0.5f;
		return magnitude <= num;
	}

	public void OnButtonClick()
	{
	}

	public void OnDrag()
	{
		this.isDraging = true;
		int activeSelf = this.backgroundSelected.gameObject.activeSelf ? 1 : 0;
		bool flag = this.IsTargetIn();
		GGUtil.SetActive(this.backgroundSelected, flag);
		bool flag2 = activeSelf == 0 && flag;
		this.initArguments.CallOnDrag();
		if (this.initArguments.useMinDistanceToConfirm && this.DistancePercent() <= this.initArguments.minDistance && this.inAnimation == null)
		{
			this.OnPurchaseConfirmed();
		}
	}

	private void UpdatePositionAndDirection()
	{
		this.dragTarget.transform.localPosition = this.screen.TransformWorldCarPointToLocalUIPosition(this.initArguments.buttonHandlePosition);
		if (this.initArguments.exactPosition)
		{
			Vector3 localPosition = this.screen.TransformWorldCarPointToLocalUIPosition(this.initArguments.directionHandlePosition) - this.dragTarget.transform.localPosition;
			this.dragSourceRectTransform.localPosition = localPosition;
			Vector3 normalized = (this.dragTarget.transform.position - this.dragButton.transform.position).normalized;
			this.arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized);
			return;
		}
		if (this.initArguments.updateDirection)
		{
			Vector3 vector = this.screen.TransformWorldCarPointToLocalUIPosition(this.initArguments.directionHandlePosition) - this.dragTarget.transform.localPosition;
			int num = Mathf.RoundToInt(Mathf.Sign(vector.x));
			int num2 = Mathf.RoundToInt(Mathf.Sign(vector.y));
			if (num == 0)
			{
				num = 1;
			}
			if (num2 == 0)
			{
				num2 = -1;
			}
			vector = new Vector3((float)num, (float)num2, 0f);
			vector = vector.normalized;
			Vector3 localPosition2 = vector * this.distanceOfSourceFromTarget;
			this.dragSourceRectTransform.localPosition = localPosition2;
			Vector3 normalized2 = (this.dragTarget.transform.position - this.dragButton.transform.position).normalized;
			this.arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized2);
		}
	}

	private void OnDisable()
	{
		if (this.initArguments.inputHandler != null)
		{
			this.initArguments.inputHandler.onClick -= this.OnInputHandlerClick;
		}
	}

	private void Update()
	{
		if (this.dragTarget == null)
		{
			return;
		}
		if (!this.isDraging)
		{
			this.UpdatePositionAndDirection();
		}
		if (this.inAnimation != null && !this.inAnimation.MoveNext())
		{
			this.inAnimation = null;
		}
		if (this.selectorAnimation != null && !this.selectorAnimation.MoveNext())
		{
			this.selectorAnimation = null;
		}
	}

	[SerializeField]
	private float distanceOfSourceFromTarget = 110f;

	[SerializeField]
	private Transform backgroundClickArea;

	[SerializeField]
	private CarDragButton dragButton;

	[SerializeField]
	public CarDragTarget dragTarget;

	[SerializeField]
	private TextMeshProUGUI dragSourceItemNameText;

	[SerializeField]
	private TextMeshProUGUI dragSourcePriceText;

	[SerializeField]
	private RectTransform arrowTransform;

	[SerializeField]
	private RectTransform constrainRect;

	[SerializeField]
	private RectTransform dragSourceRectTransform;

	[SerializeField]
	private RectTransform scaleParent;

	[SerializeField]
	private RectTransform backgroundSelected;

	[SerializeField]
	private TrailRenderer trailRenderer;

	[SerializeField]
	private RectTransform selectorTransform;

	[NonSerialized]
	private AssembleCarScreen screen;

	private IEnumerator inAnimation;

	private IEnumerator selectorAnimation;

	private CarConfirmPurchase.InitArguments initArguments;

	private bool isDraging;

	public struct InitArguments
	{
		public void CallOnDrag()
		{
			if (this.onDrag != null)
			{
				this.onDrag();
			}
		}

		public void CallOnCancel()
		{
			if (this.onCancel != null)
			{
				this.onCancel();
			}
		}

		public void CallOnSuccess()
		{
			if (this.onSuccess != null)
			{
				this.onSuccess(this);
			}
		}

		public AssembleCarScreen screen;

		public string displayName;

		public Vector3 buttonHandlePosition;

		public CarModelPart carPart;

		public bool showBackground;

		public Action onCancel;

		public Action onDrag;

		public Action<CarConfirmPurchase.InitArguments> onSuccess;

		public bool updateDirection;

		public bool useDistanceToFindIfInside;

		public bool exactPosition;

		public bool useMinDistanceToConfirm;

		public float minDistance;

		public InputHandler inputHandler;

		public Vector3 directionHandlePosition;
	}

	private sealed class _003CDoInAnimation_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoInAnimation_003Ed__20(int _003C_003E1__state)
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
			CarConfirmPurchase carConfirmPurchase = this._003C_003E4__this;
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
				this._003Ctime_003E5__2 = 0f;
				this._003Csettings_003E5__3 = Match3Settings.instance.confirmPurchasePanelSettings;
			}
			if (this._003Ctime_003E5__2 > this._003Csettings_003E5__3.inAnimationDuration)
			{
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Csettings_003E5__3.inAnimationDuration, this._003Ctime_003E5__2);
			float t = this._003Csettings_003E5__3.inAnimationzoomAnimationCurve.Evaluate(time);
			Vector3 localScale = Vector3.LerpUnclamped(this._003Csettings_003E5__3.inAnimationZoomFrom, this._003Csettings_003E5__3.inAnimationZoomTo, t);
			carConfirmPurchase.scaleParent.transform.localScale = localScale;
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

		public CarConfirmPurchase _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private ConfirmPurchasePanel.Settings _003Csettings_003E5__3;
	}
}
