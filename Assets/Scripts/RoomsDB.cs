using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsDB : ScriptableObjectSingleton<RoomsDB>
{
    public List<RoomsDB.Room> rooms
    {
        get
        {
            this.rooms_.Clear();
            for (int i = 0; i < this.roomsList.Count; i++)
            {
                RoomsDB.Room room = this.roomsList[i];
                bool flag = true;
                if (room.isOnlyForEditor)
                {
                    flag = false;
                }
                if (room.remove)
                {
                    flag = false;
                }
                if (flag)
                {
                    this.rooms_.Add(room);
                }
            }
            return this.rooms_;
        }
    }

    public RoomsDB.Room NextRoom(RoomsDB.Room room)
    {
        List<RoomsDB.Room> rooms = this.rooms;
        int num = rooms.IndexOf(room);
        int num2 = 0;
        if (num >= 0)
        {
            num2 = num + 1;
        }
        if (num2 < 0 || num2 > rooms.Count - 1)
        {
            return null;
        }
        return rooms[num2];
    }

    public RoomsDB.Room ActiveRoom
    {
        get
        {
            RoomsBackend instance = SingletonInit<RoomsBackend>.instance;
            List<RoomsDB.Room> rooms = this.rooms;
            int selectedRoomIndex = instance.selectedRoomIndex;
            return rooms[Mathf.Clamp(selectedRoomIndex, 0, rooms.Count - 1)];
        }
    }

    public int IndexOf(RoomsDB.Room room)
    {
        return this.rooms.IndexOf(room);
    }

    protected override void UpdateData()
    {
        base.UpdateData();
        List<RoomsDB.Room> rooms = this.rooms;
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].Init(this);
        }
    }

    public IEnumerator LoadRoom(RoomsDB.LoadRoomRequest roomRequest)
    {
        return new RoomsDB._003CLoadRoom_003Ed__11(0)
        {
            _003C_003E4__this = this,
            roomRequest = roomRequest
        };
    }

    [SerializeField]
    private List<RoomsDB.Room> roomsList = new List<RoomsDB.Room>();

    [NonSerialized]
    private List<RoomsDB.Room> rooms_ = new List<RoomsDB.Room>();

    [Serializable]
    public class Room
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
                int num = this.rooms.IndexOf(this);
                if (num <= 0)
                {
                    return true;
                }
                int index = num - 1;
                return this.rooms.rooms[index].isPassed;
            }
        }

        public void Init(RoomsDB rooms)
        {
            this.rooms = rooms;
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

        public bool isOnlyForEditor;

        public bool remove;

        public int totalStarsInRoom;

        private RoomsDB rooms;

        public string editorAssetPath;

        [NonSerialized]
        public DecoratingScene sceneBehaviour;

        [NonSerialized]
        public bool isSceneLoaded;

        [NonSerialized]
        public string loadedSceneName;

        [NonSerialized]
        public AssetBundle loadedAssetBundle;
    }

    public class LoadRoomRequest
    {
        public LoadRoomRequest(RoomsDB.Room room)
        {
            this.room = room;
        }

        public RoomsDB.Room room;

        public float progress;

        public bool isDone;

        public bool isError;
    }

    private sealed class _003CLoadRoom_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CLoadRoom_003Ed__11(int _003C_003E1__state)
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
                RoomsDB roomsDB = this._003C_003E4__this;
                switch (num)
                {
                    case 0:
                        this._003C_003E1__state = -1;
                        this._003Croom_003E5__2 = this.roomRequest.room;
                        this._003Cscene_003E5__3 = default(Scene);
                        this._003CroomLoaded_003E5__4 = false;
                        this._003Crooms_003E5__5 = roomsDB.rooms;
                        this._003Ci_003E5__6 = 0;
                        goto IL_149;
                    case 1:
                        this._003C_003E1__state = -1;
                        break;
                    case 2:
                        this._003C_003E1__state = -1;
                        goto IL_1E2;
                    case 3:
                        this._003C_003E1__state = -3;
                        goto IL_290;
                    case 4:
                        this._003C_003E1__state = -3;
                        goto IL_349;
                    case 5:
                        this._003C_003E1__state = -1;
                        goto IL_414;
                    default:
                        return false;
                }
                IL_E0:
                if (!this._003CasyncWait_003E5__8.isDone)
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
                this._003CasyncWait_003E5__8 = null;
                IL_F4:
                this._003Citem_003E5__7.isSceneLoaded = false;
                IL_100:
                if (this._003Citem_003E5__7.loadedAssetBundle != null)
                {
                    this._003Citem_003E5__7.loadedAssetBundle.Unload(true);
                }
                this._003Citem_003E5__7.loadedAssetBundle = null;
                this._003Citem_003E5__7 = null;
                int num2 = this._003Ci_003E5__6;
                this._003Ci_003E5__6 = num2 + 1;
                IL_149:
                if (this._003Ci_003E5__6 >= this._003Crooms_003E5__5.Count)
                {
                    if (this._003CroomLoaded_003E5__4)
                    {
                        goto IL_3EE;
                    }
                    if (string.IsNullOrEmpty(this._003Croom_003E5__2.sceneName))
                    {
                        this._003Cwww_003E5__10 = WWW.LoadFromCacheOrDownload(this._003Croom_003E5__2.assetBundleURL, 0);
                        this._003C_003E1__state = -3;
                        goto IL_290;
                    }
                    this._003CsceneName_003E5__9 = this._003Croom_003E5__2.sceneName;
                    this._003CasyncWait_003E5__8 = SceneManager.LoadSceneAsync(this._003CsceneName_003E5__9, LoadSceneMode.Additive);
                    this._003CasyncWait_003E5__8.allowSceneActivation = true;
                }
                else
                {
                    this._003Citem_003E5__7 = this._003Crooms_003E5__5[this._003Ci_003E5__6];
                    if (!this._003Citem_003E5__7.isSceneLoaded)
                    {
                        goto IL_100;
                    }
                    Scene sceneByName = SceneManager.GetSceneByName(this._003Citem_003E5__7.loadedSceneName);
                    if (sceneByName.isLoaded)
                    {
                        this._003CasyncWait_003E5__8 = SceneManager.UnloadSceneAsync(sceneByName);
                        goto IL_E0;
                    }
                    goto IL_F4;
                }
                IL_1E2:
                if (this._003CasyncWait_003E5__8.isDone)
                {
                    this._003Cscene_003E5__3 = SceneManager.GetSceneByName(this._003CsceneName_003E5__9);
                    this._003Croom_003E5__2.isSceneLoaded = true;
                    this._003Croom_003E5__2.loadedSceneName = this._003Cscene_003E5__3.name;
                    this._003CroomLoaded_003E5__4 = true;
                    this._003CsceneName_003E5__9 = null;
                    this._003CasyncWait_003E5__8 = null;
                    goto IL_3EE;
                }
                this.roomRequest.progress = this._003CasyncWait_003E5__8.progress;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
                IL_290:
                if (!this._003Cwww_003E5__10.isDone)
                {
                    this.roomRequest.progress = this._003Cwww_003E5__10.progress;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 3;
                    return true;
                }
                if (this._003Cwww_003E5__10.error != null)
                {
                    this.roomRequest.isDone = true;
                    this.roomRequest.isError = true;
                    result = false;
                    this._003C_003Em__Finally1();
                    return result;
                }
                this._003CassetBundle_003E5__11 = this._003Cwww_003E5__10.assetBundle;
                string[] allScenePaths = this._003CassetBundle_003E5__11.GetAllScenePaths();
                this._003CsceneName_003E5__9 = Path.GetFileNameWithoutExtension(allScenePaths[0]);
                this._003CasyncWait_003E5__8 = SceneManager.LoadSceneAsync(this._003CsceneName_003E5__9, LoadSceneMode.Additive);
                this._003CasyncWait_003E5__8.allowSceneActivation = true;
                IL_349:
                if (!this._003CasyncWait_003E5__8.isDone)
                {
                    this.roomRequest.progress = this._003CasyncWait_003E5__8.progress;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 4;
                    return true;
                }
                this._003Cscene_003E5__3 = SceneManager.GetSceneByName(this._003CsceneName_003E5__9);
                this._003Croom_003E5__2.isSceneLoaded = true;
                this._003Croom_003E5__2.loadedSceneName = this._003Cscene_003E5__3.name;
                this._003Croom_003E5__2.loadedAssetBundle = this._003CassetBundle_003E5__11;
                this._003CroomLoaded_003E5__4 = true;
                this._003CassetBundle_003E5__11.Unload(false);
                this._003Cwww_003E5__10.Dispose();
                this._003CroomLoaded_003E5__4 = true;
                this._003CassetBundle_003E5__11 = null;
                this._003CsceneName_003E5__9 = null;
                this._003CasyncWait_003E5__8 = null;
                this._003C_003Em__Finally1();
                this._003Cwww_003E5__10 = null;
                IL_3EE:
                if (!this._003Cscene_003E5__3.IsValid())
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 5;
                    return true;
                }
                IL_414:
                DecoratingScene sceneBehaviour = null;
                GameObject[] rootGameObjects = this._003Cscene_003E5__3.GetRootGameObjects();
                for (int i = 0; i < rootGameObjects.Length; i++)
                {
                    DecoratingScene component = rootGameObjects[i].GetComponent<DecoratingScene>();
                    if (component != null)
                    {
                        sceneBehaviour = component;
                        break;
                    }
                }
                this._003Croom_003E5__2.sceneBehaviour = sceneBehaviour;
                this.roomRequest.isDone = true;
                result = false;
            }
            catch
            {

                throw;
            }
            return result;
        }

        private void _003C_003Em__Finally1()
        {
            this._003C_003E1__state = -1;
            if (this._003Cwww_003E5__10 != null)
            {
                ((IDisposable)this._003Cwww_003E5__10).Dispose();
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

        public RoomsDB.LoadRoomRequest roomRequest;

        public RoomsDB _003C_003E4__this;

        private RoomsDB.Room _003Croom_003E5__2;

        private Scene _003Cscene_003E5__3;

        private bool _003CroomLoaded_003E5__4;

        private List<RoomsDB.Room> _003Crooms_003E5__5;

        private int _003Ci_003E5__6;

        private RoomsDB.Room _003Citem_003E5__7;

        private AsyncOperation _003CasyncWait_003E5__8;

        private string _003CsceneName_003E5__9;

        private WWW _003Cwww_003E5__10;

        private AssetBundle _003CassetBundle_003E5__11;
    }
}
