using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;

public class ConfirmPurchasePanel : MonoBehaviour
{
	public ConfirmPurchasePanel.Settings settings
	{
		get
		{
			return Match3Settings.instance.confirmPurchasePanelSettings;
		}
	}

	public void Show(DecorateRoomSceneVisualItem visualItem, DecorateRoomScreen screen)
	{
		this.visualItem = visualItem;
		this.screen = screen;
		this.showTutorialHand = visualItem.visualObjectBehaviour.visualObject.sceneObjectInfo.autoSelect;
		string iconSpriteName = visualItem.visualObjectBehaviour.visualObject.sceneObjectInfo.iconSpriteName;
		ConfirmPurchasePanel.NamedSprites namedSprites = this.iconSprites[0];
		for (int i = 1; i < this.iconSprites.Count; i++)
		{
			if (this.iconSprites[i].name == iconSpriteName)
			{
				namedSprites = this.iconSprites[i];
				break;
			}
		}
		this.dragButton.Init(this, namedSprites.iconSprite);
		this.dragTarget.Init(this, namedSprites.backgroundSprite);
		GraphicsSceneConfig.VisualObject visualObject = visualItem.visualObjectBehaviour.visualObject;
		if(this.dragSourceItemNameText != null)
			this.dragSourceItemNameText.text = visualObject.displayName;
		if (this.dragSourcePriceText != null)
			this.dragSourcePriceText.text = visualObject.sceneObjectInfo.price.cost.ToString();
		GGUtil.SetActive(this.dragSourcePriceText, visualObject.sceneObjectInfo.price.cost > 1);
		this.dragTarget.transform.position = screen.TransformPSDToWorldPoint(visualItem.visualObjectBehaviour.iconHandlePosition);
		GGUtil.uiUtil.RestrictRectTransform(this.dragTarget.transform as RectTransform, this.constrainRect);
		Vector3 localPosition = this.dragTarget.transform.localPosition;
		Vector3 zero = Vector3.zero;
		zero.x = (float)((localPosition.x <= this.centerPositionForTarget.x) ? 1 : -1);
		zero.y = (float)((localPosition.y <= this.centerPositionForTarget.y) ? 1 : -1);
		if (this.falloffDistanceForTarget.y > 0f)
		{
			zero.y *= Mathf.InverseLerp(0f, this.falloffDistanceForTarget.y, Mathf.Abs(this.centerPositionForTarget.y));
		}
		Vector3 localPosition2 = zero.normalized * this.distanceOfSourceFromTarget;
		this.dragSourceRectTransform.localPosition = localPosition2;
		GGUtil.uiUtil.RestrictRectTransform(this.dragSourceRectTransform, this.constrainRect);
		GGUtil.SetActive(this.backgroundSelected, false);
		Vector3 normalized = (this.dragTarget.transform.position - this.dragButton.transform.position).normalized;
		this.arrowTransform.localRotation = Quaternion.LookRotation(Vector3.forward, normalized);
		GGUtil.SetActive(this.arrowTransform, true);
		this.trailRenderer.enabled = false;
		this.scaleParent.transform.localScale = this.settings.inAnimationZoomFrom;
		this.inAnimation = this.DoInAnimation(this.showTutorialHand);
		this.selectorAnimation = null;
	}

	private IEnumerator DoInAnimation(bool showTutorial)
	{
		return new ConfirmPurchasePanel._003CDoInAnimation_003Ed__24(0)
		{
			_003C_003E4__this = this,
			showTutorial = showTutorial
		};
	}

	private void ShowTutorialHandIfNeeded()
	{
		if (this.showTutorialHand)
		{
			this.scaleParent.transform.localScale = Vector3.one;
			TutorialHandController.InitArguments initArguments = default(TutorialHandController.InitArguments);
			Transform transform = this.screen.transform;
			initArguments.endLocalPosition = transform.InverseTransformPoint(this.dragTarget.transform.position);
			initArguments.startLocalPosition = transform.InverseTransformPoint(this.dragSourceRectTransform.position);
			initArguments.settings = Match3Settings.instance.tutorialHandSettings;
			initArguments.repeat = true;
			this.screen.tutorialHand.Show(initArguments);
		}
	}

	private IEnumerator DoSelectorAnimation()
	{
		return new ConfirmPurchasePanel._003CDoSelectorAnimation_003Ed__26(0)
		{
			_003C_003E4__this = this
		};
	}

	private void Update()
	{
		if (this.dragTarget == null)
		{
			return;
		}
		this.dragTarget.transform.position = this.screen.TransformPSDToWorldPoint(this.visualItem.visualObjectBehaviour.iconHandlePosition);
		if (this.inAnimation != null && !this.inAnimation.MoveNext())
		{
			this.inAnimation = null;
		}
		if (this.selectorAnimation != null && !this.selectorAnimation.MoveNext())
		{
			this.selectorAnimation = null;
		}
	}

	public void OnPurchaseConfirmed()
	{
		this.showTutorialHand = false;
		this.screen.tutorialHand.Hide();
		this.screen.ConfirmPurchasePanelCallback_OnConfirm(this.visualItem);
	}

	public void OnBackgroundClicked()
	{
		this.showTutorialHand = false;
		GGUtil.SetActive(this, false);
		this.screen.tutorialHand.Hide();
		this.screen.ConfirmPurchasePanelCallback_OnClosed();
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void OnDragStart()
	{
		this.screen.tutorialHand.Hide();
		GGUtil.SetActive(this.arrowTransform, false);
		this.trailRenderer.enabled = true;
	}

	public void OnDragEnd()
	{
		this.ShowTutorialHandIfNeeded();
		GGUtil.SetActive(this.arrowTransform, true);
		this.trailRenderer.enabled = false;
		GGUtil.SetActive(this.backgroundSelected, false);
	}

	public bool IsTargetIn()
	{
		Vector3 position = this.dragButton.transform.position;
		return this.IsTargetIn(position);
	}

	public bool IsTargetIn(Vector3 dragButtonWorldPosition)
	{
		DragTarget dragTarget = this.dragTarget;
		dragButtonWorldPosition.z = dragTarget.transform.position.z;
		float magnitude = dragTarget.transform.InverseTransformPoint(dragButtonWorldPosition).magnitude;
		float num = dragTarget.GetComponent<RectTransform>().sizeDelta.x * 0.5f + this.dragButton.GetComponent<RectTransform>().sizeDelta.x * this.scaleParent.localScale.x * 0.5f;
		return magnitude <= num;
	}

	public void OnButtonClick()
	{
		this.showTutorialHand = true;
		this.ShowTutorialHandIfNeeded();
	}

	public void OnDrag()
	{
		int activeSelf = this.backgroundSelected.gameObject.activeSelf ? 1 : 0;
		bool flag = this.IsTargetIn();
		if (!flag)
		{
			this.selectorAnimation = null;
		}
		GGUtil.SetActive(this.backgroundSelected, flag);
		if (activeSelf == 0 && flag)
		{
			this.selectorAnimation = this.DoSelectorAnimation();
		}
	}

	[SerializeField]
	private float distanceOfSourceFromTarget = 110f;

	[SerializeField]
	private Vector3 centerPositionForTarget = Vector3.zero;

	[SerializeField]
	private Vector3 falloffDistanceForTarget = Vector3.zero;

	private DecorateRoomSceneVisualItem visualItem;

	private DecorateRoomScreen screen;

	[SerializeField]
	private DragButton dragButton;

	[SerializeField]
	public DragTarget dragTarget;

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
	private List<ConfirmPurchasePanel.NamedSprites> iconSprites = new List<ConfirmPurchasePanel.NamedSprites>();

	[SerializeField]
	private RectTransform backgroundSelected;

	[SerializeField]
	private TrailRenderer trailRenderer;

	[SerializeField]
	private RectTransform selectorTransform;

	private IEnumerator inAnimation;

	private IEnumerator selectorAnimation;

	private bool showTutorialHand;

	[Serializable]
	public class Settings
	{
		public AnimationCurve curve;

		public float zoomInDuration = 1f;

		public float zoomInFactor = 1.5f;

		public float moveTowardsFactor = 0.1f;

		public float zoomOutDuration = 0.5f;

		public AnimationCurve outCurve;

		public float inAnimationDuration;

		public Vector3 inAnimationZoomFrom;

		public Vector3 inAnimationZoomTo;

		public AnimationCurve inAnimationzoomAnimationCurve;

		public float selectorAnimationDuration;

		public Vector3 selectorAnimationZoomFrom;

		public float selectorAnimationAlphaFrom;

		public AnimationCurve selectorAnimationCurve;
	}

	[Serializable]
	public class NamedSprites
	{
		public string name;

		public Sprite iconSprite;

		public Sprite backgroundSprite;
	}

	private sealed class _003CDoInAnimation_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoInAnimation_003Ed__24(int _003C_003E1__state)
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
			ConfirmPurchasePanel confirmPurchasePanel = this._003C_003E4__this;
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
				if (this.showTutorial)
				{
					confirmPurchasePanel.dragButton.StopAnimation();
				}
				this._003Ctime_003E5__2 = 0f;
				this._003Csettings_003E5__3 = confirmPurchasePanel.settings;
			}
			if (this._003Ctime_003E5__2 > this._003Csettings_003E5__3.inAnimationDuration)
			{
				confirmPurchasePanel.ShowTutorialHandIfNeeded();
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Csettings_003E5__3.inAnimationDuration, this._003Ctime_003E5__2);
			float t = this._003Csettings_003E5__3.inAnimationzoomAnimationCurve.Evaluate(time);
			Vector3 localScale = Vector3.LerpUnclamped(this._003Csettings_003E5__3.inAnimationZoomFrom, this._003Csettings_003E5__3.inAnimationZoomTo, t);
			confirmPurchasePanel.scaleParent.transform.localScale = localScale;
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

		public bool showTutorial;

		public ConfirmPurchasePanel _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private ConfirmPurchasePanel.Settings _003Csettings_003E5__3;
	}

	private sealed class _003CDoSelectorAnimation_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoSelectorAnimation_003Ed__26(int _003C_003E1__state)
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
			ConfirmPurchasePanel confirmPurchasePanel = this._003C_003E4__this;
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
				this._003Csettings_003E5__3 = confirmPurchasePanel.settings;
			}
			if (this._003Ctime_003E5__2 > this._003Csettings_003E5__3.selectorAnimationDuration)
			{
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Csettings_003E5__3.selectorAnimationDuration, this._003Ctime_003E5__2);
			float t = this._003Csettings_003E5__3.selectorAnimationCurve.Evaluate(time);
			Vector3 localScale = Vector3.LerpUnclamped(this._003Csettings_003E5__3.selectorAnimationZoomFrom, Vector3.one, t);
			confirmPurchasePanel.selectorTransform.transform.localScale = localScale;
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

		public ConfirmPurchasePanel _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private ConfirmPurchasePanel.Settings _003Csettings_003E5__3;
	}
}
