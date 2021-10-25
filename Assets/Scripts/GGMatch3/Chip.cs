using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class Chip : SlotComponent
	{
		public override int sortingOrder
		{
			get
			{
				return 10;
			}
		}

		public override bool isPlaceBubbleSuspended
		{
			get
			{
				return this.chipType != ChipType.Chip && this.chipType != ChipType.Bomb;
			}
		}

		public override bool isPreventingReplaceByOtherChips
		{
			get
			{
				return this.chipType > ChipType.Chip;
			}
		}

		public void DestroyGrowingElement()
		{
			if (!this.hasGrowingElement)
			{
				return;
			}
			if (this.growingElementGraphics != null)
			{
				UnityEngine.Object.Destroy(this.growingElementGraphics.gameObject);
				this.growingElementGraphics = null;
			}
			this.hasGrowingElement = false;
		}

		public void AttachGrowingElement(TransformBehaviour growingElementGraphics)
		{
			this.hasGrowingElement = true;
			this.growingElementGraphics = growingElementGraphics;
			if (growingElementGraphics == null)
			{
				return;
			}
			base.GetComponentBehaviour<TransformBehaviour>().AddChild(growingElementGraphics);
			growingElementGraphics.localScale = Vector3.one;
			growingElementGraphics.localPosition = Vector3.zero;
		}

		public int moreMovesCount
		{
			get
			{
				if (this.chipType != ChipType.MoreMovesChip)
				{
					return 0;
				}
				return this.itemLevel + 1;
			}
		}

		public override void AddToGoalsAtStart(Match3Goals goals)
		{
			Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(this);
			goals.GetChipTypeCounter(chipTypeDef).countAtStart++;
		}

		public void Init(ChipType chipType, ItemColor itemColor)
		{
			this.chipType = chipType;
			this.itemColor = itemColor;
			if (this.isPickupElement || this.isFallingPickupElement)
			{
				this.jumpBehaviour = new ChipJumpBehaviour();
				this.jumpBehaviour.Init(Match3Settings.instance.chipJumpSettings);
			}
			if (this.isPowerup)
			{
				this.jumpBehaviour = new ChipJumpBehaviour();
				if (chipType == ChipType.HorizontalRocket)
				{
					this.jumpBehaviour.Init(Match3Settings.instance.horizontalRocketJumpSettings);
				}
				if (chipType == ChipType.VerticalRocket)
				{
					this.jumpBehaviour.Init(Match3Settings.instance.verticalRocketJumpSettings);
				}
				if (chipType == ChipType.Bomb)
				{
					this.jumpBehaviour.Init(Match3Settings.instance.bombJumpSettings);
				}
				if (chipType == ChipType.DiscoBall)
				{
					this.jumpBehaviour.Init(Match3Settings.instance.discoBallJumpSettings);
				}
				if (chipType == ChipType.SeekingMissle)
				{
					this.jumpBehaviour.Init(Match3Settings.instance.seekingMissleJumpSettings);
				}
			}
		}

		public bool canBeTappedToActivate
		{
			get
			{
				return this.chipType == ChipType.HorizontalRocket || this.chipType == ChipType.VerticalRocket || this.chipType == ChipType.Bomb || this.chipType == ChipType.SeekingMissle;
			}
		}

		public bool isRocket
		{
			get
			{
				return this.chipType == ChipType.HorizontalRocket || this.chipType == ChipType.VerticalRocket;
			}
		}

		public bool isPowerup
		{
			get
			{
				return this.chipType == ChipType.HorizontalRocket || this.chipType == ChipType.VerticalRocket || this.chipType == ChipType.Bomb || this.chipType == ChipType.SeekingMissle || this.chipType == ChipType.DiscoBall;
			}
		}

		public override bool isAttachGrowingElementSuspended
		{
			get
			{
				return this.isPreventingReplaceByOtherChips || this.hasGrowingElement || !this.canFormColorMatches;
			}
		}

		public bool isPickupElement
		{
			get
			{
				return this.chipType == ChipType.BunnyChip || this.chipType == ChipType.CookiePickup || this.chipType == ChipType.MoreMovesChip;
			}
		}

		public bool isStoppingRocket
		{
			get
			{
				return this.chipType == ChipType.CookiePickup;
			}
		}

		public bool isFallingPickupElement
		{
			get
			{
				return this.chipType == ChipType.FallingGingerbreadMan;
			}
		}

		public bool canFormColorMatches
		{
			get
			{
				return this.chipType == ChipType.Chip || this.chipType == ChipType.MonsterChip;
			}
		}

		public void SetTransformToMove(Transform t)
		{
		}

		public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return (this.hasGrowingElement && chipTypeDef.IsEqual(new Match3Goals.ChipTypeDef
			{
				chipType = ChipType.GrowingElementPiece,
				itemColor = this.itemColor
			})) || chipTypeDef.IsEqual(Match3Goals.ChipTypeDef.Create(this));
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

		public override bool isDestroyedByMatchingNextTo
		{
			get
			{
				return this.isPickupElement;
			}
		}

		public override bool isSlotGravitySuspended
		{
			get
			{
				return this.physics.isActive || this.teleportAnimation.isActive;
			}
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}

		public bool canBeDestroyed
		{
			get
			{
				if (this.isFallingPickupElement)
				{
					return false;
				}
				for (int i = 0; i < this.slotComponentLocks.Count; i++)
				{
					if (this.slotComponentLocks[i].isDestroySuspended)
					{
						return false;
					}
				}
				return true;
			}
		}

		public override bool canReactWithBomb
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

		public override void OnMovedBySlotGravity(Slot fromSlot, Slot toSlot, MoveContentsToSlotParams moveParams)
		{
			base.OnMovedBySlotGravity(fromSlot, toSlot, moveParams);
			long currentFrameIndex = fromSlot.game.board.currentFrameIndex;
			Match3Board board = fromSlot.game.board;
			bool flag = true;
			float num = 0f;
			if (currentFrameIndex - this.teleportAnimation.lastMoveFrame <= 1L)
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

		public override void Wobble(WobbleAnimation.Settings settings)
		{
			if (settings == null)
			{
				return;
			}
			this.wobbleAnimation.Init(settings, base.GetComponentBehaviour<TransformBehaviour>());
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

		public override void OnUpdate(float deltaTime)
		{
			if (this.slot == null)
			{
				return;
			}
			this.UpdateFeatherShow();
			if (this.jumpBehaviour != null)
			{
				this.jumpBehaviour.Update(this, deltaTime);
			}
			TransformBehaviour componentBehaviour = base.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour != null)
			{
				componentBehaviour.SetPartOfGoalActive(this.isPartOfActiveGoal);
			}
			this.wobbleAnimation.Update(deltaTime);
			TransformBehaviour componentBehaviour2 = base.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour2 != null)
			{
				componentBehaviour2.slotOffsetPosition = this.slot.offsetPosition;
				Vector3 vector = this.slot.offsetScale;
				GeneralSettings generalSettings = Match3Settings.instance.generalSettings;
				float num;
				if (this.isPowerup)
				{
					num = generalSettings.bombScaleMult;
				}
				else if (this.isPickupElement)
				{
					num = generalSettings.pickupScaleMult;
				}
				else
				{
					num = generalSettings.chipScaleMult;
				}
				vector = Vector3.Scale(vector, new Vector3(num, num, 1f));
				if (this.slot.isSlotGravitySuspendedByComponentOtherThan(this))
				{
					vector = Vector3.Scale(vector, new Vector3(generalSettings.trapScaleMult, generalSettings.trapScaleMult, 1f));
				}
				componentBehaviour2.slotLocalScale = vector;
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
						goto IL_237;
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
					goto IL_237;
				}
				updateParams.udpateIteration += 1f;
				updateParams.deltaTime = updateResult.leftOverDeltaTime;
			}
			return;
			IL_237:
			this.slot.OnSlotComponentMadeATransformChange(this);
			if (this.isFallingPickupElement && !this.slot.isSlotGravitySuspended && this.slot.isExitForFallingChip)
			{
				this.DoDestroyFallingPickupElement();
			}
		}

		private MonsterElements.MonsterElementPieces pieceThatWillBeFedThisPiece
		{
			get
			{
				Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(this);
				if (this.lastConnectedSlot == null)
				{
					return null;
				}
				return this.lastConnectedSlot.game.board.monsterElements.GetPieceThatNeedsFeeding(chipTypeDef);
			}
		}

		public bool isPartOfActiveGoal
		{
			get
			{
				if (this.lastConnectedSlot == null)
				{
					return false;
				}
				Match3Game game = this.lastConnectedSlot.game;
				Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(this);
				return (!this.hasGrowingElement && this.pieceThatWillBeFedThisPiece != null) || game.goals.GetActiveGoal(chipTypeDef) != null;
			}
		}

		public bool isFeatherShown
		{
			get
			{
				return this._003CisFeatherShown_003Ek__BackingField;
			}
		}

		public void UpdateFeatherShow()
		{
		}

		public override void OnAddedToSlot(Slot slot)
		{
			base.OnAddedToSlot(slot);
			this.physics.chip = this;
			TransformBehaviour componentBehaviour = base.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour != null)
			{
				componentBehaviour.SetPartOfGoalActive(this.isPartOfActiveGoal);
			}
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (!this.canBeDestroyed)
			{
				return result;
			}
			if (!this.isDestroyedByMatchingNextTo)
			{
				return result;
			}
			if (this.isSlotMatchingSuspended)
			{
				return result;
			}
			if (destroyParams.isHitByBomb && !destroyParams.isBombAllowingNeighbourDestroy)
			{
				return result;
			}
			return this.RemoveLevelOnDestroyNeighbourSlotComponent(slotBeingDestroyed, destroyParams);
		}

		public void DestroyBomb(SlotDestroyParams destroyParams)
		{
			Slot lastConnectedSlot = this.lastConnectedSlot;
			Match3Game game = lastConnectedSlot.game;
			if (this.chipType == ChipType.DiscoBall)
			{
				DiscoBallDestroyAction.DiscoParams discoParams = new DiscoBallDestroyAction.DiscoParams();
				discoParams.replaceWithBombs = false;
				discoParams.InitWithItemColor(lastConnectedSlot, game, game.BestItemColorForDiscoBomb(false), discoParams.replaceWithBombs);
				discoParams.originBomb = this;
				DiscoBallDestroyInstantAction discoBallDestroyInstantAction = new DiscoBallDestroyInstantAction();
				discoBallDestroyInstantAction.Init(discoParams);
				game.board.actionManager.AddAction(discoBallDestroyInstantAction);
				return;
			}
			if (this.chipType == ChipType.SeekingMissle)
			{
				CollectPointsAction.OnChipDestroy(this, destroyParams);
				SeekingMissileAction seekingMissileAction = new SeekingMissileAction();
				seekingMissileAction.Init(new SeekingMissileAction.Parameters
				{
					game = lastConnectedSlot.game,
					startSlot = lastConnectedSlot,
					doCrossExplosion = true,
					isHavingCarpet = destroyParams.isHavingCarpet
				});
				lastConnectedSlot.game.board.actionManager.AddAction(seekingMissileAction);
				this.RemoveFromGame();
				return;
			}
			if (this.chipType == ChipType.Bomb)
			{
				CollectPointsAction.OnChipDestroy(this, destroyParams);
				ExplosionAction explosionAction = new ExplosionAction();
				ExplosionAction.ExplosionSettings settings = default(ExplosionAction.ExplosionSettings);
				settings.position = lastConnectedSlot.position;
				settings.radius = 3;
				settings.bombChip = this;
				settings.isUsingBombAreaOfEffect = true;
				settings.isHavingCarpet = destroyParams.isHavingCarpet;
				explosionAction.Init(lastConnectedSlot.game, settings);
				lastConnectedSlot.game.board.actionManager.AddAction(explosionAction);
				return;
			}
			if (this.chipType == ChipType.HorizontalRocket || this.chipType == ChipType.VerticalRocket)
			{
				CollectPointsAction.OnChipDestroy(this, destroyParams);
				FlyLineRocketAction flyLineRocketAction = new FlyLineRocketAction();
				flyLineRocketAction.Init(new FlyLineRocketAction.Params
				{
					game = lastConnectedSlot.game,
					bombChip = this,
					position = lastConnectedSlot.position,
					prelock = true,
					isHavingCarpet = destroyParams.isHavingCarpet,
					canUseScale = (destroyParams.isFromSwap || destroyParams.isFromTap),
					rocketType = this.chipType,
					swapParams = destroyParams.swapParams
				});
				lastConnectedSlot.game.board.actionManager.AddAction(flyLineRocketAction);
			}
		}

		public void RemoveFromGameWithPickupGoal(SlotDestroyParams destroyParams)
		{
			if (this.lastConnectedSlot == null)
			{
				base.RemoveFromGame();
				return;
			}
			Match3Game game = this.lastConnectedSlot.game;
			base.RemoveFromSlot();
			Match3Goals.GoalBase goalBase = null;
			if (this.hasGrowingElement)
			{
				Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
				chipTypeDef.chipType = ChipType.GrowingElementPiece;
				chipTypeDef.itemColor = this.itemColor;
				goalBase = game.goals.GetActiveGoal(chipTypeDef);
			}
			if (goalBase == null)
			{
				Match3Goals.ChipTypeDef chipTypeDef2 = Match3Goals.ChipTypeDef.Create(this);
				goalBase = game.goals.GetActiveGoal(chipTypeDef2);
			}
			if (goalBase != null)
			{
				CollectGoalAction collectGoalAction = new CollectGoalAction();
				collectGoalAction.Init(new CollectGoalAction.CollectGoalParams
				{
					chip = this,
					chipSlot = this.slot,
					game = game,
					goal = goalBase,
					isExplosion = false,
					destroyParams = destroyParams
				});
				game.board.actionManager.AddAction(collectGoalAction);
				return;
			}
			base.RemoveFromGame();
		}

		public override SlotDestroyResolution OnDestroySlotComponent(SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (!this.canBeDestroyed)
			{
				return result;
			}
			if (destroyParams.isHitByBomb && !this.canReactWithBomb)
			{
				return result;
			}
			Slot lastConnectedSlot = this.lastConnectedSlot;
			Match3Game game = lastConnectedSlot.game;
			if (this.isPickupElement)
			{
				return this.RemoveLevelOnDestroy(destroyParams);
			}
			bool isPowerup = this.isPowerup;
			if (lastConnectedSlot != null)
			{
				lastConnectedSlot.RemoveComponent(this);
			}
			if (destroyParams.isFromTap)
			{
				game.OnUserMadeMove();
			}
			Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(this);
			Match3Goals.GoalBase goalBase = game.goals.GetActiveGoal(chipTypeDef);
			Match3Goals.GoalBase goalBase2 = null;
			if (this.hasGrowingElement)
			{
				Match3Goals.ChipTypeDef chipTypeDef2 = default(Match3Goals.ChipTypeDef);
				chipTypeDef2.chipType = ChipType.GrowingElementPiece;
				chipTypeDef2.itemColor = this.itemColor;
				goalBase2 = game.goals.GetActiveGoal(chipTypeDef2);
			}
			if (goalBase == null)
			{
				goalBase = goalBase2;
			}
			bool flag = goalBase != null;
			MonsterElements.MonsterElementPieces pieceThatWillBeFedThisPiece = this.pieceThatWillBeFedThisPiece;
			if (!isPowerup)
			{
				CollectPointsAction.OnChipDestroy(this, destroyParams);
			}
			if (this.carriesCoins > 0)
			{
				float num = 0.15f;
				for (int i = 0; i < this.carriesCoins; i++)
				{
					CollectCoinAction.InitArguments initArguments = default(CollectCoinAction.InitArguments);
					initArguments.game = game;
					initArguments.chipSlot = lastConnectedSlot;
					initArguments.delay = num * (float)i;
					CollectCoinAction collectCoinAction = new CollectCoinAction();
					collectCoinAction.Init(initArguments);
					game.board.actionManager.AddAction(collectCoinAction);
				}
			}
			if (!this.hasGrowingElement && pieceThatWillBeFedThisPiece != null)
			{
				CollectGoalAction collectGoalAction = new CollectGoalAction();
				collectGoalAction.Init(new CollectGoalAction.CollectGoalParams
				{
					chip = this,
					chipSlot = lastConnectedSlot,
					game = game,
					monsterToFeed = pieceThatWillBeFedThisPiece,
					isExplosion = destroyParams.isExplosion,
					explosionCentre = destroyParams.explosionCentre,
					destroyParams = destroyParams
				});
				game.board.actionManager.AddAction(collectGoalAction);
			}
			else if (isPowerup)
			{
				if (destroyParams.isHitByBomb && destroyParams.bombType == this.chipType && this.isRocket)
				{
					if (this.chipType == ChipType.HorizontalRocket)
					{
						this.chipType = ChipType.VerticalRocket;
					}
					else if (this.chipType == ChipType.VerticalRocket)
					{
						this.chipType = ChipType.HorizontalRocket;
					}
					SolidPieceRenderer componentBehaviour = base.GetComponentBehaviour<SolidPieceRenderer>();
					if (componentBehaviour != null)
					{
						componentBehaviour.Init(this.chipType);
					}
				}
				if (destroyParams.activationDelay > 0f)
				{
					DestroyAfterAction.InitArguments initArguments2 = default(DestroyAfterAction.InitArguments);
					initArguments2.chip = this;
					initArguments2.slot = lastConnectedSlot;
					initArguments2.delay = destroyParams.activationDelay;
					initArguments2.game = game;
					initArguments2.destroyParams = destroyParams;
					DestroyAfterAction destroyAfterAction = new DestroyAfterAction();
					destroyAfterAction.Init(initArguments2);
					game.board.actionManager.AddAction(destroyAfterAction);
				}
				else if (destroyParams.isHitByBomb && destroyParams.bombType != ChipType.DiscoBall && Match3Settings.instance.generalSettings.waitIfRocketHitsPowerup)
				{
					DestroyAfterAction.InitArguments initArguments3 = default(DestroyAfterAction.InitArguments);
					initArguments3.chip = this;
					initArguments3.slot = lastConnectedSlot;
					initArguments3.game = game;
					initArguments3.destroyParams = destroyParams;
					DestroyAfterAction destroyAfterAction2 = new DestroyAfterAction();
					destroyAfterAction2.Init(initArguments3);
					game.board.actionManager.AddAction(destroyAfterAction2);
				}
				else
				{
					this.DestroyBomb(destroyParams);
				}
			}
			else if (this.chipType == ChipType.DiscoBall)
			{
				DiscoBallDestroyAction.DiscoParams discoParams = new DiscoBallDestroyAction.DiscoParams();
				discoParams.replaceWithBombs = false;
				discoParams.InitWithItemColor(lastConnectedSlot, game, game.BestItemColorForDiscoBomb(false), discoParams.replaceWithBombs);
				discoParams.originBomb = this;
				DiscoBallDestroyInstantAction discoBallDestroyInstantAction = new DiscoBallDestroyInstantAction();
				discoBallDestroyInstantAction.Init(discoParams);
				game.board.actionManager.AddAction(discoBallDestroyInstantAction);
			}
			else if (this.chipType == ChipType.SeekingMissle)
			{
				SeekingMissileAction seekingMissileAction = new SeekingMissileAction();
				seekingMissileAction.Init(new SeekingMissileAction.Parameters
				{
					game = lastConnectedSlot.game,
					startSlot = lastConnectedSlot,
					doCrossExplosion = true,
					isHavingCarpet = destroyParams.isHavingCarpet
				});
				lastConnectedSlot.game.board.actionManager.AddAction(seekingMissileAction);
				this.RemoveFromGame();
			}
			else if (this.chipType == ChipType.Bomb)
			{
				ExplosionAction explosionAction = new ExplosionAction();
				ExplosionAction.ExplosionSettings settings = default(ExplosionAction.ExplosionSettings);
				settings.position = lastConnectedSlot.position;
				settings.radius = 3;
				settings.bombChip = this;
				settings.isUsingBombAreaOfEffect = true;
				settings.isHavingCarpet = destroyParams.isHavingCarpet;
				explosionAction.Init(lastConnectedSlot.game, settings);
				lastConnectedSlot.game.board.actionManager.AddAction(explosionAction);
			}
			else if (this.chipType == ChipType.HorizontalRocket || this.chipType == ChipType.VerticalRocket)
			{
				FlyLineRocketAction flyLineRocketAction = new FlyLineRocketAction();
				FlyLineRocketAction.Params flyParams = default(FlyLineRocketAction.Params);
				flyParams.game = lastConnectedSlot.game;
				flyParams.bombChip = this;
				flyParams.position = lastConnectedSlot.position;
				flyParams.prelock = true;
				flyParams.isHavingCarpet = destroyParams.isHavingCarpet;
				flyParams.canUseScale = (destroyParams.isFromSwap || destroyParams.isFromTap);
				flyParams.rocketType = this.chipType;
				flyParams.swapParams = destroyParams.swapParams;
				if (destroyParams.isHitByBomb && destroyParams.bombType == this.chipType)
				{
					if (this.chipType == ChipType.HorizontalRocket)
					{
						flyParams.rocketType = ChipType.VerticalRocket;
					}
					else
					{
						flyParams.rocketType = ChipType.HorizontalRocket;
					}
				}
				flyLineRocketAction.Init(flyParams);
				lastConnectedSlot.game.board.actionManager.AddAction(flyLineRocketAction);
			}
			else if (flag)
			{
				if (destroyParams.isCreatingPowerupFromThisMatch)
				{
					destroyParams.AddChipForPowerupCreateAnimation(this);
				}
				else if (this.chipType == ChipType.Chip && this.isFeatherShown)
				{
					DestroyChipAction destroyChipAction = new DestroyChipAction();
					destroyChipAction.Init(new DestroyChipAction.InitArguments
					{
						chip = this,
						slot = lastConnectedSlot,
						destroyParams = destroyParams
					});
					lastConnectedSlot.game.board.actionManager.AddAction(destroyChipAction);
					CollectGoalAction collectGoalAction2 = new CollectGoalAction();
					CollectGoalAction.CollectGoalParams collectGoalParams = new CollectGoalAction.CollectGoalParams
					{
						chip = null,
						moveTransform = game.CreateChipFeather(lastConnectedSlot, this.itemColor),
						chipSlot = lastConnectedSlot,
						smallScale = true,
						game = game,
						goal = goalBase,
						isExplosion = destroyParams.isExplosion
					};
					if (!collectGoalParams.isExplosion)
					{
						collectGoalParams.explosionCentre = lastConnectedSlot.position + IntVector2.down;
					}
					collectGoalParams.destroyParams = destroyParams;
					collectGoalAction2.Init(collectGoalParams);
					game.board.actionManager.AddAction(collectGoalAction2);
				}
				else
				{
					CollectGoalAction collectGoalAction3 = new CollectGoalAction();
					collectGoalAction3.Init(new CollectGoalAction.CollectGoalParams
					{
						chip = this,
						chipSlot = lastConnectedSlot,
						game = game,
						goal = goalBase,
						isExplosion = destroyParams.isExplosion,
						explosionCentre = destroyParams.explosionCentre,
						destroyParams = destroyParams
					});
					game.board.actionManager.AddAction(collectGoalAction3);
				}
			}
			else if (destroyParams.isCreatingPowerupFromThisMatch)
			{
				destroyParams.AddChipForPowerupCreateAnimation(this);
			}
			else if (destroyParams.isHitByBomb && destroyParams.isExplosion)
			{
				DestroyChipActionExplosion destroyChipActionExplosion = new DestroyChipActionExplosion();
				destroyChipActionExplosion.Init(this, lastConnectedSlot, destroyParams.explosionCentre, destroyParams);
				lastConnectedSlot.game.board.actionManager.AddAction(destroyChipActionExplosion);
			}
			else if (destroyParams.isFromSwap || destroyParams.isHitByBomb)
			{
				DestroyChipAction destroyChipAction2 = new DestroyChipAction();
				destroyChipAction2.Init(new DestroyChipAction.InitArguments
				{
					chip = this,
					slot = lastConnectedSlot,
					destroyParams = destroyParams
				});
				lastConnectedSlot.game.board.actionManager.AddAction(destroyChipAction2);
			}
			else
			{
				DestroyChipAction destroyChipAction3 = new DestroyChipAction();
				destroyChipAction3.Init(new DestroyChipAction.InitArguments
				{
					chip = this,
					slot = lastConnectedSlot,
					destroyParams = destroyParams
				});
				lastConnectedSlot.game.board.actionManager.AddAction(destroyChipAction3);
			}
			if (isPowerup && (destroyParams.isFromTap || destroyParams.isFromSwap))
			{
				game.board.AddMatch();
			}
			return result;
		}

		private void DoDestroyFallingPickupElement()
		{
			Slot slot = this.slot;
			if (slot == null)
			{
				return;
			}
			Match3Game game = slot.game;
			base.RemoveFromSlot();
			game.extraFallingChips.OnFallingElementPickup(this);
			Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(this);
			Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
			CollectGoalAction collectGoalAction = new CollectGoalAction();
			collectGoalAction.Init(new CollectGoalAction.CollectGoalParams
			{
				chip = this,
				chipSlot = slot,
				game = game,
				goal = activeGoal,
				isExplosion = false
			});
			game.board.actionManager.AddAction(collectGoalAction);
			game.Play(GGSoundSystem.SFXType.GingerbreadManRescue);
		}

		private SlotDestroyResolution RemoveLevelOnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (this.isSlotMatchingSuspended || this.isSlotGravitySuspended)
			{
				return result;
			}
			result.isDestroyed = true;
			this.DoDestroyLevel(slotBeingDestroyed, destroyParams);
			return result;
		}

		private SlotDestroyResolution RemoveLevelOnDestroy(SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			result.isDestroyed = true;
			result.stopPropagation = true;
			if (this.isStoppingRocket)
			{
				destroyParams.isRocketStopped = true;
			}
			this.DoDestroyLevel(this.lastConnectedSlot, destroyParams);
			return result;
		}

		private void PickupMoreMovesChip(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			Slot lastConnectedSlot = this.lastConnectedSlot;
			if (lastConnectedSlot != null)
			{
				lastConnectedSlot.RemoveComponent(this);
			}
			Match3Game game = lastConnectedSlot.game;
			CollectGoalAction collectGoalAction = new CollectGoalAction();
			CollectGoalAction.CollectGoalParams collectGoalParams = new CollectGoalAction.CollectGoalParams
			{
				chip = this,
				chipSlot = lastConnectedSlot,
				game = game,
				goal = null,
				collectMoreMovesCount = this.moreMovesCount,
				isExplosion = destroyParams.isExplosion
			};
			if (collectGoalParams.isExplosion)
			{
				collectGoalParams.explosionCentre = destroyParams.explosionCentre;
			}
			else if (destroyParams.matchIsland != null && destroyParams.matchIsland.allSlots.Count > 0)
			{
				collectGoalParams.explosionCentre = destroyParams.matchIsland.allSlots[0].position;
			}
			collectGoalParams.destroyParams = destroyParams;
			collectGoalAction.Init(collectGoalParams);
			game.board.actionManager.AddAction(collectGoalAction);
		}

		private void DoDestroyLevel(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			if (this.chipType == ChipType.MoreMovesChip)
			{
				this.PickupMoreMovesChip(slotBeingDestroyed, destroyParams);
				return;
			}
			this.itemLevel--;
			MultiLayerItemBehaviour componentBehaviour = base.GetComponentBehaviour<MultiLayerItemBehaviour>();
			if (componentBehaviour != null)
			{
				componentBehaviour.SetLayerIndex(this.itemLevel);
			}
			if (this.itemLevel >= 0)
			{
				return;
			}
			Slot lastConnectedSlot = this.lastConnectedSlot;
			if (lastConnectedSlot != null)
			{
				lastConnectedSlot.RemoveComponent(this);
			}
			Match3Game game = lastConnectedSlot.game;
			Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(this);
			Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
			if (activeGoal != null)
			{
				CollectGoalAction collectGoalAction = new CollectGoalAction();
				CollectGoalAction.CollectGoalParams collectGoalParams = new CollectGoalAction.CollectGoalParams
				{
					chip = this,
					chipSlot = lastConnectedSlot,
					game = game,
					goal = activeGoal,
					isExplosion = destroyParams.isExplosion
				};
				if (collectGoalParams.isExplosion)
				{
					collectGoalParams.explosionCentre = destroyParams.explosionCentre;
				}
				else if (destroyParams.matchIsland != null && destroyParams.matchIsland.allSlots.Count > 0)
				{
					collectGoalParams.explosionCentre = destroyParams.matchIsland.allSlots[0].position;
				}
				collectGoalParams.destroyParams = destroyParams;
				collectGoalAction.Init(collectGoalParams);
				game.board.actionManager.AddAction(collectGoalAction);
				return;
			}
			DestroyFromGravityAction destroyFromGravityAction = new DestroyFromGravityAction();
			destroyFromGravityAction.Init(this, lastConnectedSlot);
			lastConnectedSlot.game.board.actionManager.AddAction(destroyFromGravityAction);
		}

		public int chipTag;

		public bool hasGrowingElement;

		private TransformBehaviour growingElementGraphics;

		private ChipPhysics physics = new ChipPhysics();

		private TeleporterAnimation teleportAnimation = new TeleporterAnimation();

		public ChipType chipType;

		public ItemColor itemColor;

		public int itemLevel;

		public ChipJumpBehaviour jumpBehaviour;

		private WobbleAnimation wobbleAnimation = new WobbleAnimation();

		public int carriesCoins;

		private bool _003CisFeatherShown_003Ek__BackingField;
	}
}
