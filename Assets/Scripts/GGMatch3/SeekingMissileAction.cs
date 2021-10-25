using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class SeekingMissileAction : BoardAction
	{
		public SeekingMissileAction.Settings animationParameters
		{
			get
			{
				return Match3Settings.instance.seekingMissileSettings;
			}
		}

		public override void OnStart(ActionManager manager)
		{
			base.OnStart(manager);
		}

		public IntVector2 endPosition
		{
			get
			{
				return this.endSlot.position;
			}
		}

		public void Init(SeekingMissileAction.Parameters parameters)
		{
			this.targetLock = this.lockContainer.NewLock();
			this.sourceLock = this.lockContainer.NewLock();
			this.parameters = parameters;
			this.hasCarpet = parameters.isHavingCarpet;
			Slot startSlot = parameters.startSlot;
			if (startSlot.canCarpetSpreadFromHere)
			{
				this.hasCarpet = true;
			}
			for (int i = 0; i < startSlot.neigbourSlots.Count; i++)
			{
				if (startSlot.neigbourSlots[i].canCarpetSpreadFromHere)
				{
					this.hasCarpet = true;
				}
			}
			this.targetLock.isSlotGravitySuspended = true;
			this.targetLock.isChipGeneratorSuspended = true;
			this.targetLock.isAvailableForSeekingMissileSuspended = true;
			this.targetLock.isAboutToBeDestroyed = true;
			this.sourceLock.isSlotGravitySuspended = true;
			this.sourceLock.isChipGeneratorSuspended = true;
			this.sourceLock.isAvailableForSeekingMissileSuspended = true;
			this.crossExplosionSlots.Clear();
			if (parameters.doCrossExplosion)
			{
				CrossExplosionAction crossExplosionAction = new CrossExplosionAction();
				crossExplosionAction.Init(new CrossExplosionAction.Parameters
				{
					game = parameters.game,
					startPosition = parameters.startPosition,
					radius = parameters.game.SeekingMissleCrossRadius,
					isHavingCarpet = this.hasCarpet
				});
				parameters.game.board.actionManager.AddAction(crossExplosionAction);
				this.crossExplosionSlots.AddRange(crossExplosionAction.GetAffectedSlots());
			}
			Slot[] slots = parameters.game.board.slots;
			this.endSlot = parameters.startSlot;
			List<Slot> list;
			if (parameters.hasOtherChip)
			{
				list = parameters.game.goals.BestSlotsForSeekingMissleWithChip(parameters.game, parameters.startSlot, parameters.otherChipType);
			}
			else
			{
				list = parameters.game.goals.BestSlotsForSeekingMissle(parameters.game, parameters.startSlot);
			}
			if (list != null && list.Count > 0)
			{
				int index = parameters.game.RandomRange(0, list.Count);
				this.endSlot = list[index];
			}
			this.targetLock.LockSlot(this.endSlot);
			this.sourceLock.LockSlot(parameters.startSlot);
			parameters.game.Play(GGSoundSystem.SFXType.SeekingMissleTakeOff);
		}

		public override void OnUpdate(float deltaTime)
		{
			this.deltaTime = deltaTime;
			if (this.flyCoroutine == null)
			{
				this.flyCoroutine = this.DoFly();
			}
			this.flyCoroutine.MoveNext();
		}

		public IEnumerator DoFly()
		{
			return new SeekingMissileAction._003CDoFly_003Ed__18(0)
			{
				_003C_003E4__this = this
			};
		}

		private void AffectOuterCircleWithExplosion(IntVector2 center, int radius, float shockWaveOffset)
		{
			Match3Game game = this.parameters.game;
			Vector3 b = game.LocalPositionOfCenter(center);
			for (int i = center.x - radius; i <= center.x + radius; i++)
			{
				for (int j = center.y - radius; j <= center.y + radius; j++)
				{
					int a = Mathf.Abs(center.x - i);
					int b2 = Mathf.Abs(center.y - j);
					if (Mathf.Max(a, b2) == radius)
					{
						Slot slot = game.board.GetSlot(new IntVector2(i, j));
						if (slot != null)
						{
							slot.offsetPosition = (slot.localPositionOfCenter - b).normalized * shockWaveOffset;
							slot.positionIntegrator.currentPosition = slot.offsetPosition;
						}
					}
				}
			}
		}

		public SeekingMissileAction.Trajectory TrajectoryFromAngle(int bigAngle)
		{
			SeekingMissileAction.Trajectory trajectory = new SeekingMissileAction.Trajectory();
			Vector3 vector = Quaternion.AngleAxis((float)bigAngle, Vector3.forward) * Vector3.right;
			float num = this.parameters.game.slotPhysicalSize.x * this.animationParameters.bigRadiusNormalized;
			Vector3 vector2 = this.animationParameters.startPosition + vector * num;
			float num2 = (float)(bigAngle + 180) + Mathf.Lerp(0f, 360f, this.animationParameters.bigExitDistance);
			Vector3 vector3 = Quaternion.AngleAxis(num2, Vector3.forward) * Vector3.right;
			float num3 = num * this.animationParameters.radiusRatio;
			Vector3 vector4 = vector2 + vector3 * (num - num3);
			if (Vector3.Distance(vector4, this.animationParameters.endPosition) < num3 + 0.001f)
			{
				return null;
			}
			Vector3 c1Tan = SeekingMissileAction.FindOuterTangents(new SeekingMissileAction.Circle(vector4, num3), new SeekingMissileAction.Circle(this.animationParameters.endPosition, 0.001f)).c1Tan1;
			float num4 = GGUtil.SignedAngle(vector3, (c1Tan - vector4).normalized);
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			Vector3 a = vector4 + vector3 * num3;
			float num5 = GGUtil.SignedAngle(-vector, (a - vector2).normalized);
			if (num5 < 0f)
			{
				num5 += 360f;
			}
			Vector3 vector5 = c1Tan;
			Vector3 endPosition = this.animationParameters.endPosition;
			float exitDistance = Vector3.Distance(endPosition, vector5);
			trajectory.bigCenter = vector2;
			trajectory.bigDirection = vector;
			trajectory.bigAngle = (float)bigAngle;
			trajectory.bigRadius = num;
			trajectory.bigExitAngle = num5;
			trajectory.smallCenter = vector4;
			trajectory.smallDirection = vector3;
			trajectory.smallAngle = num2;
			trajectory.smallRadius = num3;
			trajectory.smallExitAngle = num4;
			trajectory.lineStart = vector5;
			trajectory.lineEnd = endPosition;
			trajectory.exitDistance = exitDistance;
			return trajectory;
		}

		public static SeekingMissileAction.Tangents FindOuterTangents(SeekingMissileAction.Circle c1, SeekingMissileAction.Circle c2)
		{
			SeekingMissileAction.Tangents tangents = default(SeekingMissileAction.Tangents);
			bool flag = false;
			if (c1.radius > c2.radius)
			{
				SeekingMissileAction.Circle circle = c1;
				c1 = c2;
				c2 = circle;
				flag = true;
			}
			tangents.c1 = c1;
			tangents.c2 = c2;
			Vector3 vector = c2.position - c1.position;
			vector.z = 0f;
			float magnitude = vector.magnitude;
			tangents.gamaRad = Mathf.Atan2(c2.position.y - c1.position.y, c2.position.x - c1.position.x);
			tangents.betaRad = Mathf.Asin((c2.radius - c1.radius) / magnitude);
			tangents.alphaRad = tangents.gamaRad + tangents.betaRad;
			tangents.tan1AngleRad = tangents.alphaRad;
			tangents.tan2AngleRad = 3.14159274f + (tangents.gamaRad - tangents.betaRad);
			Vector3 a = Quaternion.AngleAxis(tangents.tan1AngleRad * 57.29578f, Vector3.forward) * Vector3.up;
			Vector3 a2 = Quaternion.AngleAxis(tangents.tan2AngleRad * 57.29578f, Vector3.forward) * Vector3.up;
			if (flag)
			{
				tangents.c2Tan1 = c1.position + a * c1.radius;
				tangents.c1Tan1 = c2.position + a * c2.radius;
				tangents.c2Tan2 = c1.position + a2 * c1.radius;
				tangents.c1Tan2 = c2.position + a2 * c2.radius;
			}
			else
			{
				tangents.c1Tan1 = c1.position + a * c1.radius;
				tangents.c2Tan1 = c2.position + a * c2.radius;
				tangents.c1Tan2 = c1.position + a2 * c1.radius;
				tangents.c2Tan2 = c2.position + a2 * c2.radius;
			}
			return tangents;
		}

		public SeekingMissileAction.Parameters parameters;

		public List<Slot> crossExplosionSlots = new List<Slot>();

		private Lock targetLock;

		private Lock sourceLock;

		private Slot endSlot;

		private bool hasCarpet;

		private IEnumerator flyCoroutine;

		private float deltaTime;

		public class Parameters
		{
			public IntVector2 startPosition
			{
				get
				{
					return this.startSlot.position;
				}
			}

			public bool hasOtherChip;

			public ChipType otherChipType;

			public Match3Game game;

			public Slot startSlot;

			public bool isHavingCarpet;

			public bool doCrossExplosion;
		}

		[Serializable]
		public class Settings
		{
			public float bigExitDistance
			{
				get
				{
					return this.bigExitDistanceRange.Lerp(this.t);
				}
			}

			public float smallExitDistance
			{
				get
				{
					return this.smallExitDistanceRange.Lerp(1f - this.t);
				}
			}

			public float bigRadiusNormalized;

			public float radiusRatio;

			public FloatRange altutudeScale = new FloatRange(1f, 1.5f);

			public float maxAltitude = 1f;

			public float timeToMaxAltitude = 0.25f;

			public float keepParticlesFor = 2f;

			public FloatRange bigExitDistanceRange;

			public FloatRange smallExitDistanceRange;

			public SeekingMissileAction.Settings.CurveParameters bigCircleAccelerationCurve = new SeekingMissileAction.Settings.CurveParameters();

			public SeekingMissileAction.Settings.CurveParameters smallCircleAccelerationCurve = new SeekingMissileAction.Settings.CurveParameters();

			public SeekingMissileAction.Settings.CurveParameters lineAccelerationCurve = new SeekingMissileAction.Settings.CurveParameters();

			public bool overrideStartPosition;

			public Vector3 startPosition;

			public bool overrideEndPosition;

			public Vector3 endPosition;

			public bool overrideBigAngle;

			public int bigAngle;

			public bool overrideT;

			public float t;

			public IntensityChange lightIntensityChange;

			public float hitLightIntensity;

			public float shockWaveOffset = 0.2f;

			public float shockWaveOffsetR1 = 0.1f;

			public bool useCameraShakeOnLand;

			public GeneralSettings.CameraShakeSettings cameraShake = new GeneralSettings.CameraShakeSettings();

			public bool useCameraShakeOnTakeOff;

			public GeneralSettings.CameraShakeSettings takeOffCameraShake = new GeneralSettings.CameraShakeSettings();

			[Serializable]
			public class CurveParameters
			{
				public float Evaluate(float time)
				{
					float time2 = time / this.duration;
					float t = this.curve.Evaluate(time2);
					return this.range.Lerp(t);
				}

				public AnimationCurve curve;

				public FloatRange range;

				public float duration;
			}
		}

		public enum State
		{
			BigCircle,
			SmallCircle,
			Line
		}

		public class Trajectory
		{
			public Vector3 bigCenter;

			public float bigRadius;

			public float bigAngle;

			public Vector3 bigDirection;

			public float bigExitAngle;

			public Vector3 smallCenter;

			public float smallRadius;

			public float smallAngle;

			public Vector3 smallDirection;

			public float smallExitAngle;

			public Vector3 lineStart;

			public Vector3 lineEnd;

			public float exitDistance;
		}

		public struct Tangents
		{
			public float gamaRad;

			public float betaRad;

			public float alphaRad;

			public float tan1AngleRad;

			public float tan2AngleRad;

			public SeekingMissileAction.Circle c1;

			public SeekingMissileAction.Circle c2;

			public Vector3 c1Tan1;

			public Vector3 c2Tan1;

			public Vector3 c1Tan2;

			public Vector3 c2Tan2;
		}

		public class Circle
		{
			public Circle(Vector3 position, float radius)
			{
				this.position = position;
				this.radius = radius;
			}

			public Vector3 position;

			public float radius;
		}

		private sealed class _003CDoFly_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoFly_003Ed__18(int _003C_003E1__state)
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
				SeekingMissileAction seekingMissileAction = this._003C_003E4__this;
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
					this._003Csettings_003E5__2 = Match3Settings.instance.seekingMissileSettings;
					if (this._003Csettings_003E5__2.useCameraShakeOnTakeOff)
					{
						seekingMissileAction.parameters.game.ShakeCamera(this._003Csettings_003E5__2.takeOffCameraShake);
					}
					seekingMissileAction.sourceLock.UnlockAll();
					if (!seekingMissileAction.animationParameters.overrideStartPosition)
					{
						seekingMissileAction.animationParameters.startPosition = seekingMissileAction.parameters.game.LocalPositionOfCenter(seekingMissileAction.parameters.startPosition);
					}
					if (!seekingMissileAction.animationParameters.overrideEndPosition)
					{
						seekingMissileAction.animationParameters.endPosition = seekingMissileAction.parameters.game.LocalPositionOfCenter(seekingMissileAction.endPosition);
					}
					if (!seekingMissileAction.animationParameters.overrideT)
					{
						seekingMissileAction.animationParameters.t = AnimRandom.Range(0f, 1f);
					}
					ComponentPool pool = seekingMissileAction.parameters.game.GetPool(PieceType.SeekingMissile);
					this._003Cbehaviour_003E5__3 = pool.Next<SeekingMissileBehaviour>(false);
					this._003Cbehaviour_003E5__3.Init();
					this._003Cbehaviour_003E5__3.localPosition = seekingMissileAction.animationParameters.startPosition;
					this._003Cbehaviour_003E5__3.ClearTrail();
					this._003Cbehaviour_003E5__3.gameObject.SetActive(true);
					SeekingMissileAction.Trajectory trajectory = null;
					for (int i = 0; i < 360; i += 10)
					{
						SeekingMissileAction.Trajectory trajectory2 = seekingMissileAction.TrajectoryFromAngle(i);
						if (trajectory2 != null)
						{
							if (trajectory == null)
							{
								trajectory = trajectory2;
							}
							else
							{
								float num2 = seekingMissileAction.animationParameters.smallExitDistance * 360f;
								if (Mathf.Abs(trajectory2.smallExitAngle - num2) < Mathf.Abs(trajectory.smallExitAngle - num2))
								{
									trajectory = trajectory2;
								}
							}
						}
					}
					if (seekingMissileAction.animationParameters.overrideBigAngle)
					{
						trajectory = seekingMissileAction.TrajectoryFromAngle(seekingMissileAction.animationParameters.bigAngle);
					}
					Vector3 bigDirection = trajectory.bigDirection;
					this._003CbigCenter_003E5__4 = trajectory.bigCenter;
					float bigAngle = trajectory.bigAngle;
					this._003CbigRadius_003E5__5 = trajectory.bigRadius;
					this._003CbigExitAngle_003E5__6 = trajectory.bigExitAngle;
					this._003CsmallDirection_003E5__7 = trajectory.smallDirection;
					this._003CsmallCenter_003E5__8 = trajectory.smallCenter;
					float smallAngle = trajectory.smallAngle;
					this._003CsmallRadius_003E5__9 = trajectory.smallRadius;
					this._003CsmallExitAngle_003E5__10 = trajectory.smallExitAngle;
					this._003ClineStart_003E5__11 = trajectory.lineStart;
					this._003ClineEnd_003E5__12 = trajectory.lineEnd;
					this._003CexitDistance_003E5__13 = trajectory.exitDistance;
					this._003Cangle_003E5__14 = 0f;
					this._003Cdistance_003E5__15 = 0f;
					this._003Cdirection_003E5__16 = -bigDirection;
					this._003CaccelerationTimer_003E5__17 = 0f;
					this._003ClinearSpeed_003E5__18 = 0f;
					this._003Cstate_003E5__19 = SeekingMissileAction.State.BigCircle;
					seekingMissileAction.endSlot.light.AddIntensityChange(this._003Csettings_003E5__2.lightIntensityChange);
					this._003Cheight_003E5__20 = 0f;
					float maxAltitude = this._003Csettings_003E5__2.maxAltitude;
					this._003CheightWhenStartDescent_003E5__21 = this._003Cheight_003E5__20;
				}
				if (this._003Cstate_003E5__19 == SeekingMissileAction.State.BigCircle)
				{
					this._003Cbehaviour_003E5__3.localPosition = this._003CbigCenter_003E5__4 + Quaternion.AngleAxis(this._003Cangle_003E5__14, Vector3.forward) * this._003Cdirection_003E5__16 * this._003CbigRadius_003E5__5;
					Vector3 v = Vector3.Cross((this._003Cbehaviour_003E5__3.localPosition - this._003CbigCenter_003E5__4).normalized, Vector3.forward);
					this._003Cbehaviour_003E5__3.SetDirection(v);
					this._003ClinearSpeed_003E5__18 += seekingMissileAction.animationParameters.bigCircleAccelerationCurve.Evaluate(this._003CaccelerationTimer_003E5__17);
					float num3 = this._003ClinearSpeed_003E5__18 / this._003CbigRadius_003E5__5 * 57.29578f;
					this._003Cangle_003E5__14 += seekingMissileAction.deltaTime * num3;
					this._003CaccelerationTimer_003E5__17 += seekingMissileAction.deltaTime;
				}
				else if (this._003Cstate_003E5__19 == SeekingMissileAction.State.SmallCircle)
				{
					this._003Cbehaviour_003E5__3.localPosition = this._003CsmallCenter_003E5__8 + Quaternion.AngleAxis(this._003Cangle_003E5__14, Vector3.forward) * this._003Cdirection_003E5__16 * this._003CsmallRadius_003E5__9;
					Vector3 v2 = Vector3.Cross((this._003Cbehaviour_003E5__3.localPosition - this._003CsmallCenter_003E5__8).normalized, Vector3.forward);
					this._003Cbehaviour_003E5__3.SetDirection(v2);
					this._003ClinearSpeed_003E5__18 += seekingMissileAction.animationParameters.smallCircleAccelerationCurve.Evaluate(this._003CaccelerationTimer_003E5__17);
					float num4 = this._003ClinearSpeed_003E5__18 / this._003CsmallRadius_003E5__9 * 57.29578f;
					this._003Cangle_003E5__14 += seekingMissileAction.deltaTime * num4;
					this._003CaccelerationTimer_003E5__17 += seekingMissileAction.deltaTime;
				}
				else if (this._003Cstate_003E5__19 == SeekingMissileAction.State.Line)
				{
					this._003Cbehaviour_003E5__3.localPosition = this._003ClineStart_003E5__11 + this._003Cdirection_003E5__16 * this._003Cdistance_003E5__15;
					this._003Cbehaviour_003E5__3.SetDirection(-this._003Cdirection_003E5__16);
					this._003ClinearSpeed_003E5__18 += seekingMissileAction.animationParameters.lineAccelerationCurve.Evaluate(this._003CaccelerationTimer_003E5__17);
					this._003Cdistance_003E5__15 += seekingMissileAction.deltaTime * this._003ClinearSpeed_003E5__18;
					this._003CaccelerationTimer_003E5__17 += seekingMissileAction.deltaTime;
				}
				if (this._003Cstate_003E5__19 == SeekingMissileAction.State.BigCircle && this._003Cangle_003E5__14 > this._003CbigExitAngle_003E5__6)
				{
					this._003Cstate_003E5__19 = SeekingMissileAction.State.SmallCircle;
					this._003Cdirection_003E5__16 = this._003CsmallDirection_003E5__7;
					this._003Cangle_003E5__14 = 0f;
					this._003CaccelerationTimer_003E5__17 = 0f;
				}
				else if (this._003Cstate_003E5__19 == SeekingMissileAction.State.SmallCircle && this._003Cangle_003E5__14 > this._003CsmallExitAngle_003E5__10)
				{
					this._003Cstate_003E5__19 = SeekingMissileAction.State.Line;
					this._003Cdirection_003E5__16 = (this._003ClineEnd_003E5__12 - this._003ClineStart_003E5__11).normalized;
					this._003Cangle_003E5__14 = 0f;
					this._003CaccelerationTimer_003E5__17 = 0f;
				}
				else if (this._003Cstate_003E5__19 == SeekingMissileAction.State.Line && this._003Cdistance_003E5__15 > this._003CexitDistance_003E5__13)
				{
					this._003Cbehaviour_003E5__3.RemoveFromGameAfter(this._003Csettings_003E5__2.keepParticlesFor);
					seekingMissileAction.targetLock.UnlockAll();
					seekingMissileAction.sourceLock.UnlockAll();
					seekingMissileAction.endSlot.light.AddLight(this._003Csettings_003E5__2.hitLightIntensity);
					seekingMissileAction.parameters.game.particles.CreateParticles(seekingMissileAction.endSlot, Match3Particles.PositionType.MissleHitTarget);
					if (this._003Csettings_003E5__2.useCameraShakeOnLand)
					{
						seekingMissileAction.parameters.game.ShakeCamera(this._003Csettings_003E5__2.cameraShake);
					}
					if (!seekingMissileAction.parameters.hasOtherChip)
					{
						seekingMissileAction.parameters.game.Play(GGSoundSystem.SFXType.SeekingMissleLand);
						SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
						slotDestroyParams.isHitByBomb = true;
						slotDestroyParams.bombType = ChipType.SeekingMissle;
						slotDestroyParams.isHavingCarpet = seekingMissileAction.hasCarpet;
						seekingMissileAction.endSlot.OnDestroySlot(slotDestroyParams);
					}
					else if (seekingMissileAction.parameters.otherChipType == ChipType.Bomb)
					{
						ExplosionAction explosionAction = new ExplosionAction();
						ExplosionAction.ExplosionSettings settings = default(ExplosionAction.ExplosionSettings);
						settings.bombChip = null;
						settings.position = seekingMissileAction.endPosition;
						settings.radius = 3;
						settings.isHavingCarpet = seekingMissileAction.hasCarpet;
						settings.isUsingBombAreaOfEffect = true;
						explosionAction.Init(seekingMissileAction.parameters.game, settings);
						seekingMissileAction.parameters.game.board.actionManager.AddAction(explosionAction);
					}
					else if (seekingMissileAction.parameters.otherChipType == ChipType.HorizontalRocket || seekingMissileAction.parameters.otherChipType == ChipType.VerticalRocket)
					{
						FlyLineRocketAction flyLineRocketAction = new FlyLineRocketAction();
						flyLineRocketAction.Init(new FlyLineRocketAction.Params
						{
							bombChip = null,
							game = seekingMissileAction.parameters.game,
							position = seekingMissileAction.endPosition,
							rocketType = seekingMissileAction.parameters.otherChipType,
							prelock = true,
							isHavingCarpet = seekingMissileAction.hasCarpet
						});
						seekingMissileAction.parameters.game.board.actionManager.AddAction(flyLineRocketAction);
					}
					seekingMissileAction.AffectOuterCircleWithExplosion(seekingMissileAction.endPosition, 1, this._003Csettings_003E5__2.shockWaveOffset);
					seekingMissileAction.AffectOuterCircleWithExplosion(seekingMissileAction.endPosition, 2, this._003Csettings_003E5__2.shockWaveOffsetR1);
					seekingMissileAction.isAlive = false;
					return false;
				}
				if (this._003Cstate_003E5__19 == SeekingMissileAction.State.Line)
				{
					float t = Mathf.InverseLerp(0f, this._003CexitDistance_003E5__13, this._003Cdistance_003E5__15);
					this._003Cheight_003E5__20 = Mathf.Lerp(this._003CheightWhenStartDescent_003E5__21, 0f, t);
				}
				else
				{
					this._003Cheight_003E5__20 += this._003Csettings_003E5__2.maxAltitude / this._003Csettings_003E5__2.timeToMaxAltitude * seekingMissileAction.deltaTime;
					this._003Cheight_003E5__20 = Mathf.Min(this._003Cheight_003E5__20, this._003Csettings_003E5__2.maxAltitude);
					this._003CheightWhenStartDescent_003E5__21 = this._003Cheight_003E5__20;
				}
				float d = this._003Csettings_003E5__2.altutudeScale.Lerp(Mathf.InverseLerp(0f, this._003Csettings_003E5__2.maxAltitude, this._003Cheight_003E5__20));
				this._003Cbehaviour_003E5__3.localScale = Vector3.one * d;
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

			public SeekingMissileAction _003C_003E4__this;

			private SeekingMissileAction.Settings _003Csettings_003E5__2;

			private SeekingMissileBehaviour _003Cbehaviour_003E5__3;

			private Vector3 _003CbigCenter_003E5__4;

			private float _003CbigRadius_003E5__5;

			private float _003CbigExitAngle_003E5__6;

			private Vector3 _003CsmallDirection_003E5__7;

			private Vector3 _003CsmallCenter_003E5__8;

			private float _003CsmallRadius_003E5__9;

			private float _003CsmallExitAngle_003E5__10;

			private Vector3 _003ClineStart_003E5__11;

			private Vector3 _003ClineEnd_003E5__12;

			private float _003CexitDistance_003E5__13;

			private float _003Cangle_003E5__14;

			private float _003Cdistance_003E5__15;

			private Vector3 _003Cdirection_003E5__16;

			private float _003CaccelerationTimer_003E5__17;

			private float _003ClinearSpeed_003E5__18;

			private SeekingMissileAction.State _003Cstate_003E5__19;

			private float _003Cheight_003E5__20;

			private float _003CheightWhenStartDescent_003E5__21;
		}
	}
}
