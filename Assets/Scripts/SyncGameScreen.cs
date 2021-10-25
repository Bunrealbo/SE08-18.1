using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class SyncGameScreen : MonoBehaviour
{
    public void LoginToFacebook(string userId)
    {
        GGPlayerSettings instance = GGPlayerSettings.instance;
        instance.Model.applePlayerId = "";
        instance.Model.facebookPlayerId = userId;
        instance.Save();
        this.action = this.DoLogin();
        NavigationManager.instance.Push(base.gameObject, false);
    }

    public void LoginToApple(string userId)
    {
        GGPlayerSettings instance = GGPlayerSettings.instance;
        instance.Model.applePlayerId = userId;
        instance.Model.facebookPlayerId = "";
        instance.Save();
        this.action = this.DoLogin();
        NavigationManager.instance.Push(base.gameObject, false);
    }

    public void SynchronizeNow()
    {
        this.action = this.DoSyncNow();
        NavigationManager.instance.Push(base.gameObject, false);
    }

    private IEnumerator DoSyncNow()
    {
        return new SyncGameScreen._003CDoSyncNow_003Ed__5(0)
        {
            _003C_003E4__this = this
        };
    }

    private IEnumerator DoLogin()
    {
        return new SyncGameScreen._003CDoLogin_003Ed__6(0)
        {
            _003C_003E4__this = this
        };
    }

    private void Update()
    {
        if (this.action != null && !this.action.MoveNext())
        {
            this.action = null;
        }
    }

    [SerializeField]
    private Image fillImage;

    private IEnumerator action;

    private sealed class _003C_003Ec__DisplayClass5_0
    {
        internal void _003CDoSyncNow_003Eb__0(GGServerRequestsBackend.ServerRequest request)
        {
            this.synchronizingToServer = false;
            this.snapshotSync.HandleSyncRequestResult(request);
        }

        public bool synchronizingToServer;

        public GGSnapshotCloudSync snapshotSync;
    }

    private sealed class _003CDoSyncNow_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoSyncNow_003Ed__5(int _003C_003E1__state)
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
            SyncGameScreen syncGameScreen = this._003C_003E4__this;
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
                this._003C_003E8__1 = new SyncGameScreen._003C_003Ec__DisplayClass5_0();
                GGUtil.SetFill(syncGameScreen.fillImage, 0f);
                GGPlayerSettings instance = GGPlayerSettings.instance;
                GGPlayerSettings.instance.canCloudSync = true;
                this._003C_003E8__1.snapshotSync = (GGFileIOCloudSync.instance as GGSnapshotCloudSync);
                this._003C_003E8__1.synchronizingToServer = true;
                this._003C_003E8__1.snapshotSync.SynchronizeNow(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoSyncNow_003Eb__0));
                this._003CstartTime_003E5__2 = Time.unscaledTime;
                this._003CmaxDurationSec_003E5__3 = 4f;
            }
            if (this._003C_003E8__1.synchronizingToServer)
            {
                float num2 = Time.unscaledTime - this._003CstartTime_003E5__2;
                float fillAmount = Mathf.InverseLerp(0f, this._003CmaxDurationSec_003E5__3, num2);
                GGUtil.SetFill(syncGameScreen.fillImage, fillAmount);
                if (num2 < this._003CmaxDurationSec_003E5__3)
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
            }
            GGSnapshotCloudSync.StopSyncNeeded();
            GGUtil.SetFill(syncGameScreen.fillImage, 1f);
            NavigationManager.instance.Pop(true);
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

        public SyncGameScreen _003C_003E4__this;

        private SyncGameScreen._003C_003Ec__DisplayClass5_0 _003C_003E8__1;

        private float _003CstartTime_003E5__2;

        private float _003CmaxDurationSec_003E5__3;
    }

    private sealed class _003C_003Ec__DisplayClass6_0
    {
        internal void _003CDoLogin_003Eb__0(GGServerRequestsBackend.ServerRequest p)
        {
            this.requestRunning = false;
        }

        internal void _003CDoLogin_003Eb__1(GGServerRequestsBackend.ServerRequest request)
        {
            this.synchronizingToServer = false;
            this.snapshotSync.HandleSyncRequestResult(request);
        }

        internal void _003CDoLogin_003Eb__2(bool p)
        {
            this.nav.PopMultiple(2);
        }

        public bool requestRunning;

        public bool synchronizingToServer;

        public GGSnapshotCloudSync snapshotSync;

        public NavigationManager nav;
    }

    private sealed class _003CDoLogin_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoLogin_003Ed__6(int _003C_003E1__state)
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
            SyncGameScreen syncGameScreen = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new SyncGameScreen._003C_003Ec__DisplayClass6_0();
                    GGUtil.SetFill(syncGameScreen.fillImage, 0f);
                    BehaviourSingletonInit<GGServerRequestsBackend>.instance.ResetCache();
                    this._003CidRequest_003E5__2 = new GGServerRequestsBackend.IdRequest();
                    this._003CidRequest_003E5__2.cache = CacheStategy.DontCache;
                    this._003C_003E8__1.requestRunning = true;
                    BehaviourSingletonInit<GGServerRequestsBackend>.instance.ExecuteRequest(this._003CidRequest_003E5__2, new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoLogin_003Eb__0));
                    break;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_144;
                default:
                    return false;
            }
            if (this._003C_003E8__1.requestRunning)
            {
                GGUtil.SetFill(syncGameScreen.fillImage, this._003CidRequest_003E5__2.progress);
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            GGUtil.SetFill(syncGameScreen.fillImage, 1f);
            GGPlayerSettings.instance.canCloudSync = true;
            this._003C_003E8__1.snapshotSync = (GGFileIOCloudSync.instance as GGSnapshotCloudSync);
            this._003C_003E8__1.synchronizingToServer = true;
            this._003C_003E8__1.snapshotSync.SynchronizeNow(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoLogin_003Eb__1));
            IL_144:
            if (!this._003C_003E8__1.synchronizingToServer)
            {
                this._003C_003E8__1.nav = NavigationManager.instance;
                this._003C_003E8__1.nav.GetObject<Dialog>().Show("Login success", "OK", new Action<bool>(this._003C_003E8__1._003CDoLogin_003Eb__2));
                return false;
            }
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

        public SyncGameScreen _003C_003E4__this;

        private SyncGameScreen._003C_003Ec__DisplayClass6_0 _003C_003E8__1;

        private GGServerRequestsBackend.IdRequest _003CidRequest_003E5__2;
    }
}
