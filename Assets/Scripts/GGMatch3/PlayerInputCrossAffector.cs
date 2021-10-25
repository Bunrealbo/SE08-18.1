using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class PlayerInputCrossAffector : PlayerInput.AffectorBase
	{
		public PlayerInputCrossAffector.Settings settings
		{
			get
			{
				return Match3Settings.instance.playerInputCrossAffectorSettings;
			}
		}

		public override void Clear()
		{
			Slot.RemoveLocks(this.lockedSlots, this.globalLock);
			for (int i = 0; i < this.affectedSlots.Count; i++)
			{
				Slot slot = this.affectedSlots[i];
				if (slot != null)
				{
					slot.light.RemoveLight(this.distanceLight);
					slot.light.AddLight(this.distanceLight.GetCurrentIntensity(slot.light));
				}
			}
		}

		private void UpdateIntensity()
		{
			PlayerInputCrossAffector.Settings settings = this.settings;
			float num = Mathf.InverseLerp(0f, settings.timeToFullIntensity, this.timeActive);
			if (settings.intensityCurve != null)
			{
				num = settings.intensityCurve.Evaluate(num);
			}
			this.distanceLight.intensityMult = num;
		}

		public override void OnUpdate(PlayerInput.AffectorUpdateParams updateParams)
		{
			PlayerInputCrossAffector.Settings settings = this.settings;
			this.timeActive += Time.deltaTime;
			this.UpdateIntensity();
			Match3Game game = updateParams.input.game;
			Slot[] slots = game.board.slots;
			Vector3 b = game.LocalPositionOfCenter(this.startPosition);
			this.angle += Time.deltaTime * settings.angleSpeed;
			foreach (Slot slot in slots)
			{
				if (slot != null)
				{
					Vector3 a = slot.localPositionOfCenter;
					Chip slotComponent = slot.GetSlotComponent<Chip>();
					if (slotComponent != null)
					{
						TransformBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
						if (componentBehaviour != null)
						{
							a = componentBehaviour.localPosition;
						}
					}
					Vector3 vector = a - b;
					vector.z = 0f;
					float num = Mathf.Abs(vector.x);
					float num2 = Mathf.Abs(vector.y);
					float num3 = Mathf.Min(num, num2);
					float num4 = Mathf.Max(num, num2);
					bool flag = num4 <= (float)this.radius;
					bool flag2 = num3 <= (float)this.radius;
					if (vector == Vector3.zero)
					{
						slot.prevOffsetPosition = (slot.offsetPosition = Vector3.zero);
						slot.offsetScale = new Vector3(settings.originScale, settings.originScale, 1f);
						slot.positionIntegrator.ResetAcceleration();
					}
					else if (flag)
					{
						slot.prevOffsetPosition = (slot.offsetPosition = Vector3.zero);
						slot.offsetScale = new Vector3(1f, 1f, 1f);
						slot.positionIntegrator.ResetAcceleration();
					}
					else
					{
						Vector3 vector2 = Vector3.zero;
						if (num > num2)
						{
							vector2 = Vector3.right;
						}
						else
						{
							vector2 = Vector3.up;
						}
						Vector3 a2;
						float num5;
						float num6;
						if (vector2.y != 0f)
						{
							a2 = Vector3.right * Mathf.Sign(vector.x);
							num5 = Mathf.Abs(vector.y);
							num6 = Mathf.Abs(vector.x);
						}
						else
						{
							a2 = Vector3.up * Mathf.Sign(vector.y);
							num5 = Mathf.Abs(vector.x);
							num6 = Mathf.Abs(vector.y);
						}
						num6 = Mathf.Max(0f, num6 - (float)this.radius);
						if (num6 < settings.affectedOrtho)
						{
							if (flag2)
							{
								vector = vector2 * Mathf.Sign(Vector3.Dot(vector2, vector));
							}
							float num7 = Mathf.InverseLerp(0f, settings.affectedOrtho, num6);
							float num8 = Mathf.Lerp(1f, 0f, num7);
							float num9 = Mathf.InverseLerp(num4, 0f, num5);
							if (settings.displaceCurve != null)
							{
								num9 = settings.displaceCurve.Evaluate(num9);
							}
							float num10 = settings.displaceRange.Lerp(num9);
							float num11 = Mathf.Sin((settings.phaseOffsetMult * num5 + this.angle) * 57.29578f) * settings.amplitude;
							Vector3 b2 = Vector3.zero;
							float num12 = 1f;
							if (num7 != 0f)
							{
								float t = Mathf.InverseLerp(0.5f, 0f, Mathf.Max(Mathf.Abs(slot.prevOffsetPosition.x), Mathf.Abs(slot.prevOffsetPosition.y)));
								b2 = a2 * (num10 * num8);
								if (num == num2)
								{
									b2 = vector.normalized * (num10 * num8);
								}
								num12 = settings.scaleRange.Lerp(t);
							}
							else
							{
								b2 = vector.normalized * (num11 + num10);
							}
							slot.offsetPosition = Vector3.Lerp(slot.prevOffsetPosition, b2, settings.displacePull * Time.deltaTime);
							slot.prevOffsetPosition = slot.offsetPosition;
							slot.positionIntegrator.SetPosition(slot.offsetPosition);
							slot.offsetScale = Vector3.one;
							if (num == num2)
							{
								slot.offsetScale = new Vector3(num12, num12, 1f);
							}
							else
							{
								float num13 = 1f;
								if (num12 != 1f)
								{
									num13 = 1f + num12 * settings.orthoScaleInfluence;
								}
								if (vector2.x != 0f)
								{
									slot.offsetScale = new Vector3(num13, num12, 1f);
								}
								else
								{
									slot.offsetScale = new Vector3(num12, num13, 1f);
								}
							}
						}
					}
				}
			}
		}

		public IntVector2 startPosition;

		private PlayerInputCrossAffector.DistanceLight distanceLight;

		private float angle;

		private float timeActive;

		private int radius;

		public List<Slot> affectedSlots;

		public List<Slot> lockedSlots;

		public Lock globalLock;

		public class DistanceLight : LightSlotComponent.PermanentLight
		{
			public PlayerInputCrossAffector.Settings settings
			{
				get
				{
					return Match3Settings.instance.playerInputCrossAffectorSettings;
				}
			}

			public override float GetCurrentIntensity(LightSlotComponent component)
			{
				Slot slot = component.slot;
				if (slot == null)
				{
					return 0f;
				}
				int num = Mathf.Max(Mathf.Abs(slot.position.x - this.startPosition.x), Mathf.Abs(slot.position.y - this.startPosition.y));
				return this.intensityMult * this.settings.lightIntensityRange.Lerp(Mathf.InverseLerp(this.settings.maxLightDistance, 0f, (float)num));
			}

			public IntVector2 startPosition;

			public float intensityMult;
		}

		[Serializable]
		public class Settings
		{
			public float maxDistance = 10f;

			public FloatRange displaceRange = new FloatRange(0f, 1f);

			public AnimationCurve displaceCurve;

			public float affectedOrtho = 1f;

			public float displacePull = 10f;

			public float angleSpeed = 100f;

			public float phaseOffsetMult = 1f;

			public float amplitude = 0.05f;

			public float originScale = 2f;

			public float lightIntensity = 1f;

			public float distanceDelay = 0.05f;

			public FloatRange lightIntensityRange;

			public float maxLightDistance = 5f;

			public float timeToFullIntensity;

			public AnimationCurve intensityCurve;

			public bool lockLine;

			public FloatRange scaleRange = new FloatRange(1f, 0.5f);

			public float orthoScaleInfluence = 0.25f;
		}
	}
}
