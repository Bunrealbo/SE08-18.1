using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EZCameraShake;
using GGMatch3;
using UnityEngine;

public class Match3Game : MonoBehaviour
{
    public bool IsPreventAutomatedMachesIfPossible()
    {
        return this.board.isGameEnded || (this.preventAutomatchesIfPossible && (!this.level.useChanceToNotPreventChipMatching || (float)this.RandomRange(0, 100) <= this.level.chanceToNotPreventChipMatching));
    }

    public bool preventAutomatchesIfPossible
    {
        get
        {
            return this.preventAutomatchesIfPossible_ || Match3Settings.instance.generalSettings.preventAutomatchesIfPossible;
        }
    }

    public bool strictAsPossibleToprevent
    {
        get
        {
            return this.preventAutomatchesIfPossible_ || Match3Settings.instance.generalSettings.strictAsPossibleToPrevent;
        }
    }

    public long totalTimePlayed
    {
        get
        {
            return this.endTimestampTicks - this.startTimestampTicks;
        }
    }

    public int timesBoughtMoves
    {
        get
        {
            return this.moveOffersBought.Count;
        }
    }

    public int totalCoinsSpent
    {
        get
        {
            int num = 0;
            for (int i = 0; i < this.moveOffersBought.Count; i++)
            {
                BuyMovesPricesConfig.OfferConfig offerConfig = this.moveOffersBought[i];
                if (offerConfig.price.currency == CurrencyType.coins)
                {
                    num += offerConfig.price.cost;
                }
            }
            return num;
        }
    }

    public long timePlayed
    {
        get
        {
            return DateTime.UtcNow.Ticks - this.startTimestampTicks;
        }
    }

    public Match3Settings settings
    {
        get
        {
            return Match3Settings.instance;
        }
    }

    public float boardContainerScale
    {
        get
        {
            return this.boardContainer.localScale.x;
        }
    }

    public Vector3 WorldToBoardPosition(Vector3 worldPosition)
    {
        Vector3 result = this.boardContainer.InverseTransformPoint(worldPosition);
        result.z = 0f;
        return result;
    }

    public Vector3 LocalToWorldPosition(Vector3 localPosition)
    {
        Vector3 result = this.boardContainer.TransformPoint(localPosition);
        result.z = 0f;
        return result;
    }

    public Vector3 bottomLeft
    {
        get
        {
            return new Vector3
            {
                x = -this.slotPhysicalSize.x * (float)(this.board.size.x - 1) * 0.5f,
                y = -this.slotPhysicalSize.y * (float)(this.board.size.y - 1) * 0.5f
            };
        }
    }

    public ComponentPool GetPool(PieceType type)
    {
        for (int i = 0; i < this.pieceCreatorPools.Count; i++)
        {
            Match3Game.PieceCreatorPool pieceCreatorPool = this.pieceCreatorPools[i];
            if (pieceCreatorPool.type == type)
            {
                return pieceCreatorPool.pool;
            }
        }
        return null;
    }

    public ComponentPool GetPool(PieceType type, ChipType chipType, ItemColor itemColor)
    {
        for (int i = 0; i < this.pieceCreatorPools.Count; i++)
        {
            Match3Game.PieceCreatorPool pieceCreatorPool = this.pieceCreatorPools[i];
            if (pieceCreatorPool.type == type && pieceCreatorPool.chipTypeList.Contains(chipType) && pieceCreatorPool.itemColorList.Contains(itemColor))
            {
                return pieceCreatorPool.pool;
            }
        }
        return null;
    }

    public List<Slot> GetCross(IntVector2 centerPos, int radius)
    {
        List<Slot> area = this.GetArea(centerPos, radius);
        for (int i = centerPos.x - radius; i <= centerPos.x + radius; i++)
        {
            for (int j = 0; j <= this.board.size.y; j++)
            {
                if (Mathf.Abs(j - centerPos.y) > radius)
                {
                    Slot slot = this.GetSlot(new IntVector2(i, j));
                    if (slot != null)
                    {
                        area.Add(slot);
                    }
                }
            }
        }
        for (int k = centerPos.y - radius; k <= centerPos.y + radius; k++)
        {
            for (int l = 0; l <= this.board.size.x; l++)
            {
                if (Mathf.Abs(l - centerPos.x) > radius)
                {
                    Slot slot2 = this.GetSlot(new IntVector2(l, k));
                    if (slot2 != null)
                    {
                        area.Add(slot2);
                    }
                }
            }
        }
        return area;
    }

    public int SeekingMissleCrossRadius
    {
        get
        {
            if (Match3Settings.instance.generalSettings.seekingRangeType == GeneralSettings.SeekingRangeType.Normal)
            {
                return 1;
            }
            return 0;
        }
    }

    public List<Slot> GetSeekingMissleArea(IntVector2 centerPos)
    {
        GeneralSettings.SeekingRangeType seekingRangeType = Match3Settings.instance.generalSettings.seekingRangeType;
        return this.GetCrossArea(centerPos, this.SeekingMissleCrossRadius);
    }

    public List<Slot> GetCrossArea(IntVector2 centerPos, int maxRadius)
    {
        List<Slot> list = this.affectingList;
        list.Clear();
        for (int i = centerPos.x - maxRadius; i <= centerPos.x + maxRadius; i++)
        {
            for (int j = centerPos.y - maxRadius; j <= centerPos.y + maxRadius; j++)
            {
                if (Mathf.Abs(i - centerPos.x) + Mathf.Abs(j - centerPos.y) <= maxRadius)
                {
                    Slot slot = this.GetSlot(new IntVector2(i, j));
                    if (slot != null)
                    {
                        list.Add(slot);
                    }
                }
            }
        }
        return list;
    }

    public List<Slot> GetAreaOfEffect(ChipType chipType, IntVector2 centerPos)
    {
        this.affectingList.Clear();
        if (chipType == ChipType.Bomb)
        {
            return this.GetBombArea(centerPos, 2);
        }
        if (chipType == ChipType.HorizontalRocket)
        {
            return this.GetHorizontalLine(centerPos);
        }
        if (chipType == ChipType.VerticalRocket)
        {
            return this.GetVerticalLine(centerPos);
        }
        if (chipType == ChipType.SeekingMissle)
        {
            return this.GetSeekingMissleArea(centerPos);
        }
        return this.affectingList;
    }

    public List<Slot> GetBombArea(IntVector2 centerPos, int maxRadius)
    {
        GeneralSettings.BombRangeType bombRangeType = Match3Settings.instance.generalSettings.bombRangeType;
        if (bombRangeType == GeneralSettings.BombRangeType.Candy)
        {
            maxRadius = Mathf.Max(0, maxRadius - 1);
        }
        List<Slot> list = this.affectingList;
        list.Clear();
        for (int i = centerPos.x - maxRadius; i <= centerPos.x + maxRadius; i++)
        {
            int num = Mathf.Abs(i - centerPos.x);
            int num2 = maxRadius;
            if (bombRangeType == GeneralSettings.BombRangeType.Full)
            {
                num2 = maxRadius;
            }
            else if (bombRangeType == GeneralSettings.BombRangeType.Circle)
            {
                num2 = Mathf.Min(maxRadius - num + 1, maxRadius);
            }
            else if (bombRangeType == GeneralSettings.BombRangeType.Diamond)
            {
                num2 = Mathf.Min(maxRadius - num, maxRadius);
            }
            else if (bombRangeType == GeneralSettings.BombRangeType.Candy)
            {
                num2 = maxRadius;
            }
            for (int j = centerPos.y - num2; j <= centerPos.y + num2; j++)
            {
                Slot slot = this.GetSlot(new IntVector2(i, j));
                if (slot != null)
                {
                    list.Add(slot);
                }
            }
        }
        return list;
    }

    public List<Slot> GetArea(IntVector2 centerPos, int maxRadius)
    {
        List<Slot> list = this.affectingList;
        list.Clear();
        for (int i = centerPos.x - maxRadius; i <= centerPos.x + maxRadius; i++)
        {
            for (int j = centerPos.y - maxRadius; j <= centerPos.y + maxRadius; j++)
            {
                Slot slot = this.GetSlot(new IntVector2(i, j));
                if (slot != null)
                {
                    list.Add(slot);
                }
            }
        }
        return list;
    }

    public List<Slot> GetAllPlayingSlots()
    {
        List<Slot> list = this.affectingList;
        list.Clear();
        foreach (Slot slot in this.board.slots)
        {
            if (slot != null)
            {
                list.Add(slot);
            }
        }
        return list;
    }

    public List<Slot> GetVerticalLine(IntVector2 centerPos)
    {
        List<Slot> list = this.affectingList;
        list.Clear();
        for (int i = 0; i <= this.board.size.y; i++)
        {
            Slot slot = this.GetSlot(new IntVector2(centerPos.x, i));
            if (slot != null)
            {
                list.Add(slot);
            }
        }
        return list;
    }

    public List<Slot> GetHorizontalLine(IntVector2 centerPos)
    {
        List<Slot> list = this.affectingList;
        list.Clear();
        for (int i = 0; i <= this.board.size.x; i++)
        {
            Slot slot = this.GetSlot(new IntVector2(i, centerPos.y));
            if (slot != null)
            {
                list.Add(slot);
            }
        }
        return list;
    }

    public ComponentPool GetPool(PieceType type, ChipType chipType)
    {
        for (int i = 0; i < this.pieceCreatorPools.Count; i++)
        {
            Match3Game.PieceCreatorPool pieceCreatorPool = this.pieceCreatorPools[i];
            if (pieceCreatorPool.type == type && pieceCreatorPool.chipTypeList.Contains(chipType))
            {
                return pieceCreatorPool.pool;
            }
        }
        return null;
    }

    public Slot GetSlot(IntVector2 position)
    {
        return this.board.GetSlot(position);
    }

    public void Play(GGSoundSystem.SFXType sound)
    {
        if (!this.isHudEnabled)
        {
            return;
        }
        this.Play(new GGSoundSystem.PlayParameters
        {
            soundType = sound
        });
    }

    public void Play(GGSoundSystem.PlayParameters sound)
    {
        if (this.board.isGameSoundsSuspended)
        {
            return;
        }
        if (!this.isHudEnabled)
        {
            return;
        }
        sound.frameNumber = (long)Time.frameCount;
        GGSoundSystem.Play(sound);
    }

    public void TapOnSlot(IntVector2 pos1, SwapParams swapParams = null)
    {
        if (this.isUserInteractionSuspended)
        {
            return;
        }
        Slot slot = this.GetSlot(pos1);
        if (slot == null)
        {
            return;
        }
        Chip slotComponent = slot.GetSlotComponent<Chip>();
        if (slotComponent == null)
        {
            return;
        }
        if (slot.isTapToActivateSuspended)
        {
            return;
        }
        GameStats.Move move = new GameStats.Move();
        GameStats.Move move2 = move;
        move.toPosition = pos1;
        move2.fromPosition = pos1;
        move.moveType = GameStats.MoveType.PowerupTap;
        move.frameWhenActivated = this.board.currentFrameIndex;
        this.board.gameStats.moves.Add(move);
        if (slotComponent.isPowerup)
        {
            this.board.gameStats.powerupsUsedByTap++;
        }
        SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
        slotDestroyParams.isFromTap = true;
        slotDestroyParams.swapParams = swapParams;
        if (slotComponent.canBeTappedToActivate)
        {
            slot.OnDestroySlot(slotDestroyParams);
        }
    }

    public bool TrySwitchSlots(Slot slot1, Slot slot2, bool instant)
    {
        return slot1 != null && slot2 != null && this.TrySwitchSlots(slot1.position, slot2.position, instant);
    }

    public bool TrySwitchSlots(IntVector2 pos1, IntVector2 pos2, bool instant)
    {
        return this.TrySwitchSlots(new Match3Game.SwitchSlotsArguments
        {
            pos1 = pos1,
            pos2 = pos2,
            instant = instant
        });
    }

    public void TryShowSwitchNotPossible(IntVector2 pos1, IntVector2 pos2)
    {
        this.TryShowSwitchNotPossible(new Match3Game.SwitchSlotsArguments
        {
            pos1 = pos1,
            pos2 = pos2
        });
    }

    private void TryShowSwitchNotPossible(Match3Game.SwitchSlotsArguments arguments)
    {
        IntVector2 pos = arguments.pos1;
        IntVector2 pos2 = arguments.pos2;
        Slot slot = this.GetSlot(pos);
        Slot slot2 = this.GetSlot(pos2);
        if (slot == null)
        {
            return;
        }
        if (slot.isSlotMatchingSuspended || slot.isSlotGravitySuspended || slot.isSlotSwipingSuspended)
        {
            return;
        }
        ShowSwapNotPossibleAction.SwapChipsParams chipParams = default(ShowSwapNotPossibleAction.SwapChipsParams);
        chipParams.slot1 = slot;
        chipParams.positionToMoveSlot1 = pos2;
        chipParams.game = this;
        if (slot2 != null && slot2.isSlotGravitySuspendedByComponent)
        {
            chipParams.wobble = true;
        }
        ShowSwapNotPossibleAction showSwapNotPossibleAction = new ShowSwapNotPossibleAction();
        showSwapNotPossibleAction.Init(chipParams);
        this.board.actionManager.AddAction(showSwapNotPossibleAction);
    }

    public bool TrySwitchSlots(Match3Game.SwitchSlotsArguments arguments)
    {
        IntVector2 pos = arguments.pos1;
        IntVector2 pos2 = arguments.pos2;
        bool instant = arguments.instant;
        if (this.isUserInteractionSuspended)
        {
            return false;
        }
        Slot slot = this.GetSlot(pos);
        Slot slot2 = this.GetSlot(pos2);
        if (slot == null || slot2 == null)
        {
            if (slot != null)
            {
                this.TryShowSwitchNotPossible(arguments);
            }
            return false;
        }
        if (slot.isSlotMatchingSuspended || slot2.isSlotMatchingSuspended)
        {
            this.TryShowSwitchNotPossible(arguments);
            return false;
        }
        if (slot.isSlotGravitySuspended || slot2.isSlotGravitySuspended)
        {
            this.TryShowSwitchNotPossible(arguments);
            return false;
        }
        if (slot.isSlotSwipingSuspended || slot2.isSlotSwapSuspended)
        {
            this.TryShowSwitchNotPossible(arguments);
            return false;
        }
        if (slot.isSlotSwipingSuspendedForSlot(slot2) || slot.isSwipeSuspendedTo(slot2))
        {
            this.TryShowSwitchNotPossible(arguments);
            return false;
        }
        SwapToMatchAction.PowerupList powerupList = new SwapToMatchAction.PowerupList();
        powerupList.Add(slot.GetSlotComponent<Chip>());
        powerupList.Add(slot2.GetSlotComponent<Chip>());
        GameStats.Move move = new GameStats.Move();
        move.fromPosition = pos;
        move.toPosition = pos2;
        move.moveType = GameStats.MoveType.Match;
        move.frameWhenActivated = this.board.currentFrameIndex;
        if (powerupList.isMixingPowerups)
        {
            this.board.gameStats.powerupsMixed++;
            move.moveType = GameStats.MoveType.PowerupMix;
        }
        else if (powerupList.isActivatingPowerup)
        {
            this.board.gameStats.powerupsUsedBySwipe++;
            move.moveType = GameStats.MoveType.PowerupActivation;
        }
        this.board.matchCounter.OnUserMadeMove();
        this.board.gameStats.moves.Add(move);
        SwapToMatchAction swapToMatchAction = new SwapToMatchAction();
        swapToMatchAction.Init(new SwapToMatchAction.SwapActionProperties
        {
            slot1 = slot,
            slot2 = slot2,
            isInstant = instant,
            bolts = arguments.bolts,
            switchSlotsArgument = arguments
        });
        this.board.actionManager.AddAction(swapToMatchAction);
        return true;
    }

    public int SlotsDistanceToEndOfBoard(IntVector2 pos, IntVector2 direction)
    {
        if (direction == IntVector2.zero)
        {
            return 0;
        }
        if (direction.x > 0)
        {
            return this.board.size.x - pos.x - 1;
        }
        if (direction.x < 0)
        {
            return pos.x;
        }
        if (direction.y > 0)
        {
            return this.board.size.y - pos.y - 1;
        }
        if (direction.y < 0)
        {
            return pos.y;
        }
        return 0;
    }

    public Vector3 LocalPositionOfCenter(IntVector2 position)
    {
        return this.bottomLeft + new Vector3((float)position.x * this.slotPhysicalSize.x, (float)position.y * this.slotPhysicalSize.y, 0f);
    }

    public IntVector2 BoardPositionFromLocalPosition(Vector3 position)
    {
        position -= this.bottomLeft;
        int x = Mathf.FloorToInt(position.x / this.slotPhysicalSize.x);
        int y = Mathf.FloorToInt(position.y / this.slotPhysicalSize.y);
        return new IntVector2(x, y);
    }

    public IntVector2 BoardPositionFromLocalPositionRound(Vector3 position)
    {
        position -= this.bottomLeft;
        int x = Mathf.RoundToInt(position.x / this.slotPhysicalSize.x);
        int y = Mathf.RoundToInt(position.y / this.slotPhysicalSize.y);
        return new IntVector2(x, y);
    }

    public IntVector2 ClosestBoardPositionFromLocalPosition(Vector3 position)
    {
        position -= this.bottomLeft;
        int x = Mathf.FloorToInt((position.x + this.slotPhysicalSize.x * 0.5f) / this.slotPhysicalSize.x);
        int y = Mathf.FloorToInt((position.y + this.slotPhysicalSize.y * 0.5f) / this.slotPhysicalSize.y);
        return new IntVector2(x, y);
    }

    public bool isHudEnabled
    {
        get
        {
            return this.initParams == null || !this.initParams.isHudDissabled;
        }
    }

    public void SetHasBounceOnAllChips(bool hasBounce)
    {
        for (int i = 0; i < this.board.sortedSlotsUpdateList.Count; i++)
        {
            Chip slotComponent = this.board.sortedSlotsUpdateList[i].GetSlotComponent<Chip>();
            if (slotComponent != null)
            {
                ChipBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<ChipBehaviour>();
                if (!(componentBehaviour == null))
                {
                    componentBehaviour.hasBounce = hasBounce;
                }
            }
        }
    }

    private Match3Game.TutorialMatchProgress GetOrCreateTutorialMatchProgress(LevelDefinition.TutorialMatch tutorialMatch)
    {
        for (int i = 0; i < this.tutorialMatchProgressList.Count; i++)
        {
            Match3Game.TutorialMatchProgress tutorialMatchProgress = this.tutorialMatchProgressList[i];
            if (tutorialMatchProgress.tutorialMatch == tutorialMatch)
            {
                return tutorialMatchProgress;
            }
        }
        Match3Game.TutorialMatchProgress tutorialMatchProgress2 = new Match3Game.TutorialMatchProgress();
        tutorialMatchProgress2.tutorialMatch = tutorialMatch;
        this.tutorialMatchProgressList.Add(tutorialMatchProgress2);
        return tutorialMatchProgress2;
    }

    private bool isTutorialActive
    {
        get
        {
            return this.tutorialAction != null;
        }
    }

    public void UpdateTutorialMatches(bool isBoardSettled)
    {
        if (this.tutorialAction != null)
        {
            this.tutorialAction.isBoardSettled = isBoardSettled;
        }
        if (this.isUserInteractionSuspended)
        {
            return;
        }
        int userMovesCount = this.board.userMovesCount;
        if (userMovesCount >= this.level.tutorialMatches.Count)
        {
            return;
        }
        if (!isBoardSettled)
        {
            return;
        }
        LevelDefinition.TutorialMatch tutorialMatch = this.level.tutorialMatches[userMovesCount];
        Match3Game.TutorialMatchProgress orCreateTutorialMatchProgress = this.GetOrCreateTutorialMatchProgress(tutorialMatch);
        if (orCreateTutorialMatchProgress.isStarted)
        {
            return;
        }
        orCreateTutorialMatchProgress.isStarted = true;
        if (!tutorialMatch.isEnabled)
        {
            return;
        }
        ShowTutorialMaskAction.Parameters parameters = new ShowTutorialMaskAction.Parameters();
        parameters.game = this;
        parameters.match = tutorialMatch;
        if (userMovesCount != this.level.tutorialMatches.Count - 1)
        {
            parameters.onMiddle = new Action(this.OnTutorialMiddle);
            parameters.onEnd = new Action(this.OnTutorialEnd);
        }
        else
        {
            parameters.onEnd = new Action(this.OnLastTutorialEnd);
        }
        this.tutorialAction = new ShowTutorialMaskAction();
        this.tutorialAction.Init(parameters);
        this.board.nonChangeStateActionMenager.AddAction(this.tutorialAction);
    }

    private void OnLastTutorialEnd()
    {
        this.tutorialAction = null;
        this.PutBoosters(this.startGameArguments);
    }

    private void OnTutorialMiddle()
    {
        this.board.isInteractionSuspended = true;
    }

    private void OnTutorialEnd()
    {
        this.board.isInteractionSuspended = false;
        this.tutorialAction = null;
    }

    public void ShakeCamera(GeneralSettings.CameraShakeSettings shakeSettings)
    {
        if (this.cameraShaker == null)
        {
            return;
        }
        this.cameraShaker.enabled = true;
        float shakeScale = Match3Settings.instance.generalSettings.shakeScale;
        if (shakeScale <= 0f)
        {
            return;
        }
        this.cameraShaker.ShakeOnce(shakeSettings.magnitude * shakeScale, shakeSettings.roughness, shakeSettings.fadeInTime, shakeSettings.fadeOutTime, shakeSettings.posInfluence, shakeSettings.rotInfluence);
    }

    public void ShakeCamera()
    {
        GeneralSettings.CameraShakeSettings shakeSettings = Match3Settings.instance.generalSettings.shakeSettings;
        this.ShakeCamera(shakeSettings);
    }

    public void StartGame(Match3Game.StartGameArguments arguments)
    {
        this.SetHasBounceOnAllChips(true);
        this.gameStarted = true;
        this.UpdateTutorialMatches(true);
        this.startGameArguments = arguments;
        if (this.tutorialAction == null)
        {
            this.PutBoosters(arguments);
        }
        this.startTimestampTicks = DateTime.UtcNow.Ticks;
    }

    private void PutBoosters(Match3Game.StartGameArguments arguments)
    {
        if (arguments.putBoosters && this.initParams.activeBoosters != null && this.initParams.activeBoosters.Count > 0)
        {
            if (this.initParams.giftBoosterLevel > 0)
            {
                this.gameScreen.rankedBoostersStartAnimation.Show(this.initParams.giftBoosterLevel);
            }
            List<BoosterConfig> activeBoosters = this.initParams.activeBoosters;
            for (int i = 0; i < activeBoosters.Count; i++)
            {
                BoosterConfig boosterConfig = activeBoosters[i];
                PlacePowerupAction placePowerupAction = new PlacePowerupAction();
                PlacePowerupAction.Parameters parameters = new PlacePowerupAction.Parameters();
                parameters.game = this;
                parameters.internalAnimation = true;
                parameters.powerup = BoosterConfig.BoosterToChipType(boosterConfig.boosterType);
                parameters.initialDelay = (float)i * Match3Settings.instance.placePowerupActionSettings.delayBetweenPowerups;
                if (this.initParams.giftBoosterLevel > 0)
                {
                    parameters.initialDelay += this.gameScreen.rankedBoostersStartAnimation.boosterDelay;
                }
                if (i == activeBoosters.Count - 1)
                {
                    parameters.onComplete = new Action(this._003CPutBoosters_003Eb__108_0);
                }
                placePowerupAction.Init(parameters);
                this.board.actionManager.AddAction(placePowerupAction);
                new Analytics.BoosterUsedEvent
                {
                    booster = boosterConfig,
                    stageState = this.gameScreen.stageState
                }.Send();
            }
            return;
        }
        this.HighlightAllGoals();
    }

    public void Init(Camera mainCamera, GameScreen gameScreen, Match3GameParams initParams)
    {
        this.board.isInteractionSuspended = false;
        this.input.SetCamera(mainCamera);
        this.initParams = initParams;
        this.gameScreen = gameScreen;
        this.tutorialHighlighter.Init(gameScreen);
        int num = (int)DateTime.Now.Ticks;
        if (initParams.setRandomSeed)
        {
            num = initParams.randomSeed;
        }
        this.board.randomProvider.seed = num;
        this.board.randomProvider.Init();
        this.board.gameStats.initialSeed = num;
    }

    public void Callback_ShowActivatePowerup(PowerupsDB.PowerupDefinition powerup)
    {
        if (this.isTutorialActive)
        {
            return;
        }
        if (this.board.isGameEnded || this.board.isShufflingBoard || this.board.isEndConditionReached || this.isUserInteractionSuspended)
        {
            return;
        }
        if (powerup.ownedCount <= 0L)
        {
            BuyPowerupDialog.Show(new BuyPowerupDialog.InitArguments
            {
                powerup = powerup,
                onSuccess = new Action<bool>(this.Callback_OnBuyPowerupComplete)
            });
            return;
        }
        PowerupPlacementHandler.InitArguments initArguments = default(PowerupPlacementHandler.InitArguments);
        initArguments.game = this;
        initArguments.powerup = powerup;
        initArguments.onComplete = new Action<PowerupPlacementHandler.PlacementCompleteArguments>(this.Callback_OnPlacePowerup);
        this.gameScreen.powerupPlacement.Show(initArguments);
        this.board.isPowerupSelectionActive = true;
        this.showMatchAction.Stop();
    }

    private void Callback_OnPlacePowerup(PowerupPlacementHandler.PlacementCompleteArguments completeArguments)
    {
        this.board.isPowerupSelectionActive = false;
        this.gameScreen.powerupsPanel.Refresh();
        if (completeArguments.isCancel)
        {
            return;
        }
        HammerHitAction.InitArguments initArguments = default(HammerHitAction.InitArguments);
        initArguments.game = this;
        initArguments.completeArguments = completeArguments;
        HammerHitAction hammerHitAction = new HammerHitAction();
        hammerHitAction.Init(initArguments);
        PowerupsDB.PowerupDefinition powerup = completeArguments.initArguments.powerup;
        completeArguments.initArguments.powerup.usedCount = completeArguments.initArguments.powerup.usedCount + 1L;
        this.gameScreen.powerupsPanel.Refresh();
        this.board.actionManager.AddAction(hammerHitAction);
        if (powerup.type == PowerupType.Hammer)
        {
            this.gameScreen.stageState.hammersUsed++;
            return;
        }
        this.gameScreen.stageState.powerHammersUsed++;
    }

    private void Callback_OnBuyPowerupComplete(bool success)
    {
        this.gameScreen.powerupsPanel.Refresh();
    }

    public void CreateBoard(LevelDefinition level)
    {
        this.CreateBoard(new Match3Game.CreateBoardArguments
        {
            level = level
        });
    }

    private SuggestMoveType suggestMoveType
    {
        get
        {
            return this.level.suggestMoveType;
        }
    }

    private ShowPotentialMatchSetting showPotentialMatchSetting
    {
        get
        {
            return this.level.suggestMoveSetting;
        }
    }

    public void SetStageDifficulty(Match3StagesDB.Stage.Difficulty difficulty)
    {
        this.difficultyChanger.Apply(difficulty);
    }

    public void CreateBoard(Match3Game.CreateBoardArguments arguments)
    {
        this.createBoardArguments = arguments;
        LevelDefinition levelDefinition = arguments.level;
        Vector3 offset = arguments.offset;
        if (this.initParams.isHudDissabled)
        {
            GGUtil.Hide(this.boardContainer);
        }
        this.level = levelDefinition;
        int width = levelDefinition.size.width;
        int height = levelDefinition.size.height;
        this.preventAutomatchesIfPossible_ = levelDefinition.isPreventingGeneratorChipMatching;
        RectTransform gamePlayAreaContainer = this.gameScreen.gamePlayAreaContainer;
        Vector3[] array = new Vector3[4];
        gamePlayAreaContainer.GetWorldCorners(array);
        float num = 0.25f;
        Vector2 vector = new Vector2((float)levelDefinition.size.width * this.slotPhysicalSize.x + 2f * num, (float)levelDefinition.size.height * this.slotPhysicalSize.x + 2f * num);
        Vector2 vector2 = new Vector2(array[2].x - array[0].x, array[2].y - array[0].y);
        float num2 = Mathf.Min(vector2.x / vector.x, vector2.y / vector.y);
        this.boardContainer.localScale = new Vector3(num2, num2, 1f);
        this.boardContainer.position = gamePlayAreaContainer.transform.position + offset;
        this.board = new Match3Board();
        IntVector2 size = this.board.size;
        size.x = width;
        size.y = height;
        this.board.generationSettings = levelDefinition.generationSettings;
        this.board.Init(size);
        this.borderRenderer.slotSize = this.slotPhysicalSize.x;
        this.slotsRenderer.slotSize = this.slotPhysicalSize.x;
        this.borderRenderer.ShowBorderOnLevel(levelDefinition);
        this.slotsRenderer.ShowSlotsOnLevel(levelDefinition);
        bool hasMoreMoves = false;
        LevelDefinition.ConveyorBelts conveyorBelts = levelDefinition.GetConveyorBelts();
        PopulateBoard.Params @params = new PopulateBoard.Params();
        @params.randomProvider = this.board.randomProvider;
        if (levelDefinition.generationSettings.isConfigured)
        {
            List<LevelDefinition.ChipGenerationSettings.ChipSetting> chipSettings = levelDefinition.generationSettings.chipSettings;
            for (int i = 0; i < chipSettings.Count; i++)
            {
                LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = chipSettings[i];
                if (chipSetting.chipType == ChipType.Chip)
                {
                    if (@params.availableColors.Contains(chipSetting.itemColor))
                    {
                        UnityEngine.Debug.Log("DUPLICATE AVAILABLE COLORS");
                    }
                    else
                    {
                        this.board.availableColors.Add(chipSetting.itemColor);
                        @params.availableColors.Add(chipSetting.itemColor);
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < levelDefinition.numChips; j++)
            {
                @params.availableColors.Add((ItemColor)j);
                this.board.availableColors.Add((ItemColor)j);
            }
        }
        this.particles.disableParticles = this.initParams.disableParticles;
        @params.maxPotentialMatches = Match3Settings.instance.maxPotentialMatchesAtStart;
        this.board.populateBoard.RandomPopulate(levelDefinition, @params);
        for (int k = 0; k < size.y; k++)
        {
            for (int l = 0; l < size.x; l++)
            {
                IntVector2 position = new IntVector2(l, k);
                LevelDefinition.SlotDefinition slot = levelDefinition.GetSlot(position);
                if (slot.slotType != SlotType.Empty && (slot.chipType != ChipType.EmptyConveyorSpace || conveyorBelts.IsPartOfConveyor(position)))
                {
                    Slot slot2 = new Slot();
                    slot2.canGenerateChip = slot.generatorSettings.isGeneratorOn;
                    slot2.generatorSettings = slot.generatorSettings;
                    slot2.generatorSlotSettings = levelDefinition.GetGeneratorSlotSettings(slot.generatorSettings.slotGeneratorSetupIndex);
                    slot2.position = new IntVector2(l, k);
                    slot2.Init(this);
                    if (slot2.generatorSlotSettings != null)
                    {
                        this.AddSpecialGenerator(slot2);
                    }
                    if (slot.hasNet)
                    {
                        this.AddNetToSlot(slot2, slot);
                    }
                    if (slot.hasBox)
                    {
                        this.AddBoxToSlot(slot2, slot, levelDefinition);
                    }
                    if (slot.hasBubbles)
                    {
                        this.AddBubblesToSlot(slot2);
                    }
                    if (slot.hasSnowCover)
                    {
                        this.AddSnowCoverToSlot(slot2);
                    }
                    if (slot.hasBasket)
                    {
                        this.AddBasketToSlot(slot2, slot);
                    }
                    if (slot.hasChain)
                    {
                        this.AddChainToSlot(slot2, slot);
                    }
                    if (slot.wallSettings.isWallActive)
                    {
                        this.AddWallToSlot(slot2, slot);
                    }
                    if (slot.colorSlateLevel > 0 && (slot.colorSlateLevel > 1 || levelDefinition.burriedElements.HasElementsUnderPosition(slot2.position)))
                    {
                        this.AddSlotColorSlot(slot2, slot);
                    }
                    this.AddLightToSlot(slot2, slot);
                    this.AddBckLightToSlot(slot2);
                    this.board.SetSlot(slot2.position, slot2);
                    slot2.gravity.down = slot.gravitySettings.down;
                    slot2.gravity.up = slot.gravitySettings.up;
                    slot2.gravity.left = slot.gravitySettings.left;
                    slot2.gravity.right = slot.gravitySettings.right;
                    slot2.isExitForFallingChip = slot.isExitForFallingChip;
                    ItemColor itemColor = (ItemColor)this.RandomRange(0, 4);
                    PopulateBoard.BoardRepresentation.RepresentationSlot slot3 = this.board.populateBoard.board.GetSlot(slot2.position);
                    if (slot3 != null)
                    {
                        itemColor = slot3.itemColor;
                    }
                    if ((slot.chipType == ChipType.Chip || slot.chipType == ChipType.MonsterChip) && slot.itemColor != ItemColor.Unknown && slot.itemColor != ItemColor.RandomColor)
                    {
                        itemColor = slot.itemColor;
                    }
                    if (!slot.IsMonsterInSlot(levelDefinition))
                    {
                        if (slot.chipType == ChipType.EmptyConveyorSpace)
                        {
                            this.CreateEmptyConveyorSpace(slot2);
                        }
                        else if (slot.chipType == ChipType.MagicHat)
                        {
                            this.CreateMagicHat(slot2);
                        }
                        else if (slot.chipType == ChipType.MagicHatBomb || slot.chipType == ChipType.MagicHatSeekingMissle || slot.chipType == ChipType.MagicHatRocket)
                        {
                            this.CreateMagicHatBomb(slot2, slot);
                        }
                        else if (slot.chipType == ChipType.GrowingElement)
                        {
                            this.CreateGrowingElement(slot2, slot);
                        }
                        else if (slot.chipType == ChipType.FallingGingerbreadMan)
                        {
                            this.CreateFallingElement(slot2, slot.chipType);
                        }
                        else if (slot.chipType == ChipType.RockBlocker || slot.chipType == ChipType.SnowRockBlocker)
                        {
                            this.CreateRockBlocker(slot2, slot);
                        }
                        else if (slot.chipType == ChipType.MoreMovesChip)
                        {
                            hasMoreMoves = true;
                            this.CreateMoreMovesChip(slot2, slot);
                        }
                        else if (slot.chipType == ChipType.BunnyChip || slot.chipType == ChipType.CookiePickup)
                        {
                            this.CreateCharacterInSlot(slot2, slot);
                        }
                        else if (slot.chipType == ChipType.Bomb || slot.chipType == ChipType.DiscoBall || slot.chipType == ChipType.HorizontalRocket || slot.chipType == ChipType.VerticalRocket || slot.chipType == ChipType.SeekingMissle)
                        {
                            this.CreatePowerupInSlot(slot2, slot.chipType);
                        }
                        else if (slot.chipType == ChipType.MonsterChip)
                        {
                            this.CreateChipInSlot(slot2, ChipType.MonsterChip, itemColor);
                        }
                        else if (slot.chipType != ChipType.EmptyChipSlot)
                        {
                            this.CreateChipInSlot(slot2, itemColor);
                        }
                    }
                    if (slot.gravitySettings.canJumpWithGravity)
                    {
                        this.AddJumpRampToSlot(slot2);
                    }
                    if (slot.isExitForFallingChip)
                    {
                        this.AddFallingElementExitToSlot(slot2);
                    }
                    Chip slotComponent = slot2.GetSlotComponent<Chip>();
                    if (slotComponent != null)
                    {
                        slotComponent.chipTag = slot.chipTag;
                    }
                    if (slot.hasIce)
                    {
                        this.AddIceToSlot(slot2, slot);
                    }
                    if (slot.chipType != ChipType.EmptyConveyorSpace && slot.holeBlocker)
                    {
                        this.CreateEmptyConveyorSpace(slot2);
                    }
                }
            }
        }
        this.board.hasMoreMoves = hasMoreMoves;
        for (int m = 0; m < levelDefinition.generatorSetups.Count; m++)
        {
            GeneratorSetup generatorSetup = levelDefinition.generatorSetups[m];
            Slot slot4 = this.board.GetSlot(generatorSetup.position);
            if (slot4 != null)
            {
                slot4.generatorSetup = generatorSetup;
            }
        }
        this.aiPlayer.Init(this);
        this.board.burriedElements.Init(this, levelDefinition);
        this.board.carpet.Init(this, levelDefinition);
        this.board.monsterElements.Init(this, levelDefinition);
        List<LevelDefinition.Portal> allPortals = levelDefinition.GetAllPortals();
        for (int n = 0; n < allPortals.Count; n++)
        {
            LevelDefinition.Portal portal = allPortals[n];
            if (portal.isValid)
            {
                Slot slot5 = this.GetSlot(portal.entranceSlot.position);
                Slot slot6 = this.GetSlot(portal.exitSlot.position);
                if (slot5 != null && slot6 != null)
                {
                    slot5.portalDestinationSlots.Add(slot6);
                    this.AddPipe(slot5, false, n);
                    this.AddPipe(slot6, true, n);
                }
            }
        }
        foreach (Slot slot7 in this.board.slots)
        {
            if (slot7 != null)
            {
                IntVector2 position2 = slot7.position;
                if (levelDefinition.GetSlot(position2).gravitySettings.canJumpWithGravity)
                {
                    List<Slot> list = slot7.gravity.FindSlotsToWhichCanJumpTo(slot7, this);
                    for (int num4 = 0; num4 < list.Count; num4++)
                    {
                        Slot slot8 = list[num4];
                        slot7.jumpDestinationSlots.Add(slot8);
                        slot8.jumpOriginSlots.Add(slot7);
                    }
                }
            }
        }
        this.board.sortedSlotsUpdateList.Clear();
        Slot[] slots = this.board.slots;
        foreach (Slot slot9 in slots)
        {
            if (slot9 != null)
            {
                slot9.FillIncomingGravitySlots();
                slot9.SetMaxDistanceToEnd(0);
                this.board.sortedSlotsUpdateList.Add(slot9);
            }
        }
        this.board.sortedSlotsUpdateList.Sort(new Comparison<Slot>(Match3Game._003C_003Ec._003C_003E9._003CCreateBoard_003Eb__121_0));
        ListSlotsProvider listSlotsProvider = new ListSlotsProvider();
        listSlotsProvider.Init(this);
        for (int num6 = 0; num6 < levelDefinition.slots.Count; num6++)
        {
            LevelDefinition.SlotDefinition slotDefinition = levelDefinition.slots[num6];
            if (slotDefinition.hasHoleInSlot && !slotDefinition.isPartOfConveyor)
            {
                TilesSlotsProvider.Slot slot10 = new TilesSlotsProvider.Slot(slotDefinition.position, true);
                listSlotsProvider.AddSlot(slot10);
            }
        }
        for (int num7 = 0; num7 < conveyorBelts.conveyorBeltList.Count; num7++)
        {
            LevelDefinition.ConveyorBelt conveyorBelt = conveyorBelts.conveyorBeltList[num7];
            this.AddConveyorBelt(conveyorBelt, num7 + allPortals.Count);
            List<LevelDefinition.ConveyorBeltLinearSegment> segmentList = conveyorBelt.segmentList;
            for (int num8 = 0; num8 < segmentList.Count; num8++)
            {
                List<LevelDefinition.SlotDefinition> slotList = segmentList[num8].slotList;
                for (int num9 = 0; num9 < slotList.Count; num9++)
                {
                    LevelDefinition.SlotDefinition slotDefinition2 = slotList[num9];
                    TilesSlotsProvider.Slot slot11 = new TilesSlotsProvider.Slot(slotDefinition2.position, true);
                    listSlotsProvider.AddSlot(slot11);
                }
            }
        }
        bool flag = listSlotsProvider.allSlots.Count > 0;
        GGUtil.SetActive(this.conveyorSlotsRenderer, flag);
        GGUtil.SetActive(this.conveyorBorderRenderer, flag);
        GGUtil.SetActive(this.conveyorHoleRenderer, flag);
        if (flag)
        {
            if (this.conveyorSlotsRenderer != null)
            {
                this.conveyorSlotsRenderer.ShowSlotsOnLevel(listSlotsProvider);
            }
            if (this.conveyorBorderRenderer != null)
            {
                this.conveyorBorderRenderer.ShowBorderOnLevel(listSlotsProvider);
            }
            if (this.conveyorHoleRenderer != null)
            {
                this.conveyorHoleRenderer.ShowBorder(listSlotsProvider);
            }
        }
        this.goals.Init(levelDefinition, this);
        this.extraFallingChips.Init(levelDefinition.extraFallingElements);
        this.input.Init(this);
        this.SetHasBounceOnAllChips(false);
        if (this.chocolateBorderRenderer != null)
        {
            this.chocolateBorderRenderer.DisplayChocolate(this);
        }
        foreach (Slot slot12 in slots)
        {
            if (slot12 != null)
            {
                Chip slotComponent2 = slot12.GetSlotComponent<Chip>();
                if (slotComponent2 != null)
                {
                    slotComponent2.UpdateFeatherShow();
                }
            }
        }
        if (this.hiddenElementBorderRenderer != null)
        {
            this.hiddenElementBorderRenderer.Render(this);
        }
        if (this.snowCoverRenderer != null)
        {
            this.snowCoverRenderer.Render(this);
        }
        if (this.bubbleSlotsBorderRenderer != null)
        {
            this.bubbleSlotsBorderRenderer.Render(this);
        }
        BubblesBoardComponent bubblesBoardComponent = new BubblesBoardComponent();
        bubblesBoardComponent.Init(this);
        this.board.bubblesBoardComponent = bubblesBoardComponent;
        this.board.Add(bubblesBoardComponent);
    }

    private void AddSlotColorSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        SlotColorSlate slotColorSlate = new SlotColorSlate();
        slotColorSlate.Init(slotDef.colorSlateLevel);
        if (this.isHudEnabled)
        {
            MultiLayerItemBehaviour multiLayerItemBehaviour = this.GetPool(PieceType.SlotColorSlate).Next<MultiLayerItemBehaviour>(false);
            multiLayerItemBehaviour.Init(ChipType.PickupGrass, slotDef.colorSlateLevel - 1);
            multiLayerItemBehaviour.SetPattern(slot);
            GGUtil.SetActive(multiLayerItemBehaviour, true);
            slotColorSlate.Add(multiLayerItemBehaviour);
            TransformBehaviour component = multiLayerItemBehaviour.GetComponent<TransformBehaviour>();
            component.localPosition = slot.localPositionOfCenter;
            slotColorSlate.Add(component);
        }
        slot.AddComponent(slotColorSlate);
    }

    public void AddBurriedElementSlot(Slot slot, LevelDefinition.BurriedElement burriedElement)
    {
        if (slot == null)
        {
            return;
        }
        SlotBurriedElement slotBurriedElement = new SlotBurriedElement();
        slotBurriedElement.Init(burriedElement);
        if (this.isHudEnabled)
        {
            BurriedElementBehaviour burriedElementBehaviour = this.GetPool(PieceType.BurriedBunny).Next<BurriedElementBehaviour>(false);
            slotBurriedElement.Add(burriedElementBehaviour);
            GGUtil.SetActive(burriedElementBehaviour, true);
            burriedElementBehaviour.Init(burriedElement);
            TransformBehaviour component = burriedElementBehaviour.GetComponent<TransformBehaviour>();
            component.localPosition = slot.localPositionOfCenter;
            slotBurriedElement.Add(component);
        }
        slot.AddComponent(slotBurriedElement);
    }

    private void AddConveyorBelt(LevelDefinition.ConveyorBelt conveyorBelt, int index)
    {
        ConveyorBeltBehaviour conveyorBeltBehaviour = this.GetPool(PieceType.ConveyorBelt).Next<ConveyorBeltBehaviour>(false);
        conveyorBeltBehaviour.Init(this, conveyorBelt, index);
        GGUtil.SetActive(conveyorBeltBehaviour, true);
        List<LevelDefinition.ConveyorBeltLinearSegment> segmentList = conveyorBelt.segmentList;
        for (int i = 0; i < segmentList.Count; i++)
        {
            List<LevelDefinition.SlotDefinition> slotList = segmentList[i].slotList;
            for (int j = 0; j < slotList.Count; j++)
            {
                LevelDefinition.SlotDefinition slotDefinition = slotList[j];
                Slot slot = this.GetSlot(slotDefinition.position);
                if (slot != null && slot.GetSlotComponent<EmptyConveyorSpace>() == null)
                {
                    this.CreateConveyorBeltPlate(slot);
                }
            }
        }
        ConveyorBeltBoardComponent conveyorBeltBoardComponent = new ConveyorBeltBoardComponent();
        conveyorBeltBoardComponent.Init(this, conveyorBelt, conveyorBeltBehaviour);
        this.board.Add(conveyorBeltBoardComponent);
    }

    private void AddBckLightToSlot(Slot slot)
    {
        slot.backLight = new LightSlotComponent();
        if (this.isHudEnabled)
        {
            SlotLightBehaviour slotLightBehaviour = this.GetPool(PieceType.SlotBackLight).Next<SlotLightBehaviour>(false);
            slotLightBehaviour.transform.localPosition = slot.localPositionOfCenter;
            slotLightBehaviour.InitWithSlotComponent(slot.backLight);
        }
    }

    private void AddLightToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        if (this.isHudEnabled)
        {
            SlotLightBehaviour slotLightBehaviour = this.GetPool(PieceType.SlotLight).Next<SlotLightBehaviour>(false);
            slotLightBehaviour.transform.localPosition = slot.localPositionOfCenter;
            slotLightBehaviour.Init(slot, slot.isBackgroundPatternActive && slotDef.chipType != ChipType.EmptyConveyorSpace && !slotDef.isPartOfConveyor);
            return;
        }
        slot.AddComponent(new LightSlotComponent());
    }

    private void AddJumpRampToSlot(Slot slot)
    {
        if (!this.isHudEnabled)
        {
            return;
        }
        List<IntVector2> forceDirections = slot.gravity.forceDirections;
        for (int i = 0; i < forceDirections.Count; i++)
        {
            IntVector2 direction = forceDirections[i];
            this.GetPool(PieceType.JumpRamp).Next<JumpRampBehaviour>(false).Init(slot.localPositionOfCenter, direction);
        }
    }

    private void AddFallingElementExitToSlot(Slot slot)
    {
        if (!this.isHudEnabled)
        {
            return;
        }
        List<IntVector2> forceDirections = slot.gravity.forceDirections;
        for (int i = 0; i < forceDirections.Count; i++)
        {
            IntVector2 direction = forceDirections[i];
            this.GetPool(PieceType.FallingElementExit).Next<JumpRampBehaviour>(false).Init(slot.localPositionOfCenter, direction);
        }
    }

    private void AddWallToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        this.wallDirections.Clear();
        LevelDefinition.WallSettings wallSettings = slotDef.wallSettings;
        if (wallSettings.left)
        {
            this.wallDirections.Add(IntVector2.left);
        }
        if (wallSettings.right)
        {
            this.wallDirections.Add(IntVector2.right);
        }
        if (wallSettings.up)
        {
            this.wallDirections.Add(IntVector2.up);
        }
        if (wallSettings.down)
        {
            this.wallDirections.Add(IntVector2.down);
        }
        for (int i = 0; i < this.wallDirections.Count; i++)
        {
            IntVector2 direction = this.wallDirections[i];
            WallBlocker wallBlocker = new WallBlocker();
            wallBlocker.Init(direction);
            if (!this.initParams.isHudDissabled)
            {
                WallBehaviour wallBehaviour = this.GetPool(PieceType.WallBlocker).Next<WallBehaviour>(false);
                wallBehaviour.transform.localPosition = slot.localPositionOfCenter;
                wallBehaviour.Init(direction);
                TransformBehaviour component = wallBehaviour.GetComponent<TransformBehaviour>();
                if (component != null)
                {
                    wallBlocker.Add(component);
                }
            }
            slot.AddComponent(wallBlocker);
        }
    }

    private void AddChainToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        NetMatchOrDestroyNextToLock netMatchOrDestroyNextToLock = new NetMatchOrDestroyNextToLock();
        NetMatchOrDestroyNextToLock.InitProperties initProperties = new NetMatchOrDestroyNextToLock.InitProperties
        {
            sortingOrder = 50,
            level = slotDef.chainLevel,
            chipType = ChipType.Chain,
            isMoveIntoSlotSuspended = true,
            isDestroyByMatchingNeighborSuspended = true,
            isAttachGrowingElementSuspended = true,
            wobbleSettings = Match3Settings.instance.chipWobbleSettings,
            useSound = true,
            soundType = GGSoundSystem.SFXType.BreakChain
        };
        netMatchOrDestroyNextToLock.Init(initProperties);
        if (!this.initParams.isHudDissabled)
        {
            MultiLayerItemBehaviour multiLayerItemBehaviour = this.GetPool(PieceType.MultiLayerItem, ChipType.Chain).Next<MultiLayerItemBehaviour>(false);
            multiLayerItemBehaviour.transform.localPosition = slot.localPositionOfCenter;
            multiLayerItemBehaviour.Init(ChipType.Chain, initProperties.level - 1);
            TransformBehaviour component = multiLayerItemBehaviour.GetComponent<TransformBehaviour>();
            if (component != null)
            {
                netMatchOrDestroyNextToLock.Add(component);
            }
            netMatchOrDestroyNextToLock.Add(multiLayerItemBehaviour);
        }
        slot.AddComponent(netMatchOrDestroyNextToLock);
    }

    private void AddIceToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        if (slot.GetSlotComponent<Chip>() == null)
        {
            return;
        }
        IceBlocker iceBlocker = new IceBlocker();
        IceBlocker.InitProperties initProperties = default(IceBlocker.InitProperties);
        initProperties.level = slotDef.iceLevel;
        initProperties.sortingOrder = 20;
        initProperties.chip = slot.GetSlotComponent<Chip>();
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.MultiLayerItem, ChipType.IceOnChip).Next<TransformBehaviour>(false);
            if (transformBehaviour != null)
            {
                iceBlocker.Add(transformBehaviour);
                transformBehaviour.localPosition = slot.localPositionOfCenter;
                GGUtil.SetActive(transformBehaviour, true);
            }
        }
        iceBlocker.Init(initProperties);
        slot.AddComponent(iceBlocker);
    }

    public void AddIceToSlot(Slot slot, int iceLevel)
    {
        if (slot.GetSlotComponent<Chip>() == null)
        {
            return;
        }
        IceBlocker iceBlocker = new IceBlocker();
        IceBlocker.InitProperties initProperties = default(IceBlocker.InitProperties);
        initProperties.level = iceLevel;
        initProperties.sortingOrder = 20;
        initProperties.chip = slot.GetSlotComponent<Chip>();
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.MultiLayerItem, ChipType.IceOnChip).Next<TransformBehaviour>(false);
            if (transformBehaviour != null)
            {
                iceBlocker.Add(transformBehaviour);
                transformBehaviour.localPosition = slot.localPositionOfCenter;
                GGUtil.SetActive(transformBehaviour, true);
            }
        }
        iceBlocker.Init(initProperties);
        slot.AddComponent(iceBlocker);
    }

    private void AddBasketToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        if (slotDef.chipType == ChipType.EmptyChipSlot)
        {
            this.CreateMovingElementInSlot(slot);
        }
        BasketBlocker basketBlocker = new BasketBlocker();
        BasketBlocker.InitProperties initProperties = new BasketBlocker.InitProperties
        {
            level = slotDef.basketLevel,
            sortingOrder = 30,
            canFallthroughPickup = true
        };
        basketBlocker.Init(initProperties);
        bool hasEmptyChip = slotDef.chipType == ChipType.EmptyChipSlot;
        if (this.isHudEnabled)
        {
            ChipType chipType = ChipType.BasketBlocker;
            if (slotDef.chipType == ChipType.BunnyChip)
            {
                chipType = ChipType.BasetBlockerWithBunny;
            }
            MultiLayerItemBehaviour multiLayerItemBehaviour = this.GetPool(PieceType.MultiLayerItem, ChipType.BasketBlocker).Next<MultiLayerItemBehaviour>(false);
            multiLayerItemBehaviour.transform.localPosition = slot.localPositionOfCenter;
            multiLayerItemBehaviour.Init(chipType, initProperties.level - 1);
            basketBlocker.Add(multiLayerItemBehaviour);
            multiLayerItemBehaviour.SetHasEmptyChip(hasEmptyChip);
            TransformBehaviour component = multiLayerItemBehaviour.GetComponent<TransformBehaviour>();
            if (component != null)
            {
                basketBlocker.Add(component);
            }
        }
        slot.AddComponent(basketBlocker);
    }

    public void AddBubblesToSlot(Slot slot)
    {
        BubblesPieceBlocker bubblesPieceBlocker = new BubblesPieceBlocker();
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.BubblePiece).Next<TransformBehaviour>(false);
            transformBehaviour.transform.localPosition = slot.localPositionOfCenter;
            GGUtil.Show(transformBehaviour);
            bubblesPieceBlocker.Add(transformBehaviour);
        }
        slot.AddComponent(bubblesPieceBlocker);
    }

    public void AddSnowCoverToSlot(Slot slot)
    {
        SnowCover snowCover = new SnowCover();
        snowCover.Init(new SnowCover.InitProperties
        {
            sortingOrder = 110,
            wobbleSettings = Match3Settings.instance.chipWobbleSettings
        });
        slot.AddComponent(snowCover);
    }

    private void AddBoxToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef, LevelDefinition level)
    {
        NetMatchOrDestroyNextToLock netMatchOrDestroyNextToLock = new NetMatchOrDestroyNextToLock();
        NetMatchOrDestroyNextToLock.InitProperties initProperties = default(NetMatchOrDestroyNextToLock.InitProperties);
        initProperties.level = slotDef.boxLevel;
        initProperties.sortingOrder = 100;
        initProperties.isMoveIntoSlotSuspended = true;
        initProperties.canFallthroughPickup = false;
        initProperties.isAttachGrowingElementSuspended = true;
        initProperties.isSlotMatchingSuspended = true;
        initProperties.isAvailableForDiscoBombSuspended = true;
        initProperties.isBlockingBurriedElement = true;
        initProperties.chipType = ChipType.Box;
        if (slotDef.chipType == ChipType.EmptyChipSlot || slotDef.chipType == ChipType.Unknown)
        {
            if (slotDef.colorSlateLevel == 1 && level.burriedElements.HasElementsUnderPosition(slotDef.position))
            {
                initProperties.SetDisplayChipType(ChipType.BoxWithBurriedElement);
            }
            else
            {
                initProperties.SetDisplayChipType(ChipType.BoxEmpty);
            }
        }
        initProperties.wobbleSettings = Match3Settings.instance.chipWobbleSettings;
        initProperties.useSound = true;
        initProperties.soundType = GGSoundSystem.SFXType.BreakBox;
        netMatchOrDestroyNextToLock.Init(initProperties);
        if (this.isHudEnabled)
        {
            MultiLayerItemBehaviour multiLayerItemBehaviour = this.GetPool(PieceType.MultiLayerItem, ChipType.Box).Next<MultiLayerItemBehaviour>(false);
            multiLayerItemBehaviour.transform.localPosition = slot.localPositionOfCenter;
            if (!multiLayerItemBehaviour.HasChipType(initProperties.displayChipType))
            {
                initProperties.SetDisplayChipType(ChipType.Box);
            }
            multiLayerItemBehaviour.Init(initProperties.displayChipType, initProperties.level - 1);
            netMatchOrDestroyNextToLock.Add(multiLayerItemBehaviour);
            TransformBehaviour component = multiLayerItemBehaviour.GetComponent<TransformBehaviour>();
            if (component != null)
            {
                netMatchOrDestroyNextToLock.Add(component);
            }
        }
        slot.AddComponent(netMatchOrDestroyNextToLock);
    }

    private void AddNetToSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        NetMatchOrDestroyNextToLock netMatchOrDestroyNextToLock = new NetMatchOrDestroyNextToLock();
        netMatchOrDestroyNextToLock.Init(new NetMatchOrDestroyNextToLock.InitProperties
        {
            level = 1,
            sortingOrder = 50,
            isMoveIntoSlotSuspended = true,
            canFallthroughPickup = true,
            isSlotMatchingSuspended = false,
            isAvailableForDiscoBombSuspended = false,
            isAttachGrowingElementSuspended = true,
            chipType = ChipType.Box
        });
        SlotComponent slotComponent = netMatchOrDestroyNextToLock;
        if (this.isHudEnabled)
        {
            NetBehaviour netBehaviour = this.GetPool(PieceType.Net).Next<NetBehaviour>(false);
            netBehaviour.transform.localPosition = slot.localPositionOfCenter;
            slotComponent.Add(netBehaviour);
            GGUtil.Show(netBehaviour);
            TransformBehaviour component = netBehaviour.GetComponent<TransformBehaviour>();
            if (component != null)
            {
                slotComponent.Add(component);
            }
        }
        slot.AddComponent(slotComponent);
    }

    private void AddSpecialGenerator(Slot slot)
    {
        if (!this.isHudEnabled)
        {
            return;
        }
        ComponentPool pool = this.GetPool(PieceType.SpecialGenerator);
        if (pool == null)
        {
            return;
        }
        SpecialGeneratorBehaviour specialGeneratorBehaviour = pool.Next<SpecialGeneratorBehaviour>(false);
        if (specialGeneratorBehaviour == null)
        {
            return;
        }
        specialGeneratorBehaviour.transform.localPosition = slot.localPositionOfCenter;
        specialGeneratorBehaviour.Init(slot.generatorSlotSettings);
        GGUtil.Show(specialGeneratorBehaviour);
    }

    public GrowingElementChip CreateGrowingElement(Slot slot, LevelDefinition.SlotDefinition slotDefinition)
    {
        GrowingElementChip growingElementChip = new GrowingElementChip();
        growingElementChip.Init(slotDefinition.itemColor);
        if (this.isHudEnabled)
        {
            GrowingElementBehaviour growingElementBehaviour = this.GetPool(PieceType.Chip, ChipType.GrowingElement).Next<GrowingElementBehaviour>(false);
            growingElementBehaviour.Init();
            growingElementChip.Add(growingElementBehaviour);
            growingElementBehaviour.transform.localPosition = slot.localPositionOfCenter;
            TransformBehaviour component = growingElementBehaviour.GetComponent<TransformBehaviour>();
            growingElementChip.Add(component);
        }
        slot.AddComponent(growingElementChip);
        return growingElementChip;
    }

    public TransformBehaviour CreatePieceTypeBehaviour(ChipType chipType)
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        ComponentPool pool = this.GetPool(PieceType.PowerupChip, chipType);
        if (pool == null)
        {
            UnityEngine.Debug.LogError("NO POOL FOR " + chipType);
            return null;
        }
        TransformBehaviour transformBehaviour = pool.Next<TransformBehaviour>(false);
        if (transformBehaviour != null)
        {
            SolidPieceRenderer component = transformBehaviour.GetComponent<SolidPieceRenderer>();
            if (component != null)
            {
                component.Init(chipType);
            }
        }
        GGUtil.SetActive(transformBehaviour, true);
        return transformBehaviour;
    }

    public TransformBehaviour CreateGrowingElementPieceBehaviour()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        TransformBehaviour transformBehaviour = this.GetPool(PieceType.Chip, ChipType.GrowingElementPiece).Next<TransformBehaviour>(false);
        GGUtil.SetActive(transformBehaviour, true);
        return transformBehaviour;
    }

    public MonsterElementBehaviour CreateMonsterElementBehaviour()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        MonsterElementBehaviour monsterElementBehaviour = this.GetPool(PieceType.MonsterElement).Next<MonsterElementBehaviour>(false);
        GGUtil.SetActive(monsterElementBehaviour, true);
        return monsterElementBehaviour;
    }

    public FlyingSaucerBehaviour CreateFlyingSaucer()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        FlyingSaucerBehaviour flyingSaucerBehaviour = this.GetPool(PieceType.FlyingSaucer).Next<FlyingSaucerBehaviour>(false);
        GGUtil.SetActive(flyingSaucerBehaviour, true);
        return flyingSaucerBehaviour;
    }

    public void CreateEmptyConveyorSpace(Slot slot)
    {
        if (slot == null)
        {
            return;
        }
        EmptyConveyorSpace c = new EmptyConveyorSpace();
        slot.AddComponent(c);
    }

    public void CreateConveyorBeltPlate(Slot slot)
    {
        ConveyorBeltPlate conveyorBeltPlate = new ConveyorBeltPlate();
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.ConveyorBeltPlate).Next<TransformBehaviour>(false);
            conveyorBeltPlate.Add(transformBehaviour);
            transformBehaviour.localPosition = slot.localPositionOfCenter;
            GGUtil.SetActive(transformBehaviour, true);
        }
        slot.AddComponent(conveyorBeltPlate);
    }

    public MagicHat CreateMagicHat(Slot slot)
    {
        MagicHat magicHat = new MagicHat();
        MagicHatBehaviour hatBehaviour = null;
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.Chip, ChipType.MagicHat).Next<TransformBehaviour>(false);
            hatBehaviour = transformBehaviour.GetComponent<MagicHatBehaviour>();
            magicHat.Add(transformBehaviour);
            transformBehaviour.localPosition = slot.localPositionOfCenter;
            GGUtil.SetActive(transformBehaviour, true);
        }
        magicHat.Init(hatBehaviour);
        slot.AddComponent(magicHat);
        return magicHat;
    }

    public MagicHatBomb CreateMagicHatBomb(Slot slot, LevelDefinition.SlotDefinition slotDefinition)
    {
        MagicHatBomb magicHatBomb = new MagicHatBomb();
        MagicHatBehaviour hatBehaviour = null;
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.Chip, ChipType.MagicHatBomb).Next<TransformBehaviour>(false);
            hatBehaviour = transformBehaviour.GetComponent<MagicHatBehaviour>();
            magicHatBomb.Add(transformBehaviour);
            transformBehaviour.localPosition = slot.localPositionOfCenter;
            GGUtil.SetActive(transformBehaviour, true);
        }
        magicHatBomb.Init(hatBehaviour, slotDefinition.magicHatItemsCount, slotDefinition.chipType);
        slot.AddComponent(magicHatBomb);
        return magicHatBomb;
    }

    public HammerAnimationBehaviour CreateHammerAnimationBehaviour()
    {
        ComponentPool pool = this.GetPool(PieceType.HammerAnimation);
        if (pool == null)
        {
            return null;
        }
        HammerAnimationBehaviour hammerAnimationBehaviour = pool.Next<HammerAnimationBehaviour>(false);
        GGUtil.SetActive(hammerAnimationBehaviour, true);
        return hammerAnimationBehaviour;
    }

    public CarpetSpreadBehaviour CreateCarpetSpread()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        return this.GetPool(PieceType.CarpetSpread).Next<CarpetSpreadBehaviour>(false);
    }

    public BurriedElementBehaviour CreateBurriedElement()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        return this.GetPool(PieceType.BurriedBunny).Next<BurriedElementBehaviour>(false);
    }

    public Chip CreateFallingElement(Slot slot, ChipType chipType)
    {
        Chip chip = new Chip();
        chip.Init(chipType, ItemColor.Unknown);
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.Chip, chipType).Next<TransformBehaviour>(false);
            chip.Add(transformBehaviour);
            chip.SetTransformToMove(transformBehaviour.transform);
            transformBehaviour.localPosition = slot.localPositionOfCenter;
            GGUtil.SetActive(transformBehaviour, true);
        }
        slot.AddComponent(chip);
        return chip;
    }

    public RockBlocker CreateRockBlocker(Slot slot, LevelDefinition.SlotDefinition slotDefinition)
    {
        RockBlocker rockBlocker = new RockBlocker();
        RockBlocker.InitArguments initArguments = default(RockBlocker.InitArguments);
        initArguments.sortingOrder = 10;
        if (slotDefinition.chipType == ChipType.SnowRockBlocker)
        {
            initArguments.sortingOrder = 200;
            initArguments.cancelsSnow = true;
        }
        initArguments.level = slotDefinition.itemLevel;
        rockBlocker.Init(initArguments);
        ChipType chipType = ChipType.RockBlocker;
        if (this.isHudEnabled)
        {
            MultiLayerItemBehaviour multiLayerItemBehaviour = this.GetPool(PieceType.MultiLayerItem, chipType).Next<MultiLayerItemBehaviour>(false);
            int itemLevel = slotDefinition.itemLevel;
            multiLayerItemBehaviour.Init(chipType, itemLevel);
            rockBlocker.Add(multiLayerItemBehaviour);
            TransformBehaviour component = multiLayerItemBehaviour.GetComponent<TransformBehaviour>();
            rockBlocker.Add(component);
            component.localPosition = slot.localPositionOfCenter;
            GGUtil.SetActive(multiLayerItemBehaviour, true);
        }
        slot.AddComponent(rockBlocker);
        return rockBlocker;
    }

    public Chip CreateCharacterInSlot(Slot slot, ChipType chipType, int itemLevel)
    {
        Chip chip = new Chip();
        chip.Init(chipType, ItemColor.Unknown);
        chip.itemLevel = itemLevel;
        if (this.isHudEnabled)
        {
            MultiLayerItemBehaviour multiLayerItemBehaviour = this.GetPool(PieceType.MultiLayerItem, chipType).Next<MultiLayerItemBehaviour>(false);
            multiLayerItemBehaviour.Init(chipType, itemLevel);
            chip.Add(multiLayerItemBehaviour);
            TransformBehaviour component = multiLayerItemBehaviour.GetComponent<TransformBehaviour>();
            chip.Add(component);
            chip.SetTransformToMove(component.transform);
            component.localPosition = slot.localPositionOfCenter;
            GGUtil.SetActive(multiLayerItemBehaviour, true);
        }
        slot.AddComponent(chip);
        return chip;
    }

    public LightingBolt CreateLightingBolt()
    {
        return this.GetPool(PieceType.LightingBolt).Next<LightingBolt>(false);
    }

    public TransformBehaviour CreateChipFeather(Slot slot, ItemColor itemColor)
    {
        TransformBehaviour transformBehaviour = this.GetPool(PieceType.ChipFeather).Next<TransformBehaviour>(false);
        transformBehaviour.localPosition = slot.localPositionOfCenter;
        ChipFeatherBehaviour component = transformBehaviour.GetComponent<ChipFeatherBehaviour>();
        if (component != null)
        {
            component.Init(itemColor);
        }
        GGUtil.SetActive(transformBehaviour, true);
        return transformBehaviour;
    }

    public LightingBolt CreateLightingBoltChip()
    {
        return this.GetPool(PieceType.LightingBoltChip).Next<LightingBolt>(false);
    }

    public LightingBolt CreateLightingBoltPowerup()
    {
        return this.GetPool(PieceType.LightingBoltPowerup).Next<LightingBolt>(false);
    }

    public Chip CreateMoreMovesChip(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        return this.CreateCharacterInSlot(slot, slotDef.chipType, slotDef.itemLevel);
    }

    public Chip CreateCharacterInSlot(Slot slot, LevelDefinition.SlotDefinition slotDef)
    {
        return this.CreateCharacterInSlot(slot, slotDef.chipType, slotDef.itemLevel);
    }

    public void AddPipe(Slot slot, bool isExit, int index)
    {
        PipeBehaviour pipeBehaviour = this.GetPool(PieceType.Pipe).Next<PipeBehaviour>(false);
        pipeBehaviour.Init(slot, isExit);
        pipeBehaviour.SetColor(Match3Settings.instance.pipeSettings.GetColor(index));
        if (isExit)
        {
            slot.exitPipe = pipeBehaviour;
        }
        else
        {
            slot.entrancePipe = pipeBehaviour;
        }
        GGUtil.SetActive(pipeBehaviour, true);
    }

    public PipeBehaviour CreatePipeDontAddToSlot()
    {
        PipeBehaviour pipeBehaviour = this.GetPool(PieceType.Pipe).Next<PipeBehaviour>(false);
        GGUtil.SetActive(pipeBehaviour, true);
        return pipeBehaviour;
    }

    public MovingElement CreateMovingElementInSlot(Slot slot)
    {
        MovingElement movingElement = new MovingElement();
        if (this.isHudEnabled)
        {
            TransformBehaviour transformBehaviour = this.GetPool(PieceType.MovingElement).Next<TransformBehaviour>(false);
            transformBehaviour.localPosition = slot.localPositionOfCenter;
            movingElement.Add(transformBehaviour);
        }
        slot.AddComponent(movingElement);
        return movingElement;
    }

    public TransformBehaviour CreateCoin()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        ComponentPool pool = this.GetPool(PieceType.Coin);
        if (pool == null)
        {
            return null;
        }
        return pool.Next<TransformBehaviour>(false);
    }

    public TransformBehaviour CreatePointsDisplay()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        ComponentPool pool = this.GetPool(PieceType.PointsDisplay);
        if (pool == null)
        {
            return null;
        }
        return pool.Next<TransformBehaviour>(false);
    }

    public Chip CreateChipInSlot(Slot slot, ChipType chipType, ItemColor itemColor)
    {
        Chip chip = new Chip();
        chip.Init(chipType, itemColor);
        chip.lastConnectedSlot = slot;
        if (this.isHudEnabled)
        {
            ChipBehaviour chipBehaviour = this.GetPool(PieceType.Chip, chipType).Next<ChipBehaviour>(false);
            chipBehaviour.Init(chip);
            chip.Add(chipBehaviour);
            TransformBehaviour component = chipBehaviour.GetComponent<TransformBehaviour>();
            chip.Add(component);
            GGUtil.SetActive(component, true);
            if (chipBehaviour != null)
            {
                chipBehaviour.ResetVisually();
            }
            component.localPosition = slot.localPositionOfCenter;
        }
        slot.AddComponent(chip);
        return chip;
    }

    public Chip CreateChipInSlot(Slot slot, ItemColor itemColor)
    {
        Chip chip = new Chip();
        chip.Init(ChipType.Chip, itemColor);
        chip.lastConnectedSlot = slot;
        if (this.isHudEnabled)
        {
            ChipBehaviour chipBehaviour = this.GetPool(PieceType.Chip).Next<ChipBehaviour>(false);
            chipBehaviour.Init(chip);
            chip.Add(chipBehaviour);
            TransformBehaviour component = chipBehaviour.GetComponent<TransformBehaviour>();
            chip.Add(component);
            ChipBehaviour componentBehaviour = chip.GetComponentBehaviour<ChipBehaviour>();
            if (componentBehaviour != null)
            {
                componentBehaviour.gameObject.SetActive(true);
                component.localPosition = slot.localPositionOfCenter;
                componentBehaviour.ResetVisually();
            }
        }
        slot.AddComponent(chip);
        return chip;
    }

    public Chip CreatePowerupInSlot(Slot slot, ChipType chipType)
    {
        Chip chip = new Chip();
        chip.Init(chipType, ItemColor.Unknown);
        if (this.isHudEnabled)
        {
            SolidPieceRenderer solidPieceRenderer = this.GetPool(PieceType.PowerupChip, chipType).Next<SolidPieceRenderer>(false);
            solidPieceRenderer.Init(chip);
            chip.Add(solidPieceRenderer);
            TransformBehaviour component = solidPieceRenderer.GetComponent<TransformBehaviour>();
            chip.Add(component);
            component.localPosition = slot.localPositionOfCenter;
            solidPieceRenderer.gameObject.SetActive(true);
            solidPieceRenderer.ResetVisually();
        }
        slot.AddComponent(chip);
        return chip;
    }

    public GameObject CreateParticles(Chip chip, PieceType pieceType, ChipType chipType, ItemColor itemColor)
    {
        ComponentPool pool = this.GetPool(pieceType, chipType, itemColor);
        if (pool == null)
        {
            return null;
        }
        GameObject gameObject = pool.Next(true);
        if (gameObject == null)
        {
            return null;
        }
        TransformBehaviour componentBehaviour = chip.GetComponentBehaviour<TransformBehaviour>();
        if (componentBehaviour == null)
        {
            return gameObject;
        }
        gameObject.transform.localPosition = componentBehaviour.localPosition;
        return gameObject;
    }

    public RocketPieceBehaviour CreateRocketPiece()
    {
        if (!this.isHudEnabled)
        {
            return null;
        }
        RocketPieceBehaviour rocketPieceBehaviour = this.GetPool(PieceType.RocketPiece).Next<RocketPieceBehaviour>(false);
        rocketPieceBehaviour.Init();
        return rocketPieceBehaviour;
    }

    public bool isUserInteractionSuspended
    {
        get
        {
            return this.board.isInteractionSuspended || this.board.isInteractionSuspendedBecausePowerupAnimation || this.board.bubblesBoardComponent.isWaitingForBubblesToBurst;
        }
    }

    public bool isBubblesSuspended
    {
        get
        {
            return this.board.isGameEnded || this.board.isInteractionSuspended;
        }
    }

    public bool isConveyorSuspended
    {
        get
        {
            return this.board.isGameEnded || this.board.isInteractionSuspended || this.board.bubblesBoardComponent.isWaitingForBubblesToBurst;
        }
    }

    public bool isOutOfMoves
    {
        get
        {
            return this.gameScreen.stageState.MovesRemaining <= 0;
        }
    }

    public bool hasPlayedAnyMoves
    {
        get
        {
            return this.board.userMovesCount > 0;
        }
    }

    public bool isConveyorMoving
    {
        get
        {
            for (int i = 0; i < this.board.boardComponents.Count; i++)
            {
                ConveyorBeltBoardComponent conveyorBeltBoardComponent = this.board.boardComponents[i] as ConveyorBeltBoardComponent;
                if (conveyorBeltBoardComponent != null && conveyorBeltBoardComponent.isMoving)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool isBoardFullySettled
    {
        get
        {
            return this.board.currentMatchesCount == 0 && this.board.actionManager.ActionCount == 0 && !this.isAnySlotMoving;
        }
    }

    public bool isAnySlotMoving
    {
        get
        {
            List<Slot> sortedSlotsUpdateList = this.board.sortedSlotsUpdateList;
            for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
            {
                if (sortedSlotsUpdateList[i].isMoving)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void ApplySlotGravityForAllSlots()
    {
        List<Slot> sortedSlotsUpdateList = this.board.sortedSlotsUpdateList;
        for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
        {
            sortedSlotsUpdateList[i].ApplySlotGravity();
        }
    }

    private void Update()
    {
        if (this.initParams == null)
        {
            return;
        }
        if (!this.gameStarted || this.board.isUpdateSuspended)
        {
            if (this.animationEnum != null)
            {
                this.animationEnum.MoveNext();
            }
            return;
        }
        float num = Time.deltaTime;
        num *= this.initParams.timeScale;
        int num2 = Mathf.Max(1, this.initParams.iterations);
        for (int i = 0; i < num2; i++)
        {
            this.StepSymulation(num, i);
            if (this.board.isGameEnded)
            {
                break;
            }
        }
    }

    private void StepSymulation(float deltaTime, int iteration)
    {
        if (this.animationEnum != null)
        {
            this.animationEnum.MoveNext();
        }
        this.board.matchCounter.Update(deltaTime);
        this.board.isAnyConveyorMoveSuspended = true;
        if (this.board.isShufflingBoard)
        {
            this.UpdateBoardShuffle();
            return;
        }
        if (iteration == 0)
        {
            this.input.DoUpdate(deltaTime);
        }
        this.board.currentTime += deltaTime;
        this.board.currentDeltaTime = deltaTime;
        this.board.isDirtyInCurrentFrame = false;
        this.board.actionManager.OnUpdate(deltaTime);
        this.board.nonChangeStateActionMenager.OnUpdate(deltaTime);
        Slot[] slots = this.board.slots;
        if (slots == null)
        {
            return;
        }
        List<Slot> sortedSlotsUpdateList = this.board.sortedSlotsUpdateList;
        for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
        {
            sortedSlotsUpdateList[i].OnUpdate(deltaTime);
        }
        float num = 0f;
        foreach (Slot slot in slots)
        {
            if (slot != null)
            {
                num = Mathf.Max(slot.lastMoveTime, num);
            }
        }
        Matches matches = this.board.findMatches.FindAllMatches();
        PotentialMatches potentialMatches = this.board.potentialMatches;
        if (!this.settings.generalSettings.waitTillBoardStopsForMatches || !this.isAnySlotMoving)
        {
            this.ProcessMatches(matches, null);
        }
        this.board.currentMatchesCount = matches.islands.Count;
        potentialMatches.FindPotentialMatches(this);
        this.board.powerupCombines.Fill(this);
        this.board.powerupActivations.Fill(this);
        bool flag = this.board.powerupActivations.powerups.Count > 0 || this.board.powerupCombines.combines.Count > 0;
        this.board.isAnyConveyorMoveSuspended = false;
        bool flag2 = false;
        List<BoardComponent> boardComponents = this.board.boardComponents;
        for (int k = 0; k < boardComponents.Count; k++)
        {
            ConveyorBeltBoardComponent conveyorBeltBoardComponent = boardComponents[k] as ConveyorBeltBoardComponent;
            if (conveyorBeltBoardComponent != null)
            {
                this.board.moveCountWhenConveyorTookAction = Mathf.Max(this.board.moveCountWhenConveyorTookAction, conveyorBeltBoardComponent.lastMoveConveyorTookAction);
                if (conveyorBeltBoardComponent.IsMoveConveyorSuspended())
                {
                    this.board.isAnyConveyorMoveSuspended = true;
                    break;
                }
                if (conveyorBeltBoardComponent.needsToActivateConveyor)
                {
                    flag2 = true;
                }
            }
        }
        if (this.input.isActive)
        {
            this.board.isAnyConveyorMoveSuspended = true;
        }
        bool isConveyorMoving = this.isConveyorMoving;
        for (int l = 0; l < boardComponents.Count; l++)
        {
            boardComponents[l].Update(deltaTime);
        }
        this.board.burriedElements.Update(deltaTime);
        float num2 = this.board.currentTime - num;
        float num3 = this.board.currentTime - this.board.lastTimeWhenUserMadeMove;
        bool isConveyorMoving2 = this.isConveyorMoving;
        if (isConveyorMoving && !isConveyorMoving2)
        {
            matches = this.board.findMatches.FindAllMatches();
            this.board.currentMatchesCount = matches.islands.Count;
            potentialMatches.FindPotentialMatches(this);
            this.board.powerupCombines.Fill(this);
            this.board.powerupActivations.Fill(this);
        }
        this.CheckEndGameConditions();
        if (this.board.actionManager.ActionCount == 0 && !isConveyorMoving2 && !this.board.isShufflingBoard && matches.MatchesCount == 0 && potentialMatches.MatchesCount == 0 && !this.board.isGameEnded && !this.input.isActive && !flag && !this.isAnySlotMoving && !this.board.isGameEnded)
        {
            this.ShuffleBoard();
            this.board.currentFrameIndex += 1L;
            return;
        }
        bool flag3 = this.board.powerupCombines.combines.Count > 0;
        ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes potentialTimesAction = Match3Settings.instance.showPotentialMatchesSettings.GetPotentialTimesAction(this.showPotentialMatchSetting, flag3, this);
        float idleTimeBeforeShowMatch = potentialTimesAction.idleTimeBeforeShowMatch;
        float boardIdleTimeBeforeShowMatch = potentialTimesAction.boardIdleTimeBeforeShowMatch;
        bool flag4 = false;
        for (int m = 0; m < this.board.slots.Length; m++)
        {
            Slot slot2 = this.board.slots[m];
            if (slot2 != null && slot2.LockCount > 0)
            {
                flag4 = true;
                break;
            }
        }
        bool flag5 = this.board.actionManager.ActionCount == 0 && !this.isConveyorMoving && !this.board.isShufflingBoard && !this.isAnySlotMoving && !flag2 && !flag4 && matches.MatchesCount == 0;
        if (flag5)
        {
            int lastSettledMove = this.board.lastSettledMove;
            this.board.lastSettledMove = Mathf.Max(this.board.lastSettledMove, this.board.userMovesCount);
            if (this.board.lastSettledMove > lastSettledMove)
            {
                this.OnMoveSettled();
            }
        }
        this.board.isBoardSettled = flag5;
        this.UpdateTutorialMatches(flag5 && !this.board.isGameEnded);
        bool flag6 = this.board.powerupActivations.powerups.Count > 0;
        bool flag7 = potentialMatches.MatchesCount > 0 || flag3 || flag6;
        bool flag8 = num2 > boardIdleTimeBeforeShowMatch && num3 > idleTimeBeforeShowMatch && matches.MatchesCount == 0 && flag7 && this.board.actionManager.ActionCount == 0 && !this.showMatchAction.isAlive && !this.isUserInteractionSuspended && !this.input.isActive && !this.showMatchAction.isAlive && !this.board.isGameEnded && !this.board.isPowerupSelectionActive;
        if (this.initParams.isAIPlayer && (!this.isUserInteractionSuspended && this.board.actionManager.ActionCount == 0 && !this.isConveyorMoving && !this.board.isGameEnded && !this.board.isShufflingBoard && !this.isAnySlotMoving && !flag2 && !flag4 && matches.MatchesCount == 0))
        {
            this.aiPlayer.FindBestMove();
        }
        if (flag8)
        {
            ShowPotentialMatchAction.InitParams initParams = default(ShowPotentialMatchAction.InitParams);
            initParams.game = this;
            initParams.userMoveWhenShow = this.board.userMovesCount;
            initParams.movesCountWhenConveyorTookAction = this.board.moveCountWhenConveyorTookAction;
            this.matchTypeToFind.Clear();
            this.goals.FillSlotData(this);
            Match3Game.PotentialMatch potentialMatch = default(Match3Game.PotentialMatch);
            if (this.suggestMoveType == SuggestMoveType.MatchesWithoutPowerupCreate)
            {
                potentialMatch = this.GetMatchingPotentialMatchAction();
            }
            else if (this.suggestMoveType == SuggestMoveType.Normal)
            {
                potentialMatch = this.GetBestPotentialMatchAction();
            }
            else if (this.suggestMoveType == SuggestMoveType.GoodOnFirstAndLast2)
            {
                List<Match3Goals.GoalBase> activeGoals = this.goals.GetActiveGoals();
                int num4 = 0;
                for (int n = 0; n < activeGoals.Count; n++)
                {
                    Match3Goals.GoalBase goalBase = activeGoals[n];
                    if (goalBase.config.chipType != ChipType.FallingGingerbreadMan)
                    {
                        num4 += goalBase.RemainingCount;
                    }
                }
                if (this.board.userMovesCount == 1)
                {
                    potentialMatch = this.GetBestPotentialMatchAction();
                }
                else if (this.gameScreen.stageState.MovesRemaining <= 2 || num4 <= 2)
                {
                    potentialMatch = this.GetBestPotentialMatchAction();
                }
                else
                {
                    List<Match3Game.PotentialMatch> list = this.FillPotentialMatchesWithScore(true, true, true);
                    if (list.Count > 0)
                    {
                        potentialMatch = list[UnityEngine.Random.Range(0, list.Count)];
                    }
                }
            }
            else
            {
                this.potentialMatchesList.Clear();
                PowerupCombines.PowerupCombine powerupCombine = this.SelectPowerupCombine(this.goals, this.board.powerupCombines);
                if (powerupCombine != null)
                {
                    this.potentialMatchesList.Add(new Match3Game.PotentialMatch(powerupCombine, powerupCombine.GetActionScore(this, this.goals)));
                }
                PowerupActivations.PowerupActivation powerupActivation = this.SelectPowerupActivation(this.goals, this.board.powerupActivations);
                if (powerupActivation != null)
                {
                    this.potentialMatchesList.Add(new Match3Game.PotentialMatch(powerupActivation, powerupActivation.GetActionScore(this, this.goals)));
                }
                PotentialMatches.CompoundSlotsSet compoundSlotsSet = this.SelectPotentialMatch(this.goals, potentialMatches);
                if (compoundSlotsSet != null)
                {
                    this.potentialMatchesList.Add(new Match3Game.PotentialMatch(compoundSlotsSet, compoundSlotsSet.GetActionScore(this, this.goals), default(ActionScore)));
                }
                bool flag9 = false;
                for (int num5 = 0; num5 < this.potentialMatchesList.Count; num5++)
                {
                    Match3Game.PotentialMatch potentialMatch2 = this.potentialMatchesList[num5];
                    if (!flag9 || this.IsBetter(potentialMatch, potentialMatch2))
                    {
                        flag9 = true;
                        potentialMatch = potentialMatch2;
                    }
                }
            }
            if (potentialMatch.isActive)
            {
                initParams.powerupCombine = potentialMatch.powerupCombine;
                initParams.powerupActivation = potentialMatch.powerupActivation;
                initParams.potentialMatch = potentialMatch.potentialMatch;
                this.showMatchAction.Init(initParams);
                this.board.nonChangeStateActionMenager.AddAction(this.showMatchAction);
            }
        }
        if (this.chocolateBorderRenderer != null)
        {
            this.chocolateBorderRenderer.DisplayChocolate(this);
        }
        if (this.snowCoverRenderer != null)
        {
            this.snowCoverRenderer.Render(this);
        }
        if (this.hiddenElementBorderRenderer != null)
        {
            this.hiddenElementBorderRenderer.Render(this);
        }
        if (this.bubbleSlotsBorderRenderer != null)
        {
            this.bubbleSlotsBorderRenderer.Render(this);
        }
        this.board.currentFrameIndex += 1L;
    }

    private IEnumerator DoShuffleBoardAnimation()
    {
        return new Match3Game._003CDoShuffleBoardAnimation_003Ed__192(0)
        {
            _003C_003E4__this = this
        };
    }

    private void ShuffleBoard()
    {
        this.board.isShufflingBoard = true;
        this.shuffleBoardAnimation = this.DoShuffleBoardAnimation();
    }

    private void UpdateBoardShuffle()
    {
        if (this.shuffleBoardAnimation == null)
        {
            return;
        }
        this.shuffleBoardAnimation.MoveNext();
    }

    public Slot LastSlotOnDirection(Slot origin, IntVector2 direction)
    {
        if (origin == null)
        {
            return null;
        }
        if (direction.x == 0 && direction.y == 0)
        {
            return origin;
        }
        Slot result = origin;
        IntVector2 intVector = origin.position;
        for (; ; )
        {
            intVector += direction;
            if (this.board.IsOutOfBoard(intVector))
            {
                break;
            }
            Slot slot = this.GetSlot(intVector);
            if (slot != null)
            {
                result = slot;
            }
        }
        return result;
    }

    public Slot FirstSlotOnDirection(IntVector2 position, IntVector2 direction)
    {
        if (direction.x == 0 && direction.y == 0)
        {
            return null;
        }
        Slot slot = this.GetSlot(position);
        if (slot != null)
        {
            return slot;
        }
        IntVector2 intVector = position;
        Slot slot2;
        do
        {
            intVector += direction;
            if (this.board.IsOutOfBoard(intVector))
            {
                goto IL_44;
            }
            slot2 = this.GetSlot(intVector);
        }
        while (slot2 == null);
        return slot2;
        IL_44:
        return null;
    }

    public List<Slot> SlotsInDiscoBallDestroy(ItemColor itemColor, bool replaceWithBombs)
    {
        this.discoBallSlots.Clear();
        foreach (Slot slot in this.board.slots)
        {
            if (slot != null && slot.CanParticipateInDiscoBombAffectedArea(itemColor, replaceWithBombs))
            {
                this.discoBallSlots.Add(slot);
            }
        }
        return this.discoBallSlots;
    }

    private bool IsBetter(Match3Game.PotentialMatch a, Match3Game.PotentialMatch b)
    {
        int currentScoreFactor = 3;
        int goalsFactor = 2;
        if (this.gameScreen.stageState.MovesRemaining > 1)
        {
            int num = a.ScoreWithPowerupScore(currentScoreFactor, goalsFactor);
            int num2 = b.ScoreWithPowerupScore(currentScoreFactor, goalsFactor);
            if (num != num2)
            {
                return num2 > num;
            }
        }
        ActionScore actionScore = a.actionScore;
        ActionScore actionScore2 = b.actionScore;
        if (actionScore.goalsCount != actionScore2.goalsCount)
        {
            return actionScore2.goalsCount > actionScore.goalsCount;
        }
        if (actionScore2.obstaclesDestroyed != actionScore.obstaclesDestroyed)
        {
            return actionScore2.obstaclesDestroyed > actionScore.obstaclesDestroyed;
        }
        if (actionScore.powerupsCreated > 0 && actionScore2.powerupsCreated > 0)
        {
            return this.IsBetter(a.powerupCreatedScore, b.powerupCreatedScore);
        }
        return actionScore2.powerupsCreated > actionScore.powerupsCreated;
    }

    private bool IsBetter(ActionScore a, ActionScore b)
    {
        if (a.goalsCount != b.goalsCount)
        {
            return b.goalsCount > a.goalsCount;
        }
        if (b.obstaclesDestroyed != a.obstaclesDestroyed)
        {
            return b.obstaclesDestroyed > a.obstaclesDestroyed;
        }
        return b.powerupsCreated > a.powerupsCreated;
    }

    private PowerupActivations.PowerupActivation SelectPowerupActivation(Match3Goals goals, PowerupActivations powerupActivations)
    {
        List<PowerupActivations.PowerupActivation> powerups = powerupActivations.powerups;
        PowerupActivations.PowerupActivation powerupActivation = null;
        ActionScore a = default(ActionScore);
        for (int i = 0; i < powerups.Count; i++)
        {
            PowerupActivations.PowerupActivation powerupActivation2 = powerups[i];
            ActionScore actionScore = powerupActivation2.GetActionScore(this, goals);
            if (powerupActivation == null || this.IsBetter(a, actionScore))
            {
                a = actionScore;
                powerupActivation = powerupActivation2;
            }
        }
        return powerupActivation;
    }

    private PowerupCombines.PowerupCombine SelectPowerupCombine(Match3Goals goals, PowerupCombines powerupCombines)
    {
        List<PowerupCombines.PowerupCombine> combines = powerupCombines.combines;
        this.combineTypeToFind.Clear();
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DoubleDiscoBall);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DiscoBallBomb);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DiscoBallRocket);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DiscoBallSeekingMissle);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.RocketBomb);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DoubleBomb);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DoubleRocket);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DoubleSeekingMissle);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.BombSeekingMissle);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.RocketSeekingMissle);
        this.combineTypeToFind.Add(PowerupCombines.CombineType.DiscoBallColor);
        for (int i = 0; i < this.combineTypeToFind.Count; i++)
        {
            PowerupCombines.CombineType combineType = this.combineTypeToFind[i];
            List<PowerupCombines.PowerupCombine> list = powerupCombines.FilterCombines(combineType);
            bool flag = true;
            if (combineType == PowerupCombines.CombineType.DiscoBallColor)
            {
                flag = false;
                for (int j = 0; j < list.Count; j++)
                {
                    PowerupCombines.PowerupCombine powerupCombine = list[j];
                    Chip slotComponent = powerupCombine.exchangeSlot.GetSlotComponent<Chip>();
                    if (slotComponent.canFormColorMatches)
                    {
                        ItemColor itemColor = slotComponent.itemColor;
                        List<Slot> list2 = this.SlotsInDiscoBallDestroy(itemColor, false);
                        for (int k = 0; k < list2.Count; k++)
                        {
                            Slot slot = list2[k];
                            if (goals.IsDestroyingSlotCompleatingAGoal(slot, this, true))
                            {
                                return powerupCombine;
                            }
                        }
                    }
                }
            }
            if (list.Count > 0 && flag)
            {
                return list[0];
            }
        }
        return null;
    }

    private PotentialMatches.CompoundSlotsSet SelectPotentialMatch(Match3Goals goals, PotentialMatches potentialMatches)
    {
        if (this.suggestMoveType == SuggestMoveType.BombsFirst)
        {
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.Bomb);
        }
        if (this.suggestMoveType == SuggestMoveType.HorizontalVerticalMissleFirst)
        {
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.Rocket);
        }
        if (this.suggestMoveType == SuggestMoveType.SeekingMissleFirst)
        {
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.SeekingMissle);
        }
        if (this.suggestMoveType == SuggestMoveType.PreferBombs)
        {
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.DiscoBall);
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.Bomb);
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.Rocket);
            this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.SeekingMissle);
        }
        this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.DiscoBall);
        this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.CompleatingGoals);
        this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.SeekingMissle);
        this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.Rocket);
        this.matchTypeToFind.Add(PotentialMatches.CompoundSlotsSet.MatchType.Bomb);
        for (int i = 0; i < this.matchTypeToFind.Count; i++)
        {
            PotentialMatches.CompoundSlotsSet.MatchType matchType = this.matchTypeToFind[i];
            List<PotentialMatches.CompoundSlotsSet> list;
            if (matchType == PotentialMatches.CompoundSlotsSet.MatchType.CompleatingGoals)
            {
                list = potentialMatches.FilterForTypeCompleatingGoals(this);
            }
            else
            {
                list = potentialMatches.FilterForType(matchType);
            }
            if (list != null && list.Count > 0)
            {
                ActionScore a = default(ActionScore);
                PotentialMatches.CompoundSlotsSet compoundSlotsSet = null;
                for (int j = 0; j < list.Count; j++)
                {
                    PotentialMatches.CompoundSlotsSet compoundSlotsSet2 = list[j];
                    ActionScore actionScore = compoundSlotsSet2.GetActionScore(this, goals);
                    if (compoundSlotsSet == null || this.IsBetter(a, actionScore))
                    {
                        a = actionScore;
                        compoundSlotsSet = compoundSlotsSet2;
                    }
                }
                return compoundSlotsSet;
            }
        }
        List<PotentialMatches.CompoundSlotsSet> matchesList = potentialMatches.matchesList;
        if (matchesList.Count == 0)
        {
            return null;
        }
        return matchesList[AnimRandom.Range(0, matchesList.Count)];
    }

    private void CheckEndGameConditions()
    {
        if (this.board.ignoreEndConditions)
        {
            return;
        }
        bool flag = this.goals.isAllGoalsComplete;
        bool isOutOfMoves = this.isOutOfMoves;
        bool flag2 = this.board.currentTime - this.board.lastTimeWhenUserMadeMove > 0.1f;
        bool flag3 = this.board.currentMatchesCount == 0 && this.board.actionManager.ActionCount == 0 && !this.isAnySlotMoving;
        flag2 = flag3;
        if (Application.isEditor && UnityEngine.Input.GetKey(KeyCode.W))
        {
            flag = true;
            flag2 = true;
        }
        if (flag || isOutOfMoves)
        {
            this.board.isInteractionSuspended = true;
            this.board.isEndConditionReached = true;
            if (this.board.isGameEnded)
            {
                return;
            }
            if (flag)
            {
                if (flag2)
                {
                    this.board.isGameEnded = true;
                    this.EndGame(true);
                    return;
                }
            }
            else if (isOutOfMoves && flag3)
            {
                this.board.isGameEnded = true;
                this.EndGame(false);
                return;
            }
        }
        else if (this.board.isEndConditionReached)
        {
            this.board.isEndConditionReached = false;
            this.board.isInteractionSuspended = false;
        }
    }

    private void EndGame(bool isWin)
    {
        if (this.initParams.isAIPlayer && !isWin)
        {
            this.board.isGameEnded = false;
            this.board.isInteractionSuspended = false;
            this.board.additionalMoves += 10;
            return;
        }
        this.board.isGameEnded = true;
        bool isOutOfMoves = this.isOutOfMoves;
        this.board.isInteractionSuspended = true;
        this.endTimestampTicks = DateTime.UtcNow.Ticks;
        if (isWin)
        {
            GameCompleteParams gameCompleteParams = new GameCompleteParams();
            gameCompleteParams.isWin = true;
            gameCompleteParams.stageState = this.gameScreen.stageState;
            gameCompleteParams.game = this;
            UnityEngine.Debug.Log("MOVES " + this.board.userMovesCount);
            this.gameScreen.Match3GameCallback_OnGameWon(gameCompleteParams);

            // Test
            UnityEngine.Debug.Log("From test: " + this.board.userScore + " " + this.gameScreen.GetGameLevel());

            if (LoginToFBButton.ltfb.IsLogged)
            {
                ScoreController.PostScoreAsync(LoginToFBButton.ltfb.GetFacebookPlayer().Id, this.board.userScore, this.gameScreen.GetGameLevel());
            }

            

            return;
        }
       
        GameCompleteParams gameCompleteParams2 = new GameCompleteParams();
        gameCompleteParams2.isWin = false;
        gameCompleteParams2.stageState = this.gameScreen.stageState;
        gameCompleteParams2.game = this;
        this.gameScreen.Match3GameCallback_OnGameOutOfMoves(gameCompleteParams2);
    }

    public void ResumeGame()
    {
        this.board.isUpdateSuspended = false;
    }

    public void SuspendGameSounds()
    {
        this.board.isGameSoundsSuspended = true;
    }

    public void SuspendGame()
    {
        this.board.isUpdateSuspended = true;
    }

    public void QuitGame()
    {
        this.board.isGameEnded = true;
        this.board.isInteractionSuspended = true;
        this.endTimestampTicks = DateTime.UtcNow.Ticks;
    }

    public void ContinueGameAfterOffer(BuyMovesPricesConfig.OfferConfig offer)
    {
        this.board.isGameEnded = false;
        this.board.additionalMoves += offer.movesCount;
        this.board.isInteractionSuspended = false;
        this.board.isInteractionSuspended = false;
        this.moveOffersBought.Add(offer);
        float num = 0f;
        for (int i = 0; i < offer.powerups.Count; i++)
        {
            BuyMovesPricesConfig.OfferConfig.PowerupDefinition powerupDefinition = offer.powerups[i];
            for (int j = 0; j < powerupDefinition.count; j++)
            {
                PlacePowerupAction placePowerupAction = new PlacePowerupAction();
                placePowerupAction.Init(new PlacePowerupAction.Parameters
                {
                    game = this,
                    powerup = powerupDefinition.powerupType,
                    initialDelay = num,
                    internalAnimation = true
                });
                this.board.actionManager.AddAction(placePowerupAction);
                num += Match3Settings.instance.boosterPlaceDelay;
            }
        }
        this.gameScreen.powerupsPanel.ReinitPowerups();
    }

    public void OnSlotSetDirty(Slot slot)
    {
        this.board.isDirtyInCurrentFrame = true;
    }

    public void OnPickupGoal(GoalCollectParams goalCollect)
    {
        Match3Goals.GoalBase goal = goalCollect.goal;
        if (goal == null)
        {
            return;
        }
        if (goal.IsComplete())
        {
            return;
        }
        SlotDestroyParams destroyParams = goalCollect.destroyParams;
        GameStats gameStats = this.board.gameStats;
        if (destroyParams != null)
        {
            if (destroyParams.isHitByBomb)
            {
                gameStats.goalsFromPowerups++;
            }
            else if (destroyParams.isFromSwap)
            {
                gameStats.goalsFromUserMatches++;
            }
            else
            {
                gameStats.goalsFromInertion++;
            }
        }
        else
        {
            gameStats.goalsFromInertion++;
        }
        this.goals.OnPickupGoal(goal);
        GoalsPanelGoal goal2 = this.gameScreen.goalsPanel.GetGoal(goal);
        goal2.UpdateCollectedCount();
        Vector3 localPosition = this.boardContainer.InverseTransformPoint(goal2.transform.position);
        if (goal.IsComplete())
        {
            GameObject gameObject = this.particles.CreateParticles(localPosition, Match3Particles.PositionType.GoalComplete);
            gameObject.transform.parent = goal2.transform.parent;
            gameObject.transform.localPosition = goal2.transform.localPosition;
        }
        else
        {
            this.particles.CreateParticles(localPosition, Match3Particles.PositionType.OnUIGoalCollected);
        }
        this.Play(GGSoundSystem.SFXType.CollectGoal);
        if (goal.IsComplete())
        {
            this.Play(GGSoundSystem.SFXType.GoalsComplete);
            this.HighlightAllGoals();
            if (goal.config.chipType == ChipType.Chip)
            {
                List<Slot> sortedSlotsUpdateList = this.board.sortedSlotsUpdateList;
                for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
                {
                    Slot slot = sortedSlotsUpdateList[i];
                    if (slot != null)
                    {
                        Chip slotComponent = slot.GetSlotComponent<Chip>();
                        if (slotComponent != null && slotComponent.chipType == goal.config.chipType && slotComponent.itemColor == goal.config.itemColor)
                        {
                            this.particles.CreateParticles(slotComponent, Match3Particles.PositionType.OnDestroyChip, slotComponent.chipType, slotComponent.itemColor);
                        }
                    }
                }
            }
        }
        if (this.goals.isAllGoalsComplete)
        {
            this.Play(GGSoundSystem.SFXType.GoalsComplete);
        }
    }

    public void OnScoreAdded(int score)
    {
        this.board.userScore += score;
        this.gameScreen.goalsPanel.UpdateScore();
    }

    public void OnUserMadeMove()
    {
        this.board.userMovesCount++;
        this.board.lastTimeWhenUserMadeMove = this.board.currentTime;
        this.board.lastFrameWhenUserMadeMove = this.board.currentFrameIndex;
        this.gameScreen.goalsPanel.UpdateMovesCount();
        this.board.ResetMatchesInBoard();
        int movesRemaining = this.gameScreen.stageState.MovesRemaining;
        if (movesRemaining == 5 || movesRemaining == 10 || movesRemaining == 15)
        {
            this.HighlightAllGoals();
        }
        this.board.bubblesBoardComponent.OnUserMadeMove();
        bool flag = movesRemaining == 5;
        if (this.board.hasMoreMoves)
        {
            flag = (movesRemaining == 5 || movesRemaining == 3 || movesRemaining == 2 || movesRemaining == 1);
        }
        if (flag && movesRemaining > 0)
        {
            if (movesRemaining > 1)
            {
                this.gameScreen.movesContainer.Show(string.Format("{0} Moves", this.gameScreen.stageState.MovesRemaining));
                return;
            }
            if (movesRemaining == 1)
            {
                this.gameScreen.movesContainer.Show("Last Move");
                return;
            }
            this.gameScreen.movesContainer.Show(this.gameScreen.stageState.MovesRemaining.ToString());
        }
    }

    private void TryHighlightGoals()
    {
        List<Match3Goals.GoalBase> list = this.goals.goals;
        for (int i = 0; i < list.Count; i++)
        {
            Match3Goals.GoalBase goalBase = list[i];
            if (!goalBase.IsComplete())
            {
                int countAtStart = goalBase.CountAtStart;
                int remainingCount = goalBase.RemainingCount;
                bool flag = false;
                ChipType chipType = goalBase.config.chipType;
                if (chipType == ChipType.FallingGingerbreadMan)
                {
                    flag = (remainingCount <= 1);
                }
                else if (chipType == ChipType.BurriedElement)
                {
                    flag = (remainingCount < 5);
                }
                else if (chipType == ChipType.Chip)
                {
                    flag = (remainingCount <= 6);
                }
                if (flag)
                {
                    this.HighlightGoal(goalBase);
                }
            }
        }
    }

    private void HighlightGoal(Match3Goals.GoalBase goal)
    {
        if (goal == null)
        {
            return;
        }
        Match3Goals.ChipTypeDef chipTypeDef = new Match3Goals.ChipTypeDef
        {
            chipType = goal.config.chipType,
            itemColor = goal.config.itemColor
        };
        if (chipTypeDef.chipType == ChipType.BurriedElement)
        {
            this.board.burriedElements.WobbleAll();
            return;
        }
        for (int i = 0; i < this.board.sortedSlotsUpdateList.Count; i++)
        {
            Slot slot = this.board.sortedSlotsUpdateList[i];
            if (slot != null)
            {
                Chip slotComponent = slot.GetSlotComponent<Chip>();
                if (slotComponent != null && slotComponent.IsCompatibleWithPickupGoal(chipTypeDef))
                {
                    slotComponent.Wobble(Match3Settings.instance.chipWobbleSettings);
                }
            }
        }
    }

    private void HighlightAllGoals()
    {
        this.board.burriedElements.WobbleAll();
        for (int i = 0; i < this.board.sortedSlotsUpdateList.Count; i++)
        {
            Slot slot = this.board.sortedSlotsUpdateList[i];
            if (slot != null)
            {
                Chip slotComponent = slot.GetSlotComponent<Chip>();
                if (slotComponent != null && slotComponent.isPartOfActiveGoal)
                {
                    slotComponent.Wobble(Match3Settings.instance.chipWobbleSettings);
                }
            }
        }
    }

    private Match3Game.PotentialMatch GetMatchingPotentialMatchAction()
    {
        this.goals.FillSlotData(this);
        this.potentialMatchesList.Clear();
        List<PotentialMatches.CompoundSlotsSet> matchesList = this.board.potentialMatches.matchesList;
        for (int i = 0; i < matchesList.Count; i++)
        {
            PotentialMatches.CompoundSlotsSet compoundSlotsSet = matchesList[i];
            ActionScore actionScore = compoundSlotsSet.GetActionScore(this, this.goals);
            ActionScore actionScore2 = default(ActionScore);
            if (actionScore.powerupsCreated > 0)
            {
                ChipType createdPowerup = compoundSlotsSet.createdPowerup;
                if (createdPowerup != ChipType.DiscoBall)
                {
                    List<PowerupActivations.PowerupActivation> list = this.board.powerupActivations.CreatePotentialActivations(createdPowerup, this.GetSlot(compoundSlotsSet.positionOfSlotMissingForMatch));
                    for (int j = 0; j < list.Count; j++)
                    {
                        ActionScore actionScore3 = list[j].GetActionScore(this, this.goals);
                        actionScore2.goalsCount = Mathf.Max(actionScore2.goalsCount, actionScore3.goalsCount);
                        actionScore2.obstaclesDestroyed = Mathf.Max(actionScore2.obstaclesDestroyed, actionScore3.obstaclesDestroyed);
                    }
                }
                else
                {
                    actionScore2.goalsCount = 20;
                    actionScore2.obstaclesDestroyed = 20;
                }
            }
            this.potentialMatchesList.Add(new Match3Game.PotentialMatch(compoundSlotsSet, actionScore, actionScore2));
        }
        for (int k = 0; k < this.potentialMatchesList.Count; k++)
        {
            Match3Game.PotentialMatch potentialMatch = this.potentialMatchesList[k];
            if (potentialMatch.actionScore.powerupsCreated <= 0 && potentialMatch.actionScore.goalsCount > 0)
            {
                return potentialMatch;
            }
        }
        List<PowerupCombines.PowerupCombine> combines = this.board.powerupCombines.combines;
        for (int l = 0; l < combines.Count; l++)
        {
            PowerupCombines.PowerupCombine powerupCombine = combines[l];
            this.potentialMatchesList.Add(new Match3Game.PotentialMatch(powerupCombine, powerupCombine.GetActionScore(this, this.goals)));
        }
        List<PowerupActivations.PowerupActivation> powerups = this.board.powerupActivations.powerups;
        for (int m = 0; m < powerups.Count; m++)
        {
            PowerupActivations.PowerupActivation powerupActivation = powerups[m];
            this.potentialMatchesList.Add(new Match3Game.PotentialMatch(powerupActivation, powerupActivation.GetActionScore(this, this.goals)));
        }
        Match3Game.PotentialMatch potentialMatch2 = default(Match3Game.PotentialMatch);
        bool flag = false;
        for (int n = 0; n < this.potentialMatchesList.Count; n++)
        {
            Match3Game.PotentialMatch potentialMatch3 = this.potentialMatchesList[n];
            if (!flag || this.IsBetter(potentialMatch2.actionScore, potentialMatch3.actionScore))
            {
                flag = true;
                potentialMatch2 = potentialMatch3;
            }
        }
        return potentialMatch2;
    }

    private List<Match3Game.PotentialMatch> FillPotentialMatchesWithScore(bool addPowerupCombines, bool addPowerupActivations, bool onlyBestPowerups)
    {
        this.goals.FillSlotData(this);
        this.potentialMatchesList.Clear();
        if (addPowerupCombines)
        {
            List<PowerupCombines.PowerupCombine> combines = this.board.powerupCombines.combines;
            for (int i = 0; i < combines.Count; i++)
            {
                PowerupCombines.PowerupCombine powerupCombine = combines[i];
                this.potentialMatchesList.Add(new Match3Game.PotentialMatch(powerupCombine, powerupCombine.GetActionScore(this, this.goals)));
            }
        }
        if (addPowerupActivations)
        {
            List<PowerupActivations.PowerupActivation> powerups = this.board.powerupActivations.powerups;
            for (int j = 0; j < powerups.Count; j++)
            {
                PowerupActivations.PowerupActivation powerupActivation = powerups[j];
                this.potentialMatchesList.Add(new Match3Game.PotentialMatch(powerupActivation, powerupActivation.GetActionScore(this, this.goals)));
            }
        }
        if (onlyBestPowerups)
        {
            Match3Game.PotentialMatch bestPotentialMatch = this.GetBestPotentialMatch(this.potentialMatchesList);
            this.potentialMatchesList.Clear();
            if (bestPotentialMatch.isActive)
            {
                this.potentialMatchesList.Add(bestPotentialMatch);
            }
        }
        List<PotentialMatches.CompoundSlotsSet> matchesList = this.board.potentialMatches.matchesList;
        for (int k = 0; k < matchesList.Count; k++)
        {
            PotentialMatches.CompoundSlotsSet compoundSlotsSet = matchesList[k];
            ActionScore actionScore = compoundSlotsSet.GetActionScore(this, this.goals);
            ActionScore actionScore2 = default(ActionScore);
            if (actionScore.powerupsCreated > 0)
            {
                ChipType createdPowerup = compoundSlotsSet.createdPowerup;
                if (createdPowerup != ChipType.DiscoBall)
                {
                    List<PowerupActivations.PowerupActivation> list = this.board.powerupActivations.CreatePotentialActivations(createdPowerup, this.GetSlot(compoundSlotsSet.positionOfSlotMissingForMatch));
                    for (int l = 0; l < list.Count; l++)
                    {
                        ActionScore actionScore3 = list[l].GetActionScore(this, this.goals);
                        if (actionScore3.goalsCount > actionScore2.goalsCount)
                        {
                            actionScore2 = actionScore3;
                        }
                        if (actionScore3.obstaclesDestroyed > actionScore2.obstaclesDestroyed)
                        {
                            actionScore2 = actionScore3;
                        }
                    }
                }
                else
                {
                    actionScore2.goalsCount = 20;
                    actionScore2.obstaclesDestroyed = 20;
                }
            }
            this.potentialMatchesList.Add(new Match3Game.PotentialMatch(compoundSlotsSet, actionScore, actionScore2));
        }
        return this.potentialMatchesList;
    }

    private Match3Game.PotentialMatch GetBestPotentialMatch(List<Match3Game.PotentialMatch> potentialMatchesList)
    {
        Match3Game.PotentialMatch potentialMatch = default(Match3Game.PotentialMatch);
        bool flag = false;
        for (int i = 0; i < potentialMatchesList.Count; i++)
        {
            Match3Game.PotentialMatch potentialMatch2 = potentialMatchesList[i];
            if (!flag || this.IsBetter(potentialMatch, potentialMatch2))
            {
                flag = true;
                potentialMatch = potentialMatch2;
            }
        }
        return potentialMatch;
    }

    private Match3Game.PotentialMatch GetBestPotentialMatchAction()
    {
        List<Match3Game.PotentialMatch> list = this.FillPotentialMatchesWithScore(true, true, false);
        return this.GetBestPotentialMatch(list);
    }

    public int TotalSlotsGoalsRemainingCount
    {
        get
        {
            List<Match3Goals.GoalBase> list = this.goals.goals;
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                Match3Goals.GoalBase goalBase = list[i];
                if (goalBase.config.chipType == ChipType.FallingGingerbreadMan)
                {
                    int num2 = goalBase.RemainingCount;
                    if (num2 != 0)
                    {
                        int num3 = Mathf.Max(this.board.size.x, this.board.size.y);
                        for (int j = 0; j < this.board.sortedSlotsUpdateList.Count; j++)
                        {
                            Slot slot = this.board.sortedSlotsUpdateList[j];
                            if (slot != null)
                            {
                                Chip slotComponent = slot.GetSlotComponent<Chip>();
                                if (slotComponent != null && slotComponent.chipType == ChipType.FallingGingerbreadMan)
                                {
                                    num2 = Mathf.Max(0, num2 - 1);
                                    int num4 = 0;
                                    Slot slot2 = slot;
                                    while (slot2 != null && slot2 != null)
                                    {
                                        slot2 = slot2.NextSlotToPushToWithoutSandflow();
                                        num4++;
                                        if (num4 >= num3)
                                        {
                                            break;
                                        }
                                    }
                                    num += Mathf.Max(num4 - 1, 0);
                                }
                            }
                        }
                        num += num2 * num3;
                    }
                }
                else
                {
                    num += goalBase.RemainingCount;
                }
            }
            return num;
        }
    }

    private void OnMoveSettled()
    {
        this.movesSettledCount++;
        if (this.movesSettledCount % 2 == 0)
        {
            this.TryHighlightGoals();
        }
        if (this.gameScreen.stageState.MovesRemaining > 3)
        {
            this.gameScreen.powerupsPanel.ReinitPowerups();
            return;
        }
        Match3Game.PotentialMatch bestPotentialMatchAction = this.GetBestPotentialMatchAction();
        int totalSlotsGoalsRemainingCount = this.TotalSlotsGoalsRemainingCount;
        if (!bestPotentialMatchAction.isActive)
        {
            this.gameScreen.powerupsPanel.ShowArrowsOnAvailablePowerups();
            return;
        }
        if (bestPotentialMatchAction.isActive && bestPotentialMatchAction.actionScore.goalsCount < totalSlotsGoalsRemainingCount)
        {
            this.gameScreen.powerupsPanel.ShowArrowsOnAvailablePowerups();
            return;
        }
        this.gameScreen.powerupsPanel.ReinitPowerups();
    }

    public void OnCollectCoin()
    {
        this.board.currentCoins += 1L;
        this.gameScreen.goalsPanel.SetCoinsCount(this.board.currentCoins);
    }

    public void OnCollectedMoreMoves(int count)
    {
        this.board.collectedAdditionalMoves += count;
        this.gameScreen.goalsPanel.UpdateMovesCount();
        if (count > 0)
        {
            this.Play(GGSoundSystem.SFXType.CollectMoreMoves);
        }
    }

    public void ProcessMatches(Matches m, SwapParams swapParams)
    {
        bool flag = false;
        for (int i = 0; i < m.islands.Count; i++)
        {
            Island island = m.islands[i];
            bool flag2 = this.ProcessIsland(island, swapParams);
            flag = (flag || flag2);
        }
        if (flag)
        {
            this.ApplySlotGravityForAllSlots();
            this.board.matchCounter.timeLeft = 15f;
            this.board.matchCounter.eventCount++;
        }
    }

    public void StartInAnimation()
    {
        this.animationEnum = this.DoStartInAnimation();
        this.animationEnum.MoveNext();
    }

    private IEnumerator DoStartInAnimation()
    {
        return new Match3Game._003CDoStartInAnimation_003Ed__237(0)
        {
            _003C_003E4__this = this
        };
    }

    public void StartWinAnimation(WinScreen.InitArguments winScreenArguments, Action onComplete)
    {
        this.board.isUpdateSuspended = false;
        this.board.isInteractionSuspended = true;
        this.animationEnum = this.DoWinAnimation(winScreenArguments, onComplete);
        this.animationEnum.MoveNext();
    }

    private int CoinsPerChipType(ChipType chipType, int coinsPerPowerup)
    {
        int num = coinsPerPowerup;
        if (chipType == ChipType.Bomb)
        {
            num *= 2;
        }
        else if (chipType == ChipType.DiscoBall)
        {
            num *= 5;
        }
        return num;
    }

    private IEnumerator DoWinAnimation(WinScreen.InitArguments winScreenArguments, Action onComplete)
    {
        return new Match3Game._003CDoWinAnimation_003Ed__240(0)
        {
            _003C_003E4__this = this,
            winScreenArguments = winScreenArguments,
            onComplete = onComplete
        };
    }

    public void StartWinScreenBoardAnimation()
    {
        this.animationEnum = this.DoWinScreenBoardOutAnimation();
    }

    private IEnumerator DoWinScreenBoardOutAnimation()
    {
        return new Match3Game._003CDoWinScreenBoardOutAnimation_003Ed__242(0)
        {
            _003C_003E4__this = this
        };
    }

    private bool ProcessIsland(Island island, SwapParams swapParams)
    {
        List<Slot> allSlots = island.allSlots;
        SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
        slotDestroyParams.matchIsland = island;
        slotDestroyParams.isFromSwap = island.isFromSwap;
        slotDestroyParams.isCreatingPowerupFromThisMatch = island.isCreatingPowerup;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i].canCarpetSpreadFromHere)
            {
                slotDestroyParams.isHavingCarpet = true;
                break;
            }
        }
        if (island.isCreatingPowerup)
        {
            for (int j = 0; j < allSlots.Count; j++)
            {
                if (allSlots[j].isCreatePowerupWithThisSlotSuspended)
                {
                    slotDestroyParams.isCreatingPowerupFromThisMatch = false;
                    break;
                }
            }
        }
        Slot slot = null;
        bool flag = swapParams != null;
        if (slotDestroyParams.isCreatingPowerupFromThisMatch)
        {
            Slot slot2 = null;
            for (int k = 0; k < allSlots.Count; k++)
            {
                Slot slot3 = allSlots[k];
                if (slot2 == null || slot2.lastMoveFrameIndex < slot3.lastMoveFrameIndex)
                {
                    slot2 = slot3;
                }
            }
            if (swapParams != null)
            {
                if (island.ContainsPosition(swapParams.swipeToPosition))
                {
                    slot = this.GetSlot(swapParams.swipeToPosition);
                }
                else
                {
                    slot = this.GetSlot(swapParams.startPosition);
                }
            }
            else
            {
                slot = slot2;
            }
            if (slot != null && slot.isPowerupReplacementSuspended)
            {
                slot = null;
                slotDestroyParams.isCreatingPowerupFromThisMatch = false;
            }
        }
        if (slot == null)
        {
            slotDestroyParams.isCreatingPowerupFromThisMatch = false;
        }
        if (island.isFromSwap)
        {
            slotDestroyParams.swapParams = swapParams;
        }
        else
        {
            Matches matches = this.board.findMatches.matches;
            bool flag2 = false;
            for (int l = 0; l < allSlots.Count; l++)
            {
                Slot slot4 = allSlots[l];
                slot4.GetSlotComponent<Chip>();
                List<Slot> neigbourSlots = slot4.neigbourSlots;
                for (int m = 0; m < neigbourSlots.Count; m++)
                {
                    Slot slot5 = neigbourSlots[m];
                    if (matches.GetIsland(slot5.position) == null)
                    {
                        Chip slotComponent = slot5.GetSlotComponent<Chip>();
                        if (slotComponent != null && slotComponent.isSlotMatchingSuspended)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
            }
            if (flag2)
            {
                return false;
            }
        }
        DestroyMatchingIslandAction.InitArguments initArguments = default(DestroyMatchingIslandAction.InitArguments);
        initArguments.boltCollection = new BoltCollection();
        if (swapParams != null)
        {
            Match3Game.InputAffectorExport affectorExport = swapParams.affectorExport;
            Slot slot6 = null;
            if (island.ContainsPosition(swapParams.startPosition))
            {
                slot6 = this.GetSlot(swapParams.startPosition);
            }
            else if (island.ContainsPosition(swapParams.swipeToPosition))
            {
                slot6 = this.GetSlot(swapParams.swipeToPosition);
            }
            Match3Game.InputAffectorExport.InputAffectorForSlot inputAffectorForSlot = affectorExport.GetInputAffectorForSlot(slot6);
            if (inputAffectorForSlot != null)
            {
                inputAffectorForSlot.PutAllBoltsIn(initArguments.boltCollection);
            }
        }
        bool flag3 = slot != null;
        initArguments.slotDestroyParams = slotDestroyParams;
        initArguments.slotWherePowerupIsCreated = slot;
        initArguments.powerupToCreate = island.powerupToCreate;
        initArguments.allSlots = new List<Slot>();
        initArguments.allSlots.AddRange(allSlots);
        initArguments.game = this;
        initArguments.matchComboIndex = this.board.matchCounter.eventCount;
        bool flag4 = Slot.HasNeighboursAffectedByMatchingSlots(allSlots, this);
        if (allSlots.Count > 0 && !flag && Match3Settings.instance.destroyIslandBlinkSettings.useBlink && (!flag3 && flag4))
        {
            DestroyMatchingIslandBlinkAction destroyMatchingIslandBlinkAction = new DestroyMatchingIslandBlinkAction();
            destroyMatchingIslandBlinkAction.Init(initArguments);
            this.board.actionManager.AddAction(destroyMatchingIslandBlinkAction);
            return true;
        }
        if (allSlots.Count > 0 && !flag && (flag3 || flag4 || Match3Settings.instance.swipeAffectorSettings.autoMatchesProduceLighting))
        {
            DestroyMatchingIslandAction destroyMatchingIslandAction = new DestroyMatchingIslandAction();
            destroyMatchingIslandAction.Init(initArguments);
            this.board.actionManager.AddAction(destroyMatchingIslandAction);
            return true;
        }
        if (allSlots.Count > 0 && !flag && Match3Settings.instance.destroyIslandBlinkSettings.useBlink)
        {
            DestroyMatchingIslandBlinkAction destroyMatchingIslandBlinkAction2 = new DestroyMatchingIslandBlinkAction();
            destroyMatchingIslandBlinkAction2.Init(initArguments);
            this.board.actionManager.AddAction(destroyMatchingIslandBlinkAction2);
            return true;
        }
        this.FinishDestroySlots(initArguments);
        return allSlots.Count > 0;
    }

    public void FinishDestroySlots(DestroyMatchingIslandAction.InitArguments arguments)
    {
        List<Slot> allSlots = arguments.allSlots;
        SlotDestroyParams slotDestroyParams = arguments.slotDestroyParams;
        SwapParams swapParams = slotDestroyParams.swapParams;
        Slot slotWherePowerupIsCreated = arguments.slotWherePowerupIsCreated;
        bool flag = false;
        for (int i = 0; i < allSlots.Count; i++)
        {
            Chip slotComponent = allSlots[i].GetSlotComponent<Chip>();
            if (slotComponent != null && slotComponent.isPartOfActiveGoal)
            {
                flag = true;
                break;
            }
        }
        if (!slotDestroyParams.isCreatingPowerupFromThisMatch && !flag)
        {
            this.Play(new GGSoundSystem.PlayParameters
            {
                soundType = GGSoundSystem.SFXType.PlainMatch,
                variationIndex = arguments.matchComboIndex
            });
        }
        CollectPointsAction.OnIslandDestroy(arguments);
        for (int j = 0; j < allSlots.Count; j++)
        {
            allSlots[j].OnDestroySlot(slotDestroyParams);
        }
        this.board.AddMatch();
        this.destroyNeighbourSlots.Clear();
        this.allNeighbourSlots.Clear();
        if (!slotDestroyParams.isNeigbourDestroySuspended)
        {
            for (int k = 0; k < allSlots.Count; k++)
            {
                Slot slot = allSlots[k];
                if (!slotDestroyParams.IsNeigborDestraySuspended(slot))
                {
                    List<Slot> neigbourSlots = slot.neigbourSlots;
                    for (int l = 0; l < neigbourSlots.Count; l++)
                    {
                        Slot slot2 = neigbourSlots[l];
                        if (!allSlots.Contains(slot2) && !this.allNeighbourSlots.Contains(slot2))
                        {
                            this.allNeighbourSlots.Add(slot2);
                            this.destroyNeighbourSlots.Add(new Match3Game.SlotDestroyNeighbour(slot, slot2));
                        }
                    }
                }
            }
        }
        for (int m = 0; m < this.destroyNeighbourSlots.Count; m++)
        {
            Match3Game.SlotDestroyNeighbour slotDestroyNeighbour = this.destroyNeighbourSlots[m];
            slotDestroyNeighbour.neighbourSlot.OnDestroyNeighbourSlot(slotDestroyNeighbour.slotBeingDestroyed, slotDestroyParams);
        }
        if (slotDestroyParams.isCreatingPowerupFromThisMatch)
        {
            CreatePowerupAction createPowerupAction = new CreatePowerupAction();
            if (slotDestroyParams.chipsAvailableForPowerupCreateAnimation != null && slotDestroyParams.chipsAvailableForPowerupCreateAnimation.Count > 0)
            {
                for (int n = 0; n < slotDestroyParams.chipsAvailableForPowerupCreateAnimation.Count; n++)
                {
                    Chip chip = slotDestroyParams.chipsAvailableForPowerupCreateAnimation[n];
                    LightingBolt boltEndingOnSlot = arguments.boltCollection.GetBoltEndingOnSlot(chip.lastConnectedSlot);
                    arguments.boltCollection.AddUsedBolt(boltEndingOnSlot);
                    createPowerupAction.AddChip(chip, boltEndingOnSlot);
                }
            }
            createPowerupAction.Init(new CreatePowerupAction.CreateParams
            {
                game = this,
                positionWherePowerupWillBeCreated = slotWherePowerupIsCreated.position,
                powerupToCreate = arguments.powerupToCreate
            });
            this.board.actionManager.AddAction(createPowerupAction);
        }
        bool flag2 = swapParams != null;
        GameStats gameStats = this.board.gameStats;
        if (flag2)
        {
            gameStats.matchesFromUser++;
        }
        else
        {
            gameStats.matchesFromInertion++;
        }
        if (slotDestroyParams.isCreatingPowerupFromThisMatch)
        {
            if (flag2)
            {
                gameStats.powerupsCreatedFromUser++;
            }
            else
            {
                gameStats.powerupsCreatedFromInertion++;
            }
        }
        arguments.boltCollection.Clear();
    }

    public List<Slot> SlotsThatCanParticipateInDiscoBallAffectedArea(ItemColor itemColor, bool replaceWithBombs)
    {
        List<Slot> list = new List<Slot>();
        for (int i = 0; i < this.board.sortedSlotsUpdateList.Count; i++)
        {
            Slot slot = this.board.sortedSlotsUpdateList[i];
            if (slot != null && slot.CanParticipateInDiscoBombAffectedArea(itemColor, replaceWithBombs))
            {
                list.Add(slot);
            }
        }
        return list;
    }

    public ItemColor BestItemColorForDiscoBomb(bool replaceWithBombs)
    {
        this.maxColorHelper.Clear();
        foreach (Slot slot in this.board.slots)
        {
            this.maxColorHelper.AddSlot(slot, replaceWithBombs);
        }
        return this.maxColorHelper.MaxColor.color;
    }

    public void ClearSlotLocks()
    {
        foreach (Slot slot in this.board.slots)
        {
            if (slot != null)
            {
                slot.ClearLocks();
            }
        }
    }

    public void AddLockToAllSlots(Lock slotLock)
    {
        if (slotLock == null)
        {
            return;
        }
        for (int i = 0; i < this.board.slots.Length; i++)
        {
            Slot slot = this.board.slots[i];
            if (slot != null)
            {
                slotLock.LockSlot(slot);
            }
        }
    }

    public int RandomRange(int min, int max)
    {
        return this.board.RandomRange(min, max);
    }

    public float RandomRange(float min, float max)
    {
        return this.board.RandomRange(min, max);
    }

    public void RemoveLockFromAllSlots(Lock slotLock)
    {
        for (int i = 0; i < this.board.slots.Length; i++)
        {
            Slot slot = this.board.slots[i];
            if (slot != null)
            {
                slot.RemoveLock(slotLock);
            }
        }
    }

    public void Leave1Move()
    {
        this.board.userMovesCount += this.gameScreen.stageState.MovesRemaining - 1;
        this.gameScreen.goalsPanel.UpdateMovesCount();
    }

    private void _003CPutBoosters_003Eb__108_0()
    {
        this.HighlightAllGoals();
    }

    public float timeScale = 1f;

    private HeuristicAIPlayer aiPlayer = new HeuristicAIPlayer();

    public string levelName;

    private long startTimestampTicks;

    private long endTimestampTicks;

    [NonSerialized]
    public LevelDefinition level;

    private bool gameStarted;

    private bool preventAutomatchesIfPossible_;

    private List<BuyMovesPricesConfig.OfferConfig> moveOffersBought = new List<BuyMovesPricesConfig.OfferConfig>();

    [NonSerialized]
    public GameScreen gameScreen;

    [SerializeField]
    private Match3Game.DifficultyChanger difficultyChanger = new Match3Game.DifficultyChanger();

    [SerializeField]
    private CameraShaker cameraShaker;

    [SerializeField]
    public PlayerInput input;

    [SerializeField]
    public SlotsRendererPool slotsRendererPool;

    [SerializeField]
    public Match3Particles particles;

    [SerializeField]
    private TilesBorderRenderer borderRenderer;

    [SerializeField]
    private HiddenElementBorderRenderer hiddenElementBorderRenderer;

    [SerializeField]
    private SnowCoverBorderRenderer snowCoverRenderer;

    [SerializeField]
    private BubbleSlotsBorderRenderer bubbleSlotsBorderRenderer;

    [SerializeField]
    private TilesBorderRenderer slotsRenderer;

    [SerializeField]
    private TilesBorderRenderer conveyorBorderRenderer;

    [SerializeField]
    private BorderTilemapRenderer conveyorHoleRenderer;

    [SerializeField]
    private TilesBorderRenderer conveyorSlotsRenderer;

    [SerializeField]
    private ChocolateBorderRenderer chocolateBorderRenderer;

    [SerializeField]
    public Match3Game.TutorialSlotHighlighter tutorialHighlighter = new Match3Game.TutorialSlotHighlighter();

    [SerializeField]
    private Transform boardContainer;

    public Vector2 slotPhysicalSize = new Vector2(1f, 1f);

    private ShowPotentialMatchAction showMatchAction = new ShowPotentialMatchAction();

    public float gravity = 9f;

    [SerializeField]
    private List<Match3Game.PieceCreatorPool> pieceCreatorPools = new List<Match3Game.PieceCreatorPool>();

    private List<Slot> affectingList = new List<Slot>();

    [NonSerialized]
    public Match3Board board = new Match3Board();

    [NonSerialized]
    public Match3Goals goals = new Match3Goals();

    [NonSerialized]
    public ExtraFallingChips extraFallingChips = new ExtraFallingChips();

    [NonSerialized]
    public Match3GameParams initParams;

    private Match3Game.StartGameArguments startGameArguments;

    private List<Match3Game.TutorialMatchProgress> tutorialMatchProgressList = new List<Match3Game.TutorialMatchProgress>();

    private ShowTutorialMaskAction tutorialAction;

    public Match3Game.CreateBoardArguments createBoardArguments;

    private List<IntVector2> wallDirections = new List<IntVector2>();

    private IEnumerator shuffleBoardAnimation;

    public Dictionary<ItemColor, int> attachedElementsPerItemColor = new Dictionary<ItemColor, int>();

    private List<PotentialMatches.CompoundSlotsSet.MatchType> matchTypeToFind = new List<PotentialMatches.CompoundSlotsSet.MatchType>();

    private List<PowerupCombines.CombineType> combineTypeToFind = new List<PowerupCombines.CombineType>();

    private List<Slot> discoBallSlots = new List<Slot>();

    protected List<Match3Game.PotentialMatch> potentialMatchesList = new List<Match3Game.PotentialMatch>();

    private bool isWellDoneShown;

    private bool isWellDoneShownDone;

    private int movesSettledCount;

    private IEnumerator animationEnum;

    private List<Slot> allNeighbourSlots = new List<Slot>();

    private List<Match3Game.SlotDestroyNeighbour> destroyNeighbourSlots = new List<Match3Game.SlotDestroyNeighbour>();

    private Match3Game.MaximumColorHelper maxColorHelper = new Match3Game.MaximumColorHelper();

    [Serializable]
    public class DifficultyChanger
    {
        public void Apply(Match3StagesDB.Stage.Difficulty difficulty)
        {
            for (int i = 0; i < this.changes.Count; i++)
            {
                Match3Game.DifficultyChanger.MaterialChange materialChange = this.changes[i];
                if (materialChange.difficulty == difficulty)
                {
                    materialChange.Change();
                }
            }
        }

        [SerializeField]
        private List<Match3Game.DifficultyChanger.MaterialChange> changes = new List<Match3Game.DifficultyChanger.MaterialChange>();

        [Serializable]
        public class MaterialChange
        {
            public void Change()
            {
                if (this.renderer == null)
                {
                    return;
                }
                this.renderer.material = this.material;
            }

            [SerializeField]
            public Match3StagesDB.Stage.Difficulty difficulty;

            [SerializeField]
            private Material material;

            [SerializeField]
            private MeshRenderer renderer;
        }
    }

    [Serializable]
    public class PieceCreatorPool
    {
        public PieceType type;

        public List<ChipType> chipTypeList = new List<ChipType>();

        public List<ItemColor> itemColorList = new List<ItemColor>();

        public ComponentPool pool;
    }

    public interface IAffectorExportAction
    {
        void Execute();

        void OnCancel();
    }

    public class InputAffectorExport
    {
        public void AddChipAffector(ChipAffectorBase chipAffector)
        {
            this.chipAffectors.Add(chipAffector);
        }

        public void ExecuteOnAfterDestroy()
        {
            for (int i = 0; i < this.chipAffectors.Count; i++)
            {
                this.chipAffectors[i].OnAfterDestroy();
            }
        }

        public bool hasActions
        {
            get
            {
                return this.actions.Count > 0;
            }
        }

        public void AddAction(Match3Game.IAffectorExportAction action)
        {
            if (this.actions.Contains(action))
            {
                return;
            }
            this.actions.Add(action);
        }

        public void ExecuteActions()
        {
            for (int i = 0; i < this.actions.Count; i++)
            {
                this.actions[i].Execute();
            }
            this.actions.Clear();
        }

        public LightingBolt GetLigtingBoltForSlots(IntVector2 startPosition, IntVector2 endPosition)
        {
            for (int i = 0; i < this.affectorExports.Count; i++)
            {
                LightingBolt ligtingBoltForSlots = this.affectorExports[i].GetLigtingBoltForSlots(startPosition, endPosition);
                if (ligtingBoltForSlots != null)
                {
                    return ligtingBoltForSlots;
                }
            }
            return null;
        }

        public Match3Game.InputAffectorExport.InputAffectorForSlot GetInputAffectorForSlot(Slot slot)
        {
            for (int i = 0; i < this.affectorExports.Count; i++)
            {
                Match3Game.InputAffectorExport.InputAffectorForSlot inputAffectorForSlot = this.affectorExports[i];
                if (inputAffectorForSlot.slot == slot)
                {
                    return inputAffectorForSlot;
                }
            }
            return null;
        }

        public void Clear()
        {
            for (int i = 0; i < this.affectorExports.Count; i++)
            {
                this.affectorExports[i].Clear();
            }
            for (int j = 0; j < this.actions.Count; j++)
            {
                this.actions[j].OnCancel();
            }
        }

        private List<Match3Game.IAffectorExportAction> actions = new List<Match3Game.IAffectorExportAction>();

        private List<ChipAffectorBase> chipAffectors = new List<ChipAffectorBase>();

        public List<Match3Game.InputAffectorExport.InputAffectorForSlot> affectorExports = new List<Match3Game.InputAffectorExport.InputAffectorForSlot>();

        public class InputAffectorForSlot
        {
            public void Clear()
            {
                DiscoBallAffector.RemoveFromGame(this.bolts);
                this.bolts.Clear();
            }

            public void PutAllBoltsIn(BoltCollection boltCollection)
            {
                boltCollection.bolts.AddRange(this.bolts);
                this.bolts.Clear();
            }

            public LightingBolt GetLigtingBoltForSlots(IntVector2 startPosition, IntVector2 endPosition)
            {
                for (int i = 0; i < this.bolts.Count; i++)
                {
                    LightingBolt lightingBolt = this.bolts[i];
                    if (lightingBolt.isSlotPositionsSet && lightingBolt.startSlotPosition == startPosition && lightingBolt.endSlotPosition == endPosition)
                    {
                        this.bolts.Remove(lightingBolt);
                        return lightingBolt;
                    }
                }
                return null;
            }

            public Slot slot;

            public List<LightingBolt> bolts = new List<LightingBolt>();
        }
    }

    public struct SwitchSlotsArguments
    {
        public void Clear()
        {
            if (this.affectorExport_ == null)
            {
                return;
            }
            this.affectorExport_.Clear();
        }

        public Match3Game.InputAffectorExport affectorExport
        {
            get
            {
                if (this.affectorExport_ == null)
                {
                    this.affectorExport_ = new Match3Game.InputAffectorExport();
                }
                return this.affectorExport_;
            }
        }

        public IntVector2 pos1;

        public IntVector2 pos2;

        public bool instant;

        public List<LightingBolt> bolts;

        public float affectorDuration;

        public bool isAlreadySwitched;

        private Match3Game.InputAffectorExport affectorExport_;
    }

    public struct StartGameArguments
    {
        public bool putBoosters;
    }

    public class TutorialMatchProgress
    {
        public LevelDefinition.TutorialMatch tutorialMatch;

        public bool isStarted;
    }

    [Serializable]
    public class TutorialSlotHighlighter
    {
        public void Init(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        public void SetTutorialBackgroundActive(bool active)
        {
            GGUtil.SetActive(this.tutorialBackground, active);
        }

        public void ShowGameScreenTutorialMask()
        {
            if (this.gameScreen != null)
            {
                GGUtil.SetActive(this.gameScreen.tutorialMask, true);
                this.SetTutorialBackgroundActive(false);
            }
        }

        public void Show(TilesSlotsProvider provider)
        {
            this.SetActive(true);
            this.tilesMaskRenderer.ShowSlotsOnLevel(provider);
            this.borderMaskRenderer.ShowBorderOnLevel(provider);
            this.borderRenderer.ShowBorderOnLevel(provider);
        }

        public void Hide()
        {
            this.SetActive(false);
            if (this.gameScreen != null)
            {
                GGUtil.SetActive(this.gameScreen.tutorialMask, false);
            }
        }

        private void SetActive(bool flag)
        {
            GGUtil.SetActive(this.tutorialBackground, flag);
            GGUtil.SetActive(this.tilesMaskRenderer, flag);
            GGUtil.SetActive(this.borderMaskRenderer, flag);
            GGUtil.SetActive(this.borderRenderer, flag);
        }

        [SerializeField]
        private TilesBorderRenderer tilesMaskRenderer;

        [SerializeField]
        private TilesBorderRenderer borderMaskRenderer;

        [SerializeField]
        private TilesBorderRenderer borderRenderer;

        [SerializeField]
        private Transform tutorialBackground;

        [NonSerialized]
        private GameScreen gameScreen;
    }

    public struct CreateBoardArguments
    {
        public LevelDefinition level;

        public Vector3 offset;
    }

    protected struct PotentialMatch
    {
        public int ScoreWithPowerupScore(int currentScoreFactor, int goalsFactor)
        {
            return currentScoreFactor * this.actionScore.GoalsAndObstaclesScore(goalsFactor) + this.powerupCreatedScore.GoalsAndObstaclesScore(goalsFactor);
        }

        public bool isActive
        {
            get
            {
                return this.powerupCombine != null || this.powerupActivation != null || this.potentialMatch != null;
            }
        }

        public PotentialMatch(PowerupCombines.PowerupCombine powerupCombine, ActionScore actionScore)
        {
            this.actionScore = actionScore;
            this.powerupCombine = powerupCombine;
            this.powerupActivation = null;
            this.potentialMatch = null;
            this.powerupCreatedScore = default(ActionScore);
        }

        public PotentialMatch(PowerupActivations.PowerupActivation powerupActivation, ActionScore actionScore)
        {
            this.actionScore = actionScore;
            this.powerupCombine = null;
            this.powerupActivation = powerupActivation;
            this.potentialMatch = null;
            this.powerupCreatedScore = default(ActionScore);
        }

        public PotentialMatch(PotentialMatches.CompoundSlotsSet potentialMatch, ActionScore actionScore, ActionScore powerupCreatedScore)
        {
            this.actionScore = actionScore;
            this.powerupCombine = null;
            this.powerupActivation = null;
            this.potentialMatch = potentialMatch;
            this.powerupCreatedScore = powerupCreatedScore;
        }

        public ActionScore actionScore;

        public PowerupCombines.PowerupCombine powerupCombine;

        public PowerupActivations.PowerupActivation powerupActivation;

        public PotentialMatches.CompoundSlotsSet potentialMatch;

        public ActionScore powerupCreatedScore;
    }

    private struct SlotDestroyNeighbour
    {
        public SlotDestroyNeighbour(Slot slotBeingDestroyed, Slot neighbourSlot)
        {
            this.slotBeingDestroyed = slotBeingDestroyed;
            this.neighbourSlot = neighbourSlot;
        }

        public Slot slotBeingDestroyed;

        public Slot neighbourSlot;
    }

    public class MaximumColorHelper
    {
        public void Clear()
        {
            this.colorList.Clear();
        }

        public Match3Game.MaximumColorHelper.Color MaxColor
        {
            get
            {
                Match3Game.MaximumColorHelper.Color color = default(Match3Game.MaximumColorHelper.Color);
                color.color = ItemColor.Red;
                for (int i = 0; i < this.colorList.Count; i++)
                {
                    Match3Game.MaximumColorHelper.Color color2 = this.colorList[i];
                    if (color2.count > color.count)
                    {
                        color = color2;
                    }
                }
                return color;
            }
        }

        private void AddColor(ItemColor color)
        {
            for (int i = 0; i < this.colorList.Count; i++)
            {
                Match3Game.MaximumColorHelper.Color color2 = this.colorList[i];
                if (color2.color == color)
                {
                    color2.count++;
                    this.colorList[i] = color2;
                    return;
                }
            }
            Match3Game.MaximumColorHelper.Color item = default(Match3Game.MaximumColorHelper.Color);
            item.color = color;
            item.count++;
            this.colorList.Add(item);
        }

        public void AddSlot(Slot slot, bool replaceWithBombs)
        {
            if (slot == null)
            {
                return;
            }
            Chip slotComponent = slot.GetSlotComponent<Chip>();
            if (slotComponent == null)
            {
                return;
            }
            if (!slotComponent.canFormColorMatches)
            {
                return;
            }
            ItemColor itemColor = slotComponent.itemColor;
            if (!slot.CanParticipateInDiscoBombAffectedArea(itemColor, replaceWithBombs))
            {
                return;
            }
            this.AddColor(itemColor);
        }

        private List<Match3Game.MaximumColorHelper.Color> colorList = new List<Match3Game.MaximumColorHelper.Color>();

        public struct Color
        {
            public ItemColor color;

            public int count;
        }
    }

    [Serializable]
    private sealed class _003C_003Ec
    {
        internal int _003CCreateBoard_003Eb__121_0(Slot x, Slot y)
        {
            return x.maxDistanceToEnd.CompareTo(y.maxDistanceToEnd);
        }

        public static readonly Match3Game._003C_003Ec _003C_003E9 = new Match3Game._003C_003Ec();

        public static Comparison<Slot> _003C_003E9__121_0;
    }

    private sealed class _003CDoShuffleBoardAnimation_003Ed__192 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShuffleBoardAnimation_003Ed__192(int _003C_003E1__state)
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
            Match3Game match3Game = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    match3Game.board.isShufflingBoard = true;
                    match3Game.board.bubblesBoardComponent.CancelSpread();
                    this._003CshuffleInAnimation_003E5__2 = match3Game.gameScreen.shuffleContainer.DoShow();
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_38D;
                case 3:
                    this._003C_003E1__state = -1;
                    goto IL_3CA;
                default:
                    return false;
            }
            if (this._003CshuffleInAnimation_003E5__2.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            PopulateBoard.BoardRepresentation boardRepresentation = new PopulateBoard.BoardRepresentation();
            boardRepresentation.Init(match3Game, false);
            match3Game.attachedElementsPerItemColor.Clear();
            PopulateBoard.Params @params = new PopulateBoard.Params();
            @params.randomProvider = match3Game.board.randomProvider;
            if (match3Game.level.generationSettings.isConfigured)
            {
                List<LevelDefinition.ChipGenerationSettings.ChipSetting> chipSettings = match3Game.level.generationSettings.chipSettings;
                for (int i = 0; i < chipSettings.Count; i++)
                {
                    LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = chipSettings[i];
                    if (chipSetting.chipType == ChipType.Chip)
                    {
                        @params.availableColors.Add(chipSetting.itemColor);
                    }
                }
            }
            else
            {
                for (int j = 0; j < match3Game.level.numChips; j++)
                {
                    @params.availableColors.Add((ItemColor)j);
                }
            }
            @params.maxPotentialMatches = Match3Settings.instance.maxPotentialMatchesAtStart;
            if (!match3Game.board.populateBoard.RandomPopulate(boardRepresentation, @params))
            {
                for (int k = 0; k < match3Game.board.sortedSlotsUpdateList.Count; k++)
                {
                    Chip slotComponent = match3Game.board.sortedSlotsUpdateList[k].GetSlotComponent<Chip>();
                    if (slotComponent != null && slotComponent.hasGrowingElement)
                    {
                        if (!match3Game.attachedElementsPerItemColor.ContainsKey(slotComponent.itemColor))
                        {
                            match3Game.attachedElementsPerItemColor[slotComponent.itemColor] = 0;
                        }
                        Dictionary<ItemColor, int> attachedElementsPerItemColor = match3Game.attachedElementsPerItemColor;
                        ItemColor itemColor = slotComponent.itemColor;
                        int num2 = attachedElementsPerItemColor[itemColor];
                        attachedElementsPerItemColor[itemColor] = num2 + 1;
                    }
                }
                boardRepresentation.Init(match3Game, true);
                match3Game.board.populateBoard.RandomPopulate(boardRepresentation, @params);
            }
            foreach (Slot slot in match3Game.board.slots)
            {
                if (slot != null)
                {
                    PopulateBoard.BoardRepresentation.RepresentationSlot slot2 = match3Game.board.populateBoard.board.GetSlot(slot.position);
                    if (slot2.needsToBeGenerated && slot2.isGenerated)
                    {
                        Chip slotComponent2 = slot.GetSlotComponent<Chip>();
                        if (slotComponent2.chipType == ChipType.Chip)
                        {
                            ChipBehaviour componentBehaviour = slotComponent2.GetComponentBehaviour<ChipBehaviour>();
                            slotComponent2.itemColor = slot2.itemColor;
                            if (componentBehaviour != null)
                            {
                                componentBehaviour.ChangeClothTexture(slotComponent2.itemColor);
                            }
                            int num3 = 0;
                            if (match3Game.attachedElementsPerItemColor.ContainsKey(slotComponent2.itemColor))
                            {
                                num3 = match3Game.attachedElementsPerItemColor[slotComponent2.itemColor];
                            }
                            bool flag = num3 > 0;
                            num3 = Mathf.Max(0, num3 - 1);
                            match3Game.attachedElementsPerItemColor[slotComponent2.itemColor] = num3;
                            if (slotComponent2.hasGrowingElement && !flag)
                            {
                                slotComponent2.DestroyGrowingElement();
                            }
                            else if (!slotComponent2.hasGrowingElement && flag)
                            {
                                TransformBehaviour growingElementGraphics = AnimateGrowingElementOnChip.CreateGrowingElementPieceBehaviour(match3Game);
                                slotComponent2.AttachGrowingElement(growingElementGraphics);
                            }
                        }
                    }
                }
            }
            this._003Ctime_003E5__3 = 0f;
            this._003Cduration_003E5__4 = 1f;
            IL_38D:
            if (this._003Ctime_003E5__3 < this._003Cduration_003E5__4)
            {
                this._003Ctime_003E5__3 += Time.deltaTime;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            this._003CoutAnimation_003E5__5 = match3Game.gameScreen.shuffleContainer.DoHide();
            IL_3CA:
            if (!this._003CoutAnimation_003E5__5.MoveNext())
            {
                match3Game.board.isShufflingBoard = false;
                return false;
            }
            this._003C_003E2__current = null;
            this._003C_003E1__state = 3;
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

        public Match3Game _003C_003E4__this;

        private IEnumerator _003CshuffleInAnimation_003E5__2;

        private float _003Ctime_003E5__3;

        private float _003Cduration_003E5__4;

        private IEnumerator _003CoutAnimation_003E5__5;
    }

    private sealed class _003CDoStartInAnimation_003Ed__237 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoStartInAnimation_003Ed__237(int _003C_003E1__state)
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
            Match3Game match3Game = this._003C_003E4__this;
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
                this._003Ctime_003E5__2 = 0f;
                this._003Csettings_003E5__3 = Match3Settings.instance.winScreenInAnimation;
                this._003Cduration_003E5__4 = this._003Csettings_003E5__3.duration;
                this._003Ct_003E5__5 = match3Game.boardContainer;
                this._003CendPos_003E5__6 = this._003Ct_003E5__5.localPosition;
                this._003CstartPos_003E5__7 = this._003CendPos_003E5__6 + this._003Csettings_003E5__3.offset;
                this._003CendScale_003E5__8 = this._003Ct_003E5__5.localScale;
                this._003CstartScale_003E5__9 = this._003Csettings_003E5__3.scale;
            }
            if (this._003Ctime_003E5__2 > this._003Cduration_003E5__4)
            {
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__4, this._003Ctime_003E5__2);
            if (this._003Csettings_003E5__3.curve != null)
            {
                num2 = this._003Csettings_003E5__3.curve.Evaluate(num2);
            }
            Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPos_003E5__7, this._003CendPos_003E5__6, num2);
            Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__9, this._003CendScale_003E5__8, num2);
            this._003Ct_003E5__5.localPosition = localPosition;
            this._003Ct_003E5__5.localScale = localScale;
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

        public Match3Game _003C_003E4__this;

        private float _003Ctime_003E5__2;

        private WinScreenBoardInAnimation _003Csettings_003E5__3;

        private float _003Cduration_003E5__4;

        private Transform _003Ct_003E5__5;

        private Vector3 _003CendPos_003E5__6;

        private Vector3 _003CstartPos_003E5__7;

        private Vector3 _003CendScale_003E5__8;

        private Vector3 _003CstartScale_003E5__9;
    }

    private sealed class _003C_003Ec__DisplayClass240_0
    {
        internal void _003CDoWinAnimation_003Eb__1()
        {
            this.isWellDoneDone = true;
        }

        internal void _003CDoWinAnimation_003Eb__0()
        {
            this._003C_003E4__this.animationEnum = null;
            this._003C_003E4__this.gameScreen.tapToContinueContainer.Hide();
            if (this.onComplete != null)
            {
                this.onComplete();
            }
        }

        public bool isWellDoneDone;

        public Match3Game _003C_003E4__this;

        public Action onComplete;
    }

    private sealed class _003CDoWinAnimation_003Ed__240 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoWinAnimation_003Ed__240(int _003C_003E1__state)
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
            Match3Game match3Game = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new Match3Game._003C_003Ec__DisplayClass240_0();
                    this._003C_003E8__1._003C_003E4__this = this._003C_003E4__this;
                    this._003C_003E8__1.onComplete = this.onComplete;
                    this._003CcoinsPerMove_003E5__2 = 1;
                    this._003C_003E8__1.isWellDoneDone = false;
                    if (!match3Game.isWellDoneShown)
                    {
                        Action action = new Action(this._003C_003E8__1._003CDoWinAnimation_003Eb__1);
                        WellDoneContainer.InitArguments initArguments = new WellDoneContainer.InitArguments(action);
                        match3Game.gameScreen.wellDoneContainer.Show(initArguments);
                        match3Game.gameScreen.ShowConfetti();
                        match3Game.board.currentCoins = this.winScreenArguments.baseStageWonCoins;
                        match3Game.gameScreen.goalsPanel.ShowCoins();
                        match3Game.gameScreen.goalsPanel.SetCoinsCount(match3Game.board.currentCoins);
                        goto IL_16B;
                    }
                    this._003Ctime_003E5__6 = 0f;
                    this._003CmaxDuration_003E5__7 = 8f;
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_244;
                case 3:
                    this._003C_003E1__state = -1;
                    goto IL_3AE;
                case 4:
                    this._003C_003E1__state = -1;
                    goto IL_3AE;
                case 5:
                    this._003C_003E1__state = -1;
                    goto IL_4D6;
                case 6:
                    this._003C_003E1__state = -1;
                    goto IL_4D6;
                default:
                    return false;
            }
            if (!match3Game.isBoardFullySettled || !match3Game.isWellDoneShownDone)
            {
                this._003Ctime_003E5__6 += Time.deltaTime;
                if (this._003Ctime_003E5__6 <= this._003CmaxDuration_003E5__7)
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
            }
            this._003C_003E8__1.isWellDoneDone = true;
            IL_16B:
            this._003CadditionalCoins_003E5__3 = 0;
            for (int i = 0; i < match3Game.board.sortedSlotsUpdateList.Count; i++)
            {
                Chip slotComponent = match3Game.board.sortedSlotsUpdateList[i].GetSlotComponent<Chip>();
                if (slotComponent != null && slotComponent.isPowerup)
                {
                    int num2 = this._003CcoinsPerMove_003E5__2;
                    if (slotComponent.chipType == ChipType.Bomb)
                    {
                        num2 *= 2;
                    }
                    else if (slotComponent.chipType == ChipType.DiscoBall)
                    {
                        num2 *= 5;
                    }
                    slotComponent.carriesCoins = match3Game.CoinsPerChipType(slotComponent.chipType, this._003CcoinsPerMove_003E5__2);
                    this._003CadditionalCoins_003E5__3 += slotComponent.carriesCoins;
                }
            }
            this._003CmovesRemaining_003E5__4 = match3Game.gameScreen.stageState.MovesRemaining;
            IL_244:
            if (this._003C_003E8__1.isWellDoneDone)
            {
                float num3 = 0.1f;
                int b = 20;
                Mathf.Max(this._003CmovesRemaining_003E5__4, b);
                for (int j = 0; j < this._003CmovesRemaining_003E5__4; j++)
                {
                    PlacePowerupAction placePowerupAction = new PlacePowerupAction();
                    PlacePowerupAction.Parameters parameters = new PlacePowerupAction.Parameters();
                    parameters.game = match3Game;
                    int num4 = UnityEngine.Random.Range(0, 5);
                    parameters.powerup = ChipType.HorizontalRocket;
                    if (num4 == 0)
                    {
                        parameters.powerup = ChipType.HorizontalRocket;
                    }
                    else if (num4 == 1)
                    {
                        parameters.powerup = ChipType.VerticalRocket;
                    }
                    else if (num4 == 2)
                    {
                        parameters.powerup = ChipType.Bomb;
                    }
                    else if (num4 == 3)
                    {
                        parameters.powerup = ChipType.SeekingMissle;
                    }
                    else
                    {
                        parameters.powerup = ChipType.Bomb;
                    }
                    parameters.initialDelay = (float)j * num3;
                    if (j <= this._003CmovesRemaining_003E5__4)
                    {
                        parameters.addCoins = match3Game.CoinsPerChipType(parameters.powerup, this._003CcoinsPerMove_003E5__2);
                    }
                    placePowerupAction.Init(parameters);
                    match3Game.board.actionManager.AddAction(placePowerupAction);
                    this._003CadditionalCoins_003E5__3 += parameters.addCoins;
                }
                this.winScreenArguments.additionalCoins = (long)this._003CadditionalCoins_003E5__3;
                match3Game.gameScreen.tapToContinueContainer.Show(new Action(this._003C_003E8__1._003CDoWinAnimation_003Eb__0));
                this._003C_003E2__current = null;
                this._003C_003E1__state = 3;
                return true;
            }
            this._003C_003E2__current = null;
            this._003C_003E1__state = 2;
            return true;
            IL_3AE:
            if (!match3Game.isBoardFullySettled)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 4;
                return true;
            }
            this._003CpowerupChips_003E5__5 = new List<Chip>();
            IL_3C1:
            this._003CpowerupChips_003E5__5.Clear();
            for (int k = 0; k < match3Game.board.sortedSlotsUpdateList.Count; k++)
            {
                Chip slotComponent2 = match3Game.board.sortedSlotsUpdateList[k].GetSlotComponent<Chip>();
                if (slotComponent2 != null && slotComponent2.isPowerup)
                {
                    this._003CpowerupChips_003E5__5.Add(slotComponent2);
                }
            }
            if (this._003CpowerupChips_003E5__5.Count != 0 || !match3Game.isBoardFullySettled)
            {
                GGUtil.Shuffle<Chip>(this._003CpowerupChips_003E5__5);
                int b2 = 5;
                for (int l = 0; l < Mathf.Min(this._003CpowerupChips_003E5__5.Count, b2); l++)
                {
                    Chip chip = this._003CpowerupChips_003E5__5[l];
                    if (chip != null && chip.slot != null)
                    {
                        chip.slot.OnDestroySlot(new SlotDestroyParams
                        {
                            activationDelay = 0.05f * (float)l
                        });
                    }
                }
                this._003C_003E2__current = null;
                this._003C_003E1__state = 5;
                return true;
            }
            match3Game.gameScreen.tapToContinueContainer.Hide();
            if (this._003C_003E8__1.onComplete != null)
            {
                this._003C_003E8__1.onComplete();
            }
            return false;
            IL_4D6:
            if (match3Game.isBoardFullySettled)
            {
                goto IL_3C1;
            }
            this._003C_003E2__current = null;
            this._003C_003E1__state = 6;
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

        public Match3Game _003C_003E4__this;

        public Action onComplete;

        public WinScreen.InitArguments winScreenArguments;

        private Match3Game._003C_003Ec__DisplayClass240_0 _003C_003E8__1;

        private int _003CcoinsPerMove_003E5__2;

        private int _003CadditionalCoins_003E5__3;

        private int _003CmovesRemaining_003E5__4;

        private List<Chip> _003CpowerupChips_003E5__5;

        private float _003Ctime_003E5__6;

        private float _003CmaxDuration_003E5__7;
    }

    private sealed class _003CDoWinScreenBoardOutAnimation_003Ed__242 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoWinScreenBoardOutAnimation_003Ed__242(int _003C_003E1__state)
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
            Match3Game match3Game = this._003C_003E4__this;
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
                this._003Ctime_003E5__2 = 0f;
                this._003Csettings_003E5__3 = Match3Settings.instance.winScreenBoardOutAnimation;
                this._003Cduration_003E5__4 = this._003Csettings_003E5__3.duration;
                this._003Ct_003E5__5 = match3Game.boardContainer;
                this._003CstartPos_003E5__6 = this._003Ct_003E5__5.localPosition;
                Vector3 localScale = this._003Ct_003E5__5.localScale;
            }
            if (this._003Ctime_003E5__2 > this._003Cduration_003E5__4)
            {
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__4, this._003Ctime_003E5__2);
            if (this._003Csettings_003E5__3.outCurve != null)
            {
                num2 = this._003Csettings_003E5__3.outCurve.Evaluate(num2);
            }
            Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPos_003E5__6, this._003Csettings_003E5__3.offset + this._003CstartPos_003E5__6, num2);
            this._003Ct_003E5__5.localPosition = localPosition;
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

        public Match3Game _003C_003E4__this;

        private float _003Ctime_003E5__2;

        private WinScreenBoardOutAnimation _003Csettings_003E5__3;

        private float _003Cduration_003E5__4;

        private Transform _003Ct_003E5__5;

        private Vector3 _003CstartPos_003E5__6;
    }
}
