using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssembleCarScreen : MonoBehaviour, Match3GameListener
{
    private void ShowConfettiParticle()
    {
        GGUtil.SetActive(UnityEngine.Object.Instantiate<GameObject>(this.confettiParticle, base.transform), true);
    }

    public CarScene scene
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
            return this.activeRoom != null && this.activeRoom.sceneBehaviour != null && this.activeRoom.loadedCar == ScriptableObjectSingleton<CarsDB>.instance.Active;
        }
    }

    private void OnEnable()
    {
        this.Init();
    }

    private void OnDisable()
    {
        if (!this.isRoomLoaded)
        {
            return;
        }
        GGUtil.SetActive(this.scene, false);
    }

    public void Init()
    {
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        GGUtil.ChangeText(this.levelCountLabel, currentStage.index + 1);
        if (this.isRoomLoaded)
        {
            GGUtil.SetActive(this.scene, true);
            GGUtil.SetActive(this.widgetsToHide, false);
            this.roomLoadedStyle.Apply();
            this.InitScene(this.scene, true);
            return;
        }
        CarsDB.Car active = ScriptableObjectSingleton<CarsDB>.instance.Active;
        this.LoadScene(active);
    }

    private void StopReactingToClick()
    {
        this.inputHandler.onClick -= this.OnInputHandlerClick;
    }

    private void StartReactingToClick()
    {
        this.inputHandler.onClick -= this.OnInputHandlerClick;
        this.inputHandler.onClick += this.OnInputHandlerClick;
    }

    private void OnInputHandlerClick(Vector2 position)
    {
        if (this.scene == null)
        {
            return;
        }
        CarCamera camera = this.scene.camera;
        if (camera == null)
        {
            return;
        }
        RaycastHit raycastHit;
        if (!Physics.Raycast(camera.ScreenPointToRay(position), out raycastHit))
        {
            return;
        }
        PartCollider component = raycastHit.collider.gameObject.GetComponent<PartCollider>();
        if (component == null)
        {
            return;
        }
        CarModelInfo.VariantGroup variantGroup = null;
        if (component.subpart != null)
        {
            variantGroup = component.subpart.firstVariantGroup;
        }
        CarModelPart part = component.part;
        if (part != null && variantGroup == null)
        {
            variantGroup = part.firstVariantGroup;
        }
        if (variantGroup == null)
        {
            return;
        }
        this.OnChangeVariant(variantGroup);
    }

    private void LoadScene(CarsDB.Car car)
    {
        GGUtil.SetActive(this.widgetsToHide, false);
        this.loadingSceneStyle.Apply();
        this.updateEnumerator = this.DoLoadScene(car);
    }

    private IEnumerator DoLoadScene(CarsDB.Car car)
    {
        return new AssembleCarScreen._003CDoLoadScene_003Ed__40(0)
        {
            _003C_003E4__this = this,
            car = car
        };
    }

    private void InitScene(CarScene carScene, bool isFirstTime)
    {
        GGUtil.SetActive(this.controlWidgets, true);
        this.StartReactingToClick();
        carScene.carModel.SetCollidersActive(true);
        carScene.carModel.InitExplodeAnimation();
        this.tasksButton.Show(carScene);
        this.visualItemsPool.Clear();
        this.uiButtons.Clear();
        this.interactionButtons.Clear();
        List<CarModelPart> parts = carScene.carModel.parts;
        this.scene.camera.SetStandardSettings();
        this.variationInteractionItemsPool.Clear();
        for (int i = 0; i < parts.Count; i++)
        {
            CarModelPart carModelPart = parts[i];
            if (carModelPart.partInfo.isOwned)
            {
                List<CarModelSubpart> subpartsWithInteraction = carModelPart.subpartsWithInteraction;
                for (int j = 0; j < subpartsWithInteraction.Count; j++)
                {
                    CarModelSubpart carModelSubpart = subpartsWithInteraction[j];
                    CarVariantInteractionButton carVariantInteractionButton = this.variationInteractionItemsPool.Next<CarVariantInteractionButton>(true);
                    carVariantInteractionButton.Init(new CarVariantInteractionButton.InitParams
                    {
                        positionToAttachTo = carModelSubpart.transform.position,
                        screen = this,
                        variantGroup = null,
                        subpart = carModelSubpart,
                        forwardTransform = carModelSubpart.transform,
                        forwardDirection = carModelSubpart.subpartInfo.rotateSettings.forwardDirection,
                        onClick = new Action<CarVariantInteractionButton>(this.OnRotateSubpart)
                    });
                    this.interactionButtons.Add(carVariantInteractionButton);
                }
            }
        }
        this.variationInteractionItemsPool.HideNotUsed();
        for (int k = 0; k < parts.Count; k++)
        {
            parts[k].SetActiveIfOwned();
        }
        this.visualItemsPool.HideNotUsed();
        this.slider.Init(this);
        GGUtil.SetActive(this.slider, false);
    }

    private void OnChangeVariant(CarVariantInteractionButton button)
    {
        this.OnChangeVariant(button.variantGroup);
    }

    private void OnRotateSubpart(CarVariantInteractionButton button)
    {
        if (button == null || button.subpart == null)
        {
            return;
        }
        button.subpart.ChangeRotation();
    }

    private void OnChangeVariant(CarModelInfo.VariantGroup variantGroup)
    {
        this.HideAllButtons();
        this.StopReactingToClick();
        this.scene.carModel.SetCollidersActive(false);
        CarVariationPanel.InitParams initParams = default(CarVariationPanel.InitParams);
        initParams.screen = this;
        initParams.variantGroup = variantGroup;
        initParams.onChange = new Action<CarVariationPanel>(this.OnVariantPanelChanged);
        initParams.onClosed = new Action<CarVariationPanel>(this.OnVariantPanelClosed);
        initParams.inputHandler = this.inputHandler;
        initParams.showBackground = false;
        this.variationPanel.Show(initParams);
        CarCamera.Settings carCamera = this.scene.camera.GetCarCamera(variantGroup.cameraName);
        if (carCamera != null)
        {
            this.scene.camera.AnimateIntoSettings(carCamera);
        }
        CarModelSubpart.ShowChange(this.scene.carModel.AllOwnedSubpartsInVariantGroup(variantGroup), 1f);
    }

    private void OnVariantPanelChanged(CarVariationPanel panel)
    {
        CarModelSubpart.ShowChange(this.scene.carModel.AllOwnedSubpartsInVariantGroup(panel.variantGroup), 1f);
    }

    private void OnVariantPanelClosed(CarVariationPanel panel)
    {
        CarModelSubpart.ShowChange(this.scene.carModel.AllOwnedSubpartsInVariantGroup(panel.variantGroup), 0.4f);
        this.InitScene(this.scene, false);
    }

    private void HideAllButtons()
    {
        GGUtil.SetActive(this.slider, false);
        for (int i = 0; i < this.uiButtons.Count; i++)
        {
            this.uiButtons[i].HideButton();
        }
        for (int j = 0; j < this.interactionButtons.Count; j++)
        {
            this.interactionButtons[j].HideButton();
        }
    }

    public void VisualItemCallback_OnBuyItemPressed(CarVisualItemButton button)
    {
        this.HideAllButtons();
        this.StopReactingToClick();
        CarConfirmPurchase.InitArguments initArguments = default(CarConfirmPurchase.InitArguments);
        initArguments.screen = this;
        initArguments.buttonHandlePosition = button.part.buttonHandlePosition;
        initArguments.displayName = button.part.partInfo.displayName;
        initArguments.carPart = button.part;
        initArguments.onSuccess = new Action<CarConfirmPurchase.InitArguments>(this.ConfirmPurchasePanelCallback_OnConfirm);
        initArguments.onCancel = new Action(this.ConfirmPurchasePanelCallback_OnClosed);
        initArguments.updateDirection = true;
        initArguments.directionHandlePosition = button.part.directionHandlePosition;
        initArguments.showBackground = false;
        initArguments.inputHandler = this.inputHandler;
        this.confirmPurchase.Show(initArguments);
        CarCamera.Settings carCamera = this.scene.camera.GetCarCamera(button.part.partInfo.cameraName);
        if (carCamera != null)
        {
            this.scene.camera.AnimateIntoSettings(carCamera);
        }
    }

    public void ConfirmPurchasePanelCallback_OnConfirm(CarConfirmPurchase.InitArguments initArguments)
    {
        CarModelPart carPart = initArguments.carPart;
        SingleCurrencyPrice price = new SingleCurrencyPrice(1, CurrencyType.diamonds);
        WalletManager walletManager = GGPlayerSettings.instance.walletManager;
        if (!walletManager.CanBuyItemWithPrice(price))
        {
            this.ButtonCallback_PlayButtonClick();
            return;
        }
        walletManager.BuyItem(price);
        this.currencyPanel.SetLabels();
        carPart.partInfo.isOwned = true;
        this.StopReactingToClick();
        this.animationEnumerator = this.ShowNewPart(carPart);
        this.animationEnumerator.MoveNext();
    }

    public void ConfirmPurchasePanelCallback_OnClosed()
    {
        this.InitScene(this.scene, false);
    }

    private IEnumerator ShowCarSpray(CarModelPart part)
    {
        return new AssembleCarScreen._003CShowCarSpray_003Ed__52(0)
        {
            _003C_003E4__this = this,
            part = part
        };
    }

    private IEnumerator ShowNewPart(CarModelPart part)
    {
        return new AssembleCarScreen._003CShowNewPart_003Ed__53(0)
        {
            _003C_003E4__this = this,
            part = part
        };
    }

    private void OnCompleteRoom()
    {
        AssembleCarScreen._003C_003Ec__DisplayClass54_0 _003C_003Ec__DisplayClass54_ = new AssembleCarScreen._003C_003Ec__DisplayClass54_0();
        _003C_003Ec__DisplayClass54_.nav = NavigationManager.instance;
        _003C_003Ec__DisplayClass54_.giftBoxScreen = _003C_003Ec__DisplayClass54_.nav.GetObject<GiftBoxScreen>();
        _003C_003Ec__DisplayClass54_.loadedRoom = this.activeRoom.loadedCar;
        _003C_003Ec__DisplayClass54_.arguments = default(GiftBoxScreen.ShowArguments);
        _003C_003Ec__DisplayClass54_.arguments.title = "Car Complete";
        _003C_003Ec__DisplayClass54_.arguments.giftsDefinition = _003C_003Ec__DisplayClass54_.loadedRoom.giftDefinition;
        _003C_003Ec__DisplayClass54_.arguments.onComplete = new Action(_003C_003Ec__DisplayClass54_._003COnCompleteRoom_003Eb__0);
        _003C_003Ec__DisplayClass54_.nav.GetObject<WellDoneScreen>().Show(new WellDoneScreen.InitArguments
        {
            mainText = "Car Complete",
            onComplete = new Action(_003C_003Ec__DisplayClass54_._003COnCompleteRoom_003Eb__1)
        });
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
        return new AssembleCarScreen._003CDoShowMessageEnumerator_003Ed__56(0)
        {
            _003C_003E4__this = this,
            messageArguments = messageArguments
        };
    }

    private IEnumerator DoShowMessageEnumerator(ProgressMessageList progressMessages, Action onComplete = null)
    {
        return new AssembleCarScreen._003CDoShowMessageEnumerator_003Ed__57(0)
        {
            _003C_003E4__this = this,
            progressMessages = progressMessages,
            onComplete = onComplete
        };
    }

    public void ButtonCallback_Tasks()
    {
        CompletionDialog @object = NavigationManager.instance.GetObject<CompletionDialog>();
        List<CarModelPart> list = this.scene.carModel.AvailablePartsAsTasks();
        this.tasksButton.HideAnimation();
        CompletionDialog.InitArguments initArguments = new CompletionDialog.InitArguments();
        initArguments.onComplete = new Action<CompletionDialog.InitArguments.Task>(this.OnTaskSelected);
        initArguments.onCancel = new Action(this.OnTaskDialogClosed);
        initArguments.showModal = true;
        for (int i = 0; i < list.Count; i++)
        {
            CarModelPart carModelPart = list[i];
            CompletionDialog.InitArguments.Task item = default(CompletionDialog.InitArguments.Task);
            item.name = carModelPart.partInfo.displayName;
            item.price = 1;
            item.part = carModelPart;
            initArguments.tasks.Add(item);
        }
        @object.Show(initArguments);
    }

    private void OnTaskDialogClosed()
    {
        NavigationManager.instance.Pop(true);
        this.InitScene(this.scene, false);
    }

    private void OnTaskSelected(CompletionDialog.InitArguments.Task task)
    {
        NavigationManager.instance.Pop(true);
        CarModelPart part = task.part;
        if (part == null)
        {
            return;
        }
        WalletManager walletManager = GGPlayerSettings.instance.walletManager;
        SingleCurrencyPrice price = new SingleCurrencyPrice(task.price, CurrencyType.diamonds);
        if (!walletManager.CanBuyItemWithPrice(price))
        {
            this.ButtonCallback_PlayButtonClick();
            return;
        }
        this.HideAllButtons();
        this.StopReactingToClick();
        CarCamera.Settings carCamera = this.scene.camera.GetCarCamera(part.partInfo.cameraName);
        if (carCamera != null)
        {
            this.scene.camera.AnimateIntoSettings(carCamera);
        }
        walletManager.BuyItem(price);
        this.currencyPanel.SetLabels();
        part.partInfo.isOwned = true;
        this.StopReactingToClick();
        this.animationEnumerator = this.ShowNewPart(part);
        this.animationEnumerator.MoveNext();
    }

    public void ButtonCallback_OnRetry()
    {
        this.Init();
    }

    public Vector3 TransformWorldCarPointToLocalUIPosition(Vector3 worldCarPoint)
    {
        Camera camera = NavigationManager.instance.GetCamera();
        if (this.scene == null || this.scene.camera == null || camera == null)
        {
            return Vector3.zero;
        }
        Vector3 position = this.scene.camera.WorldToScreenPoint(worldCarPoint);
        Vector3 position2 = camera.ScreenToWorldPoint(position);
        return base.transform.InverseTransformPoint(position2);
    }

    public bool IsFacingCamera(Vector3 forward)
    {
        Camera camera = NavigationManager.instance.GetCamera();
        return this.scene == null || this.scene.camera == null || camera == null || Vector3.Dot(this.scene.camera.cameraForward, forward) < 0f;
    }

    private void Update()
    {
        if (this.updateEnumerator != null && !this.updateEnumerator.MoveNext())
        {
            this.updateEnumerator = null;
        }
        if (this.animationEnumerator != null && !this.animationEnumerator.MoveNext())
        {
            this.animationEnumerator = null;
        }
        if (this.scene != null && this.animationEnumerator == null)
        {
            if (Application.isEditor && UnityEngine.Input.GetKeyDown(KeyCode.N))
            {
                CarModelPart carModelPart = null;
                List<CarModelPart> parts = this.scene.carModel.parts;
                for (int i = 0; i < parts.Count; i++)
                {
                    CarModelPart carModelPart2 = parts[i];
                    if (!carModelPart2.partInfo.isOwned && (carModelPart == null || carModelPart.partInfo.groupIndex > carModelPart2.partInfo.groupIndex))
                    {
                        carModelPart = carModelPart2;
                    }
                }
                if (carModelPart != null)
                {
                    carModelPart.partInfo.isOwned = true;
                }
                this.InitScene(this.scene, false);
            }
            if (Application.isEditor && UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                CarModelPart carModelPart3 = null;
                List<CarModelPart> parts2 = this.scene.carModel.parts;
                for (int j = 0; j < parts2.Count; j++)
                {
                    CarModelPart carModelPart4 = parts2[j];
                    if (carModelPart4.partInfo.isOwned && (carModelPart3 == null || carModelPart3.partInfo.groupIndex < carModelPart4.partInfo.groupIndex))
                    {
                        carModelPart3 = carModelPart4;
                    }
                }
                if (carModelPart3 != null)
                {
                    carModelPart3.partInfo.isOwned = false;
                }
                this.InitScene(this.scene, false);
            }
        }
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
        initParams.disableBackground = true;
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
        if (this.scene != null)
        {
            GGUtil.SetActive(this.scene, true);
        }
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
        this.StopReactingToClick();
        this.HideAllButtons();
        GGUtil.SetActive(this.widgetsToHide, false);
        Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
        @object.Show(currentStage, null, new Action(this.Init), new Action<Match3GameParams>(this.StartGame));
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
        }
        if (stage != null)
        {
            stage.OnGameComplete(completeParams);
        }
        if (completeParams.isWin)
        {
            AssembleCarScreen._003C_003Ec__DisplayClass72_0 _003C_003Ec__DisplayClass72_ = new AssembleCarScreen._003C_003Ec__DisplayClass72_0();
            NavigationManager instance = NavigationManager.instance;
            _003C_003Ec__DisplayClass72_.winScreen = instance.GetObject<WinScreen>();
            WalletManager walletManager = GGPlayerSettings.instance.walletManager;
            _003C_003Ec__DisplayClass72_.winScreenArguments = new WinScreen.InitArguments();
            _003C_003Ec__DisplayClass72_.winScreenArguments.previousCoins = walletManager.CurrencyCount(CurrencyType.coins);
            _003C_003Ec__DisplayClass72_.winScreenArguments.baseStageWonCoins = 50L;
            _003C_003Ec__DisplayClass72_.winScreenArguments.previousStars = walletManager.CurrencyCount(CurrencyType.diamonds);
            _003C_003Ec__DisplayClass72_.winScreenArguments.currentStars = _003C_003Ec__DisplayClass72_.winScreenArguments.previousStars + 1L;
            _003C_003Ec__DisplayClass72_.winScreenArguments.onMiddle = new Action(this.OnWinScreenAnimationMiddle);
            _003C_003Ec__DisplayClass72_.winScreenArguments.game = game;
            _003C_003Ec__DisplayClass72_.winScreenArguments.decorateRoomScreen = null;
            _003C_003Ec__DisplayClass72_.winScreenArguments.starRect = this.starRect;
            _003C_003Ec__DisplayClass72_.winScreenArguments.coinRect = this.coinRect;
            _003C_003Ec__DisplayClass72_.winScreenArguments.currencyPanel = this.currencyPanel;
            game.StartWinAnimation(_003C_003Ec__DisplayClass72_.winScreenArguments, new Action(_003C_003Ec__DisplayClass72_._003COnGameComplete_003Eb__0));
            this.wonCoinsCount = stage.coinsCount;
            if (!flag)
            {
                GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.diamonds, Match3Settings.instance.moneyPickupAnimationSettings.numberOfStars);
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
        if (currentGift != null && currentGift.isAvailableToCollect)
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
        instance.Pop(true);
        this.currencyPanel.SetLabels();
    }

    private void OnWinScreenAnimationComplete()
    {
        GameScreen @object = NavigationManager.instance.GetObject<GameScreen>();
        NavigationManager.instance.Pop(true);
        @object.DestroyCreatedGameObjects();
        this.currencyPanel.SetLabels();
    }

    private void OnCoinsGiven()
    {
        this.currencyPanel.SetLabels();
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

    [SerializeField]
    private TasksButton tasksButton;

    [SerializeField]
    private CarSprayTool sprayTool;

    [SerializeField]
    public ScrewdriverTool screwdriverTool;

    [SerializeField]
    public CarConfirmPurchase confirmPurchase;

    [SerializeField]
    public InputHandler inputHandler;

    [SerializeField]
    private Image progressBarSprite;

    [SerializeField]
    private GameObject confettiParticle;

    [SerializeField]
    private TextMeshProUGUI levelCountLabel;

    [SerializeField]
    private ComponentPool visualItemsPool = new ComponentPool();

    [SerializeField]
    private ComponentPool variationInteractionItemsPool = new ComponentPool();

    [SerializeField]
    private List<RectTransform> widgetsToHide = new List<RectTransform>();

    [SerializeField]
    private List<RectTransform> controlWidgets = new List<RectTransform>();

    [SerializeField]
    private VisualStyleSet loadingSceneStyle = new VisualStyleSet();

    [SerializeField]
    private VisualStyleSet retryLoadingStyle = new VisualStyleSet();

    [SerializeField]
    private VisualStyleSet roomLoadedStyle = new VisualStyleSet();

    [SerializeField]
    public RectTransform coinRect;

    [SerializeField]
    public RectTransform starRect;

    [SerializeField]
    public CurrencyPanel currencyPanel;

    [SerializeField]
    public CarVariationPanel variationPanel;

    [SerializeField]
    public TutorialHandController tutorialHand;

    [SerializeField]
    private ExplodeSlider slider;

    private VisibilityHelper visibilityHelper = new VisibilityHelper();

    private List<CarVisualItemButton> uiButtons = new List<CarVisualItemButton>();

    private List<CarVariantInteractionButton> interactionButtons = new List<CarVariantInteractionButton>();

    private IEnumerator updateEnumerator;

    private IEnumerator animationEnumerator;

    public AssembleCarScreen.LoadedAsset activeRoom;

    private List<string> toSayAfterOpen = new List<string>();

    private int wonCoinsCount;

    public class LoadedAsset
    {
        public LoadedAsset(string name, CarScene sceneBehaviour)
        {
            this.name = name;
            this.sceneBehaviour = sceneBehaviour;
        }

        public string name;

        public CarsDB.Car loadedCar;

        public Scene scene;

        public CarScene sceneBehaviour;
    }

    private sealed class _003CDoLoadScene_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoLoadScene_003Ed__40(int _003C_003E1__state)
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
            AssembleCarScreen assembleCarScreen = this._003C_003E4__this;
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
                GGUtil.SetActive(assembleCarScreen.widgetsToHide, false);
                assembleCarScreen.loadingSceneStyle.Apply();
                this._003CroomRequest_003E5__2 = new CarsDB.LoadCarRequest(this.car);
                CarsDB instance = ScriptableObjectSingleton<CarsDB>.instance;
                this._003CupdateEnum_003E5__3 = instance.Load(this._003CroomRequest_003E5__2);
                GGUtil.SetFill(assembleCarScreen.progressBarSprite, 0f);
            }
            if (this._003CupdateEnum_003E5__3.MoveNext())
            {
                float progress = this._003CroomRequest_003E5__2.progress;
                GGUtil.SetFill(assembleCarScreen.progressBarSprite, progress);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            if (this.car.rootTransform == null)
            {
                GGUtil.SetActive(assembleCarScreen.widgetsToHide, false);
                assembleCarScreen.retryLoadingStyle.Apply();
                return false;
            }
            GGUtil.SetActive(assembleCarScreen.widgetsToHide, false);
            assembleCarScreen.roomLoadedStyle.Apply();
            CarScene sceneBehaviour = this.car.sceneBehaviour;
            assembleCarScreen.activeRoom = new AssembleCarScreen.LoadedAsset(this.car.name, this.car.sceneBehaviour);
            assembleCarScreen.activeRoom.loadedCar = this.car;
            GGUtil.SetActive(sceneBehaviour, true);
            sceneBehaviour.carModel.InitForRuntime();
            sceneBehaviour.camera.Init(assembleCarScreen.inputHandler);
            GGUtil.SetActive(assembleCarScreen.widgetsToHide, false);
            assembleCarScreen.roomLoadedStyle.Apply();
            assembleCarScreen.InitScene(this.car.sceneBehaviour, true);
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

        public AssembleCarScreen _003C_003E4__this;

        public CarsDB.Car car;

        private CarsDB.LoadCarRequest _003CroomRequest_003E5__2;

        private IEnumerator _003CupdateEnum_003E5__3;
    }

    private sealed class _003C_003Ec__DisplayClass52_0
    {
        internal void _003CShowCarSpray_003Eb__0()
        {
            this.isSprayingDone = true;
        }

        public bool isSprayingDone;
    }

    private sealed class _003CShowCarSpray_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CShowCarSpray_003Ed__52(int _003C_003E1__state)
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
            AssembleCarScreen assembleCarScreen = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this.part.HideSubparts();
                    for (int i = 0; i < this.part.paintTransformations.Count; i++)
                    {
                        GGUtil.Hide(this.part.paintTransformations[i]);
                    }
                    this._003CtoSayBeforeOpen_003E5__2 = this.part.partInfo.toSayBefore;
                    this._003Ci_003E5__3 = 0;
                    goto IL_20C;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_1BE;
                default:
                    return false;
            }
            IL_144:
            if (this._003CbeforeDialog_003E5__5.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003CbeforeDialog_003E5__5 = null;
            IL_158:
            this._003C_003E8__1.isSprayingDone = false;
            CarSprayTool.InitArguments initArguments = default(CarSprayTool.InitArguments);
            initArguments.screen = assembleCarScreen;
            initArguments.paintTransformation = this._003CpaintTransformation_003E5__4;
            initArguments.onDone = new Action(this._003C_003E8__1._003CShowCarSpray_003Eb__0);
            assembleCarScreen.sprayTool.Init(initArguments);
            IL_1BE:
            if (!this._003C_003E8__1.isSprayingDone)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            GGUtil.Hide(this._003CpaintTransformation_003E5__4);
            this._003CpaintTransformation_003E5__4.ReleaseAll();
            GGUtil.Hide(assembleCarScreen.sprayTool);
            this._003C_003E8__1 = null;
            this._003CpaintTransformation_003E5__4 = null;
            int num2 = this._003Ci_003E5__3;
            this._003Ci_003E5__3 = num2 + 1;
            IL_20C:
            if (this._003Ci_003E5__3 >= this.part.paintTransformations.Count)
            {
                for (int j = 0; j < this.part.paintTransformations.Count; j++)
                {
                    PaintTransformation paintTransformation = this.part.paintTransformations[j];
                    GGUtil.Hide(paintTransformation);
                    paintTransformation.ReleaseAll();
                }
                this.part.SetActiveIfOwned();
                return false;
            }
            this._003C_003E8__1 = new AssembleCarScreen._003C_003Ec__DisplayClass52_0();
            this._003CpaintTransformation_003E5__4 = this.part.paintTransformations[this._003Ci_003E5__3];
            GGUtil.Show(this._003CpaintTransformation_003E5__4);
            this._003CpaintTransformation_003E5__4.Init();
            this._003CpaintTransformation_003E5__4.ClearTexturesToColor(Color.black);
            if (this._003Ci_003E5__3 == 0 && this._003CtoSayBeforeOpen_003E5__2.Count > 0)
            {
                TalkingDialog @object = NavigationManager.instance.GetObject<TalkingDialog>();
                TalkingDialog.ShowArguments showArguments = new TalkingDialog.ShowArguments();
                showArguments.inputHandler = assembleCarScreen.inputHandler;
                showArguments.thingsToSay.AddRange(this._003CtoSayBeforeOpen_003E5__2);
                this._003CbeforeDialog_003E5__5 = @object.DoShow(showArguments);
                goto IL_144;
            }
            goto IL_158;
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

        public CarModelPart part;

        public AssembleCarScreen _003C_003E4__this;

        private AssembleCarScreen._003C_003Ec__DisplayClass52_0 _003C_003E8__1;

        private List<string> _003CtoSayBeforeOpen_003E5__2;

        private int _003Ci_003E5__3;

        private PaintTransformation _003CpaintTransformation_003E5__4;

        private IEnumerator _003CbeforeDialog_003E5__5;
    }

    private sealed class _003CShowNewPart_003Ed__53 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CShowNewPart_003Ed__53(int _003C_003E1__state)
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
            AssembleCarScreen assembleCarScreen = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    GGUtil.Hide(assembleCarScreen.controlWidgets);
                    assembleCarScreen.slider.StopSlider();
                    if (!assembleCarScreen.slider.isExploded)
                    {
                        goto IL_96;
                    }
                    this._003CslideBackEnum_003E5__4 = assembleCarScreen.slider.Unexplode();
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_100;
                case 3:
                    this._003C_003E1__state = -1;
                    goto IL_196;
                case 4:
                    this._003C_003E1__state = -1;
                    goto IL_1D5;
                case 5:
                    this._003C_003E1__state = -1;
                    goto IL_358;
                default:
                    return false;
            }
            if (this._003CslideBackEnum_003E5__4.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            this._003CslideBackEnum_003E5__4 = null;
            IL_96:
            this.part.model.RefreshVisibilityOnParts();
            this.part.SetActiveIfOwned();
            this.part.partInfo.selectedVariation = 0;
            if (this.part.paintTransformations.Count > 0)
            {
                this._003CslideBackEnum_003E5__4 = assembleCarScreen.ShowCarSpray(this.part);
            }
            else
            {
                this.part.ShowSubpartsIfRemoving();
                List<string> toSayBefore = this.part.partInfo.toSayBefore;
                if (toSayBefore.Count > 0)
                {
                    TalkingDialog @object = NavigationManager.instance.GetObject<TalkingDialog>();
                    TalkingDialog.ShowArguments showArguments = new TalkingDialog.ShowArguments();
                    showArguments.inputHandler = assembleCarScreen.inputHandler;
                    showArguments.thingsToSay.AddRange(toSayBefore);
                    this._003CbeforeDialog_003E5__5 = @object.DoShow(showArguments);
                    goto IL_196;
                }
                goto IL_1AA;
            }
            IL_100:
            if (!this._003CslideBackEnum_003E5__4.MoveNext())
            {
                this._003CslideBackEnum_003E5__4 = null;
                goto IL_1E9;
            }
            this._003C_003E2__current = null;
            this._003C_003E1__state = 2;
            return true;
            IL_196:
            if (this._003CbeforeDialog_003E5__5.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 3;
                return true;
            }
            this._003CbeforeDialog_003E5__5 = null;
            IL_1AA:
            this._003CslideBackEnum_003E5__4 = this.part.AnimateIn(assembleCarScreen);
            IL_1D5:
            if (this._003CslideBackEnum_003E5__4.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 4;
                return true;
            }
            this._003CslideBackEnum_003E5__4 = null;
            IL_1E9:
            assembleCarScreen.toSayAfterOpen.Clear();
            if (this.part.partInfo.hasSomethingToSay)
            {
                assembleCarScreen.toSayAfterOpen.Add(this.part.partInfo.thingToSay);
            }
            if (assembleCarScreen.toSayAfterOpen.Count == 0)
            {
                assembleCarScreen.toSayAfterOpen.Add("Great job!");
            }
            bool isPassed = assembleCarScreen.scene.carModel.isPassed;
            ProgressMessageList progressMessageList = new ProgressMessageList();
            progressMessageList.messageList = assembleCarScreen.toSayAfterOpen;
            CarModel.ProgressState progressState = assembleCarScreen.scene.carModel.GetProgressState();
            this._003CisRoomComplete_003E5__2 = false;
            if (progressState.isPassed && !isPassed)
            {
                assembleCarScreen.scene.carModel.isPassed = true;
                this._003CisRoomComplete_003E5__2 = true;
            }
            progressMessageList.progressChange = new ProgressMessageList.ProgressChange();
            progressMessageList.progressChange.fromProgress = progressState.Progress(1);
            progressMessageList.progressChange.toProgress = progressState.progress;
            this._003CenumeratorToAnimate_003E5__3 = null;
            if (assembleCarScreen.toSayAfterOpen.Count > 0)
            {
                assembleCarScreen.ShowConfettiParticle();
                TalkingDialog object2 = NavigationManager.instance.GetObject<TalkingDialog>();
                TalkingDialog.ShowArguments showArguments2 = new TalkingDialog.ShowArguments();
                showArguments2.inputHandler = assembleCarScreen.inputHandler;
                showArguments2.thingsToSay.AddRange(assembleCarScreen.toSayAfterOpen);
                this._003CenumeratorToAnimate_003E5__3 = object2.DoShow(showArguments2);
                assembleCarScreen.scene.camera.SetStandardSettings();
            }
            if (this._003CenumeratorToAnimate_003E5__3 == null)
            {
                goto IL_375;
            }
            IL_358:
            if (this._003CenumeratorToAnimate_003E5__3.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 5;
                return true;
            }
            assembleCarScreen.scene.carModel.ShowChnage();
            IL_375:
            if (this._003CisRoomComplete_003E5__2)
            {
                assembleCarScreen.OnCompleteRoom();
            }
            else
            {
                assembleCarScreen.InitScene(assembleCarScreen.scene, false);
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

        public AssembleCarScreen _003C_003E4__this;

        public CarModelPart part;

        private bool _003CisRoomComplete_003E5__2;

        private IEnumerator _003CenumeratorToAnimate_003E5__3;

        private IEnumerator _003CslideBackEnum_003E5__4;

        private IEnumerator _003CbeforeDialog_003E5__5;
    }

    private sealed class _003C_003Ec__DisplayClass54_0
    {
        internal void _003COnCompleteRoom_003Eb__0()
        {
            this.nav.Pop(false);
            ScrollableSelectCarScreen.ChangeCarArguments changeCarArguments = new ScrollableSelectCarScreen.ChangeCarArguments();
            changeCarArguments.passedCar = this.loadedRoom;
            changeCarArguments.unlockedCar = ScriptableObjectSingleton<CarsDB>.instance.NextCar(this.loadedRoom);
            this.nav.GetObject<ScrollableSelectCarScreen>().Show(changeCarArguments);
        }

        internal void _003COnCompleteRoom_003Eb__1()
        {
            this.nav.Pop(true);
            this.giftBoxScreen.Show(this.arguments);
        }

        public NavigationManager nav;

        public CarsDB.Car loadedRoom;

        public GiftBoxScreen giftBoxScreen;

        public GiftBoxScreen.ShowArguments arguments;
    }

    private sealed class _003C_003Ec__DisplayClass56_0
    {
        internal void _003CDoShowMessageEnumerator_003Eb__0()
        {
            this.canContiue = true;
        }

        public bool canContiue;
    }

    private sealed class _003CDoShowMessageEnumerator_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowMessageEnumerator_003Ed__56(int _003C_003E1__state)
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
            AssembleCarScreen assembleCarScreen = this._003C_003E4__this;
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
                this._003C_003E8__1 = new AssembleCarScreen._003C_003Ec__DisplayClass56_0();
                assembleCarScreen.visibilityHelper.Clear();
                assembleCarScreen.visibilityHelper.SaveIsVisible(assembleCarScreen.widgetsToHide);
                GGUtil.SetActive(assembleCarScreen.widgetsToHide, false);
                this.messageArguments.message = this.messageArguments.message.Replace("\\n", "\n");
                TextDialog @object = NavigationManager.instance.GetObject<TextDialog>();
                this._003C_003E8__1.canContiue = false;
                @object.ShowOk(this.messageArguments, new Action(this._003C_003E8__1._003CDoShowMessageEnumerator_003Eb__0));
            }
            if (this._003C_003E8__1.canContiue)
            {
                assembleCarScreen.visibilityHelper.Complete();
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

        public AssembleCarScreen _003C_003E4__this;

        public TextDialog.MessageArguments messageArguments;

        private AssembleCarScreen._003C_003Ec__DisplayClass56_0 _003C_003E8__1;
    }

    private sealed class _003CDoShowMessageEnumerator_003Ed__57 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoShowMessageEnumerator_003Ed__57(int _003C_003E1__state)
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
            AssembleCarScreen assembleCarScreen = this._003C_003E4__this;
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
                    goto IL_135;
                }
                goto IL_14B;
            }
            IL_118:
            if (this._003Cenumerator_003E5__3.MoveNext())
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            int num2 = this._003Ci_003E5__5;
            this._003Ci_003E5__5 = num2 + 1;
            IL_135:
            if (this._003Ci_003E5__5 < this._003Cmessages_003E5__2.Count)
            {
                string message = this._003Cmessages_003E5__2[this._003Ci_003E5__5];
                if (this._003Ci_003E5__5 == this._003Cmessages_003E5__2.Count - 1)
                {
                    this._003CmessageArguments_003E5__4.message = message;
                    this._003Cenumerator_003E5__3 = assembleCarScreen.DoShowMessageEnumerator(this._003CmessageArguments_003E5__4);
                    goto IL_118;
                }
                this._003Cenumerator_003E5__3 = assembleCarScreen.DoShowMessageEnumerator(message);
                goto IL_118;
            }
            IL_14B:
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

        public ProgressMessageList progressMessages;

        public AssembleCarScreen _003C_003E4__this;

        public Action onComplete;

        private List<string> _003Cmessages_003E5__2;

        private IEnumerator _003Cenumerator_003E5__3;

        private TextDialog.MessageArguments _003CmessageArguments_003E5__4;

        private int _003Ci_003E5__5;
    }

    private sealed class _003C_003Ec__DisplayClass72_0
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
}
