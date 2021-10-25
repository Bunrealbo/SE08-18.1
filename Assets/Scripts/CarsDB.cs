using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarsDB : ScriptableObjectSingleton<CarsDB>
{
    public CarSettings GetCarSettings(string carName)
    {
        for (int i = 0; i < this.carSettings.Count; i++)
        {
            CarSettings carSettings = this.carSettings[i];
            if (carSettings.carName == carName)
            {
                return carSettings;
            }
        }
        return null;
    }

    public CarCamera.Settings GetCarCamera(string carName, string cameraName)
    {
        CarSettings carSettings = this.GetCarSettings(carName);
        if (carSettings == null)
        {
            return null;
        }
        return carSettings.GetSettings(cameraName);
    }

    public CarsDB.Car NextCar(CarsDB.Car car)
    {
        int num = this.carsList.IndexOf(car);
        int num2 = 0;
        if (num >= 0)
        {
            num2 = num + 1;
        }
        if (num2 < 0 || num2 > this.carsList.Count - 1)
        {
            return null;
        }
        return this.carsList[num2];
    }

    public CarsDB.Car Active
    {
        get
        {
            int selectedRoomIndex = SingletonInit<RoomsBackend>.instance.selectedRoomIndex;
            return this.carsList[Mathf.Clamp(selectedRoomIndex, 0, this.carsList.Count - 1)];
        }
    }

    public int IndexOf(CarsDB.Car car)
    {
        return this.carsList.IndexOf(car);
    }

    protected override void UpdateData()
    {
        base.UpdateData();
        for (int i = 0; i < this.carsList.Count; i++)
        {
            this.carsList[i].Init(this);
        }
    }

    public IEnumerator Load(CarsDB.LoadCarRequest loadRequest)
    {
        return new CarsDB._003CLoad_003Ed__15(0)
        {
            _003C_003E4__this = this,
            loadRequest = loadRequest
        };
    }

    [SerializeField]
    public List<CarsDB.Car> carsList = new List<CarsDB.Car>();

    [SerializeField]
    public CarCamera.BlendSettings blendSettings = new CarCamera.BlendSettings();

    [SerializeField]
    public ExplodeSlider.ExplosionSettings explosionSettings = new ExplodeSlider.ExplosionSettings();

    [SerializeField]
    private List<CarSettings> carSettings = new List<CarSettings>();

    public CarModelSubpart.Settings subpartInSettings = new CarModelSubpart.Settings();

    public CarModelSubpart.BlinkSettings subpartBlinkSettings = new CarModelSubpart.BlinkSettings();

    [Serializable]
    public class Car
    {
        public string assetBundleURL
        {
            get
            {
                if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                {
                    return this.assetBundleURLOSX;
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return this.assetBundleURLIOS;
                }
                RuntimePlatform platform = Application.platform;
                return this.assetBundleURLAndroid;
            }
        }

        public bool isPassed
        {
            get
            {
                return this.roomAccessor.isPassed;
            }
        }

        public bool isLocked
        {
            get
            {
                return !this.isOpen;
            }
        }

        public bool isOpen
        {
            get
            {
                int num = this.cars.IndexOf(this);
                if (num <= 0)
                {
                    return true;
                }
                int index = num - 1;
                return this.cars.carsList[index].isPassed;
            }
        }

        public void Init(CarsDB cars)
        {
            this.cars = cars;
        }

        private RoomsBackend.RoomAccessor roomAccessor
        {
            get
            {
                return SingletonInit<RoomsBackend>.instance.GetRoom(this.name);
            }
        }

        public string name;

        public string sceneName;

        public string displayName;

        public string description;

        public bool getFromAssetBundleOnEditor;

        public string assetBundleURLOSX;

        public string assetBundleURLAndroid;

        public string assetBundleURLIOS;

        public Sprite cardSprite;

        public GiftBoxScreen.GiftsDefinition giftDefinition = new GiftBoxScreen.GiftsDefinition();

        private CarsDB cars;

        public string editorAssetPath;

        [NonSerialized]
        public CarScene sceneBehaviour;

        [NonSerialized]
        public Transform rootTransform;

        [NonSerialized]
        public bool isSceneLoaded;

        [NonSerialized]
        public string loadedSceneName;

        [NonSerialized]
        public AssetBundle loadedAssetBundle;
    }

    public class LoadCarRequest
    {
        public LoadCarRequest(CarsDB.Car car)
        {
            this.car = car;
        }

        public CarsDB.Car car;

        public float progress;

        public bool isDone;

        public bool isError;
    }

    private sealed class _003CLoad_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CLoad_003Ed__15(int _003C_003E1__state)
        {
            this._003C_003E1__state = _003C_003E1__state;
        }

        [DebuggerHidden]
        void IDisposable.Dispose()
        {
            int num = this._003C_003E1__state;
            if (num == -3 || num - 3 <= 1)
            {
                try
                {
                }
                finally
                {
                    this._003C_003Em__Finally1();
                }
            }
        }

        bool IEnumerator.MoveNext()
        {
            bool result;
            try
            {
                int num = this._003C_003E1__state;
                CarsDB carsDB = this._003C_003E4__this;
                switch (num)
                {
                    case 0:
                        this._003C_003E1__state = -1;
                        this._003Ccar_003E5__2 = this.loadRequest.car;
                        this._003Cscene_003E5__3 = default(Scene);
                        this._003CassetLoaded_003E5__4 = false;
                        this._003Ci_003E5__5 = 0;
                        goto IL_13D;
                    case 1:
                        this._003C_003E1__state = -1;
                        break;
                    case 2:
                        this._003C_003E1__state = -1;
                        goto IL_1D6;
                    case 3:
                        this._003C_003E1__state = -3;
                        goto IL_284;
                    case 4:
                        this._003C_003E1__state = -3;
                        goto IL_33D;
                    case 5:
                        this._003C_003E1__state = -1;
                        goto IL_40B;
                    default:
                        return false;
                }
                IL_D4:
                if (!this._003CasyncWait_003E5__7.isDone)
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
                this._003CasyncWait_003E5__7 = null;
                IL_E8:
                this._003Citem_003E5__6.isSceneLoaded = false;
                IL_F4:
                if (this._003Citem_003E5__6.loadedAssetBundle != null)
                {
                    this._003Citem_003E5__6.loadedAssetBundle.Unload(true);
                }
                this._003Citem_003E5__6.loadedAssetBundle = null;
                this._003Citem_003E5__6 = null;
                int num2 = this._003Ci_003E5__5;
                this._003Ci_003E5__5 = num2 + 1;
                IL_13D:
                if (this._003Ci_003E5__5 >= carsDB.carsList.Count)
                {
                    if (this._003CassetLoaded_003E5__4)
                    {
                        goto IL_3E2;
                    }
                    if (string.IsNullOrEmpty(this._003Ccar_003E5__2.sceneName))
                    {
                        this._003Cwww_003E5__9 = WWW.LoadFromCacheOrDownload(this._003Ccar_003E5__2.assetBundleURL, 0);
                        this._003C_003E1__state = -3;
                        goto IL_284;
                    }
                    this._003CsceneName_003E5__8 = this._003Ccar_003E5__2.sceneName;
                    this._003CasyncWait_003E5__7 = SceneManager.LoadSceneAsync(this._003CsceneName_003E5__8, LoadSceneMode.Additive);
                    this._003CasyncWait_003E5__7.allowSceneActivation = true;
                }
                else
                {
                    this._003Citem_003E5__6 = carsDB.carsList[this._003Ci_003E5__5];
                    if (!this._003Citem_003E5__6.isSceneLoaded)
                    {
                        goto IL_F4;
                    }
                    Scene sceneByName = SceneManager.GetSceneByName(this._003Citem_003E5__6.loadedSceneName);
                    if (sceneByName.isLoaded)
                    {
                        this._003CasyncWait_003E5__7 = SceneManager.UnloadSceneAsync(sceneByName);
                        goto IL_D4;
                    }
                    goto IL_E8;
                }
                IL_1D6:
                if (this._003CasyncWait_003E5__7.isDone)
                {
                    this._003Cscene_003E5__3 = SceneManager.GetSceneByName(this._003CsceneName_003E5__8);
                    this._003Ccar_003E5__2.isSceneLoaded = true;
                    this._003Ccar_003E5__2.loadedSceneName = this._003Cscene_003E5__3.name;
                    this._003CassetLoaded_003E5__4 = true;
                    this._003CsceneName_003E5__8 = null;
                    this._003CasyncWait_003E5__7 = null;
                    goto IL_3E2;
                }
                this.loadRequest.progress = this._003CasyncWait_003E5__7.progress;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
                IL_284:
                if (!this._003Cwww_003E5__9.isDone)
                {
                    this.loadRequest.progress = this._003Cwww_003E5__9.progress;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 3;
                    return true;
                }
                if (this._003Cwww_003E5__9.error != null)
                {
                    this.loadRequest.isDone = true;
                    this.loadRequest.isError = true;
                    result = false;
                    this._003C_003Em__Finally1();
                    return result;
                }
                this._003CassetBundle_003E5__10 = this._003Cwww_003E5__9.assetBundle;
                string[] allScenePaths = this._003CassetBundle_003E5__10.GetAllScenePaths();
                this._003CsceneName_003E5__8 = Path.GetFileNameWithoutExtension(allScenePaths[0]);
                this._003CasyncWait_003E5__7 = SceneManager.LoadSceneAsync(this._003CsceneName_003E5__8, LoadSceneMode.Additive);
                this._003CasyncWait_003E5__7.allowSceneActivation = true;
                IL_33D:
                if (!this._003CasyncWait_003E5__7.isDone)
                {
                    this.loadRequest.progress = this._003CasyncWait_003E5__7.progress;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 4;
                    return true;
                }
                this._003Cscene_003E5__3 = SceneManager.GetSceneByName(this._003CsceneName_003E5__8);
                this._003Ccar_003E5__2.isSceneLoaded = true;
                this._003Ccar_003E5__2.loadedSceneName = this._003Cscene_003E5__3.name;
                this._003Ccar_003E5__2.loadedAssetBundle = this._003CassetBundle_003E5__10;
                this._003CassetLoaded_003E5__4 = true;
                this._003CassetBundle_003E5__10.Unload(false);
                this._003Cwww_003E5__9.Dispose();
                this._003CassetLoaded_003E5__4 = true;
                this._003CassetBundle_003E5__10 = null;
                this._003CsceneName_003E5__8 = null;
                this._003CasyncWait_003E5__7 = null;
                this._003C_003Em__Finally1();
                this._003Cwww_003E5__9 = null;
                IL_3E2:
                if (!this._003Cscene_003E5__3.IsValid())
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 5;
                    return true;
                }
                IL_40B:
                CarScene sceneBehaviour = null;
                GameObject[] rootGameObjects = this._003Cscene_003E5__3.GetRootGameObjects();
                if (rootGameObjects.Length != 0)
                {
                    this._003Ccar_003E5__2.rootTransform = rootGameObjects[0].transform;
                }
                else
                {
                    this._003Ccar_003E5__2.rootTransform = null;
                }
                for (int i = 0; i < rootGameObjects.Length; i++)
                {
                    CarScene component = rootGameObjects[i].GetComponent<CarScene>();
                    if (component != null)
                    {
                        sceneBehaviour = component;
                        break;
                    }
                }
                this._003Ccar_003E5__2.sceneBehaviour = sceneBehaviour;
                this.loadRequest.isDone = true;
                result = false;
            }
            catch
            {
                //this.System.IDisposable.Dispose();
                throw;
            }
            return result;
        }

        private void _003C_003Em__Finally1()
        {
            this._003C_003E1__state = -1;
            if (this._003Cwww_003E5__9 != null)
            {
                ((IDisposable)this._003Cwww_003E5__9).Dispose();
            }
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

        public CarsDB.LoadCarRequest loadRequest;

        public CarsDB _003C_003E4__this;

        private CarsDB.Car _003Ccar_003E5__2;

        private Scene _003Cscene_003E5__3;

        private bool _003CassetLoaded_003E5__4;

        private int _003Ci_003E5__5;

        private CarsDB.Car _003Citem_003E5__6;

        private AsyncOperation _003CasyncWait_003E5__7;

        private string _003CsceneName_003E5__8;

        private WWW _003Cwww_003E5__9;

        private AssetBundle _003CassetBundle_003E5__10;
    }
}
