using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class MovingElement : SlotComponent
	{
		public override int sortingOrder
		{
			get
			{
				return 10;
			}
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}

		public override bool isMovedByGravity
		{
			get
			{
				return true;
			}
		}

		public override bool isPreventingOtherChipsToFallIntoSlot
		{
			get
			{
				return true;
			}
		}

		public override long lastMoveFrameIndex
		{
			get
			{
				return Math.Max(this.physics.lastMoveFrameIndex, this.teleportAnimation.lastMoveFrame);
			}
		}

		public override float lastMoveTime
		{
			get
			{
				return Mathf.Max(this.physics.lastMoveTime, this.teleportAnimation.lastMoveTime);
			}
		}

		public override bool isSlotSwapSuspended
		{
			get
			{
				return this.physics.isActive || this.teleportAnimation.isActive;
			}
		}

		public override bool isSlotMatchingSuspended
		{
			get
			{
				return this.physics.isActive || this.teleportAnimation.isActive;
			}
		}

		public override bool isMoving
		{
			get
			{
				return this.physics.isActive || this.teleportAnimation.isActive;
			}
		}

		public override bool isMoveByConveyorSuspended
		{
			get
			{
				return this.physics.isActive || this.teleportAnimation.isActive;
			}
		}

		public override bool isSlotGravitySuspended
		{
			get
			{
				return this.physics.isActive || this.teleportAnimation.isActive;
			}
		}

		public override void OnCreatedBySlot(Slot toSlot)
		{
			Match3Board board = toSlot.game.board;
			IntVector2 intVector = new IntVector2(0, 0);
			List<IntVector2> forceDirections = toSlot.gravity.forceDirections;
			for (int i = 0; i < forceDirections.Count; i++)
			{
				IntVector2 b = forceDirections[i];
				intVector += b;
			}
			this.physics.StartMove(toSlot.game.LocalPositionOfCenter(toSlot.position - intVector), toSlot.localPositionOfCenter, board.currentFrameIndex, board.currentTime);
			this.slot.OnSlotComponentMadeATransformChange(this);
		}

		public override void OnMovedBySlotGravity(Slot fromSlot, Slot toSlot, MoveContentsToSlotParams moveParams)
		{
			base.OnMovedBySlotGravity(fromSlot, toSlot, moveParams);
			long currentFrameIndex = fromSlot.game.board.currentFrameIndex;
			Match3Board board = fromSlot.game.board;
			float num = 0f;
			long num2 = currentFrameIndex - this.teleportAnimation.lastMoveFrame;
			bool flag = true;
			if (num2 <= 1L)
			{
				num = Mathf.Min(this.teleportAnimation.currentSpeed, Match3Settings.instance.pipeSettings.maxContinueVelocity);
				flag = false;
			}
			if (currentFrameIndex - this.physics.lastMoveFrameIndex <= 1L)
			{
				num = this.physics.speed;
				flag = false;
			}
			if (moveParams.isFromPortal)
			{
				IntVector2 intVector = toSlot.gravity.forceDirections[0];
				TeleporterAnimation.MoveParams mp = default(TeleporterAnimation.MoveParams);
				mp.chip = this;
				mp.game = this.lastConnectedSlot.game;
				mp.positionToMoveFrom = fromSlot.position;
				mp.directionToMoveFrom = fromSlot.gravity.forceDirections[0];
				mp.entrancePipe = fromSlot.entrancePipe;
				mp.exitPipe = toSlot.exitPipe;
				mp.positionToMoveTo = toSlot.position;
				mp.directionToMoveTo = toSlot.gravity.forceDirections[0];
				mp.initialSpeed = num;
				mp.currentFrameIndex = currentFrameIndex;
				mp.currentTime = board.currentTime;
				this.teleportAnimation.StartMove(mp);
			}
			else
			{
				this.physics.speed = num;
				this.physics.StartMove(fromSlot.localPositionOfCenter, toSlot.localPositionOfCenter, currentFrameIndex, board.currentTime);
			}
			if (flag)
			{
				SlotComponent.SlotStartMoveParams startMoveParams = default(SlotComponent.SlotStartMoveParams);
				startMoveParams.fromSlot = fromSlot;
				startMoveParams.toSlot = toSlot;
				startMoveParams.slotComponent = this;
				this.slot.OnSlotComponentMadeAStartMove(startMoveParams);
			}
			this.slot.OnSlotComponentMadeATransformChange(this);
		}

		public override void OnUpdate(float deltaTime)
		{
			if (this.slot == null)
			{
				return;
			}
			TransformBehaviour componentBehaviour = base.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour != null)
			{
				componentBehaviour.slotOffsetPosition = this.slot.offsetPosition;
				Vector3 offsetScale = this.slot.offsetScale;
				componentBehaviour.slotLocalScale = offsetScale;
			}
			UpdateParams updateParams = default(UpdateParams);
			updateParams.deltaTime = deltaTime;
			for (;;)
			{
				UpdateResult updateResult = default(UpdateResult);
				if (this.teleportAnimation.isActive)
				{
					updateParams.udpateIteration = 0f;
					do
					{
						updateResult = this.teleportAnimation.OnUpdate(updateParams);
						updateParams.udpateIteration += 1f;
						if (updateResult.wasTraveling && !this.teleportAnimation.isActive)
						{
							this.slot.ApplySlotGravity();
						}
					}
					while (updateResult.leftOverDeltaTime > 0f && this.teleportAnimation.isActive);
					if (updateResult.leftOverDeltaTime <= 0f)
					{
						goto IL_164;
					}
					updateParams.deltaTime = updateResult.leftOverDeltaTime;
					updateParams.udpateIteration = 0f;
				}
				if (this.slot.isChipGravitySuspended)
				{
					break;
				}
				updateResult = this.physics.OnUpdate(updateParams);
				if (!this.physics.isActive && updateResult.wasTraveling)
				{
					this.slot.ApplySlotGravity();
				}
				if (updateResult.leftOverDeltaTime <= 0f)
				{
					goto IL_164;
				}
				updateParams.udpateIteration += 1f;
				updateParams.deltaTime = updateResult.leftOverDeltaTime;
			}
			return;
			IL_164:
			this.slot.OnSlotComponentMadeATransformChange(this);
			if (!this.slot.isMovingElementRequired)
			{
				base.RemoveFromSlot();
				base.RemoveFromGame();
			}
		}

		public override void OnAddedToSlot(Slot slot)
		{
			base.OnAddedToSlot(slot);
			this.physics.chip = this;
		}

		private ChipPhysics physics = new ChipPhysics();

		private TeleporterAnimation teleportAnimation = new TeleporterAnimation();
	}
}
