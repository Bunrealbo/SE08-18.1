using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarModel : MonoBehaviour
{
	public List<CarModelPart> AvailablePartsAsTasks()
	{
		this.availableCarModelParts_.Clear();
		for (int i = 0; i < this.parts.Count; i++)
		{
			CarModelPart carModelPart = this.parts[i];
			if (!carModelPart.partInfo.isDefault && !carModelPart.partInfo.isOwned && carModelPart.partInfo.isUnlocked)
			{
				this.availableCarModelParts_.Add(carModelPart);
			}
		}
		return this.availableCarModelParts_;
	}

	private CarModelColliders GetOrCreateCarModelColliders()
	{
		if (this.colliders != null)
		{
			return this.colliders;
		}
		string name = "ColliderRootTransform";
		foreach (object obj in base.transform)
		{
			CarModelColliders component = ((Transform)obj).GetComponent<CarModelColliders>();
			if (component != null)
			{
				return component;
			}
		}
		return new GameObject(name)
		{
			transform = 
			{
				parent = base.transform,
				localPosition = Vector3.zero,
				localRotation = Quaternion.identity,
				localScale = Vector3.one
			}
		}.AddComponent<CarModelColliders>();
	}

	public void SetCollidersActive(bool active)
	{
		GGUtil.SetActive(this.colliders, active);
	}

	public void InitializeParts()
	{
		this.parts.Clear();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (!transform.name.ToLower().Contains("_ignore") && !transform.GetComponent<CarModelColliders>())
			{
				CarModelPart carModelPart = transform.GetComponent<CarModelPart>();
				if (carModelPart == null)
				{
					carModelPart = transform.gameObject.AddComponent<CarModelPart>();
				}
				this.parts.Add(carModelPart);
				carModelPart.Init(this);
				if (this.indexAsGroup)
				{
					carModelPart.partInfo.groupIndex = this.parts.Count - 1;
				}
			}
		}
		this.InitPhysics();
	}

	public void InitPhysics()
	{
		this.colliders = this.GetOrCreateCarModelColliders();
		this.colliders.Init(this);
	}

	public bool IsAllElementsPickedUpInGroup(int index)
	{
		for (int i = 0; i < this.parts.Count; i++)
		{
			CarModelPart carModelPart = this.parts[i];
			if (carModelPart.partInfo.groupIndex == index && !carModelPart.partInfo.isOwned)
			{
				return false;
			}
		}
		return true;
	}

	public int SelectedIndexForVariantGroup(string groupName)
	{
		CarModelInfo.VariantGroup variantGroup = this.modelInfo.GetVariantGroup(groupName);
		if (variantGroup == null)
		{
			return 0;
		}
		return variantGroup.selectedVariationIndex;
	}

	public void RefreshVariations()
	{
		for (int i = 0; i < this.parts.Count; i++)
		{
			this.parts[i].SetActiveIfOwned();
		}
	}

	private RoomsBackend.RoomAccessor roomBackend
	{
		get
		{
			return SingletonInit<RoomsBackend>.instance.GetRoom(base.name);
		}
	}

	public bool isPassed
	{
		get
		{
			return this.roomBackend.isPassed;
		}
		set
		{
			this.roomBackend.isPassed = value;
		}
	}

	public void InitForRuntime()
	{
		RoomsBackend.RoomAccessor room = SingletonInit<RoomsBackend>.instance.GetRoom(base.name);
		this.modelInfo.Init(room);
		for (int i = 0; i < this.parts.Count; i++)
		{
			this.parts[i].InitForRuntime(room);
		}
		for (int j = 0; j < this.parts.Count; j++)
		{
			this.parts[j].SetActiveIfOwned();
		}
		this.InitExplodeAnimation();
	}

	public void RefreshVisibilityOnParts()
	{
		for (int i = 0; i < this.parts.Count; i++)
		{
			this.parts[i].SetActiveIfOwned();
		}
	}

	public CarModel.ProgressState GetProgressState()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.parts.Count; i++)
		{
			CarModelPart carModelPart = this.parts[i];
			if (!carModelPart.partInfo.isDefault)
			{
				num2++;
				if (carModelPart.partInfo.isOwned)
				{
					num++;
				}
			}
		}
		return new CarModel.ProgressState
		{
			completed = num,
			total = num2
		};
	}

	public void InitExplodeAnimation()
	{
		this.explodeAnimation.Init(this);
	}

	public List<CarModelSubpart> AllOwnedSubpartsInVariantGroup(CarModelInfo.VariantGroup group)
	{
		this.subpartsHelperList_.Clear();
		for (int i = 0; i < this.parts.Count; i++)
		{
			CarModelPart carModelPart = this.parts[i];
			if (carModelPart.partInfo.isOwned)
			{
				bool flag = carModelPart.partInfo.animateChangeWithVariations.Contains(group.name);
				List<CarModelSubpart> subparts = carModelPart.subparts;
				for (int j = 0; j < subparts.Count; j++)
				{
					CarModelSubpart carModelSubpart = subparts[j];
					if (flag || carModelSubpart.HasVariantForGroup(group))
					{
						this.subpartsHelperList_.Add(carModelSubpart);
					}
				}
			}
		}
		return this.subpartsHelperList_;
	}

	public void ShowChnage()
	{
		this.animation = this.DoShowChange(1f);
		this.animation.MoveNext();
	}

	private IEnumerator DoShowChange(float scaleMult)
	{
		return new CarModel._003CDoShowChange_003Ed__29(0)
		{
			_003C_003E4__this = this,
			scaleMult = scaleMult
		};
	}

	private void Update()
	{
		if (this.animation != null && !this.animation.MoveNext())
		{
			this.animation = null;
		}
	}

	[SerializeField]
	public CarNutsPool nuts = new CarNutsPool();

	[SerializeField]
	public List<CarModelPart> parts = new List<CarModelPart>();

	[SerializeField]
	public CarModelInfo modelInfo = new CarModelInfo();

	[SerializeField]
	private CarModelColliders colliders;

	[SerializeField]
	private bool indexAsGroup;

	[NonSerialized]
	private IEnumerator animation;

	public ExplodeAnimation explodeAnimation = new ExplodeAnimation();

	private List<CarModelPart> availableCarModelParts_ = new List<CarModelPart>();

	private List<CarModelSubpart> subpartsHelperList_ = new List<CarModelSubpart>();

	public struct ProgressState
	{
		public bool isPassed
		{
			get
			{
				return this.completed >= this.total;
			}
		}

		public float Progress(int removeCompleted)
		{
			return Mathf.InverseLerp(0f, (float)this.total, (float)(this.completed - removeCompleted));
		}

		public float progress
		{
			get
			{
				return Mathf.InverseLerp(0f, (float)this.total, (float)this.completed);
			}
		}

		public int completed;

		public int total;
	}

	private sealed class _003CDoShowChange_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowChange_003Ed__29(int _003C_003E1__state)
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
			CarModel carModel = this._003C_003E4__this;
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
				CarModelSubpart.BlinkSettings subpartBlinkSettings = ScriptableObjectSingleton<CarsDB>.instance.subpartBlinkSettings;
				this._003Ctime_003E5__2 = 0f;
				this._003Cduration_003E5__3 = subpartBlinkSettings.inDuration;
				Vector3 up = Vector3.up;
				this._003CstartPosition_003E5__4 = Vector3.zero + up * subpartBlinkSettings.changeOffset * this.scaleMult;
				this._003Ccurve_003E5__5 = subpartBlinkSettings.moveCurve;
				this._003CendPosition_003E5__6 = Vector3.zero;
			}
			if (this._003Ctime_003E5__2 > this._003Cduration_003E5__3)
			{
				carModel.transform.localScale = Vector3.one;
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
			float t = this._003Ccurve_003E5__5.Evaluate(time);
			Vector3.LerpUnclamped(this._003CstartPosition_003E5__4, this._003CendPosition_003E5__6, t);
			carModel.transform.localScale = Vector3.LerpUnclamped(Vector3.one * 1.025f, Vector3.one, t);
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

		public float scaleMult;

		public CarModel _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private float _003Cduration_003E5__3;

		private Vector3 _003CstartPosition_003E5__4;

		private AnimationCurve _003Ccurve_003E5__5;

		private Vector3 _003CendPosition_003E5__6;
	}
}
