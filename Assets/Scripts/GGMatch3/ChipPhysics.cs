using System;
using UnityEngine;

namespace GGMatch3
{
	public class ChipPhysics
	{
		public float speed
		{
			get
			{
				return Mathf.Max(this.velocity.x, this.velocity.y);
			}
			set
			{
				this.velocity.y = value;
				this.velocity.x = value;
			}
		}

		public void StartMove(Vector3 movingFrom, Vector3 movingTo, long frameIndex, float currentTime)
		{
			this.isActive = true;
			this.isFalling = true;
			this.movingTo = movingTo;
			this.movingFrom = movingFrom;
			this.currentPosition = movingFrom;
			this.lastMoveFrameIndex = frameIndex;
			this.lastMoveTime = currentTime;
		}

		public bool isArrivedToDestination
		{
			get
			{
				return this.currentPosition == this.movingTo;
			}
		}

		public UpdateResult OnUpdate(UpdateParams updateParams)
		{
			float deltaTime = updateParams.deltaTime;
			UpdateResult result = default(UpdateResult);
			if (!this.isActive)
			{
				return result;
			}
			if (this.chip.slot == null)
			{
				return result;
			}
			if (this.isArrivedToDestination)
			{
				return result;
			}
			result.wasTraveling = true;
			Match3Game game = this.chip.slot.game;
			this.lastMoveTime = game.board.currentTime;
			this.lastMoveFrameIndex = game.board.currentFrameIndex;
			GravitySettings gravitySettings = this.chip.slot.game.settings.gravitySettings;
			IntVector2 position = this.chip.slot.position;
			Vector3 vector = this.currentPosition;
			Vector2 normalizedPositionWithinBoard = this.chip.slot.normalizedPositionWithinBoard;
			float num = gravitySettings.gravityRange.Lerp(normalizedPositionWithinBoard.y);
			float a = gravitySettings.minVelocityRange.Lerp(normalizedPositionWithinBoard.y);
			this.velocity.x = Mathf.Max(a, this.velocity.x);
			this.velocity.y = Mathf.Max(a, this.velocity.y);
			if (updateParams.udpateIteration == 0f)
			{
				if (this.currentPosition.x != this.movingTo.x)
				{
					this.velocity.x = this.velocity.x + num * deltaTime;
				}
				if (this.currentPosition.y != this.movingTo.y)
				{
					this.velocity.y = this.velocity.y + num * deltaTime;
				}
			}
			float num2 = Mathf.Max(this.velocity.x, this.velocity.y);
			num2 = Mathf.Min(gravitySettings.maxVelocity, num2);
			float num3 = Mathf.Max(Mathf.Abs(this.currentPosition.x - this.movingTo.x), Mathf.Abs(this.currentPosition.y - this.movingTo.y));
			if (num2 * deltaTime > num3)
			{
				result.leftOverDeltaTime = deltaTime - num3 / num2;
			}
			this.currentPosition.x = Mathf.MoveTowards(this.currentPosition.x, this.movingTo.x, num2 * deltaTime);
			this.currentPosition.y = Mathf.MoveTowards(this.currentPosition.y, this.movingTo.y, num2 * deltaTime);
			this.currentPosition.z = this.movingTo.z;
			TransformBehaviour componentBehaviour = this.chip.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour != null)
			{
				componentBehaviour.localPosition = this.currentPosition;
			}
			if (this.isArrivedToDestination)
			{
				this.isActive = false;
			}
			return result;
		}

		public SlotComponent chip;

		public Vector3 movingFrom;

		public Vector3 movingTo;

		public Vector3 velocity;

		public Vector3 currentPosition;

		public bool isActive;

		public bool isFalling;

		public long frameIndexWhenDeactivated;

		public float lastMoveTime = -2f;

		public long lastMoveFrameIndex = -2L;
	}
}
