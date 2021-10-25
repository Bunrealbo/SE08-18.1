using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DecorateRoomScreen : UILayer, Match3GameListener
{
    public void SetSpeachBubbleAlpha(float alpha)
    {
        this.bubbleGroup.alpha = alpha;
    }

    public CharacterSpeachBubble GetSpeachBubble(CharacterAvatar avatar)
    {
        string name = avatar.name;
        List<GameObject> usedObjects = this.speachBubblePool.usedObjects;
        CharacterSpeachBubble characterSpeachBubble = null;
        for (int i = 0; i < usedObjects.Count; i++)
        {
            CharacterSpeachBubble component = usedObjects[i].GetComponent<CharacterSpeachBubble>();
            if (component.characterName == name)
            {
                characterSpeachBubble = component;
                break;
            }
        }
        if (characterSpeachBubble == null)
        {
            characterSpeachBubble = this.speachBubblePool.Next<CharacterSpeachBubble>(false);
            GGUtil.SetActive(characterSpeachBubble, false);
        }
        characterSpeachBubble.characterName = name;
        characterSpeachBubble.avatar = avatar;
        return characterSpeachBubble;
    }

    private void ShowConfettiParticle()
    {
        GGUtil.SetActive(UnityEngine.Object.Instantiate<GameObject>(this.confettiParticle, base.transform), true);
    }

    public DecoratingScene scene
    {
        get
        {
            if (this.activeRoom == null)
            {
                return null;
            }
            return this.activeRoom.sceneBehaviour;
        }
    }

    private bool isRoomLoaded
    {
        get
        {
            return this.activeRoom != null && this.activeRoom.sceneBehaviour != null && this.activeRoom.loadedRoom == ScriptableObjectSingleton<RoomsDB>.instance.ActiveRoom;
        }
    }

    private void OnEnable()
    {
        this.Init();
        this.accelerometer.Init();
    }

    private void OnDisable()
    {
        if (!this.isRoomLoaded)
        {
            return;
        }
        GGUtil.SetActive(this.scene, false);
    }

    private void OnApplicationFocus(bool pause)
    {
        if (this.updateEnumerator != null)
        {
            return;
        }
        if (pause)
        {
            return;
        }
        GGSnapshotCloudSync.CallOnFocus(pause);
        if (GGSnapshotCloudSync.syncNeeded)
        {
            this.Init();
        }
    }

    public void Init()
    {
        NavigationManager instance = NavigationManager.instance;
        if (GGSnapshotCloudSync.syncNeeded)
        {
            instance.GetObject<SyncGameScreen>().SynchronizeNow();
            return;
        }
        this.FinishInit();
    }

    private void FinishInit()
    {
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        this.itemSelect.Init(this);
        GGUtil.ChangeText(this.levelCountLabel, currentStage.index + 1);
        if (this.isRoomLoaded)
        {
            GGUtil.SetActive(this.scene, true);
            GGUtil.SetActive(this.widgetsToHide, false);
            this.roomLoadedStyle.Apply();
            this.InitScene(this.scene, true);
            InAppBackend instance = BehaviourSingletonInit<InAppBackend>.instance;
            return;
        }
        RoomsDB.Room room = ScriptableObjectSingleton<RoomsDB>.instance.ActiveRoom;
        this.LoadScene(room);
    }

    private void LoadScene(RoomsDB.Room room)
    {
        GGUtil.SetActive(this.widgetsToHide, false);
        this.loadingSceneStyle.Apply();
        this.updateEnumerator = this.DoLoadScene(room);
    }

    private IEnumerator DoLoadScene(RoomsDB.Room room)
    {
        return new DecorateRoomScreen._003CDoLoadScene_003Ed__56(0)
        {
            _003C_003E4__this = this,
            room = room
        };
    }

    private void InitRetry()
    {
        GGUtil.SetActive(this.widgetsToHide, false);
        this.retryLoadingStyle.Apply();
        this.speachBubblePool.Clear();
        this.speachBubblePool.HideNotUsed();
        this.visualItemsPool.Clear();
        this.visualItemsPool.HideNotUsed();
        this.uiVisualItems.Clear();
        GGUtil.SetActive(this.controlWidgets, true);
        this.currencyPanel.Show();
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        for (int i = 0; i < this.uiItemSetups.Count; i++)
        {
            DecorateRoomScreen.UIItemSetup uiitemSetup = this.uiItemSetups[i];
            bool active = !currentStage.hideUIElements;
            if (Application.isEditor && uiitemSetup.name == DecorateRoomScreen.UIItemName.SettingsButton)
            {
                active = true;
            }
            GGUtil.SetActive(uiitemSetup.widget, active);
        }
        GGUtil.Hide(this.levelDifficultyWidgets);
        if (currentStage.difficulty == Match3StagesDB.Stage.Difficulty.Normal)
        {
            this.normalDifficultySlyle.Apply();
            return;
        }
        if (currentStage.difficulty == Match3StagesDB.Stage.Difficulty.Hard)
        {
            this.hardDifficultySlyle.Apply();
            return;
        }
        if (currentStage.difficulty == Match3StagesDB.Stage.Difficulty.Nightmare)
        {
            this.nightmareDifficultySlyle.Apply();
        }
    }

    private void InitScene(DecoratingScene scene, bool isFirstTime)
    {
        DecorateRoomScreen._003C_003Ec__DisplayClass58_0 _003C_003Ec__DisplayClass58_ = new DecorateRoomScreen._003C_003Ec__DisplayClass58_0();
        _003C_003Ec__DisplayClass58_._003C_003E4__this = this;
        _003C_003Ec__DisplayClass58_.scene = scene;
        _003C_003Ec__DisplayClass58_.isFirstTime = isFirstTime;
        if (_003C_003Ec__DisplayClass58_.scene == null)
        {
            this.InitRetry();
            return;
        }
        if (Application.isEditor)
        {
            _003C_003Ec__DisplayClass58_.scene.InitRuntimeData();
        }
        this.speachBubblePool.Clear();
        this.speachBubblePool.HideNotUsed();
        this.groupFooter.Init(_003C_003Ec__DisplayClass58_.scene);
        if (_003C_003Ec__DisplayClass58_.isFirstTime)
        {
            _003C_003Ec__DisplayClass58_.scene.SetCharacterAlpha(0f);
            _003C_003Ec__DisplayClass58_.scene.StopCharacterAnimation();
        }
        ChangeAnimationArguments noAnimation = ChangeAnimationArguments.NoAnimation;
        List<DecoratingSceneConfig.AnimationSequence> availableSequences = _003C_003Ec__DisplayClass58_.scene.GetAvailableSequences();
        DecoratingSceneConfig.AnimationSequence animationSequence = null;
        if (availableSequences != null && availableSequences.Count > 0)
        {
            animationSequence = availableSequences[0];
        }
        if (animationSequence != null)
        {
            noAnimation.animation = animationSequence;
        }
        _003C_003Ec__DisplayClass58_.scene.ZoomOut();
        this.visualItemsPool.Clear();
        List<VisualObjectBehaviour> visualObjectBehaviours = _003C_003Ec__DisplayClass58_.scene.visualObjectBehaviours;
        this.uiVisualItems.Clear();
        int num = 0;
        float num2 = 0f;
        for (int i = 0; i < visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = visualObjectBehaviours[i];
            GraphicsSceneConfig.VisualObject visualObject = visualObjectBehaviour.visualObject;
            if (_003C_003Ec__DisplayClass58_.isFirstTime && visualObject.isOwned && !visualObject.hasDefaultVariation)
            {
                num2 = Mathf.Max(num2, visualObjectBehaviour.iconHandlePosition.x / (float)_003C_003Ec__DisplayClass58_.scene.config.width * 0.2f);
            }
        }
        num2 = 0f;
        DecorateRoomSceneVisualItem decorateRoomSceneVisualItem = null;
        for (int j = 0; j < visualObjectBehaviours.Count; j++)
        {
            VisualObjectBehaviour visualObjectBehaviour2 = visualObjectBehaviours[j];
            GraphicsSceneConfig.VisualObject visualObject2 = visualObjectBehaviour2.visualObject;
            DecorateRoomSceneVisualItem decorateRoomSceneVisualItem2 = this.visualItemsPool.Next<DecorateRoomSceneVisualItem>(false);
            GGUtil.SetActive(decorateRoomSceneVisualItem2, true);
            decorateRoomSceneVisualItem2.Init(visualObjectBehaviour2, this, num, num2);
            bool flag = visualObject2.IsUnlocked(_003C_003Ec__DisplayClass58_.scene) && !visualObject2.isOwned;
            if (flag)
            {
                num++;
            }
            this.uiVisualItems.Add(decorateRoomSceneVisualItem2);
            if (flag && visualObject2.sceneObjectInfo.autoSelect)
            {
                decorateRoomSceneVisualItem = decorateRoomSceneVisualItem2;
            }
        }
        this.visualItemsPool.HideNotUsed();
        GGUtil.SetActive(this.controlWidgets, true);
        this.currencyPanel.Show();
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        for (int k = 0; k < this.uiItemSetups.Count; k++)
        {
            DecorateRoomScreen.UIItemSetup uiitemSetup = this.uiItemSetups[k];
            bool active = !currentStage.hideUIElements;
            if (Application.isEditor && uiitemSetup.name == DecorateRoomScreen.UIItemName.SettingsButton)
            {
                active = true;
            }
            GGUtil.SetActive(uiitemSetup.widget, active);
        }
        GGUtil.Hide(this.levelDifficultyWidgets);
        if (currentStage.difficulty == Match3StagesDB.Stage.Difficulty.Normal)
        {
            this.normalDifficultySlyle.Apply();
        }
        else if (currentStage.difficulty == Match3StagesDB.Stage.Difficulty.Hard)
        {
            this.hardDifficultySlyle.Apply();
        }
        else if (currentStage.difficulty == Match3StagesDB.Stage.Difficulty.Nightmare)
        {
            this.nightmareDifficultySlyle.Apply();
        }
        if (!GGPlayerSettings.instance.Model.acceptedTermsOfService)
        {
            DecorateRoomScreen._003C_003Ec__DisplayClass58_1 _003C_003Ec__DisplayClass58_2 = new DecorateRoomScreen._003C_003Ec__DisplayClass58_1();
            _003C_003Ec__DisplayClass58_2.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass58_;
            _003C_003Ec__DisplayClass58_2.nav = NavigationManager.instance;
            _003C_003Ec__DisplayClass58_2.nav.GetObject<TermsOfServiceDialog>().Show(new Action<bool>(_003C_003Ec__DisplayClass58_2._003CInitScene_003Eb__0));
            return;
        }
        if (_003C_003Ec__DisplayClass58_.isFirstTime && currentStage.startMessages.Count > 0 && !currentStage.isIntroMessageShown)
        {
            _003C_003Ec__DisplayClass58_.scene.StopCharacterAnimation();
            this.animationEnumerator = this.DoShowMessageEnumerator(currentStage.startMessages, null, null);
            this.animationEnumerator.MoveNext();
            currentStage.isIntroMessageShown = true;
            return;
        }
        if (decorateRoomSceneVisualItem != null)
        {
            this.VisualItemCallback_OnBuyItemPressed(decorateRoomSceneVisualItem);
            return;
        }
        if (noAnimation.isAnimationAvailable)
        {
            if (_003C_003Ec__DisplayClass58_.scene.runningAnimation != noAnimation.animation)
            {
                this.animationEnumerator = this.DoShowCharacterAnimation(noAnimation, null);
                this.animationEnumerator.MoveNext();
                return;
            }
            _003C_003Ec__DisplayClass58_.scene.AnimateCharacterAlphaTo(1f);
        }
    }

    private IEnumerator DoShowCharacterAnimation(List<ChangeAnimationArguments> animationParamsList, Action onComplete = null)
    {
        return new DecorateRoomScreen._003CDoShowCharacterAnimation_003Ed__59(0)
        {
            _003C_003E4__this = this,
            animationParamsList = animationParamsList,
            onComplete = onComplete
        };
    }

    private IEnumerator DoShowCharacterAnimation(ChangeAnimationArguments animationParams, Action onComplete = null)
    {
        return new DecorateRoomScreen._003CDoShowCharacterAnimation_003Ed__60(0)
        {
            _003C_003E4__this = this,
            animationParams = animationParams,
            onComplete = onComplete
        };
    }

    private IEnumerator DoShowMessageEnumerator(ProgressMessageList progressMessages, Action onComplete = null)
    {
        return new DecorateRoomScreen._003CDoShowMessageEnumerator_003Ed__61(0)
        {
            _003C_003E4__this = this,
            progressMessages = progressMessages,
            onComplete = onComplete
        };
    }

    private IEnumerator DoShowMessageEnumerator(List<string> messages, List<ChangeAnimationArguments> animationParamsList, Action onComplete = null)
    {
        return new DecorateRoomScreen._003CDoShowMessageEnumerator_003Ed__62(0)
        {
            _003C_003E4__this = this,
            messages = messages,
            animationParamsList = animationParamsList,
            onComplete = onComplete
        };
    }

    private IEnumerator DoShowMessageEnumerator(string message)
    {
        return this.DoShowMessageEnumerator(new TextDialog.MessageArguments
        {
            message = message
        });
    }

    private IEnumerator DoShowMessageEnumerator(TextDialog.MessageArguments messageArguments)
    {
        return new DecorateRoomScreen._003CDoShowMessageEnumerator_003Ed__64(0)
        {
            _003C_003E4__this = this,
            messageArguments = messageArguments
        };
    }

    private void HideSelectionUI()
    {
        for (int i = 0; i < this.uiVisualItems.Count; i++)
        {
            DecorateRoomSceneVisualItem decorateRoomSceneVisualItem = this.uiVisualItems[i];
            GGUtil.SetActive(decorateRoomSceneVisualItem, false);
            decorateRoomSceneVisualItem.visualObjectBehaviour.SetMarkersActive(false);
        }
    }

    private void ShowStarConsumeAnimation(DecorateRoomSceneVisualItem visualItem, Action onEnd)
    {
        StarConsumeAnimation.InitParams initParams = default(StarConsumeAnimation.InitParams);
        initParams.screen = this;
        initParams.visualItem = visualItem;
        initParams.onEnd = onEnd;
        this.starConsumeAnimation.Show(initParams);
    }

    public void ButtonCallback_OnRetry()
    {
        this.Init();
    }

    public void VariationPanelCallback_OnClosed(VariationPanel variationPanel)
    {
        GraphicsSceneConfig.VisualObject visualObject = variationPanel.uiItem.visualObjectBehaviour.visualObject;
        List<string> list = visualObject.sceneObjectInfo.toSayAfterOpen;
        bool isPurchased = variationPanel.initParams.isPurchased;
        DecorateRoomSceneVisualItem uiItem = variationPanel.uiItem;
        VisualObjectBehaviour visualObjectBehaviour = uiItem.visualObjectBehaviour;
        this.scene.AnimationForVisualBehaviour(visualObjectBehaviour);
        if (isPurchased || variationPanel.isVariationChanged)
        {
            this.visualObjectParticles.CreateParticles(VisualObjectParticles.PositionType.BuySuccess, this.scene.rootTransform.gameObject, uiItem.visualObjectBehaviour);
            GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonConfirm);
        }
        DecoratingScene.RoomProgressState roomProgressState = this.scene.GetRoomProgressState();
        RoomsBackend.RoomAccessor roomBackend = this.scene.roomBackend;
        bool isPassed = roomBackend.isPassed;
        bool flag = false;
        if (roomProgressState.isPassed && !roomBackend.isPassed)
        {
            roomBackend.isPassed = true;
            flag = true;
        }
        if (isPurchased)
        {
            new List<ChangeAnimationArguments>();
            DecoratingScene.GroupDefinition groupForIndex = this.scene.GetGroupForIndex(visualObject.sceneObjectInfo.groupIndex);
            if (groupForIndex != null && this.scene.IsAllElementsPickedUpInGroup(visualObject.sceneObjectInfo.groupIndex) && groupForIndex.toSayAfterGroupCompletes.Count > 0)
            {
                list = groupForIndex.toSayAfterGroupCompletes;
            }
            if (list.Count == 0)
            {
                list.Add("Great job!");
            }
            ProgressMessageList progressMessageList = new ProgressMessageList();
            progressMessageList.messageList = list;
            if (!isPassed)
            {
                progressMessageList.progressChange = new ProgressMessageList.ProgressChange();
                progressMessageList.progressChange.fromProgress = roomProgressState.Progress(1);
                progressMessageList.progressChange.toProgress = roomProgressState.progress;
            }
            Action onComplete = null;
            if (flag || (Application.isEditor && this.alwaysPlaySceneCompleteAnimation))
            {
                onComplete = new Action(this.OnCompleteRoom);
            }
            IEnumerator enumerator = null;
            if (list.Count > 0)
            {
                enumerator = this.DoShowMessageEnumerator(progressMessageList, onComplete);
            }
            if (enumerator != null)
            {
                this.animationEnumerator = enumerator;
                this.animationEnumerator.MoveNext();
                this.ShowConfettiParticle();
                return;
            }
        }
        this.InitScene(this.scene, false);
    }

    private void OnCompleteRoom()
    {
        DecorateRoomScreen._003C_003Ec__DisplayClass69_0 _003C_003Ec__DisplayClass69_ = new DecorateRoomScreen._003C_003Ec__DisplayClass69_0();
        _003C_003Ec__DisplayClass69_.nav = NavigationManager.instance;
        _003C_003Ec__DisplayClass69_.giftBoxScreen = _003C_003Ec__DisplayClass69_.nav.GetObject<GiftBoxScreen>();
        _003C_003Ec__DisplayClass69_.loadedRoom = this.activeRoom.loadedRoom;
        _003C_003Ec__DisplayClass69_.arguments = default(GiftBoxScreen.ShowArguments);
        _003C_003Ec__DisplayClass69_.arguments.title = "Room Complete";
        _003C_003Ec__DisplayClass69_.arguments.giftsDefinition = _003C_003Ec__DisplayClass69_.loadedRoom.giftDefinition;
        _003C_003Ec__DisplayClass69_.arguments.onComplete = new Action(_003C_003Ec__DisplayClass69_._003COnCompleteRoom_003Eb__0);
        _003C_003Ec__DisplayClass69_.nav.GetObject<WellDoneScreen>().Show(new WellDoneScreen.InitArguments
        {
            mainText = "Room Complete",
            onComplete = new Action(_003C_003Ec__DisplayClass69_._003COnCompleteRoom_003Eb__1)
        });
    }

    private void ShowVariations(DecorateRoomSceneVisualItem uiItem, VariationPanel.InitParams initParams)
    {
        this.HideSelectionUI();
        this.scene.ZoomIn(uiItem.visualObjectBehaviour);
        List<VisualObjectBehaviour> visualObjectBehaviours = this.scene.visualObjectBehaviours;
        this.uiVisualItems.Clear();
        for (int i = 0; i < visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = visualObjectBehaviours[i];
            visualObjectBehaviour.SetVisualState();
            visualObjectBehaviour.SetMarkersActive(false);
        }
        uiItem.HideButton();
        uiItem.ShowMarkers();
        GGUtil.SetActive(this.controlWidgets, false);
        this.variationPanel.Show(this, uiItem, initParams);
        if (initParams.isPurchased)
        {
            this.ShowStarConsumeAnimation(uiItem, null);
        }
        this.scene.AnimateCharacterAlphaTo(0f);
        bool hideCharacterWhenSelectingVariations = uiItem.visualObjectBehaviour.visualObject.sceneObjectInfo.hideCharacterWhenSelectingVariations;
        this.visualObjectParticles.CreateParticles(VisualObjectParticles.PositionType.ChangeSuccess, this.scene.rootTransform.gameObject, uiItem.visualObjectBehaviour);
    }

    public void VisualItemCallback_OnBuyItemPressed(DecorateRoomSceneVisualItem uiItem)
    {
        this.HideSelectionUI();
        GGUtil.SetActive(uiItem, true);
        uiItem.ShowMarkers();
        uiItem.HideButton();
        GGUtil.SetActive(this.confirmPurchasePanel, true);
        this.confirmPurchasePanel.Show(uiItem, this);
        this.scene.ZoomIn(uiItem.visualObjectBehaviour);
        GGUtil.SetActive(this.controlWidgets, false);
        this.scene.AnimateCharacterAlphaTo(0f);
        GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
    }

    private void UpdateBubblePosition()
    {
        if (this.scene == null)
        {
            return;
        }
        List<GameObject> usedObjects = this.speachBubblePool.usedObjects;
        for (int i = 0; i < usedObjects.Count; i++)
        {
            CharacterSpeachBubble component = usedObjects[i].GetComponent<CharacterSpeachBubble>();
            if (component.avatar == null)
            {
                GGUtil.SetActive(component, false);
            }
            Vector3 localPosition = component.transform.parent.InverseTransformPoint(this.scene.CharacterBubblePosition(component.avatar));
            component.transform.localPosition = localPosition;
        }
    }

    private void Update()
    {
        if (this.useAccelerometer)
        {
            this.accelerometer.Update();
        }
        this.UpdateBubblePosition();
        if (this.scene != null && this.useAccelerometer)
        {
            Vector3 rootTransformOffsetAcceleration = new Vector3(this.accelerometer.currentPosition.x * this.marginsPsdSize, -this.accelerometer.currentPosition.y * this.marginsPsdSize, 0f);
            rootTransformOffsetAcceleration.x *= this.scene.psdTransformationScale.x;
            rootTransformOffsetAcceleration.y *= this.scene.psdTransformationScale.y;
            this.scene.rootTransformOffsetAcceleration = rootTransformOffsetAcceleration;
        }
        if (this.updateEnumerator != null && !this.updateEnumerator.MoveNext())
        {
            this.updateEnumerator = null;
        }
        if (this.animationEnumerator != null && !this.animationEnumerator.MoveNext())
        {
            this.animationEnumerator = null;
        }
    }

    public void ButtonCallback_OnSceneClick()
    {
        Vector3 position = UnityEngine.Input.mousePosition;
        if (UnityEngine.Input.touchCount > 0)
        {
            position = UnityEngine.Input.GetTouch(0).position;
        }
        Vector3 v = NavigationManager.instance.GetCamera().ScreenToWorldPoint(position);
        Vector3 v2 = this.TransformWorldToPSDPoint(v);
        DecorateRoomSceneVisualItem decorateRoomSceneVisualItem = null;
        int num = 0;
        for (int i = 0; i < this.uiVisualItems.Count; i++)
        {
            DecorateRoomSceneVisualItem decorateRoomSceneVisualItem2 = this.uiVisualItems[i];
            if (decorateRoomSceneVisualItem2.visualObjectBehaviour.visualObject.isOwned)
            {
                GraphicsSceneConfig.VisualObject.HitResult hitResult = decorateRoomSceneVisualItem2.visualObjectBehaviour.visualObject.GetHitResult(v2);
                if (hitResult.isHit)
                {
                    int hitDepth = hitResult.hitDepth;
                    if (!(decorateRoomSceneVisualItem != null) || num <= hitDepth)
                    {
                        decorateRoomSceneVisualItem = decorateRoomSceneVisualItem2;
                        num = hitDepth;
                    }
                }
            }
        }
        if (decorateRoomSceneVisualItem == null)
        {
            GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
            return;
        }
        VariationPanel.InitParams initParams = default(VariationPanel.InitParams);
        GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
        this.ShowVariations(decorateRoomSceneVisualItem, initParams);
    }

    public void StartGame(Match3GameParams initParams)
    {
        if (!BehaviourSingleton<EnergyManager>.instance.HasEnergyForOneLife())
        {
            OutOfLivesDialog @object = NavigationManager.instance.GetObject<OutOfLivesDialog>();
            LivesPriceConfig.PriceConfig priceForLevelOrDefault = ScriptableObjectSingleton<LivesPriceConfig>.instance.GetPriceForLevelOrDefault(initParams.levelIndex);
            @object.Show(priceForLevelOrDefault, new Action(this.OnAllLifesRefilled), new Action(this.OnFirstLifeRefilled), new Action(this.Init));
            return;
        }
        GameScreen object2 = NavigationManager.instance.GetObject<GameScreen>();
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        initParams.level = currentStage.levelReference.level;
        if (currentStage.multiLevelReference.Count > 0)
        {
            List<Match3StagesDB.LevelReference> multiLevelReference = currentStage.multiLevelReference;
            for (int i = 0; i < multiLevelReference.Count; i++)
            {
                LevelDefinition level = multiLevelReference[i].level;
                if (level != null)
                {
                    initParams.levelsList.Add(level);
                }
            }
        }
        initParams.stage = currentStage;
        initParams.levelIndex = currentStage.index;
        initParams.listener = this;
        GiftsDefinitionDB.BuildupBooster.BoosterGift boosterGift = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.buildupBooster.GetBoosterGift();
        if (boosterGift != null)
        {
            initParams.giftBoosterLevel = boosterGift.level;
            List<BoosterConfig> boosterConfig = boosterGift.boosterConfig;
            for (int j = 0; j < boosterConfig.Count; j++)
            {
                BoosterConfig item = boosterConfig[j];
                initParams.activeBoosters.Add(item);
            }
        }
        object2.Show(initParams);
        GGSoundSystem.Play(GGSoundSystem.MusicType.GameMusic);
    }

    public void OnAllLifesRefilled()
    {
        NavigationManager.instance.GetObject<OutOfLivesDialog>().Hide();
        this.ShowPregameDialog();
    }

    public void OnFirstLifeRefilled()
    {
        NavigationManager.instance.GetObject<OutOfLivesDialog>().Hide();
        this.ShowPregameDialog();
    }

    public void ButtonCallback_PlayButtonClick()
    {
        this.ShowPregameDialog();
    }

    public void ShowPregameDialog()
    {
        PreGameDialog @object = NavigationManager.instance.GetObject<PreGameDialog>();
        this.HideSelectionUI();
        if (this.scene != null)
        {
            List<VisualObjectBehaviour> visualObjectBehaviours = this.scene.visualObjectBehaviours;
            this.uiVisualItems.Clear();
            for (int i = 0; i < visualObjectBehaviours.Count; i++)
            {
                visualObjectBehaviours[i].SetMarkersActive(false);
            }
        }
        GGUtil.SetActive(this.widgetsToHide, false);
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        @object.Show(currentStage, this, new Action(this.Init), null);
    }

    public void OnGameStarted(GameStartedParams startedParams)
    {
        BehaviourSingleton<EnergyManager>.instance.SpendLifeIfNotFreeEnergy();
        List<BoosterConfig> activeBoosters = startedParams.game.initParams.activeBoosters;
        PlayerInventory instance = PlayerInventory.instance;
        for (int i = 0; i < activeBoosters.Count; i++)
        {
            BoosterConfig boosterConfig = activeBoosters[i];
            instance.SetOwned(boosterConfig.boosterType, instance.OwnedCount(boosterConfig.boosterType) - 1L);
            instance.SetUsedCount(boosterConfig.boosterType, instance.UsedCount(boosterConfig.boosterType) + 1L);
        }
    }

    public void OnGameComplete(GameCompleteParams completeParams)
    {
        Match3Game game = completeParams.game;
        Match3StagesDB.Stage stage = game.initParams.stage;
        bool flag = false;
        if (stage != null)
        {
            flag = stage.isPassed;
            stage.OnGameComplete(completeParams);
        }
        if (completeParams.isWin)
        {
            DecorateRoomScreen._003C_003Ec__DisplayClass82_0 _003C_003Ec__DisplayClass82_ = new DecorateRoomScreen._003C_003Ec__DisplayClass82_0();
            NavigationManager instance = NavigationManager.instance;
            _003C_003Ec__DisplayClass82_.winScreen = instance.GetObject<WinScreen>();
            WalletManager walletManager = GGPlayerSettings.instance.walletManager;
            _003C_003Ec__DisplayClass82_.winScreenArguments = new WinScreen.InitArguments();
            _003C_003Ec__DisplayClass82_.winScreenArguments.previousCoins = walletManager.CurrencyCount(CurrencyType.coins);
            _003C_003Ec__DisplayClass82_.winScreenArguments.baseStageWonCoins = 50L;
            _003C_003Ec__DisplayClass82_.winScreenArguments.previousStars = walletManager.CurrencyCount(CurrencyType.diamonds);
            _003C_003Ec__DisplayClass82_.winScreenArguments.currentStars = _003C_003Ec__DisplayClass82_.winScreenArguments.previousStars + 1L;
            _003C_003Ec__DisplayClass82_.winScreenArguments.onMiddle = new Action(this.OnWinScreenAnimationMiddle);
            _003C_003Ec__DisplayClass82_.winScreenArguments.game = game;
            _003C_003Ec__DisplayClass82_.winScreenArguments.decorateRoomScreen = this;
            _003C_003Ec__DisplayClass82_.winScreenArguments.starRect = this.starRect;
            _003C_003Ec__DisplayClass82_.winScreenArguments.coinRect = this.coinRect;
            _003C_003Ec__DisplayClass82_.winScreenArguments.currencyPanel = this.currencyPanel;
            game.StartWinAnimation(_003C_003Ec__DisplayClass82_.winScreenArguments, new Action(_003C_003Ec__DisplayClass82_._003COnGameComplete_003Eb__0));
            this.wonCoinsCount = stage.coinsCount;
            if (!flag)
            {
                GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.diamonds, this.moneyPickupAnimation.settings.numberOfStars);
            }
            BehaviourSingleton<EnergyManager>.instance.AddLifeIfNotFreeEnergy();
            return;
        }
        GGSoundSystem.Play(GGSoundSystem.MusicType.MainMenuMusic);
        if (!game.hasPlayedAnyMoves)
        {
            BehaviourSingleton<EnergyManager>.instance.AddLifeIfNotFreeEnergy();
        }
        GameScreen gameScreen = game.gameScreen;
        NavigationManager.instance.Pop(true);
        gameScreen.DestroyCreatedGameObjects();
        if (game.hasPlayedAnyMoves)
        {
            this.ShowPregameDialog();
        }
    }

    private void OnWinScreenAnimationMiddle()
    {
        NavigationManager instance = NavigationManager.instance;
        instance.GetObject<GameScreen>().DestroyCreatedGameObjects();
        GiftsDefinitionDB.GiftDefinition currentGift = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.currentGift;
        bool flag = currentGift != null && currentGift.isAvailableToCollect;
        RateCallerSettings instance2 = ScriptableObjectSingleton<RateCallerSettings>.instance;
        if (flag)
        {
            instance.Pop(false);
            GiftBoxScreen @object = instance.GetObject<GiftBoxScreen>();
            GiftBoxScreen.ShowArguments showArguments = default(GiftBoxScreen.ShowArguments);
            showArguments.giftsDefinition = currentGift.gifts;
            showArguments.title = "";
            currentGift.ClaimGifts();
            @object.Show(showArguments);
            return;
        }
        if (instance2.ShouldShow(Match3StagesDB.instance.passedStages))
        {
            instance.Pop(true);
            RatingScreen object2 = NavigationManager.instance.GetObject<RatingScreen>();
            NavigationManager.instance.Push(object2.gameObject, true);
            instance2.OnDialogShow();
            return;
        }
        instance.Pop(true);
        this.InitScene(this.scene, true);
        this.currencyPanel.SetLabels();
    }

    private void OnCoinsGiven()
    {
        this.InitScene(this.scene, true);
        this.currencyPanel.SetLabels();
    }

    public Vector3 TransformPSDToWorldPoint(Vector2 point)
    {
        return this.scene.PSDToWorldPoint(point);
    }

    public Vector3 TransformWorldToPSDPoint(Vector2 point)
    {
        return this.scene.WorldToPSDPoint(point);
    }

    public void ConfirmPurchasePanelCallback_OnConfirm(DecorateRoomSceneVisualItem uiItem)
    {
        SingleCurrencyPrice price = uiItem.visualObjectBehaviour.visualObject.sceneObjectInfo.price;
        WalletManager walletManager = GGPlayerSettings.instance.walletManager;
        GGUtil.SetActive(this.confirmPurchasePanel, false);
        UnityEngine.Debug.LogFormat("Price is {0} {1}", new object[]
        {
            price.cost,
            price.currency
        });
        if ((!Application.isEditor || !this.noCoinsForPurchase) && !walletManager.CanBuyItemWithPrice(price))
        {
            this.ButtonCallback_PlayButtonClick();
            return;
        }
        uiItem.visualObjectBehaviour.visualObject.isOwned = true;
        this.ShowVariations(uiItem, new VariationPanel.InitParams
        {
            isPurchased = true
        });
        walletManager.BuyItem(price);
        this.currencyPanel.SetLabels();
        Analytics.RoomItemBoughtEvent roomItemBoughtEvent = new Analytics.RoomItemBoughtEvent();
        roomItemBoughtEvent.price = price;
        roomItemBoughtEvent.screen = this;
        roomItemBoughtEvent.visualObject = uiItem.visualObjectBehaviour.visualObject;
        int ownedVariationIndex = uiItem.visualObjectBehaviour.visualObject.ownedVariationIndex;
        roomItemBoughtEvent.variation = uiItem.visualObjectBehaviour.visualObject.variations[ownedVariationIndex];
        roomItemBoughtEvent.numberOfItemsOwned = this.scene.ownedItemsCount;
        roomItemBoughtEvent.Send();
        GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
    }

    public void ConfirmPurchasePanelCallback_OnClosed()
    {
        this.InitScene(this.scene, false);
    }

    public void ButtonCallback_OnLivesClicked()
    {
        GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
        if (BehaviourSingleton<EnergyManager>.instance.isFreeEnergy)
        {
            return;
        }
        if (BehaviourSingleton<EnergyManager>.instance.ownedPlayCoins == EnergyControlConfig.instance.totalCoin)
        {
            return;
        }
        GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
        OutOfLivesDialog @object = NavigationManager.instance.GetObject<OutOfLivesDialog>();
        int index = Match3StagesDB.instance.currentStage.index;
        LivesPriceConfig.PriceConfig priceForLevelOrDefault = ScriptableObjectSingleton<LivesPriceConfig>.instance.GetPriceForLevelOrDefault(index);
        @object.Show(priceForLevelOrDefault, null, null, new Action(this.Init));
    }

    public void ButtonCallback_OnSettingsButtonPress()
    {
        NavigationManager instance = NavigationManager.instance;
        if (ConfigBase.instance.debug)
        {
            SettingsScreen @object = instance.GetObject<SettingsScreen>();
            instance.Push(@object, false);
            return;
        }
        InGameSettingsScreen object2 = instance.GetObject<InGameSettingsScreen>();
        instance.Push(object2, false);
    }

    public override void OnGoBack(NavigationManager nav)
    {
        DecorateRoomScreen._003C_003Ec__DisplayClass93_0 _003C_003Ec__DisplayClass93_ = new DecorateRoomScreen._003C_003Ec__DisplayClass93_0();
        _003C_003Ec__DisplayClass93_.nav = nav;
        base.OnGoBack(_003C_003Ec__DisplayClass93_.nav);
        Dialog.instance.Show("Exit Game?", "Stay", "Exit", new Action<bool>(_003C_003Ec__DisplayClass93_._003COnGoBack_003Eb__0));
    }

    [SerializeField]
    private List<Transform> levelDifficultyWidgets = new List<Transform>();

    [SerializeField]
    private VisualStyleSet normalDifficultySlyle = new VisualStyleSet();

    [SerializeField]
    private VisualStyleSet hardDifficultySlyle = new VisualStyleSet();

    [SerializeField]
    private VisualStyleSet nightmareDifficultySlyle = new VisualStyleSet();

    [SerializeField]
    private bool alwaysPlaySceneCompleteAnimation;

    [SerializeField]
    public TutorialHandController tutorialHand;

    [SerializeField]
    private ComponentPool speachBubblePool = new ComponentPool();

    [SerializeField]
    private CanvasGroup bubbleGroup;

    [SerializeField]
    private GroupFooter groupFooter;

    [SerializeField]
    private WideAspectBars aspectBars;

    [SerializeField]
    private StarConsumeAnimation starConsumeAnimation;

    [SerializeField]
    private List<DecorateRoomScreen.UIItemSetup> uiItemSetups = new List<DecorateRoomScreen.UIItemSetup>();

    [SerializeField]
    private float marginsPsdSize = 30f;

    [SerializeField]
    private bool useAccelerometer;

    [SerializeField]
    private ItemSelectionButton itemSelect;

    [SerializeField]
    private GameObject confettiParticle;

    [SerializeField]
    private VisualObjectParticles visualObjectParticles;

    private VisibilityHelper visibilityHelper = new VisibilityHelper();

    [SerializeField]
    private DecorateRoomScreen.Accelerometer accelerometer = new DecorateRoomScreen.Accelerometer();

    [SerializeField]
    public bool noCoinsForPurchase;

    [SerializeField]
    private TextMeshProUGUI levelCountLabel;

    [SerializeField]
    private ComponentPool visualItemsPool = new ComponentPool();

    [SerializeField]
    private List<RectTransform> widgetsToHide = new List<RectTransform>();

    [SerializeField]
    private List<RectTransform> controlWidgets = new List<RectTransform>();

    [SerializeField]
    private Image progressBarSprite;

    [SerializeField]
    private VisualStyleSet loadingSceneStyle = new VisualStyleSet();

    [SerializeField]
    private VisualStyleSet retryLoadingStyle = new VisualStyleSet();

    [SerializeField]
    private VisualStyleSet roomLoadedStyle = new VisualStyleSet();

    [SerializeField]
    private VariationPanel variationPanel;

    [SerializeField]
    private ConfirmPurchasePanel confirmPurchasePanel;

    [SerializeField]
    private MoneyPickupAnimation moneyPickupAnimation;

    [SerializeField]
    public RectTransform coinRect;

    [SerializeField]
    public RectTransform starRect;

    [SerializeField]
    public CurrencyPanel currencyPanel;

    [NonSerialized]
    private List<DecorateRoomSceneVisualItem> uiVisualItems = new List<DecorateRoomSceneVisualItem>();

    [SerializeField]
    private ScreenMessagePanel messagePanel;

    private IEnumerator updateEnumerator;

    private IEnumerator animationEnumerator;

    public DecorateRoomScreen.Room activeRoom;

    private int wonCoinsCount;

    public enum UIItemName
    {
        PlayButton,
        SettingsButton,
        CoinsBar,
        HeartsBar
    }

    [Serializable]
    public class UIItemSetup
    {
        public DecorateRoomScreen.UIItemName name;

        public Transform widget;
    }

    [Serializable]
    public class Accelerometer
    {
        public Vector3 currentPosition
        {
            get
            {
                return this._003CcurrentPosition_003Ek__BackingField;
            }
            protected set
            {
                this._003CcurrentPosition_003Ek__BackingField = value;
            }
        }

        public Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, -1f, 1f);
            position.y = Mathf.Clamp(position.y, -1f, 1f);
            position.z = Mathf.Clamp(position.z, -1f, 1f);
            return position;
        }

        public void Init()
        {
            this.currentPosition = this.ClampPosition(Input.acceleration);
        }

        public void Update()
        {
            Vector3 b = this.ClampPosition(Input.acceleration);
            this.currentPosition = Vector3.Lerp(this.currentPosition, b, this.lowPassFilter * Time.deltaTime);
        }

        private Vector3 _003CcurrentPosition_003Ek__BackingField;

        public float lowPassFilter = 0.5f;
    }

    public class Room
    {
        public Room(string name, DecoratingScene sceneBehaviour)
        {
            this.name = name;
            this.sceneBehaviour = sceneBehaviour;
        }

        public string name;

        public RoomsDB.Room loadedRoom;

        public Scene scene;

        public DecoratingScene sceneBehaviour;
    }

    private sealed class _003CDoLoadScene_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoLoadScene_003Ed__56(int _003C_003E1__state)
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
            DecorateRoomScreen decorateRoomScreen = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    {
                        this._003C_003E1__state = -1;
                        this._003CinAppPurchaseConfirm_003E5__2 = NavigationManager.instance.GetObject<InAppPurchaseConfirmScreen>();
                        if (this._003CinAppPurchaseConfirm_003E5__2 != null)
                        {
                            this._003CinAppPurchaseConfirm_003E5__2.SuspendShow();
                        }
                        InAppBackend instance = BehaviourSingletonInit<InAppBackend>.instance;
                        GGUtil.SetActive(decorateRoomScreen.widgetsToHide, false);
                        decorateRoomScreen.loadingSceneStyle.Apply();
                        this._003CroomRequest_003E5__3 = new RoomsDB.LoadRoomRequest(this.room);
                        RoomsDB instance2 = ScriptableObjectSingleton<RoomsDB>.instance;
                        this._003CupdateEnum_003E5__4 = instance2.LoadRoom(this._003CroomRequest_003E5__3);
                        GGUtil.SetFill(decorateRoomScreen.progressBarSprite, 0f);
                        break;
                    }
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    GGUtil.SetActive(decorateRoomScreen.widgetsToHide, false);
                    decorateRoomScreen.roomLoadedStyle.Apply();
                    if (this._003CinAppPurchaseConfirm_003E5__2 != null)
                    {
                        this._003CinAppPurchaseConfirm_003E5__2.ResumeShow();
                    }
                    decorateRoomScreen.InitScene(decorateRoomScreen.scene, true);
                    return false;
                default:
                    return false;
            }
            if (this._003CupdateEnum_003E5__4.MoveNext())
            {
                float progress = this._003CroomRequest_003E5__3.progress;
                GGUtil.SetFill(decorateRoomScreen.progressBarSprite, progress);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            DecoratingScene sceneBehaviour = this.room.sceneBehaviour;
            if (sceneBehaviour == null)
            {
                decorateRoomScreen.InitRetry();
                return false;
            }
            decorateRoomScreen.activeRoom = new DecorateRoomScreen.Room(this.room.name, sceneBehaviour);
            decorateRoomScreen.activeRoom.loadedRoom = this.room;
            GGUtil.SetActive(sceneBehaviour, true);
            decorateRoomScreen.scene.Init(NavigationManager.instance.GetCamera(), decorateRoomScreen.marginsPsdSize, decorateRoomScreen);
            this._003C_003E2__current = null;
            this._003C_003E1__state = 2;
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

        public DecorateRoomScreen _003C_003E4__this;

        public RoomsDB.Room room;

        private InAppPurchaseConfirmScreen _003CinAppPurchaseConfirm_003E5__2;

        private RoomsDB.LoadRoomRequest _003CroomRequest_003E5__3;

        private IEnumerator _003CupdateEnum_003E5__4;
    }

    private sealed class _003C_003Ec__DisplayClass58_0
    {
        public DecorateRoomScreen _003C_003E4__this;

        public DecoratingScene scene;

        public bool isFirstTime;
    }

    private sealed class _003C_003Ec__DisplayClass58_1
    {
        internal void _003CInitScene_003Eb__0(bool success)
        {
            if (!success)
            {
                Application.Quit();
                return;
            }
            GGPlayerSettings.instance.Model.acceptedTermsOfService = true;
            GGPlayerSettings.instance.Save();
            this.nav.Pop(true);
            this.CS_0024_003C_003E8__locals1._003C_003E4__this.InitScene(this.CS_0024_003C_003E8__locals1.scene, this.CS_0024_003C_003E8__locals1.isFirstTime);
        }

        public NavigationManager nav;

        public DecorateRoomScreen._003C_003Ec__DisplayClass58_0 CS_0024_003C_003E8__locals1;
    }

    private sealed class _003C_003Ec__DisplayClass59_0
    {
        internal void _003CDoShowCharacterAnimation_003Eb__0()
        {
            this.isComplete = true;
        }

        public bool isComplete;
    }

    private sealed class _003CDoShowCharacterAnimation_003Ed__59 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowCharacterAnimation_003Ed__59(int _003C_003E1__state)
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
            DecorateRoomScreen decorateRoomScreen = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003CwideBarsShown_003E5__2 = false;
                    this._003Ci_003E5__3 = 0;
                    goto IL_1F7;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_1C6;
                default:
                    return false;
            }
            IL_14E:
            if (!this._003C_003E8__1.isComplete)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            if (!this._003ChasMoreAnimations_003E5__4 && this._003CanimationParams_003E5__5.animation.leaveAfterInit)
            {
                goto IL_1D4;
            }
            decorateRoomScreen.scene.AnimateCharacterAlphaTo(0f);
            this._003Ctime_003E5__6 = 0f;
            this._003Cduration_003E5__7 = 0.25f;
            IL_1C6:
            if (this._003Ctime_003E5__6 <= this._003Cduration_003E5__7)
            {
                this._003Ctime_003E5__6 += Time.deltaTime;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            IL_1D4:
            this._003C_003E8__1 = null;
            this._003CanimationParams_003E5__5 = default(ChangeAnimationArguments);
            IL_1E7:
            int num2 = this._003Ci_003E5__3;
            this._003Ci_003E5__3 = num2 + 1;
            IL_1F7:
            if (this._003Ci_003E5__3 >= this.animationParamsList.Count)
            {
                if (this._003CwideBarsShown_003E5__2)
                {
                    decorateRoomScreen.aspectBars.AnimateHide();
                    this._003CwideBarsShown_003E5__2 = false;
                }
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
                else
                {
                    decorateRoomScreen.InitScene(decorateRoomScreen.scene, false);
                }
                return false;
            }
            this._003C_003E8__1 = new DecorateRoomScreen._003C_003Ec__DisplayClass59_0();
            bool flag = this._003Ci_003E5__3 == this.animationParamsList.Count - 1;
            this._003ChasMoreAnimations_003E5__4 = !flag;
            this._003CanimationParams_003E5__5 = this.animationParamsList[this._003Ci_003E5__3];
            if (this._003CanimationParams_003E5__5.isAnimationAvailable)
            {
                if (this._003CanimationParams_003E5__5.showWideBars && !this._003CwideBarsShown_003E5__2)
                {
                    decorateRoomScreen.aspectBars.AnimateShow();
                    this._003CwideBarsShown_003E5__2 = true;
                }
                if (this._003CwideBarsShown_003E5__2 && !this._003CanimationParams_003E5__5.showWideBars)
                {
                    decorateRoomScreen.aspectBars.AnimateHide();
                    this._003CwideBarsShown_003E5__2 = false;
                }
                decorateRoomScreen.scene.AnimateCharacterAlphaTo(1f);
                decorateRoomScreen.scene.SetCharacterAnimationAlpha(1f);
                this._003C_003E8__1.isComplete = false;
                this._003CanimationParams_003E5__5.onComplete = new Action(this._003C_003E8__1._003CDoShowCharacterAnimation_003Eb__0);
                decorateRoomScreen.scene.PlayCharacterAnimation(this._003CanimationParams_003E5__5);
                goto IL_14E;
            }
            goto IL_1E7;
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

        public List<ChangeAnimationArguments> animationParamsList;

        public DecorateRoomScreen _003C_003E4__this;

        private DecorateRoomScreen._003C_003Ec__DisplayClass59_0 _003C_003E8__1;

        public Action onComplete;

        private bool _003CwideBarsShown_003E5__2;

        private int _003Ci_003E5__3;

        private bool _003ChasMoreAnimations_003E5__4;

        private ChangeAnimationArguments _003CanimationParams_003E5__5;

        private float _003Ctime_003E5__6;

        private float _003Cduration_003E5__7;
    }

    private sealed class _003C_003Ec__DisplayClass60_0
    {
        internal void _003CDoShowCharacterAnimation_003Eb__0()
        {
            this.isComplete = true;
        }

        public bool isComplete;
    }

    private sealed class _003CDoShowCharacterAnimation_003Ed__60 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowCharacterAnimation_003Ed__60(int _003C_003E1__state)
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
            DecorateRoomScreen decorateRoomScreen = this._003C_003E4__this;
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
                if (!this.animationParams.isAnimationAvailable)
                {
                    goto IL_E4;
                }
                this._003C_003E8__1 = new DecorateRoomScreen._003C_003Ec__DisplayClass60_0();
                decorateRoomScreen.scene.AnimateCharacterAlphaTo(1f);
                decorateRoomScreen.scene.SetCharacterAnimationAlpha(1f);
                this._003C_003E8__1.isComplete = false;
                this.animationParams.onComplete = new Action(this._003C_003E8__1._003CDoShowCharacterAnimation_003Eb__0);
                decorateRoomScreen.scene.PlayCharacterAnimation(this.animationParams);
            }
            if (!this._003C_003E8__1.isComplete)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            if (!this.animationParams.animation.leaveAfterInit)
            {
                decorateRoomScreen.scene.AnimateCharacterAlphaTo(0f);
            }
            this._003C_003E8__1 = null;
            IL_E4:
            if (this.onComplete != null)
            {
                this.onComplete();
            }
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

        public ChangeAnimationArguments animationParams;

        public DecorateRoomScreen _003C_003E4__this;

        private DecorateRoomScreen._003C_003Ec__DisplayClass60_0 _003C_003E8__1;

        public Action onComplete;
    }

    private sealed class _003CDoShowMessageEnumerator_003Ed__61 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowMessageEnumerator_003Ed__61(int _003C_003E1__state)
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
            DecorateRoomScreen decorateRoomScreen = this._003C_003E4__this;
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
                this._003Cmessages_003E5__2 = this.progressMessages.messageList;
                decorateRoomScreen.HideSelectionUI();
                this._003Cenumerator_003E5__3 = null;
                this._003CmessageArguments_003E5__4 = default(TextDialog.MessageArguments);
                if (this.progressMessages.progressChange != null)
                {
                    this._003CmessageArguments_003E5__4.showProgress = true;
                    this._003CmessageArguments_003E5__4.fromProgress = this.progressMessages.progressChange.fromProgress;
                    this._003CmessageArguments_003E5__4.toProgress = this.progressMessages.progressChange.toProgress;
                }
                if (this._003Cmessages_003E5__2 != null)
                {
                    this._003Ci_003E5__5 = 0;
                    goto IL_13B;
                }
                goto IL_151;
            }
            IL_11E:
            if (this._003Cenumerator_003E5__3.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            int num2 = this._003Ci_003E5__5;
            this._003Ci_003E5__5 = num2 + 1;
            IL_13B:
            if (this._003Ci_003E5__5 < this._003Cmessages_003E5__2.Count)
            {
                string message = this._003Cmessages_003E5__2[this._003Ci_003E5__5];
                if (this._003Ci_003E5__5 == this._003Cmessages_003E5__2.Count - 1)
                {
                    this._003CmessageArguments_003E5__4.message = message;
                    this._003Cenumerator_003E5__3 = decorateRoomScreen.DoShowMessageEnumerator(this._003CmessageArguments_003E5__4);
                    goto IL_11E;
                }
                this._003Cenumerator_003E5__3 = decorateRoomScreen.DoShowMessageEnumerator(message);
                goto IL_11E;
            }
            IL_151:
            if (this.onComplete != null)
            {
                this.onComplete();
            }
            else
            {
                decorateRoomScreen.InitScene(decorateRoomScreen.scene, false);
            }
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

        public ProgressMessageList progressMessages;

        public DecorateRoomScreen _003C_003E4__this;

        public Action onComplete;

        private List<string> _003Cmessages_003E5__2;

        private IEnumerator _003Cenumerator_003E5__3;

        private TextDialog.MessageArguments _003CmessageArguments_003E5__4;

        private int _003Ci_003E5__5;
    }

    [Serializable]
    private sealed class _003C_003Ec
    {
        internal void _003CDoShowMessageEnumerator_003Eb__62_0()
        {
        }

        public static readonly DecorateRoomScreen._003C_003Ec _003C_003E9 = new DecorateRoomScreen._003C_003Ec();

        public static Action _003C_003E9__62_0;
    }

    private sealed class _003CDoShowMessageEnumerator_003Ed__62 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowMessageEnumerator_003Ed__62(int _003C_003E1__state)
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
            DecorateRoomScreen decorateRoomScreen = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    decorateRoomScreen.HideSelectionUI();
                    this._003Cenumerator_003E5__2 = null;
                    if (this.messages != null)
                    {
                        this._003Ci_003E5__3 = 0;
                        goto IL_9C;
                    }
                    goto IL_AF;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_101;
                default:
                    return false;
            }
            IL_7F:
            if (this._003Cenumerator_003E5__2.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            int num2 = this._003Ci_003E5__3;
            this._003Ci_003E5__3 = num2 + 1;
            IL_9C:
            if (this._003Ci_003E5__3 < this.messages.Count)
            {
                string message = this.messages[this._003Ci_003E5__3];
                this._003Cenumerator_003E5__2 = decorateRoomScreen.DoShowMessageEnumerator(message);
                goto IL_7F;
            }
            IL_AF:
            if (this.animationParamsList == null)
            {
                goto IL_115;
            }
            this._003CanimationParamEnum_003E5__4 = decorateRoomScreen.DoShowCharacterAnimation(this.animationParamsList, new Action(DecorateRoomScreen._003C_003Ec._003C_003E9._003CDoShowMessageEnumerator_003Eb__62_0));
            IL_101:
            if (this._003CanimationParamEnum_003E5__4.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            this._003CanimationParamEnum_003E5__4 = null;
            IL_115:
            if (this.onComplete != null)
            {
                this.onComplete();
            }
            else
            {
                decorateRoomScreen.InitScene(decorateRoomScreen.scene, false);
            }
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

        public DecorateRoomScreen _003C_003E4__this;

        public List<string> messages;

        public List<ChangeAnimationArguments> animationParamsList;

        public Action onComplete;

        private IEnumerator _003Cenumerator_003E5__2;

        private int _003Ci_003E5__3;

        private IEnumerator _003CanimationParamEnum_003E5__4;
    }

    private sealed class _003C_003Ec__DisplayClass64_0
    {
        internal void _003CDoShowMessageEnumerator_003Eb__0()
        {
            this.canContiue = true;
        }

        public bool canContiue;
    }

    private sealed class _003CDoShowMessageEnumerator_003Ed__64 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowMessageEnumerator_003Ed__64(int _003C_003E1__state)
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
            DecorateRoomScreen decorateRoomScreen = this._003C_003E4__this;
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
                this._003C_003E8__1 = new DecorateRoomScreen._003C_003Ec__DisplayClass64_0();
                decorateRoomScreen.visibilityHelper.Clear();
                decorateRoomScreen.visibilityHelper.SaveIsVisible(decorateRoomScreen.widgetsToHide);
                GGUtil.SetActive(decorateRoomScreen.widgetsToHide, false);
                this.messageArguments.message = this.messageArguments.message.Replace("\\n", "\n");
                TextDialog @object = NavigationManager.instance.GetObject<TextDialog>();
                this._003C_003E8__1.canContiue = false;
                @object.ShowOk(this.messageArguments, new Action(this._003C_003E8__1._003CDoShowMessageEnumerator_003Eb__0));
            }
            if (this._003C_003E8__1.canContiue)
            {
                decorateRoomScreen.visibilityHelper.Complete();
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

        public DecorateRoomScreen _003C_003E4__this;

        public TextDialog.MessageArguments messageArguments;

        private DecorateRoomScreen._003C_003Ec__DisplayClass64_0 _003C_003E8__1;
    }

    private sealed class _003C_003Ec__DisplayClass69_0
    {
        internal void _003COnCompleteRoom_003Eb__0()
        {
            this.nav.Pop(false);
            ScrollableSelectRoomScreen.ChangeRoomArguments changeRoomArguments = new ScrollableSelectRoomScreen.ChangeRoomArguments();
            changeRoomArguments.passedRoom = this.loadedRoom;
            changeRoomArguments.unlockedRoom = ScriptableObjectSingleton<RoomsDB>.instance.NextRoom(this.loadedRoom);
            this.nav.GetObject<ScrollableSelectRoomScreen>().Show(changeRoomArguments);
        }

        internal void _003COnCompleteRoom_003Eb__1()
        {
            this.nav.Pop(true);
            this.giftBoxScreen.Show(this.arguments);
        }

        public NavigationManager nav;

        public RoomsDB.Room loadedRoom;

        public GiftBoxScreen giftBoxScreen;

        public GiftBoxScreen.ShowArguments arguments;
    }

    private sealed class _003C_003Ec__DisplayClass82_0
    {
        internal void _003COnGameComplete_003Eb__0()
        {
            GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.coins, (int)this.winScreenArguments.coinsWon);
            this.winScreen.Show(this.winScreenArguments);
            GGSoundSystem.Play(GGSoundSystem.MusicType.MainMenuMusic);
        }

        public WinScreen.InitArguments winScreenArguments;

        public WinScreen winScreen;
    }

    private sealed class _003C_003Ec__DisplayClass93_0
    {
        internal void _003COnGoBack_003Eb__0(bool success)
        {
            if (success)
            {
                this.nav.Pop(true);
                return;
            }
            Application.Quit();
        }

        public NavigationManager nav;
    }
}
