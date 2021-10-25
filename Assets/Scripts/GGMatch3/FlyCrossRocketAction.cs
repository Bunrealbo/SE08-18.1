using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class FlyCrossRocketAction : BoardAction
	{
		private FlyCrossRocketAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.flyCrossRocketActionSettings;
			}
		}

		public void Init(FlyCrossRocketAction.FlyParams flyParams)
		{
			this.flyParams = flyParams;
			this.globalLock = this.lockContainer.NewLock();
			this.globalLock.isChipGeneratorSuspended = true;
			this.globalLock.isSlotGravitySuspended = true;
			this.globalLock.isAvailableForDiscoBombSuspended = true;
			List<Chip> bombChips = flyParams.bombChips;
			for (int i = 0; i < bombChips.Count; i++)
			{
				Chip chip = bombChips[i];
				Slot lastConnectedSlot = chip.lastConnectedSlot;
				if (lastConnectedSlot != null)
				{
					this.globalLock.LockSlot(lastConnectedSlot);
					this.lockedSlots.Add(lastConnectedSlot);
				}
				chip.RemoveFromGame();
			}
			IntVector2 originPosition = flyParams.originPosition;
			IntVector2 a = originPosition - new IntVector2(flyParams.columns / 2, 0);
			Match3Game game = flyParams.game;
			float num = 0f;
			if (flyParams.affectorExport != null)
			{
				this.affectorExport = new Match3Game.InputAffectorExport();
				this.affectorExport.affectorExports.AddRange(flyParams.affectorExport.affectorExports);
				flyParams.affectorExport.affectorExports.Clear();
			}
			for (int j = 0; j < flyParams.columns; j++)
			{
				IntVector2 intVector = a + new IntVector2(j, 0);
				if (!game.board.IsOutOfBoard(intVector))
				{
					float delay = (float)Mathf.Abs(intVector.x - originPosition.x) * this.settings.delayForDistance + num;
					this.AddFlyDescriptorPair(intVector, IntVector2.up, delay);
				}
			}
			if (flyParams.useDelayBetweenRowsAndColumns)
			{
				num += this.settings.delayBetweenRowsAndColumns;
			}
			IntVector2 a2 = originPosition - new IntVector2(0, flyParams.rows / 2);
			for (int k = 0; k < flyParams.rows; k++)
			{
				IntVector2 intVector2 = a2 + new IntVector2(0, k);
				if (!game.board.IsOutOfBoard(intVector2))
				{
					float delay2 = (float)Mathf.Abs(intVector2.y - originPosition.y) * this.settings.delayForDistance + num;
					this.AddFlyDescriptorPair(intVector2, IntVector2.left, delay2);
				}
			}
			if (flyParams.prelockAll)
			{
				for (int l = 0; l < this.flyDescriptors.Count; l++)
				{
					FlyCrossRocketAction.FlyDescriptor flyDescriptor = this.flyDescriptors[l];
					flyDescriptor.action = this.CreateFlyPieceAction(flyDescriptor);
					List<Slot> pathSlots = flyDescriptor.action.GetPathSlots();
					for (int m = 0; m < pathSlots.Count; m++)
					{
						Slot slot = pathSlots[m];
						this.globalLock.LockSlot(slot);
						this.lockedSlots.Add(slot);
					}
				}
			}
		}

		private void AddFlyDescriptorPair(IntVector2 position, IntVector2 direction, float delay)
		{
			for (int i = 0; i < 2; i++)
			{
				FlyCrossRocketAction.FlyDescriptor flyDescriptor = new FlyCrossRocketAction.FlyDescriptor();
				flyDescriptor.position = position;
				if (i == 0)
				{
					flyDescriptor.direction = direction;
				}
				else
				{
					flyDescriptor.direction = -direction;
				}
				flyDescriptor.ignoreOriginSlot = (i == 0);
				flyDescriptor.delay = delay;
				this.flyDescriptors.Add(flyDescriptor);
			}
		}

		private FlyRocketPieceAction CreateFlyPieceAction(FlyCrossRocketAction.FlyDescriptor flyDescriptor)
		{
			FlyRocketPieceAction flyRocketPieceAction = new FlyRocketPieceAction();
			flyRocketPieceAction.Init(new FlyRocketPieceAction.Params
			{
				direction = flyDescriptor.direction,
				game = this.flyParams.game,
				ignoreOriginSlot = flyDescriptor.ignoreOriginSlot,
				position = flyDescriptor.position,
				prelock = true,
				canUseScale = true,
				isHavingCarpet = this.flyParams.isHavingCarpet,
				affectorExport = this.affectorExport
			});
			return flyRocketPieceAction;
		}

		private IEnumerator DoFly()
		{
			return new FlyCrossRocketAction._003CDoFly_003Ed__15(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (this.animation == null)
			{
				this.animation = this.DoFly();
			}
			this.animation.MoveNext();
		}

		private FlyCrossRocketAction.FlyParams flyParams;

		private Lock globalLock;

		private List<Slot> lockedSlots = new List<Slot>();

		private float deltaTime;

		private IEnumerator animation;

		public Match3Game.InputAffectorExport affectorExport;

		public List<FlyCrossRocketAction.FlyDescriptor> flyDescriptors = new List<FlyCrossRocketAction.FlyDescriptor>();

		public struct FlyParams
		{
			public List<Chip> bombChips
			{
				get
				{
					if (this.bombChips_ == null)
					{
						this.bombChips_ = new List<Chip>();
					}
					return this.bombChips_;
				}
			}

			public Match3Game game;

			public IntVector2 originPosition;

			public int rows;

			public int columns;

			public bool prelockAll;

			public bool useDelayBetweenRowsAndColumns;

			public bool isHavingCarpet;

			public List<Chip> bombChips_;

			public Match3Game.InputAffectorExport affectorExport;
		}

		[Serializable]
		public class Settings
		{
			public float delayBetweenRowsAndColumns = 0.3f;

			public float delayForDistance = 0.05f;
		}

		public class FlyDescriptor
		{
			public IntVector2 position;

			public IntVector2 direction;

			public float delay;

			public bool ignoreOriginSlot;

			public FlyRocketPieceAction action;

			public bool isActivated;
		}

		private sealed class _003CDoFly_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoFly_003Ed__15(int _003C_003E1__state)
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
				FlyCrossRocketAction flyCrossRocketAction = this._003C_003E4__this;
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
					this._003Cgame_003E5__2 = flyCrossRocketAction.flyParams.game;
					this._003Ctime_003E5__3 = 0f;
					GGSoundSystem.PlayParameters sound = default(GGSoundSystem.PlayParameters);
					sound.soundType = GGSoundSystem.SFXType.FlyCrossRocketAction;
					this._003Cgame_003E5__2.Play(sound);
				}
				this._003Ctime_003E5__3 += flyCrossRocketAction.deltaTime;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < flyCrossRocketAction.flyDescriptors.Count; i++)
				{
					FlyCrossRocketAction.FlyDescriptor flyDescriptor = flyCrossRocketAction.flyDescriptors[i];
					if (!flyDescriptor.isActivated && this._003Ctime_003E5__3 >= flyDescriptor.delay)
					{
						flyDescriptor.isActivated = true;
						if (flyDescriptor.action == null)
						{
							flyDescriptor.action = flyCrossRocketAction.CreateFlyPieceAction(flyDescriptor);
						}
						this._003Cgame_003E5__2.board.actionManager.AddAction(flyDescriptor.action);
					}
					if (flyDescriptor.isActivated)
					{
						num2++;
						if (!flyDescriptor.action.isAlive)
						{
							num3++;
						}
					}
				}
				bool flag = num2 >= flyCrossRocketAction.flyDescriptors.Count;
				int count = flyCrossRocketAction.flyDescriptors.Count;
				if (!flag)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				if (flyCrossRocketAction.affectorExport != null)
				{
					flyCrossRocketAction.affectorExport.Clear();
				}
				flyCrossRocketAction.globalLock.UnlockAll();
				flyCrossRocketAction.isAlive = false;
				return false;
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

			public FlyCrossRocketAction _003C_003E4__this;

			private Match3Game _003Cgame_003E5__2;

			private float _003Ctime_003E5__3;
		}
	}
}
