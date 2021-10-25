using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class StarConsumeAnimation : MonoBehaviour
{
	public StarConsumeAnimation.Settings settings
	{
		get
		{
			return Match3Settings.instance.starConsumeSettings;
		}
	}

	public void Show(StarConsumeAnimation.InitParams initParams)
	{
		this.initParams = initParams;
		GGUtil.Show(this);
		this.animationEnumerator = this.DoAnimation();
		this.animationEnumerator.MoveNext();
	}

	private IEnumerator DoAnimation()
	{
		return new StarConsumeAnimation._003CDoAnimation_003Ed__9(0)
		{
			_003C_003E4__this = this
		};
	}

	private void Update()
	{
		if (this.animationEnumerator != null && !this.animationEnumerator.MoveNext())
		{
			this.animationEnumerator = null;
		}
	}

	[SerializeField]
	private StarConsumeAnimationStar star;

	[SerializeField]
	private RectTransform originStarTransform;

	private StarConsumeAnimation.InitParams initParams;

	private IEnumerator animationEnumerator;

	[Serializable]
	public class Settings
	{
		public float moveStartScale;

		public float moveEndScale;

		public float moveDuration;

		public float animationDelayDuration;

		public float moveEndAlpha;

		public AnimationCurve moveCurve;

		public float endScale;

		public float scaleDuration;

		public float whiteoutAlphaEnd = 1f;

		public AnimationCurve scaleCurve;
	}

	public struct InitParams
	{
		public DecorateRoomScreen screen;

		public DecorateRoomSceneVisualItem visualItem;

		public Action onEnd;
	}

	private sealed class _003CDoAnimation_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimation_003Ed__9(int _003C_003E1__state)
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
			StarConsumeAnimation starConsumeAnimation = this._003C_003E4__this;
			switch (num)
			{
			case 0:
			{
				this._003C_003E1__state = -1;
				this._003Ctime_003E5__2 = 0f;
				this._003Csettings_003E5__3 = starConsumeAnimation.settings;
				DecoratingScene scene = starConsumeAnimation.initParams.screen.scene;
				starConsumeAnimation.star.Init();
				starConsumeAnimation.initParams.visualItem.visualObjectBehaviour.activeVariation.ScaleAnimation(this._003Csettings_003E5__3.animationDelayDuration, !starConsumeAnimation.initParams.visualItem.visualObjectBehaviour.visualObject.hasDefaultVariation);
				this._003CstartLocalPos_003E5__4 = starConsumeAnimation.transform.InverseTransformPoint(starConsumeAnimation.originStarTransform.position);
				this._003CendLocalPos_003E5__5 = starConsumeAnimation.transform.InverseTransformPoint(scene.PSDToWorldPoint(starConsumeAnimation.initParams.visualItem.visualObjectBehaviour.iconHandlePosition));
				this._003CstartLocalPos_003E5__4.z = (this._003CendLocalPos_003E5__5.z = 0f);
				break;
			}
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_3AE;
			default:
				return false;
			}
			if (this._003Ctime_003E5__2 <= this._003Csettings_003E5__3.moveDuration)
			{
				this._003Ctime_003E5__2 += Time.deltaTime;
				float time = Mathf.InverseLerp(0f, this._003Csettings_003E5__3.moveDuration, this._003Ctime_003E5__2);
				float t = this._003Csettings_003E5__3.moveCurve.Evaluate(time);
				Vector3 localScale = Vector3.LerpUnclamped(Vector3.one * this._003Csettings_003E5__3.moveStartScale, Vector3.one * this._003Csettings_003E5__3.moveEndScale, t);
				Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartLocalPos_003E5__4, this._003CendLocalPos_003E5__5, t);
				float alpha = Mathf.Lerp(1f, this._003Csettings_003E5__3.moveEndAlpha, t);
				GGUtil.SetAlpha(starConsumeAnimation.star.mainGroup, alpha);
				starConsumeAnimation.star.transform.localPosition = localPosition;
				starConsumeAnimation.star.transform.localScale = localScale;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			starConsumeAnimation.star.transform.localPosition = this._003CendLocalPos_003E5__5;
			starConsumeAnimation.star.transform.localScale = Vector3.one * this._003Csettings_003E5__3.moveEndScale;
			GGUtil.SetAlpha(starConsumeAnimation.star.mainGroup, this._003Csettings_003E5__3.moveEndAlpha);
			this._003CstartScale_003E5__6 = starConsumeAnimation.star.transform.localScale;
			GGUtil.SetAlpha(starConsumeAnimation.star.whiteOutImage, 0f);
			GGUtil.SetActive(starConsumeAnimation.star.whiteOutImage, true);
			this._003CstartAlpha_003E5__7 = starConsumeAnimation.star.mainGroup.alpha;
			this._003Ctime_003E5__2 = 0f;
			IL_3AE:
			if (this._003Ctime_003E5__2 > this._003Csettings_003E5__3.scaleDuration)
			{
				GGUtil.Hide(starConsumeAnimation);
				if (starConsumeAnimation.initParams.onEnd != null)
				{
					starConsumeAnimation.initParams.onEnd();
				}
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float time2 = Mathf.InverseLerp(0f, this._003Csettings_003E5__3.scaleDuration, this._003Ctime_003E5__2);
			float t2 = this._003Csettings_003E5__3.scaleCurve.Evaluate(time2);
			Vector3 localScale2 = Vector3.LerpUnclamped(this._003CstartScale_003E5__6, this._003Csettings_003E5__3.endScale * Vector3.one, t2);
			starConsumeAnimation.star.transform.localScale = localScale2;
			float alpha2 = Mathf.Lerp(this._003CstartAlpha_003E5__7, 0f, t2);
			GGUtil.SetAlpha(starConsumeAnimation.star.mainGroup, alpha2);
			float alpha3 = Mathf.Lerp(0f, this._003Csettings_003E5__3.whiteoutAlphaEnd, t2);
			GGUtil.SetAlpha(starConsumeAnimation.star.whiteOutImage, alpha3);
			this._003C_003E2__current = null;
			this._003C_003E1__state = 2;
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

		public StarConsumeAnimation _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private StarConsumeAnimation.Settings _003Csettings_003E5__3;

		private Vector3 _003CstartLocalPos_003E5__4;

		private Vector3 _003CendLocalPos_003E5__5;

		private Vector3 _003CstartScale_003E5__6;

		private float _003CstartAlpha_003E5__7;
	}
}
