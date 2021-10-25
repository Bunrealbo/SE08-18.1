using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class TeleporterAnimation
	{
		public bool isActive
		{
			get
			{
				return this.activeMoveIndex < this.moves.Count;
			}
		}

		public void StartMove(TeleporterAnimation.MoveParams mp)
		{
			this.mp = mp;
			Match3Game game = mp.game;
			this.lastMoveFrame = mp.currentFrameIndex;
			this.lastMoveTime = mp.currentTime;
			this.moves.Clear();
			TeleporterAnimation.LinearMove item = default(TeleporterAnimation.LinearMove);
			item.startPosition = game.LocalPositionOfCenter(mp.positionToMoveFrom);
			int b = game.SlotsDistanceToEndOfBoard(mp.positionToMoveFrom, mp.directionToMoveFrom) + 2;
			b = 1;
			item.endPosition = game.LocalPositionOfCenter(mp.positionToMoveFrom + mp.directionToMoveFrom * b);
			item.pipe = mp.entrancePipe;
			item.isEntrance = true;
			this.moves.Add(item);
			TeleporterAnimation.LinearMove item2 = default(TeleporterAnimation.LinearMove);
			int b2 = game.SlotsDistanceToEndOfBoard(mp.positionToMoveTo, -mp.directionToMoveTo) + 2;
			b2 = 1;
			item2.startPosition = game.LocalPositionOfCenter(mp.positionToMoveTo - mp.directionToMoveTo * b2);
			item2.endPosition = game.LocalPositionOfCenter(mp.positionToMoveTo);
			item2.resetVisuallyWhenStart = true;
			item2.pipe = mp.exitPipe;
			this.moves.Add(item2);
			this.currentSpeed = mp.initialSpeed;
			this.activeMoveIndex = 0;
		}

		private void SetPipeScale(PipeBehaviour pipe, float scale)
		{
			if (pipe == null)
			{
				return;
			}
			pipe.SetScale(scale);
		}

		public UpdateResult OnUpdate(UpdateParams updateParams)
		{
			float deltaTime = updateParams.deltaTime;
			UpdateResult result = default(UpdateResult);
			if (!this.isActive)
			{
				return result;
			}
			SlotComponent chip = this.mp.chip;
			Match3Game game = this.mp.game;
			if (this.activeMoveIndex >= this.moves.Count)
			{
				return result;
			}
			if (game == null)
			{
				return result;
			}
			this.lastMoveTime = game.board.currentTime;
			this.lastMoveFrame = game.board.currentFrameIndex;
			result.wasTraveling = true;
			TeleporterAnimation.LinearMove linearMove = this.moves[this.activeMoveIndex];
			PipeSettings pipeSettings = game.settings.pipeSettings;
			if (!linearMove.isStarted)
			{
				linearMove.isStarted = true;
				this.SetPipeScale(linearMove.pipe, Match3Settings.instance.pipeSettings.pipeScale);
				this.currentPosition = linearMove.startPosition;
				if (linearMove.resetVisuallyWhenStart)
				{
					ChipBehaviour componentBehaviour = chip.GetComponentBehaviour<ChipBehaviour>();
					if (componentBehaviour != null)
					{
						componentBehaviour.localPosition = this.currentPosition;
						componentBehaviour.ResetCloth();
					}
				}
				if (linearMove.pipe != null && linearMove.isEntrance)
				{
					game.particles.CreateParticlesWorld(linearMove.pipe.exitTransform.position, Match3Particles.PositionType.PipeEnterParticle, ChipType.Chip, ItemColor.Unknown);
				}
			}
			float gravity = pipeSettings.gravity;
			float minVelocity = pipeSettings.minVelocity;
			if (updateParams.udpateIteration == 0f)
			{
				this.currentSpeed += gravity * deltaTime;
			}
			this.currentSpeed = Mathf.Max(minVelocity, this.currentSpeed);
			this.currentSpeed = Mathf.Min(pipeSettings.maxVelocity, this.currentSpeed);
			Vector3 endPosition = linearMove.endPosition;
			float num = Mathf.Max(Mathf.Abs(this.currentPosition.x - endPosition.x), Mathf.Abs(this.currentPosition.y - endPosition.y));
			if (this.currentSpeed * deltaTime > num)
			{
				result.leftOverDeltaTime = deltaTime - num / this.currentSpeed;
			}
			this.currentPosition.x = Mathf.MoveTowards(this.currentPosition.x, endPosition.x, this.currentSpeed * deltaTime);
			this.currentPosition.y = Mathf.MoveTowards(this.currentPosition.y, endPosition.y, this.currentSpeed * deltaTime);
			this.currentPosition.z = endPosition.z;
			bool flag = this.currentPosition == endPosition;
			this.moves[this.activeMoveIndex] = linearMove;
			if (flag)
			{
				if (linearMove.pipe != null && !linearMove.isEntrance)
				{
					game.particles.CreateParticlesWorld(linearMove.pipe.exitTransform.position, Match3Particles.PositionType.PipeExitParticle, ChipType.Chip, ItemColor.Unknown);
				}
				this.SetPipeScale(linearMove.pipe, 1f);
				this.activeMoveIndex++;
			}
			float num2 = game.settings.pipeSettings.scale;
			float num3 = game.settings.pipeSettings.orthoScale;
			if (!this.isActive)
			{
				num2 = 1f;
				num3 = 1f;
			}
			TransformBehaviour componentBehaviour2 = chip.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour2 != null)
			{
				componentBehaviour2.localPosition = this.currentPosition;
				Vector3 ptr = linearMove.endPosition - linearMove.startPosition;
				Vector3 one = Vector3.one;
				if (Mathf.Abs(ptr.x) > 0f)
				{
					one.y = num2;
					one.x = num3;
				}
				else
				{
					one.x = num2;
					one.y = num3;
				}
				if (componentBehaviour2.scalerTransform != null)
				{
					componentBehaviour2.scalerTransform.localScale = one;
				}
			}
			return result;
		}

		public List<TeleporterAnimation.LinearMove> moves = new List<TeleporterAnimation.LinearMove>();

		private Vector3 currentPosition;

		public float currentSpeed;

		private int activeMoveIndex;

		public float lastMoveTime = -2f;

		public long lastMoveFrame = -2L;

		private TeleporterAnimation.MoveParams mp;

		public struct MoveParams
		{
			public Match3Game game;

			public SlotComponent chip;

			public float initialSpeed;

			public PipeBehaviour entrancePipe;

			public IntVector2 positionToMoveFrom;

			public IntVector2 directionToMoveFrom;

			public PipeBehaviour exitPipe;

			public IntVector2 positionToMoveTo;

			public IntVector2 directionToMoveTo;

			public long currentFrameIndex;

			public float currentTime;
		}

		public struct LinearMove
		{
			public Vector3 startPosition;

			public Vector3 endPosition;

			public bool isStarted;

			public bool resetVisuallyWhenStart;

			public PipeBehaviour pipe;

			public bool isEntrance;
		}
	}
}
