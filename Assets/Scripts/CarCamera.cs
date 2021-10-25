using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
	public Vector3 ScreenToViewPortPoint(Vector3 screenPoint)
	{
		return this.camera.ScreenToViewportPoint(screenPoint);
	}

	public Vector3 WorldToScreenPoint(Vector3 position)
	{
		return this.camera.WorldToScreenPoint(position);
	}

	public Ray ScreenPointToRay(Vector3 position)
	{
		return this.camera.ScreenPointToRay(position);
	}

	public Vector3 cameraForward
	{
		get
		{
			return this.camera.transform.forward;
		}
	}

	public Vector3 cameraPosition
	{
		get
		{
			return this.camera.transform.position;
		}
	}

	public CarCamera.Settings GetCarCamera(string name)
	{
		if (this.cameraSetups == null)
		{
			return null;
		}
		return this.cameraSetups.GetCarCamera(name);
	}

	public CarCamera.BlendSettings blendSettings
	{
		get
		{
			return ScriptableObjectSingleton<CarsDB>.instance.blendSettings;
		}
	}

	public void Init(InputHandler inputHandler)
	{
		this.inputHandler = inputHandler;
		this.usedSettings = this.settings;
		this.state.horizontalAngle = this.settings.startHorizontalAngle;
		this.state.verticalAngle = this.settings.startVerticalAngle;
		this.UpdateCameraPosition();
	}

	public void SetStandardSettings()
	{
		this.AnimateIntoSettings(new CarCamera.AnimateIntoSettingsArguments
		{
			newSettings = this.settings,
			updateAngle = false
		});
	}

	public void AnimateIntoSettings(CarCamera.Settings newSettings)
	{
		this.AnimateIntoSettings(new CarCamera.AnimateIntoSettingsArguments
		{
			newSettings = newSettings,
			updateAngle = newSettings.changeAnglesAtStart
		});
	}

	public void AnimateIntoSettings(CarCamera.AnimateIntoSettingsArguments arguments)
	{
		this.inputState.inertia = Vector3.zero;
		this.animation = this.DoAnimateIntoSettings(arguments);
		this.animation.MoveNext();
	}

	private IEnumerator DoAnimateIntoSettings(CarCamera.AnimateIntoSettingsArguments arguments)
	{
		return new CarCamera._003CDoAnimateIntoSettings_003Ed__30(0)
		{
			_003C_003E4__this = this,
			arguments = arguments
		};
	}

	public void Move(Vector2 distance)
	{
		CarCamera.Settings settings = this.usedSettings;
		this.state.horizontalAngle = this.state.horizontalAngle + distance.x * settings.horizontalAngleSpeed;
		this.state.verticalAngle = settings.verticalAngleRange.Clamp(-distance.y * settings.verticalAngleSpeed + this.state.verticalAngle);
		this.UpdateCameraPosition();
	}

	private void UpdateCameraPosition()
	{
		if (this.camera == null || this.usedSettings == null)
		{
			return;
		}
		CarCamera.Settings settings = this.usedSettings;
		Transform transform = this.camera.transform;
		Vector3 vector = base.transform.position;
		if (settings.enableRotationCenter)
		{
			vector = settings.rotationCenter;
		}
		Vector3 a = vector;
		Quaternion rotation = Quaternion.AngleAxis(this.state.horizontalAngle, Vector3.up) * Quaternion.AngleAxis(this.state.verticalAngle, Vector3.right);
		transform.position = vector + rotation * (Vector3.forward * settings.cameraDistance);
		transform.rotation = Quaternion.LookRotation(a - transform.position);
		if (this.camera.fieldOfView != settings.fov)
		{
			this.camera.fieldOfView = settings.fov;
		}
	}

	private void UpdateInertia()
	{
		if (Mathf.Max(this.inputState.inertia.x, this.inputState.inertia.y) < this.inertiaSettings.minInertia)
		{
			return;
		}
		this.inputState.inertia = Vector3.Lerp(this.inputState.inertia, Vector3.zero, Time.unscaledDeltaTime * this.inertiaSettings.dragSpeed);
		this.Move(this.inputState.inertia);
	}

	private void UpdateInputHandler()
	{
		if (this.inputHandler == null)
		{
			return;
		}
		InputHandler.PointerData pointerData = this.inputHandler.FirstDownPointer();
		if (!this.inputState.isDown && pointerData != null)
		{
			this.inputState = default(CarCamera.InputState);
			this.inputState.isDown = pointerData.isDown;
			this.inputState.touchId = pointerData.pointerId;
			this.inputState.lastPosition = pointerData.position;
			return;
		}
		if (!this.inputState.isDown)
		{
			this.UpdateInertia();
			return;
		}
		if (!this.inputHandler.IsDown(this.inputState.touchId))
		{
			this.inputState.isDown = false;
			return;
		}
		Vector2 lastPosition = this.inputState.lastPosition;
		Vector2 vector = this.inputHandler.Position(this.inputState.touchId);
		Vector2 vector2 = vector - lastPosition;
		this.inputState.inertia = Vector3.Lerp(this.inputState.inertia, this.inertiaSettings.GetInertia(vector2), this.inertiaSettings.affinityToNew);
		this.inputState.lastPosition = vector;
		this.Move(vector2);
	}

	private void Update()
	{
		if (this.animation != null)
		{
			if (!this.animation.MoveNext())
			{
				this.animation = null;
			}
			return;
		}
		this.UpdateInputHandler();
		if (Application.isEditor)
		{
			if (this.useSettingsStartAngles && this.usedSettings != null)
			{
				this.state.verticalAngle = this.usedSettings.startVerticalAngle;
				this.state.horizontalAngle = this.usedSettings.startHorizontalAngle;
			}
			this.UpdateCameraPosition();
		}
	}

	[SerializeField]
	private Camera camera;

	[SerializeField]
	private bool useSettingsStartAngles;

	[SerializeField]
	public CameraSetups cameraSetups;

	[SerializeField]
	private CarCamera.Settings settings = new CarCamera.Settings();

	[NonSerialized]
	private CarCamera.Settings usedSettings;

	private IEnumerator animation;

	[SerializeField]
	public CarCamera.InertiaSettings inertiaSettings;

	[NonSerialized]
	private CarCamera.InputState inputState;

	[NonSerialized]
	private InputHandler inputHandler;

	private CarCamera.State state;

	[Serializable]
	public class BlendSettings
	{
		public float blendDuration;

		public AnimationCurve blendCurve;
	}

	[Serializable]
	public class Settings
	{
		public Vector3 RotationCenter(Transform cameraParentTransform)
		{
			Vector3 position = cameraParentTransform.position;
			if (this.enableRotationCenter)
			{
				position = this.rotationCenter;
			}
			return position;
		}

		public string settingsName;

		public float cameraDistance;

		public GGMath.FloatRange verticalAngleRange;

		public float fov = 33f;

		public float startVerticalAngle;

		public float startHorizontalAngle;

		public float horizontalAngleSpeed;

		public float verticalAngleSpeed;

		public bool enableRotationCenter;

		public Vector3 rotationCenter;

		public bool changeAnglesAtStart;

		public Transform originalTransform;
	}

	[Serializable]
	public class InertiaSettings
	{
		public Vector2 GetInertia(Vector2 displace)
		{
			return new Vector2
			{
				x = Mathf.Sign(displace.x) * Mathf.Min(Mathf.Abs(displace.x), this.maxVelocity),
				y = Mathf.Sign(displace.y) * Mathf.Min(Mathf.Abs(displace.y), this.maxVelocity)
			};
		}

		[SerializeField]
		public float maxVelocity = 8f;

		[SerializeField]
		public float dragSpeed = 1f;

		[SerializeField]
		public float minInertia = 1f;

		[SerializeField]
		public float affinityToNew = 0.5f;
	}

	public struct State
	{
		public float horizontalAngle;

		public float verticalAngle;
	}

	private struct InputState
	{
		private Vector3 previousPosition;

		public bool isDown;

		public int touchId;

		public Vector2 lastPosition;

		public Vector2 inertia;
	}

	public struct AnimateIntoSettingsArguments
	{
		public CarCamera.Settings newSettings;

		public bool updateAngle;
	}

	private sealed class _003CDoAnimateIntoSettings_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoAnimateIntoSettings_003Ed__30(int _003C_003E1__state)
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
			CarCamera carCamera = this._003C_003E4__this;
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
				this._003CnewSettings_003E5__2 = this.arguments.newSettings;
				CarCamera.Settings usedSettings = carCamera.usedSettings;
				this._003Cduration_003E5__3 = carCamera.blendSettings.blendDuration;
				this._003Ccurve_003E5__4 = carCamera.blendSettings.blendCurve;
				this._003CrotationCenterStart_003E5__5 = usedSettings.RotationCenter(carCamera.transform);
				this._003CrotationCenterEnd_003E5__6 = this._003CnewSettings_003E5__2.RotationCenter(carCamera.transform);
				this._003ChorizontalAngleStart_003E5__7 = carCamera.state.horizontalAngle;
				this._003CverticalAngleStart_003E5__8 = carCamera.state.verticalAngle;
				this._003ChorizontalAngleEnd_003E5__9 = this._003CnewSettings_003E5__2.startHorizontalAngle;
				this._003CverticalAngleEnd_003E5__10 = this._003CnewSettings_003E5__2.startVerticalAngle;
				this._003CstartDistance_003E5__11 = usedSettings.cameraDistance;
				this._003CendDistance_003E5__12 = this._003CnewSettings_003E5__2.cameraDistance;
				this._003CcameraTransform_003E5__13 = carCamera.camera.transform;
				this._003CstartFOV_003E5__14 = carCamera.camera.fieldOfView;
				this._003CendFOV_003E5__15 = this._003CnewSettings_003E5__2.fov;
				this._003Ctime_003E5__16 = 0f;
			}
			if (this._003Ctime_003E5__16 > this._003Cduration_003E5__3)
			{
				carCamera.usedSettings = this._003CnewSettings_003E5__2;
				carCamera.UpdateCameraPosition();
				return false;
			}
			this._003Ctime_003E5__16 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__16);
			float t = this._003Ccurve_003E5__4.Evaluate(time);
			Vector3 vector = Vector3.Lerp(this._003CrotationCenterStart_003E5__5, this._003CrotationCenterEnd_003E5__6, t);
			float horizontalAngle = Mathf.LerpAngle(this._003ChorizontalAngleStart_003E5__7, this._003ChorizontalAngleEnd_003E5__9, t);
			float verticalAngle = Mathf.LerpAngle(this._003CverticalAngleStart_003E5__8, this._003CverticalAngleEnd_003E5__10, t);
			float fieldOfView = Mathf.Lerp(this._003CstartFOV_003E5__14, this._003CendFOV_003E5__15, t);
			float d = Mathf.Lerp(this._003CstartDistance_003E5__11, this._003CendDistance_003E5__12, t);
			if (this.arguments.updateAngle)
			{
				carCamera.state.horizontalAngle = horizontalAngle;
				carCamera.state.verticalAngle = verticalAngle;
			}
			Vector3 a = vector;
			Quaternion rotation = Quaternion.AngleAxis(carCamera.state.horizontalAngle, Vector3.up) * Quaternion.AngleAxis(carCamera.state.verticalAngle, Vector3.right);
			this._003CcameraTransform_003E5__13.position = vector + rotation * (Vector3.forward * d);
			this._003CcameraTransform_003E5__13.rotation = Quaternion.LookRotation(a - this._003CcameraTransform_003E5__13.position);
			carCamera.camera.fieldOfView = fieldOfView;
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

		public CarCamera.AnimateIntoSettingsArguments arguments;

		public CarCamera _003C_003E4__this;

		private CarCamera.Settings _003CnewSettings_003E5__2;

		private float _003Cduration_003E5__3;

		private AnimationCurve _003Ccurve_003E5__4;

		private Vector3 _003CrotationCenterStart_003E5__5;

		private Vector3 _003CrotationCenterEnd_003E5__6;

		private float _003ChorizontalAngleStart_003E5__7;

		private float _003CverticalAngleStart_003E5__8;

		private float _003ChorizontalAngleEnd_003E5__9;

		private float _003CverticalAngleEnd_003E5__10;

		private float _003CstartDistance_003E5__11;

		private float _003CendDistance_003E5__12;

		private Transform _003CcameraTransform_003E5__13;

		private float _003CstartFOV_003E5__14;

		private float _003CendFOV_003E5__15;

		private float _003Ctime_003E5__16;
	}
}
