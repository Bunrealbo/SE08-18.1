using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TutorialHandController : MonoBehaviour
{
	public void Show(TutorialHandController.InitArguments initArguments)
	{
		base.gameObject.SetActive(true);
		this.initArguments = initArguments;
		this.animation = this.DoAnimation(initArguments);
		this.animation.MoveNext();
	}

	public void Hide()
	{
		this.animation = null;
		base.gameObject.SetActive(false);
	}

	private TutorialHandController.Settings settngs
	{
		get
		{
			return this.initArguments.settings;
		}
	}

	private IEnumerator DoAnimation(TutorialHandController.InitArguments initArguments)
	{
		return new TutorialHandController._003CDoAnimation_003Ed__12(0)
		{
			_003C_003E4__this = this,
			initArguments = initArguments
		};
	}

	private IEnumerator DoMove()
	{
		return new TutorialHandController._003CDoMove_003Ed__13(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator ScaleIn()
	{
		return new TutorialHandController._003CScaleIn_003Ed__14(0)
		{
			_003C_003E4__this = this
		};
	}

	private void Update()
	{
		if (this.animation != null)
		{
			this.animation.MoveNext();
		}
	}

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private Transform handContainer;

	[SerializeField]
	private TrailRenderer trail;

	[SerializeField]
	private CanvasGroup handAlpha;

	private TutorialHandController.InitArguments initArguments;

	private IEnumerator animation;

	[Serializable]
	public class Settings
	{
		public float scaleHandFrom = 2f;

		public float scaleHandTo = 1f;

		public float alphaHandFrom;

		public float alphaHandTo = 1f;

		public float scaleInDuration = 0.5f;

		public AnimationCurve scaleInCurve;

		public float delayAfterScale;

		public float moveDuration = 2f;

		public AnimationCurve moveCurve;

		public float waitOnDestination;

		public float fromAlphaMove = 1f;

		public float toAlphaMove = 1f;
	}

	public struct InitArguments
	{
		public Vector3 startLocalPosition;

		public Vector3 endLocalPosition;

		public bool repeat;

		public TutorialHandController.Settings settings;
	}

	private sealed class _003CDoAnimation_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimation_003Ed__12(int _003C_003E1__state)
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
			TutorialHandController tutorialHandController = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				break;
			case 1:
				this._003C_003E1__state = -1;
				goto IL_A6;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_E9;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_154;
			case 4:
				this._003C_003E1__state = -1;
				goto IL_197;
			default:
				return false;
			}
			IL_31:
			GGUtil.Hide(tutorialHandController.widgetsToHide);
			tutorialHandController.handContainer.localPosition = this.initArguments.startLocalPosition;
			this._003CanimEnum_003E5__2 = null;
			this._003Ctime_003E5__3 = 0f;
			GGUtil.Show(tutorialHandController.handContainer);
			if (tutorialHandController.settngs.scaleInDuration <= 0f)
			{
				goto IL_FC;
			}
			this._003CanimEnum_003E5__2 = tutorialHandController.ScaleIn();
			IL_A6:
			if (this._003CanimEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003Ctime_003E5__3 = 0f;
			IL_E9:
			if (this._003Ctime_003E5__3 < tutorialHandController.settngs.delayAfterScale)
			{
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			IL_FC:
			tutorialHandController.trail.Clear();
			GGUtil.Show(tutorialHandController.trail);
			if (!(this.initArguments.startLocalPosition != this.initArguments.endLocalPosition))
			{
				goto IL_161;
			}
			this._003CanimEnum_003E5__2 = tutorialHandController.DoMove();
			IL_154:
			if (this._003CanimEnum_003E5__2.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 3;
				return true;
			}
			IL_161:
			this._003Ctime_003E5__3 = 0f;
			IL_197:
			if (this._003Ctime_003E5__3 < tutorialHandController.settngs.waitOnDestination)
			{
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 4;
				return true;
			}
			if (!this.initArguments.repeat)
			{
				tutorialHandController.Hide();
				return false;
			}
			this._003CanimEnum_003E5__2 = null;
			goto IL_31;
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

		public TutorialHandController _003C_003E4__this;

		public TutorialHandController.InitArguments initArguments;

		private IEnumerator _003CanimEnum_003E5__2;

		private float _003Ctime_003E5__3;
	}

	private sealed class _003CDoMove_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoMove_003Ed__13(int _003C_003E1__state)
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
			TutorialHandController tutorialHandController = this._003C_003E4__this;
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
				TutorialHandController.Settings settngs = tutorialHandController.settngs;
				this._003CfromPosition_003E5__2 = tutorialHandController.initArguments.startLocalPosition;
				this._003CtoPosition_003E5__3 = tutorialHandController.initArguments.endLocalPosition;
				Vector3 localPosition = Vector3.one * tutorialHandController.settngs.scaleHandTo;
				tutorialHandController.handContainer.localPosition = localPosition;
				this._003Cduration_003E5__4 = tutorialHandController.settngs.moveDuration;
				this._003Ctime_003E5__5 = 0f;
			}
			if (this._003Ctime_003E5__5 > this._003Cduration_003E5__4)
			{
				return false;
			}
			this._003Ctime_003E5__5 += Time.deltaTime;
			float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__4, this._003Ctime_003E5__5);
			float t = tutorialHandController.settngs.moveCurve.Evaluate(num2);
			Vector3 localPosition2 = Vector3.LerpUnclamped(this._003CfromPosition_003E5__2, this._003CtoPosition_003E5__3, t);
			float alpha = Mathf.Lerp(tutorialHandController.settngs.fromAlphaMove, tutorialHandController.settngs.toAlphaMove, num2);
			GGUtil.SetAlpha(tutorialHandController.handAlpha, alpha);
			tutorialHandController.handContainer.localPosition = localPosition2;
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

		public TutorialHandController _003C_003E4__this;

		private Vector3 _003CfromPosition_003E5__2;

		private Vector3 _003CtoPosition_003E5__3;

		private float _003Cduration_003E5__4;

		private float _003Ctime_003E5__5;
	}

	private sealed class _003CScaleIn_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CScaleIn_003Ed__14(int _003C_003E1__state)
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
			TutorialHandController tutorialHandController = this._003C_003E4__this;
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
				TutorialHandController.Settings settngs = tutorialHandController.settngs;
				this._003CfromScale_003E5__2 = Vector3.one * tutorialHandController.settngs.scaleHandFrom;
				this._003CtoScale_003E5__3 = Vector3.one * tutorialHandController.settngs.scaleHandTo;
				this._003Ctime_003E5__4 = 0f;
			}
			if (this._003Ctime_003E5__4 > tutorialHandController.settngs.scaleInDuration)
			{
				return false;
			}
			this._003Ctime_003E5__4 += Time.deltaTime;
			float num2 = Mathf.InverseLerp(0f, tutorialHandController.settngs.scaleInDuration, this._003Ctime_003E5__4);
			float t = tutorialHandController.settngs.scaleInCurve.Evaluate(num2);
			Vector3 localScale = Vector3.LerpUnclamped(this._003CfromScale_003E5__2, this._003CtoScale_003E5__3, t);
			float alpha = Mathf.Lerp(tutorialHandController.settngs.alphaHandFrom, tutorialHandController.settngs.alphaHandTo, num2);
			GGUtil.SetAlpha(tutorialHandController.handAlpha, alpha);
			tutorialHandController.handContainer.localScale = localScale;
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

		public TutorialHandController _003C_003E4__this;

		private Vector3 _003CfromScale_003E5__2;

		private Vector3 _003CtoScale_003E5__3;

		private float _003Ctime_003E5__4;
	}
}
