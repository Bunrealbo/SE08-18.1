using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZCameraShake
{
	public class CameraShaker : MonoBehaviour
	{
		private void Awake()
		{
			CameraShaker.Instance = this;
            if (!CameraShaker.instanceList.ContainsKey(base.gameObject.name))
            {

                CameraShaker.instanceList.Add(base.gameObject.name, this);
            }
		}

		private void Update()
		{
			this.posAddShake = Vector3.zero;
			this.rotAddShake = Vector3.zero;
			int num = 0;
			while (num < this.cameraShakeInstances.Count && num < this.cameraShakeInstances.Count)
			{
				CameraShakeInstance cameraShakeInstance = this.cameraShakeInstances[num];
				if (cameraShakeInstance.CurrentState == CameraShakeState.Inactive && cameraShakeInstance.DeleteOnInactive)
				{
					this.cameraShakeInstances.RemoveAt(num);
					num--;
				}
				else if (cameraShakeInstance.CurrentState != CameraShakeState.Inactive)
				{
					this.posAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.PositionInfluence);
					this.rotAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.RotationInfluence);
				}
				num++;
			}
			base.transform.localPosition = this.posAddShake + this.RestPositionOffset;
			base.transform.localEulerAngles = this.rotAddShake + this.RestRotationOffset;
		}

		public static CameraShaker GetInstance(string name)
		{
			CameraShaker result;
			if (CameraShaker.instanceList.TryGetValue(name, out result))
			{
				return result;
			}
			UnityEngine.Debug.LogError("CameraShake " + name + " not found!");
			return null;
		}

		public CameraShakeInstance Shake(CameraShakeInstance shake)
		{
			this.cameraShakeInstances.Add(shake);
			return shake;
		}

		public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
		{
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
			cameraShakeInstance.PositionInfluence = this.DefaultPosInfluence;
			cameraShakeInstance.RotationInfluence = this.DefaultRotInfluence;
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence)
		{
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
			cameraShakeInstance.PositionInfluence = posInfluence;
			cameraShakeInstance.RotationInfluence = rotInfluence;
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
		{
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
			cameraShakeInstance.PositionInfluence = this.DefaultPosInfluence;
			cameraShakeInstance.RotationInfluence = this.DefaultRotInfluence;
			cameraShakeInstance.StartFadeIn(fadeInTime);
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence)
		{
			CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
			cameraShakeInstance.PositionInfluence = posInfluence;
			cameraShakeInstance.RotationInfluence = rotInfluence;
			cameraShakeInstance.StartFadeIn(fadeInTime);
			this.cameraShakeInstances.Add(cameraShakeInstance);
			return cameraShakeInstance;
		}

		public List<CameraShakeInstance> ShakeInstances
		{
			get
			{
				return new List<CameraShakeInstance>(this.cameraShakeInstances);
			}
		}

		private void OnDestroy()
		{
			CameraShaker.instanceList.Remove(base.gameObject.name);
		}

		public static CameraShaker Instance;

		private static Dictionary<string, CameraShaker> instanceList = new Dictionary<string, CameraShaker>();

		public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);

		public Vector3 DefaultRotInfluence = new Vector3(1f, 1f, 1f);

		public Vector3 RestPositionOffset = new Vector3(0f, 0f, 0f);

		public Vector3 RestRotationOffset = new Vector3(0f, 0f, 0f);

		private Vector3 posAddShake;

		private Vector3 rotAddShake;

		private List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();
	}
}
