using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltTransformTrackerScript : MonoBehaviour
	{
		private void Start()
		{
			if (this.LightningScript != null)
			{
				this.LightningScript.CustomTransformHandler.RemoveAllListeners();
				this.LightningScript.CustomTransformHandler.AddListener(new UnityAction<LightningCustomTransformStateInfo>(this.CustomTransformHandler));
			}
		}

		private static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
		{
			Vector2 normalized = (vec2 - vec1).normalized;
			return Vector2.Angle(Vector2.right, normalized) * Mathf.Sign(vec2.y - vec1.y);
		}

		private static void UpdateTransform(LightningCustomTransformStateInfo state, LightningBoltPrefabScript script, RangeOfFloats scaleLimit)
		{
			if (state.Transform == null || state.StartTransform == null)
			{
				return;
			}
			if (state.EndTransform == null)
			{
				state.Transform.position = state.StartTransform.position - state.BoltStartPosition;
				return;
			}
			Quaternion rotation;
			if ((script.CameraMode == CameraMode.Auto && script.Camera.orthographic) || script.CameraMode == CameraMode.OrthographicXY)
			{
				float num = LightningBoltTransformTrackerScript.AngleBetweenVector2(state.BoltStartPosition, state.BoltEndPosition);
				rotation = Quaternion.AngleAxis(LightningBoltTransformTrackerScript.AngleBetweenVector2(state.StartTransform.position, state.EndTransform.position) - num, Vector3.forward);
			}
			if (script.CameraMode == CameraMode.OrthographicXZ)
			{
				float num2 = LightningBoltTransformTrackerScript.AngleBetweenVector2(new Vector2(state.BoltStartPosition.x, state.BoltStartPosition.z), new Vector2(state.BoltEndPosition.x, state.BoltEndPosition.z));
				rotation = Quaternion.AngleAxis(LightningBoltTransformTrackerScript.AngleBetweenVector2(new Vector2(state.StartTransform.position.x, state.StartTransform.position.z), new Vector2(state.EndTransform.position.x, state.EndTransform.position.z)) - num2, Vector3.up);
			}
			else
			{
				Quaternion rotation2 = Quaternion.LookRotation((state.BoltEndPosition - state.BoltStartPosition).normalized);
				rotation = Quaternion.LookRotation((state.EndTransform.position - state.StartTransform.position).normalized) * Quaternion.Inverse(rotation2);
			}
			state.Transform.rotation = rotation;
			float num3 = Vector3.Distance(state.BoltStartPosition, state.BoltEndPosition);
			float num4 = Vector3.Distance(state.EndTransform.position, state.StartTransform.position);
			float num5 = Mathf.Clamp((num3 < Mathf.Epsilon) ? 1f : (num4 / num3), scaleLimit.Minimum, scaleLimit.Maximum);
			state.Transform.localScale = new Vector3(num5, num5, num5);
			Vector3 b = rotation * (num5 * state.BoltStartPosition);
			state.Transform.position = state.StartTransform.position - b;
		}

		public void CustomTransformHandler(LightningCustomTransformStateInfo state)
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.LightningScript == null)
			{
				UnityEngine.Debug.LogError("LightningScript property must be set to non-null.");
				return;
			}
			if (state.State == LightningCustomTransformState.Executing)
			{
				LightningBoltTransformTrackerScript.UpdateTransform(state, this.LightningScript, this.ScaleLimit);
				return;
			}
			if (state.State == LightningCustomTransformState.Started)
			{
				state.StartTransform = this.StartTarget;
				state.EndTransform = this.EndTarget;
				this.transformStartPositions[base.transform] = state;
				return;
			}
			this.transformStartPositions.Remove(base.transform);
		}

		public LightningBoltPrefabScript LightningScript;

		public Transform StartTarget;

		public Transform EndTarget;

		public RangeOfFloats ScaleLimit = new RangeOfFloats
		{
			Minimum = 0.1f,
			Maximum = 10f
		};

		private readonly Dictionary<Transform, LightningCustomTransformStateInfo> transformStartPositions = new Dictionary<Transform, LightningCustomTransformStateInfo>();
	}
}
