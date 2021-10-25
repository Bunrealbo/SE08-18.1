using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class PlayerInput : MonoBehaviour
	{
		public PlayerInput.Settings settings
		{
			get
			{
				return Match3Settings.instance.playerInputSettings;
			}
		}

		public bool isActive
		{
			get
			{
				return this.mouseParams.mouseUpEnum != null || (this.mouseParams.firstHitSlot != null && this.mouseParams.isMouseDown);
			}
		}

		private void SetFollowerLocalPosition(Vector3 localPosition)
		{
			if (this.follower == null)
			{
				return;
			}
			this.follower.transform.localPosition = localPosition;
		}

		public void UpdateDisplaceIntegration()
		{
			Slot[] slots = this.game.board.slots;
			PlayerInput.Settings settings = this.settings;
			foreach (Slot slot in slots)
			{
				if (slot != null)
				{
					slot.positionIntegrator.Update(Time.deltaTime, settings.dampingFactor, settings.stiffness);
					slot.offsetPosition = slot.positionIntegrator.currentPosition;
				}
			}
		}

		public void ResetDisplace()
		{
			foreach (Slot slot in this.game.board.slots)
			{
				if (slot != null)
				{
					slot.prevOffsetPosition = (slot.offsetPosition = Vector3.zero);
					slot.offsetScale = Vector3.one;
					slot.positionIntegrator.ResetAcceleration();
				}
			}
		}

		public void ApplyDisplace(Slot firstHitSlot, Vector3 startPos, Vector3 currentPosition, Vector3 velocity, Slot ignoreSlot_, bool isNoDrag)
		{
			PlayerInput.Settings settings = this.settings;
			if (settings.switchSlotPosition)
			{
				Slot firstHitSlot2 = this.mouseParams.firstHitSlot;
			}
			if (settings.switchSlotPosition)
			{
				Slot slotToSwitchWith = this.mouseParams.slotToSwitchWith;
			}
			Slot[] slots = this.game.board.slots;
			float maxDistance = settings.maxDistance;
			Vector3 normalized = velocity.normalized;
			foreach (Slot slot in slots)
			{
				if (slot != null)
				{
					if (slot.isSlotGravitySuspendedByComponent)
					{
						slot.prevOffsetPosition = (slot.offsetPosition = Vector3.zero);
						slot.offsetScale = Vector3.one;
						slot.positionIntegrator.ResetAcceleration();
					}
					else
					{
						Vector3 b = slot.localPositionOfCenter;
						Chip slotComponent = slot.GetSlotComponent<Chip>();
						if (slotComponent != null)
						{
							TransformBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
							if (componentBehaviour != null)
							{
								b = componentBehaviour.localPosition;
							}
						}
						Vector3 vector = currentPosition - b;
						vector.z = 0f;
						float num = Mathf.Abs(vector.x) + Mathf.Abs(vector.y);
						if (num >= maxDistance)
						{
							slot.prevOffsetPosition = (slot.offsetPosition = Vector3.zero);
							slot.offsetScale = Vector3.one;
							slot.positionIntegrator.ResetAcceleration();
						}
						else
						{
							float time = Mathf.InverseLerp(0f, maxDistance, num);
							float t = settings.displaceCurve.Evaluate(time);
							float num2 = Vector3.Dot(vector, normalized);
							float num3 = (num2 > 0f) ? settings.velocityDisplaceFwd : settings.velocityDisplaceBck;
							float d = Mathf.Lerp((num2 > 0f) ? settings.maxDisplaceFwd : settings.maxDisplaceBck, 0f, t);
							if (isNoDrag)
							{
								num3 *= settings.noDragFactor;
							}
							float num4 = Mathf.Lerp(num3, 0f, settings.velocityDisplaceCurve.Evaluate(time));
							num4 = Mathf.Sign(num4) * Mathf.Min(Mathf.Abs(num4), settings.maxOffsetVelocity);
							Vector3 b2 = normalized * num4;
							Vector3 a = vector.normalized * d;
							if (velocity != Vector3.zero)
							{
								a = Vector3.zero;
							}
							Vector3 b3 = a + b2;
							slot.offsetPosition = Vector3.Lerp(slot.prevOffsetPosition, b3, settings.displacePull * Time.deltaTime);
							Mathf.Abs(slot.offsetPosition.x);
							Mathf.Abs(slot.offsetPosition.y);
							slot.prevOffsetPosition = slot.offsetPosition;
							slot.positionIntegrator.SetPosition(slot.offsetPosition);
							slot.offsetScale = Vector3.Lerp(settings.displaceScale, Vector3.one, settings.scaleCurve.Evaluate(time));
							IntVector2 intVector = slot.position - firstHitSlot.position;
							if ((Mathf.Abs(normalized.x) <= 0f || intVector.x != 0) && (Mathf.Abs(normalized.y) <= 0f || intVector.y != 0))
							{
								slot.offsetScale = Vector3.one;
							}
							if (float.IsNaN(slot.offsetPosition.x) || float.IsNaN(slot.offsetPosition.y))
							{
								UnityEngine.Debug.LogError("NAN");
							}
						}
					}
				}
			}
		}

		public void SetCamera(Camera mainCamera)
		{
			this.mainCamera = mainCamera;
		}

		private InputHandler input
		{
			get
			{
				return this.game.gameScreen.inputHandler;
			}
		}

		public void Init(Match3Game game)
		{
			this.game = game;
			this.mainLock = this.lockContainer.NewLock();
			this.mainLock.isSlotGravitySuspended = true;
			this.mainLock.isAvailableForSeekingMissileSuspended = true;
			this.mainLock.isSlotMatchingSuspended = true;
			this.mainLock.isAvailableForDiscoBombSuspended = true;
			this.mainLock.isDestroySuspended = true;
			this.mainLock.isChipGeneratorSuspended = true;
			this.mainLock.isChipGravitySuspended = true;
			this.mainLock.isAttachGrowingElementSuspended = true;
			this.findMatches.Init(game.board);
		}

		public Slot GetSlotFromMousePosition(Vector3 mousePosition)
		{
			Vector3 localPosition = this.LocalMatch3Pos(mousePosition);
			return this.GetSlot(localPosition);
		}

		private Vector3 LocalMatch3Pos(Vector3 mousePosScreen)
		{
			Ray ray = this.mainCamera.ScreenPointToRay(mousePosScreen);
			float distance = Mathf.Abs(this.match3ParentTransform.position.z - this.mainCamera.transform.position.z);
			Vector3 point = ray.GetPoint(distance);
			point.z = this.match3ParentTransform.position.z;
			return this.match3ParentTransform.InverseTransformPoint(point);
		}

		private Vector3 LocalUIPos(Vector3 mousePosScreen)
		{
			Ray ray = this.mainCamera.ScreenPointToRay(mousePosScreen);
			float distance = Mathf.Abs(this.match3ParentTransform.position.z - this.mainCamera.transform.position.z);
			Vector3 point = ray.GetPoint(distance);
			point.z = this.match3ParentTransform.position.z;
			Vector3 result = this.game.gameScreen.transform.InverseTransformPoint(point);
			result.z = 0f;
			return result;
		}

		private Slot GetSlot(Vector3 localPosition)
		{
			IntVector2 position = this.game.ClosestBoardPositionFromLocalPosition(localPosition);
			return this.game.GetSlot(position);
		}

		private void UpdateDisplace()
		{
			PlayerInput.MouseParams mouseParams = this.mouseParamsDisplace;
			if (mouseParams.IsDown(this.input))
			{
				mouseParams.Clear();
				mouseParams.SetPointerIdToFirstDownPointer(this.input);
				Vector3 vector = this.LocalMatch3Pos(mouseParams.MousePosition(this.input));
				Slot slot = this.GetSlot(vector);
				mouseParams.isMouseDown = true;
				mouseParams.mouseDownPositon = vector;
				mouseParams.firstHitSlot = slot;
				if (slot == null || slot.isSlotSwapSuspended)
				{
					this.ResetDisplace();
					mouseParams.Clear();
				}
			}
			if (mouseParams.IsUp(this.input))
			{
				this.ResetDisplace();
				mouseParams.Clear();
			}
			if (this.settings.useMouse1ForBouncy && !this.mouseParams.isMouseDown)
			{
				this.UpdateDisplaceIntegration();
				return;
			}
			if (!mouseParams.isMouseDown)
			{
				this.UpdateDisplaceIntegration();
				return;
			}
			Vector3 vector2 = this.LocalMatch3Pos(mouseParams.MousePosition(this.input));
			if (mouseParams.firstHitSlot == null || mouseParams.firstHitSlot.isSlotSwapSuspended)
			{
				vector2 = mouseParams.mouseDownPositon;
			}
			Vector3 vector3 = vector2 - mouseParams.mouseDownPositon;
			vector2 = mouseParams.mouseDownPositon + vector3.normalized * Mathf.Min(vector3.magnitude, this.settings.maxDistanceCurrentPos);
			float num = vector2.x - mouseParams.mouseDownPositon.x;
			float num2 = vector2.y - mouseParams.mouseDownPositon.y;
			Vector3 velocity = Vector3.zero;
			if (Mathf.Abs(num) > Mathf.Abs(num2))
			{
				velocity = Vector3.right * Mathf.Sign(num);
			}
			else
			{
				velocity = Vector3.up * Mathf.Sign(num2);
			}
			if (num == 0f && num2 == 0f)
			{
				velocity = Vector3.zero;
			}
			float num3 = this.game.slotPhysicalSize.x * this.sizeToSwitch;
			bool isNoDrag = Mathf.Abs(num) < num3 && Mathf.Abs(num2) < num3;
			this.UpdateDisplaceIntegration();
			if (this.settings.inputLineMode)
			{
				return;
			}
			Slot ignoreSlot_ = null;
			if (this.settings.switchSlotPosition)
			{
				ignoreSlot_ = this.mouseParams.firstHitSlot;
			}
			if (this.settings.switchImmediateIfPossible && !this.mouseParams.isMouseDown)
			{
				return;
			}
			PlayerInput.AffectorBase activeAffector = this.mouseParams.activeAffector;
			if (activeAffector != null)
			{
				activeAffector.OnUpdate(new PlayerInput.AffectorUpdateParams
				{
					currentPosition = vector2,
					mouseParams = this.mouseParams,
					input = this
				});
				return;
			}
			this.ApplyDisplace(mouseParams.firstHitSlot, mouseParams.mouseDownPositon, vector2, velocity, ignoreSlot_, isNoDrag);
		}

		private void OnMouseUpStart()
		{
			PlayerInput.AffectorBase activeAffector = this.mouseParams.activeAffector;
			if (activeAffector == null)
			{
				this.OnMouseUp();
				return;
			}
			if (!activeAffector.canFinish)
			{
				this.mouseParams.mouseUpEnum = this.DoWaitForActiveAffector(activeAffector);
				return;
			}
			this.OnMouseUp();
		}

		private IEnumerator DoWaitForActiveAffector(PlayerInput.AffectorBase activeAffector)
		{
			return new PlayerInput._003CDoWaitForActiveAffector_003Ed__42(0)
			{
				_003C_003E4__this = this,
				activeAffector = activeAffector
			};
		}

		private void OnMouseUp()
		{
			PlayerInput.AffectorBase activeAffector = this.mouseParams.activeAffector;
			bool isSlotSwitched = this.mouseParams.isSlotSwitched;
			if (this.mouseParams.firstHitSlot != null)
			{
				this.mouseParams.firstHitSlot.light.AddLight(this.exchangeLight.currentIntensity);
			}
			if (this.mouseParams.slotToSwitchWith != null)
			{
				this.mouseParams.slotToSwitchWith.light.AddLight(this.exchangeLight.currentIntensity);
			}
			for (int i = 0; i < this.mouseParams.affectedSlotsForMatch.Count; i++)
			{
				this.mouseParams.affectedSlotsForMatch[i].light.AddLight(this.matchLight.currentIntensity);
			}
			Match3Game.SwitchSlotsArguments switchSlotsArguments = default(Match3Game.SwitchSlotsArguments);
			switchSlotsArguments.isAlreadySwitched = isSlotSwitched;
			if (activeAffector != null)
			{
				activeAffector.AddToSwitchSlotArguments(ref switchSlotsArguments);
			}
			this.lockContainer.UnlockAll();
			this.mainLock.UnlockAll();
			this.mouseParams.Reset(this.mainLock, this);
			if (this.mouseParams.firstHitSlot != null && this.mouseParams.slotToSwitchWith == null && !this.mouseParams.firstHitSlot.isSlotTouchingSuspended)
			{
				SwapParams swapParams = new SwapParams();
				swapParams.startPosition = (swapParams.swipeToPosition = this.mouseParams.firstHitSlot.position);
				swapParams.affectorExport = switchSlotsArguments.affectorExport;
				this.game.TapOnSlot(this.mouseParams.firstHitSlot.position, swapParams);
				switchSlotsArguments.Clear();
				this.mouseParams.Clear();
				return;
			}
			Slot firstHitSlot = this.mouseParams.firstHitSlot;
			Slot slotToSwitchWith = this.mouseParams.slotToSwitchWith;
			bool flag = false;
			if (firstHitSlot != null && slotToSwitchWith != null)
			{
				switchSlotsArguments.pos1 = firstHitSlot.position;
				switchSlotsArguments.pos2 = slotToSwitchWith.position;
				switchSlotsArguments.instant = true;
				flag = this.game.TrySwitchSlots(switchSlotsArguments);
				if (!flag)
				{
					DiscoBallAffector.RemoveFromGame(switchSlotsArguments.bolts);
				}
				else if (activeAffector != null)
				{
					activeAffector.OnBeforeDestroy();
				}
			}
			else
			{
				DiscoBallAffector.RemoveFromGame(switchSlotsArguments.bolts);
			}
			this.mouseParams.Clear();
			if (flag)
			{
				switchSlotsArguments.affectorExport.ExecuteOnAfterDestroy();
			}
			switchSlotsArguments.Clear();
		}

		private void TryActivateTap(PlayerInput.MouseParams mouseParams)
		{
			Slot firstHitSlot = mouseParams.firstHitSlot;
			if (firstHitSlot == null || mouseParams.slotToSwitchWith != null || mouseParams.state != PlayerInput.MouseParams.State.Touch)
			{
				return;
			}
			this.mainLock.UnlockAllAndSaveToTemporaryList();
			if (!firstHitSlot.canBeTappedToActivate)
			{
				this.mainLock.LockTemporaryListAndClear();
				return;
			}
			if (firstHitSlot.isSlotSwapSuspended)
			{
				this.mainLock.LockTemporaryListAndClear();
				return;
			}
			this.mainLock.LockTemporaryListAndClear();
			mouseParams.state = PlayerInput.MouseParams.State.TapActivated;
			this.mainLock.LockSlot(firstHitSlot);
			CompositeAffector.InitArguments initArguments = new CompositeAffector.InitArguments();
			initArguments.game = this.game;
			initArguments.AddSwipedSlot(mouseParams.firstHitSlot);
			this.compositeAffector.Init(initArguments);
			mouseParams.activeAffector = this.compositeAffector;
			this.game.Play(GGSoundSystem.SFXType.ChipTap);
		}

		private void UpdateFollower()
		{
			PlayerInput.MouseParams mouseParams = this.followerMouseParams;
			if (mouseParams.IsDown(this.input))
			{
				mouseParams.SetPointerIdToFirstDownPointer(this.input);
				Vector3 followerLocalPosition = this.LocalMatch3Pos(mouseParams.MousePosition(this.input));
				if (this.follower != null)
				{
					this.follower.SetActive(false);
				}
				this.SetFollowerLocalPosition(followerLocalPosition);
				mouseParams.isMouseDown = true;
				if (this.follower != null)
				{
					this.follower.SetActive(true);
					this.follower.Clear();
				}
				return;
			}
			if (mouseParams.IsUp(this.input) && mouseParams.isMouseDown)
			{
				mouseParams.isMouseDown = false;
			}
			if (!mouseParams.isMouseDown)
			{
				return;
			}
			Vector3 followerLocalPosition2 = this.LocalMatch3Pos(mouseParams.MousePosition(this.input));
			this.SetFollowerLocalPosition(followerLocalPosition2);
		}

		private void UpdateSimple()
		{
			if (this.mouseParams.mouseUpEnum != null)
			{
				if (!this.mouseParams.mouseUpEnum.MoveNext())
				{
					this.mouseParams.mouseUpEnum = null;
				}
				return;
			}
			if (this.game.isUserInteractionSuspended)
			{
				this.lockContainer.UnlockAll();
				this.mouseParams.Reset(this.mainLock, this);
				this.mouseParams.Clear();
				return;
			}
			if (this.mouseParams.IsDown(this.input))
			{
				if (this.mouseParams.isMouseDown)
				{
					this.OnMouseUp();
					return;
				}
				this.mouseParams.Clear();
				this.mouseParams.SetPointerIdToFirstDownPointer(this.input);
				Vector3 vector = this.LocalMatch3Pos(this.mouseParams.MousePosition(this.input));
				this.DoTapParticleOnPosition(vector);
				Slot slot = this.GetSlot(vector);
				if (slot != null)
				{
					slot.Wobble(Match3Settings.instance.chipWobbleSettings);
				}
				if (slot == null || slot.isSlotSwapSuspended)
				{
					this.mouseParams.isMouseDown = true;
					this.mouseParams.firstHitSlot = null;
					this.mouseParams.state = PlayerInput.MouseParams.State.Touch;
					return;
				}
				if (this.settings.tapToDestroy && Application.isEditor && slot.isSomethingMoveableByGravityInSlot && !slot.isDestroySuspended && !slot.isSlotGravitySuspended)
				{
					SlotDestroyParams destroyParams = new SlotDestroyParams();
					Chip slotComponent = slot.GetSlotComponent<Chip>();
					slot.OnDestroySlot(destroyParams);
					return;
				}
				this.mouseParams.isMouseDown = true;
				this.mouseParams.firstHitSlot = slot;
				this.mouseParams.chip = slot.GetSlotComponent<Chip>();
				if (this.mouseParams.chip != null)
				{
					this.mouseParams.chip.GetComponentBehaviour<TransformBehaviour>().SetBrightness(this.chipBrightness);
				}
				this.mouseParams.mouseDownPositon = vector;
				this.mouseParams.mouseDownUIPosition = this.LocalUIPos(this.mouseParams.MousePosition(this.input));
				this.mouseParams.state = PlayerInput.MouseParams.State.Touch;
				this.mainLock.LockSlot(slot);
				this.TryActivateTap(this.mouseParams);
			}
			bool flag = this.mouseParams.IsUp(this.input) && this.mouseParams.isMouseDown;
			if (this.mouseParams.activeAffector != null && this.mouseParams.activeAffector.wantsToEnd && this.mouseParams.state == PlayerInput.MouseParams.State.SwapActivated)
			{
				flag = true;
				this.mouseParams.isMouseDown = false;
			}
			if (flag)
			{
				if (this.mouseParams.firstHitSlot == null)
				{
					this.mouseParams.Clear();
					return;
				}
				this.OnMouseUpStart();
				return;
			}
			else
			{
				if (!this.mouseParams.isMouseDown || this.mouseParams.firstHitSlot == null)
				{
					return;
				}
				if (this.mouseParams.state == PlayerInput.MouseParams.State.Touch)
				{
					this.TryActivateTap(this.mouseParams);
				}
				if (this.mouseParams.slotToSwitchWith != null)
				{
					return;
				}
				Vector3 vector2 = this.LocalMatch3Pos(this.mouseParams.MousePosition(this.input));
				float num = vector2.x - this.mouseParams.mouseDownPositon.x;
				float num2 = vector2.y - this.mouseParams.mouseDownPositon.y;
				float num3 = this.game.slotPhysicalSize.x * this.sizeToSwitch;
				if (this.useUISizeToSwitch)
				{
					Vector3 vector3 = this.LocalUIPos(this.mouseParams.MousePosition(this.input));
					num = vector3.x - this.mouseParams.mouseDownUIPosition.x;
					num2 = vector3.y - this.mouseParams.mouseDownUIPosition.y;
					num3 = this.uiSizeToSwitch;
				}
				if (Mathf.Abs(num) <= Mathf.Abs(num2) || Mathf.Abs(num) <= num3)
				{
					if (Mathf.Abs(num2) > Mathf.Abs(num) && Mathf.Abs(num2) > num3)
					{
						if (Mathf.Abs(Vector3.Dot(Vector3.up, new Vector3(num, num2).normalized)) < this.angleToSwitch)
						{
							this.Cancel();
							return;
						}
						IntVector2 b = new IntVector2(0, (int)Mathf.Sign(num2));
						IntVector2 intVector = this.mouseParams.firstHitSlot.position + b;
						Slot slot2 = this.game.GetSlot(intVector);
						this.TrySwitchSlotsSimple(slot2, intVector);
					}
					return;
				}
				if (Mathf.Abs(Vector3.Dot(Vector3.right, new Vector3(num, num2).normalized)) < this.angleToSwitch)
				{
					this.Cancel();
					return;
				}
				IntVector2 b2 = new IntVector2((int)Mathf.Sign(num), 0);
				IntVector2 intVector2 = this.mouseParams.firstHitSlot.position + b2;
				Slot slot3 = this.game.GetSlot(intVector2);
				this.TrySwitchSlotsSimple(slot3, intVector2);
				return;
			}
		}

		private void Cancel()
		{
			this.mouseParams.Reset(this.mainLock, this);
			this.ResetDisplace();
			this.mouseParams.Clear();
			this.lockContainer.UnlockAll();
			this.mouseParamsDisplace.Clear();
		}

		private void TrySwitchSlotsSimple(Slot slotToSwitchWith, IntVector2 otherSlotPosition)
		{
			this.mainLock.UnlockAllAndSaveToTemporaryList();
			PlayerInput.AffectorBase activeAffector = this.mouseParams.activeAffector;
			if (activeAffector != null)
			{
				activeAffector.ReleaseLocks();
			}
			if (this.TrySwitchSlotsSimpleInner(slotToSwitchWith))
			{
				this.DoSwipeParticlesBetweenSlots(this.mouseParams.firstHitSlot, slotToSwitchWith);
				return;
			}
			this.game.Play(GGSoundSystem.SFXType.ChipSwap);
			if (this.mouseParams.state == PlayerInput.MouseParams.State.SwapToNothingActivated)
			{
				Slot firstHitSlot = this.mouseParams.firstHitSlot;
				this.Cancel();
				if (firstHitSlot != null)
				{
					this.game.TrySwitchSlots(firstHitSlot.position, otherSlotPosition, false);
					this.DoSwipeParticlesBetweenSlots(firstHitSlot.position, otherSlotPosition);
					return;
				}
				this.game.TrySwitchSlots(firstHitSlot, slotToSwitchWith, false);
				return;
			}
			else
			{
				if (this.mouseParams.state == PlayerInput.MouseParams.State.CancelSwap)
				{
					Slot firstHitSlot2 = this.mouseParams.firstHitSlot;
					this.Cancel();
					if (firstHitSlot2 != null)
					{
						this.game.TryShowSwitchNotPossible(firstHitSlot2.position, otherSlotPosition);
						this.DoSwipeParticlesBetweenSlots(firstHitSlot2.position, otherSlotPosition);
					}
					return;
				}
				this.mainLock.LockTemporaryListAndClear();
				if (activeAffector != null)
				{
					activeAffector.ApplyLocks();
				}
				return;
			}
		}

		private bool TrySwitchSlotsSimpleInner(Slot slotToSwitchWith)
		{
			Slot firstHitSlot = this.mouseParams.firstHitSlot;
			bool flag = this.mouseParams.state == PlayerInput.MouseParams.State.TapActivated;
			if (slotToSwitchWith == this.mouseParams.slotToSwitchWith && this.mouseParams.slotToSwitchWith != null)
			{
				return false;
			}
			if (slotToSwitchWith == null)
			{
				if (!flag)
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.SwapToNothingActivated;
				}
				else
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.CancelSwap;
				}
				return false;
			}
			if (this.mouseParams.lastTestedSlotToSwitchWith == slotToSwitchWith)
			{
				return false;
			}
			this.mouseParams.lastTestedSlotToSwitchWith = slotToSwitchWith;
			if (slotToSwitchWith.isSlotSwapSuspended)
			{
				if (!flag)
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.SwapToNothingActivated;
				}
				else
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.CancelSwap;
				}
				return false;
			}
			if (firstHitSlot.isSlotSwipingSuspended)
			{
				if (!flag)
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.SwapToNothingActivated;
				}
				else
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.CancelSwap;
				}
				return false;
			}
			if (firstHitSlot.isSlotSwipingSuspendedForSlot(slotToSwitchWith) || firstHitSlot.isSwipeSuspendedTo(slotToSwitchWith))
			{
				if (!flag)
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.SwapToNothingActivated;
				}
				else
				{
					this.mouseParams.state = PlayerInput.MouseParams.State.CancelSwap;
				}
				return false;
			}
			if (firstHitSlot.GetSlotComponent<Chip>() == null)
			{
				return false;
			}
			this.mainLock.UnlockAll();
			Slot.SwitchChips(firstHitSlot, slotToSwitchWith, false);
			Matches matches = this.findMatches.FindAllMatches();
			Island island = matches.GetIsland(firstHitSlot.position);
			Island island2 = matches.GetIsland(slotToSwitchWith.position);
			if (island2 == island)
			{
				island2 = null;
			}
			bool flag2 = island != null || island2 != null;
			Slot.SwitchChips(firstHitSlot, slotToSwitchWith, false);
			PlayerInput.MixClass mixClass = this.mouseParams.mixClass;
			mixClass.Clear();
			mixClass.TryAdd(firstHitSlot.GetSlotComponent<Chip>());
			mixClass.TryAdd(slotToSwitchWith.GetSlotComponent<Chip>());
			this.mainLock.LockSlot(firstHitSlot);
			if (!flag2 && mixClass.chips.Count == 0)
			{
				this.mouseParams.state = PlayerInput.MouseParams.State.SwapToNothingActivated;
				return false;
			}
			Chip chip = null;
			Chip chip2 = null;
			bool flag3 = false;
			if (mixClass.chips.Count == 1 && mixClass.CountOfType(ChipType.DiscoBall) == 1)
			{
				chip = mixClass.chips[0];
				if (firstHitSlot.GetSlotComponent<Chip>() == chip)
				{
					chip2 = slotToSwitchWith.GetSlotComponent<Chip>();
				}
				else
				{
					chip2 = firstHitSlot.GetSlotComponent<Chip>();
				}
				if (chip2 == null)
				{
					return false;
				}
				if (!chip2.canFormColorMatches)
				{
					return false;
				}
				flag3 = true;
			}
			this.mouseParams.slotToSwitchWith = slotToSwitchWith;
			this.mainLock.LockSlot(slotToSwitchWith);
			this.mouseParams.state = PlayerInput.MouseParams.State.SwapActivated;
			CompositeAffector.InitArguments initArguments = new CompositeAffector.InitArguments();
			initArguments.game = this.game;
			if (mixClass.chips.Count == 2)
			{
				initArguments.AddSwipedSlot(firstHitSlot).SetMix(slotToSwitchWith);
			}
			else if (flag3)
			{
				this.mouseParams.isSlotSwitched = true;
				CompositeAffector.SwipedSlot swipedSlot = initArguments.AddSwipedSlot(chip.lastConnectedSlot);
				swipedSlot.isDiscoCombine = true;
				swipedSlot.mixSlot = chip2.lastConnectedSlot;
			}
			else
			{
				Slot.SwitchChips(firstHitSlot, slotToSwitchWith, true);
				this.mouseParams.isSlotSwitched = true;
				CompositeAffector.SwipedSlot swipedSlot2 = initArguments.AddSwipedSlot(firstHitSlot);
				swipedSlot2.SetMatch(island);
				swipedSlot2.cameFromPosition = slotToSwitchWith.position;
				swipedSlot2.SetOtherChipMatch(slotToSwitchWith, island2);
				CompositeAffector.SwipedSlot swipedSlot3 = initArguments.AddSwipedSlot(slotToSwitchWith);
				swipedSlot3.SetMatch(island2);
				swipedSlot3.cameFromPosition = firstHitSlot.position;
				swipedSlot3.SetOtherChipMatch(firstHitSlot, island);
			}
			this.compositeAffector.Init(initArguments);
			this.mouseParams.activeAffector = this.compositeAffector;
			this.game.Play(GGSoundSystem.SFXType.ChipSwap);
			return true;
		}

		private void DoTapParticleOnPosition(Vector3 localPosition)
		{
			this.game.particles.CreateParticles(localPosition, Match3Particles.PositionType.ChipTap, Quaternion.identity);
		}

		private void DoSwipeParticlesBetweenSlots(Slot firstHitSlot, Slot slotToSwitchWith)
		{
			Vector3 localPositionOfCenter = Vector3.Lerp(firstHitSlot.localPositionOfCenter, slotToSwitchWith.localPositionOfCenter, 0f);
			if (slotToSwitchWith.position.x != firstHitSlot.position.x)
			{
				Quaternion rotation = (slotToSwitchWith.position.x > firstHitSlot.position.x) ? Quaternion.identity : Quaternion.AngleAxis(180f, Vector3.forward);
				this.game.particles.CreateParticles(localPositionOfCenter, Match3Particles.PositionType.ChipSwipeHorizontal, rotation);
				return;
			}
			Quaternion rotation2 = (slotToSwitchWith.position.y > firstHitSlot.position.y) ? Quaternion.AngleAxis(90f, Vector3.forward) : Quaternion.AngleAxis(-90f, Vector3.forward);
			this.game.particles.CreateParticles(localPositionOfCenter, Match3Particles.PositionType.ChipSwipeHorizontal, rotation2);
		}

		private void DoSwipeParticlesBetweenSlots(IntVector2 pos1, IntVector2 pos2)
		{
			Vector3 a = this.game.LocalPositionOfCenter(pos1);
			Vector3 b = this.game.LocalPositionOfCenter(pos2);
			Vector3 localPositionOfCenter = Vector3.Lerp(a, b, 0f);
			if (pos1.x != pos2.x)
			{
				Quaternion rotation = (pos2.x > pos1.x) ? Quaternion.identity : Quaternion.AngleAxis(180f, Vector3.forward);
				this.game.particles.CreateParticles(localPositionOfCenter, Match3Particles.PositionType.ChipSwipeHorizontal, rotation);
				return;
			}
			Quaternion rotation2 = (pos2.y > pos1.y) ? Quaternion.AngleAxis(90f, Vector3.forward) : Quaternion.AngleAxis(-90f, Vector3.forward);
			this.game.particles.CreateParticles(localPositionOfCenter, Match3Particles.PositionType.ChipSwipeHorizontal, rotation2);
		}

		private void OnEnable()
		{
			if (this.follower != null)
			{
				this.follower.SetActive(false);
			}
		}

		public void DoUpdate(float deltaTime)
		{
			if (this.game == null)
			{
				return;
			}
			PlayerInput.Settings settings = this.settings;
			if (this.mouseParams.mouseUpEnum != null)
			{
				PlayerInput.AffectorBase activeAffector = this.mouseParams.activeAffector;
				if (activeAffector != null)
				{
					activeAffector.OnUpdate(new PlayerInput.AffectorUpdateParams
					{
						currentPosition = this.mouseParams.mouseDownPositon,
						mouseParams = this.mouseParams,
						input = this
					});
				}
			}
			else if (settings.enableBouncyMode || settings.inputLineMode)
			{
				this.UpdateDisplace();
			}
			else
			{
				this.UpdateDisplaceIntegration();
				if (this.mouseParams.isMouseDown)
				{
					Vector3 currentPosition = this.LocalMatch3Pos(this.mouseParams.MousePosition(this.input));
					PlayerInput.AffectorBase activeAffector2 = this.mouseParams.activeAffector;
					if (activeAffector2 != null)
					{
						activeAffector2.OnUpdate(new PlayerInput.AffectorUpdateParams
						{
							currentPosition = currentPosition,
							mouseParams = this.mouseParams,
							input = this
						});
					}
				}
			}
			this.UpdateSimple();
			this.UpdateFollower();
		}

		[NonSerialized]
		public FindMatches findMatches = new FindMatches();

		private CompositeAffector compositeAffector = new CompositeAffector();

		[SerializeField]
		private Camera mainCamera;

		[SerializeField]
		private PlayerInputFollower follower;

		[SerializeField]
		private Transform match3ParentTransform;

		[SerializeField]
		private float sizeToSwitch = 0.5f;

		[SerializeField]
		private bool useUISizeToSwitch;

		[SerializeField]
		private float uiSizeToSwitch = 4f;

		[SerializeField]
		private float chipBrightness = 2f;

		[SerializeField]
		private float angleToSwitch = 0.85f;

		private PlayerInput.MouseParams mouseParams = new PlayerInput.MouseParams();

		private PlayerInput.MouseParams followerMouseParams = new PlayerInput.MouseParams();

		private PlayerInput.MouseParams mouseParamsDisplace = new PlayerInput.MouseParams();

		[NonSerialized]
		public Match3Game game;

		private Lock mainLock;

		private LightSlotComponent.PermanentLight matchLight = new LightSlotComponent.PermanentLight();

		private LightSlotComponent.PermanentLight exchangeLight = new LightSlotComponent.PermanentLight();

		private LockContainer lockContainer = new LockContainer();

		private ItemColor lastColor;

		public struct AffectorUpdateParams
		{
			public Vector3 currentPosition;

			public PlayerInput.MouseParams mouseParams;

			public PlayerInput input;
		}

		public class AffectorBase
		{
			public virtual bool canFinish
			{
				get
				{
					return this.affectorDuration >= this.minAffectorDuration;
				}
			}

			public virtual bool wantsToEnd
			{
				get
				{
					return this._003CwantsToEnd_003Ek__BackingField;
				}
			}

			public virtual float minAffectorDuration
			{
				get
				{
					return this._003CminAffectorDuration_003Ek__BackingField;
				}
			}

			public virtual void ReleaseLocks()
			{
			}

			public virtual void ApplyLocks()
			{
			}

			public virtual void Clear()
			{
			}

			public virtual void OnUpdate(PlayerInput.AffectorUpdateParams updateParams)
			{
			}

			public virtual void OnBeforeDestroy()
			{
			}

			public virtual void AddToSwitchSlotArguments(ref Match3Game.SwitchSlotsArguments switchSlotsArguments)
			{
				switchSlotsArguments.affectorDuration = this.affectorDuration;
			}

			public float affectorDuration;

			private readonly bool _003CwantsToEnd_003Ek__BackingField;

			private readonly float _003CminAffectorDuration_003Ek__BackingField;
		}

		public class ExplosionAffector : PlayerInput.AffectorBase
		{
			[Serializable]
			public class Settings
			{
				public int radius = 2;

				public float maxDistance = 10f;

				public FloatRange displaceRange = new FloatRange(0f, 1f);

				public AnimationCurve displaceCurve;

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

				public float orthoScaleInfluence = 0.2f;
			}
		}

		public class MoveFromLineAffector : PlayerInput.AffectorBase
		{
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

		[Serializable]
		public class Settings
		{
			public bool tapToDestroy;

			public float maxDistance = 1f;

			public float maxDisplace = 1f;

			public float maxDistanceCurrentPos = 1f;

			public float maxDisplaceFwd = 1f;

			public float maxDisplaceBck = 1f;

			public float noDragFactor = 1f;

			public float maxAcceleration;

			public Vector3 displaceScale;

			public AnimationCurve displaceCurve;

			public AnimationCurve velocityDisplaceCurve;

			public AnimationCurve scaleCurve;

			public float maxOffsetVelocity = 0.2f;

			public float stiffness = 1f;

			public float dampingFactor = 1f;

			public float displacePull = 0.9f;

			public bool enableBouncyMode = true;

			public bool inputDirectMode;

			public bool switchImmediateIfPossible;

			public bool switchSlotPosition;

			public bool inputLineMode;

			public bool useMouse1ForBouncy;

			public float lightIntensityForMatch;

			public float lightIntensityForMatchOff;

			public bool addPowerupVis;

			public float addedLightIntensityForMatch;

			public float velocityDisplace;

			public float velocityDisplaceFwd;

			public float velocityDisplaceBck;

			public float centreMaxDistance;

			public float centreOffset;

			public float bckMaxDistanceX;

			public float bckMaxDistanceY;

			public FloatRange bckOffsetX;

			public FloatRange bckOffsetY;

			public AnimationCurve bckCurveX;

			public AnimationCurve bckCurveY;

			public float fwdMaxDistanceX;

			public float fwdMaxDistanceY;

			public FloatRange fwdOffsetX;

			public FloatRange fwdOffsetY;

			public float totalOffsetForFirstSlot = 1f;

			public float totalOffsetForFirstSlotMatching = 1f;

			public float directionInfluence;

			public float directionInfluenceFwd;

			public AnimationCurve pullCurve;

			public float directionInfluenceControl;

			public bool disableLightingInAffectors;

			public bool useSimpleLineBolts;

			public bool disableBombLighting;
		}

		public class MouseParams
		{
			public PlayerInput.AffectorBase activeAffector
			{
				get
				{
					return this.activeAffector_;
				}
				set
				{
					if (this.activeAffector_ == value)
					{
						return;
					}
					if (this.activeAffector_ != null)
					{
						this.activeAffector_.Clear();
					}
					this.activeAffector_ = value;
				}
			}

			public void ResetAffectedSlotsForMatch(PlayerInput input)
			{
				for (int i = 0; i < this.affectedSlotsForMatch.Count; i++)
				{
					this.affectedSlotsForMatch[i].light.RemoveLight(input.matchLight);
				}
				this.affectedSlotsForMatch.Clear();
				this.mixClass.Clear();
				this.affectedSlotsForMix.Clear();
				this.activeAffector = null;
			}

			public Vector2 MousePosition(InputHandler input)
			{
				return input.Position(this.touchId);
			}

			public void SetPointerIdToFirstDownPointer(InputHandler input)
			{
				InputHandler.PointerData pointerData = input.FirstDownPointer();
				if (pointerData == null)
				{
					return;
				}
				this.touchId = pointerData.pointerId;
			}

			public bool IsDown(InputHandler input)
			{
				if (this.isMouseDown)
				{
					return false;
				}
				InputHandler.PointerData pointerData = input.FirstDownPointer();
				return pointerData != null && Time.frameCount - pointerData.downFrame <= 1;
			}

			public bool IsUp(InputHandler input)
			{
				return this.isMouseDown && !input.IsDown(this.touchId);
			}

			public void Clear()
			{
				this.isSlotSwitched = false;
				this.state = PlayerInput.MouseParams.State.Touch;
				this.mouseUpEnum = null;
				this.activeAffector = null;
				this.affectedSlotsForMix.Clear();
				this.mixClass.Clear();
				this.isMouseDown = false;
				this.firstHitSlot = null;
				this.chip = null;
				this.slotToSwitchWith = null;
				this.chipToSwitchWith = null;
				this.affectedSlotsForMatch.Clear();
				this.affectedChipsForMatch.Clear();
				this.isMatching = false;
				this.isSlotToSwitchWithOffsetPositionSet = false;
				this.lastTestedSlotToSwitchWith = null;
			}

			public void Reset(Lock mainLock, PlayerInput input)
			{
				this.ResetAffectedSlotsForMatch(input);
				mainLock.UnlockAll();
				Slot.RemoveLocks(this.firstHitSlot, mainLock);
				Slot.RemoveLocks(this.slotToSwitchWith, mainLock);
				if (this.firstHitSlot != null)
				{
					this.firstHitSlot.light.RemoveLight(input.exchangeLight);
					this.firstHitSlot.light.RemoveLight(input.matchLight);
				}
				if (this.slotToSwitchWith != null)
				{
					this.slotToSwitchWith.light.RemoveLight(input.exchangeLight);
					this.slotToSwitchWith.light.RemoveLight(input.matchLight);
				}
				if (this.chip != null)
				{
					TransformBehaviour componentBehaviour = this.chip.GetComponentBehaviour<TransformBehaviour>();
					if (componentBehaviour != null)
					{
						componentBehaviour.localScale = Vector3.one;
						componentBehaviour.SetBrightness(1f);
					}
				}
				if (this.chipToSwitchWith != null)
				{
					TransformBehaviour componentBehaviour2 = this.chipToSwitchWith.GetComponentBehaviour<TransformBehaviour>();
					if (componentBehaviour2 != null)
					{
						componentBehaviour2.localScale = Vector3.one;
					}
				}
			}

			public PlayerInput.MouseParams.State state;

			public bool isMouseDown;

			public bool isSlotSwitched;

			public Vector3 mouseDownPositon;

			public Vector3 mouseDownUIPosition;

			public int touchId;

			public Slot firstHitSlot;

			public Chip chip;

			public Slot slotToSwitchWith;

			public Slot lastTestedSlotToSwitchWith;

			public Chip chipToSwitchWith;

			public PlayerInput.MixClass mixClass = new PlayerInput.MixClass();

			public bool isSlotToSwitchWithOffsetPositionSet;

			public Vector3 slotToSwitchWithOffsetPosition;

			public bool isMatching;

			public List<Slot> affectedSlotsForMatch = new List<Slot>();

			public List<Chip> affectedChipsForMatch = new List<Chip>();

			public List<Slot> affectedSlotsForMix = new List<Slot>();

			public IEnumerator mouseUpEnum;

			private PlayerInput.AffectorBase activeAffector_;

			public enum State
			{
				Touch,
				TapActivated,
				SwapActivated,
				SwapToNothingActivated,
				CancelSwap,
				SwapToNoMatchActivated
			}
		}

		public class MixClass
		{
			public void Clear()
			{
				this.chips.Clear();
			}

			public int CountOfType(ChipType type, ChipType type2)
			{
				int num = 0;
				for (int i = 0; i < this.chips.Count; i++)
				{
					Chip chip = this.chips[i];
					if (chip.chipType == type || chip.chipType == type2)
					{
						num++;
					}
				}
				return num;
			}

			public Chip GetChip(ChipType type)
			{
				for (int i = 0; i < this.chips.Count; i++)
				{
					Chip chip = this.chips[i];
					if (chip.chipType == type)
					{
						return chip;
					}
				}
				return null;
			}

			public Chip GetOtherChip(ChipType type)
			{
				for (int i = 0; i < this.chips.Count; i++)
				{
					Chip chip = this.chips[i];
					if (chip.chipType != type)
					{
						return chip;
					}
				}
				return null;
			}

			public int CountOfType(ChipType type)
			{
				int num = 0;
				for (int i = 0; i < this.chips.Count; i++)
				{
					if (this.chips[i].chipType == type)
					{
						num++;
					}
				}
				return num;
			}

			public void TryAdd(Chip chip)
			{
				if (chip == null)
				{
					return;
				}
				if (!chip.isPowerup)
				{
					return;
				}
				this.chips.Add(chip);
			}

			public List<Chip> chips = new List<Chip>();
		}

		private sealed class _003CDoWaitForActiveAffector_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoWaitForActiveAffector_003Ed__42(int _003C_003E1__state)
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
				PlayerInput playerInput = this._003C_003E4__this;
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
				}
				if (this.activeAffector.canFinish)
				{
					playerInput.OnMouseUp();
					return false;
				}
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

			public PlayerInput.AffectorBase activeAffector;

			public PlayerInput _003C_003E4__this;
		}
	}
}
