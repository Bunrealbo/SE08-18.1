using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ProtoModels;
using UnityEngine;

public class GGServerRequestsBackend : BehaviourSingletonInit<GGServerRequestsBackend>
{
    public string cachedPlayerId
    {
        get
        {
            if (!string.IsNullOrEmpty(this.memoryCachedPlayerId))
            {
                return this.memoryCachedPlayerId;
            }
            if (!string.IsNullOrEmpty(this.storage.lastKnownPid))
            {
                return this.storage.lastKnownPid;
            }
            return "";
        }
        protected set
        {
            this.memoryCachedPlayerId = value;
            this.storage.lastKnownPid = value;
            this.Save();
        }
    }

    public void ResetCache()
    {
        this.cachedPlayerId = "";
        Singleton<GGRequestCache>.instance.Clear();
        SingletonInit<GGRequestFileCache>.instance.Clear();
    }

    public void Save()
    {
        ProtoIO.SaveToFile<RequestsBackendStorage>(this.storageFilename, this.storage);
    }

    public override void Init()
    {
        base.Init();
        if (!ProtoIO.LoadFromFile<RequestsBackendStorage>(this.storageFilename, out this.storage))
        {
            this.storage = new RequestsBackendStorage();
            this.Save();
        }
    }

    public void GetCompetitionLeaderboards(GGServerRequestsBackend.LeaderboardsRequest lead, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("GetLeaderboards");
        base.StartCoroutine(this.DoExecuteRequestWithPid<Lead>(lead, onComplete));
    }

    public Coroutine CallWhenRequestComplete(GGServerRequestsBackend.ServerRequest request, Action<GGServerRequestsBackend.ServerRequest> onComplete)
    {
        return base.StartCoroutine(this.DoCallWhenRequestComplete(request, onComplete));
    }

    private IEnumerator DoCallWhenRequestComplete(GGServerRequestsBackend.ServerRequest request, Action<GGServerRequestsBackend.ServerRequest> onComplete)
    {
        return new GGServerRequestsBackend._003CDoCallWhenRequestComplete_003Ed__61(0)
        {
            request = request,
            onComplete = onComplete
        };
    }

    public void GetEventsLeaderboards(GGServerRequestsBackend.EventLeadRequest lead, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("Get Events Leaderboards");
        base.StartCoroutine(this.DoExecuteRequestWithPid<EventLeads>(lead, onComplete));
    }

    public void UpdateEventsLeaderboards(GGServerRequestsBackend.EventScoreUpdateRequest updateRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoUpdateEventsLeaderboards(updateRequest, onComplete));
    }

    private IEnumerator DoUpdateEventsLeaderboards(GGServerRequestsBackend.EventScoreUpdateRequest updateRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoUpdateEventsLeaderboards_003Ed__64(0)
        {
            _003C_003E4__this = this,
            updateRequest = updateRequest,
            onComplete = onComplete
        };
    }

    public void GetAppMessagesRequest(GGServerRequestsBackend.AppMessagesRequest messageRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoExecuteRequestWithPid<MessageList>(messageRequest, onComplete));
    }

    public void UpdateAppMessagesRequest(GGServerRequestsBackend.UpdateAppMessageRead messageRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoExecuteRequestWithPid<StatusMessage>(messageRequest, onComplete));
    }

    public void GetSegmentedCompetitionLeaderboards(GGServerRequestsBackend.SegmentedLeaderboardsRequest lead, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("GetLeaderboards");
        base.StartCoroutine(this.DoExecuteRequestWithPid<CombinationLeads>(lead, onComplete));
    }

    public void UpdateUser(GGServerRequestsBackend.UpdateRequest update, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("UpdateUser");
        base.StartCoroutine(this.DoUpdateUser(update, onComplete));
    }

    private IEnumerator DoUpdateUser(GGServerRequestsBackend.UpdateRequest update, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoUpdateUser_003Ed__69(0)
        {
            _003C_003E4__this = this,
            update = update,
            onComplete = onComplete
        };
    }

    public void ExecuteGetPlayerId(GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.GetPlayerId(onComplete));
    }

    public IEnumerator GetPlayerId(GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CGetPlayerId_003Ed__71(0)
        {
            _003C_003E4__this = this,
            onComplete = onComplete
        };
    }

    public IEnumerator GetFacebookLogin(GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CGetFacebookLogin_003Ed__72(0)
        {
            _003C_003E4__this = this,
            onComplete = onComplete
        };
    }

    public void GetActiveCompetition(GGServerRequestsBackend.ActiveCompetitionRequest req, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoRequest(req, onComplete));
    }

    private IEnumerator DoRequest(GGServerRequestsBackend.ActiveCompetitionRequest req, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoRequest_003Ed__74(0)
        {
            _003C_003E4__this = this,
            req = req,
            onComplete = onComplete
        };
    }

    public void GetPrizes(GGServerRequestsBackend.GetPrizesRequest getPrizesRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("GetPrizes");
        base.StartCoroutine(this.DoExecuteRequestWithPid<Lead>(getPrizesRequest, onComplete));
    }

    public void GetCombinationPrizes(GGServerRequestsBackend.GetPrizesRequestCombinationLead getPrizesRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("GetPrizes");
        base.StartCoroutine(this.DoExecuteRequestWithPid<CombinationLeads>(getPrizesRequest, onComplete));
    }

    public void AckPrizes(GGServerRequestsBackend.AckPrizesRequest ackPrizesRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("AckPrizes");
        base.StartCoroutine(this.DoExecuteRequestWithPid<StatusMessage>(ackPrizesRequest, onComplete));
    }

    public Coroutine GetCSData(GGServerRequestsBackend.CloudSyncRequest dataRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        return base.StartCoroutine(this.DoSyncCsData(dataRequest, onComplete));
    }

    public void UpdateCSData(GGServerRequestsBackend.UpdateCloudSyncDataRequest dataRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("UpdateCSData");
        base.StartCoroutine(this.DoSyncCsData(dataRequest, onComplete));
    }

    private IEnumerator DoSyncCsData(GGServerRequestsBackend.CloudSyncRequest dataRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoSyncCsData_003Ed__80(0)
        {
            _003C_003E4__this = this,
            dataRequest = dataRequest,
            onComplete = onComplete
        };
    }

    public void UploadLeadData(GGServerRequestsBackend.UploadLeadDataRequest dataRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("UpdateLeadData");
        base.StartCoroutine(this.DoUploadLeadData(dataRequest, onComplete));
    }

    private IEnumerator DoUploadLeadData(GGServerRequestsBackend.UploadLeadDataRequest dataRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoUploadLeadData_003Ed__82(0)
        {
            _003C_003E4__this = this,
            dataRequest = dataRequest,
            onComplete = onComplete
        };
    }

    public void GetFacebookInvites(GGServerRequestsBackend.FacebookInviteFriends inviteRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("inviteRequest");
        inviteRequest.cache = CacheStategy.GetFromCache;
        inviteRequest.cacheTimeToLive = TimeSpan.FromSeconds(30.0);
        base.StartCoroutine(this.DoExecuteRequestWithPid<InvitableFriends>(inviteRequest, onComplete));
    }

    public void GetFacebookPlayers(GGServerRequestsBackend.FacebookPlayingFriends playersRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("playersRequest");
        playersRequest.cache = CacheStategy.GetFromCache;
        playersRequest.cacheTimeToLive = TimeSpan.FromSeconds(30.0);
        base.StartCoroutine(this.DoExecuteRequestWithPid<InvitableFriends>(playersRequest, onComplete));
    }

    public void GetMessagesForPlayer(GGServerRequestsBackend.GGServerPlayerMessages messagesRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("messagesRequest");
        messagesRequest.cache = CacheStategy.GetFromCache;
        messagesRequest.cacheTimeToLive = TimeSpan.FromMinutes(10.0);
        base.StartCoroutine(this.DoExecuteRequestWithPid<ServerMessages>(messagesRequest, onComplete));
    }

    public void MarkMessagesAsRead(GGServerRequestsBackend.GGServerMarkMessagesAsRead markAsReadRequest, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("markAsReadRequest");
        base.StartCoroutine(this.DoExecuteRequestWithPid<StatusMessage>(markAsReadRequest, onComplete));
    }

    public void GetFriendProfiles(GGServerRequestsBackend.GGServerGetFriendProfiles request, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("GetFriendProfiles");
        base.StartCoroutine(this.DoExecuteRequestWithPid<FriendsProfiles>(request, onComplete));
    }

    public void GetPlayerPositionList(GGServerRequestsBackend.GetPlayerPositionsRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        UnityEngine.Debug.Log("GetFriendProfiles");
        base.StartCoroutine(this.DoExecuteRequestWithPid<PlayerPositions>(request, onComplete));
    }

    public void ExecuteRequest(GGServerRequestsBackend.ServerRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoExecuteRequest(request, onComplete));
    }

    public void ExecuteRequestWithPid<T>(GGServerRequestsBackend.ProtoRequestPid<T> request, GGServerRequestsBackend.OnComplete onComplete) where T : class
    {
        base.StartCoroutine(this.DoExecuteRequestWithPid<T>(request, onComplete));
    }

    public void ExecuteRequestWithNonce(GGServerRequestsBackend.ServerRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoExecuteNonceRequest(request, onComplete));
    }

    public void ExecuteRequestAllInterfacesRequest(GGServerRequestsBackend.ServerRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        base.StartCoroutine(this.DoExecuteAllInterfacesRequest(request, onComplete));
    }

    private IEnumerator DoExecuteAllInterfacesRequest(GGServerRequestsBackend.ServerRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoExecuteAllInterfacesRequest_003Ed__93(0)
        {
            _003C_003E4__this = this,
            request = request,
            onComplete = onComplete
        };
    }

    private IEnumerator DoExecuteNonceRequest(GGServerRequestsBackend.ServerRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoExecuteNonceRequest_003Ed__94(0)
        {
            _003C_003E4__this = this,
            request = request,
            onComplete = onComplete
        };
    }

    private IEnumerator DoExecuteRequest(GGServerRequestsBackend.ServerRequest request, GGServerRequestsBackend.OnComplete onComplete)
    {
        return new GGServerRequestsBackend._003CDoExecuteRequest_003Ed__95(0)
        {
            _003C_003E4__this = this,
            request = request,
            onComplete = onComplete
        };
    }

    private IEnumerator DoExecuteRequestWithPid<T>(GGServerRequestsBackend.ProtoRequestPid<T> request, GGServerRequestsBackend.OnComplete onComplete) where T : class
    {
        return new GGServerRequestsBackend._DoExecuteRequestWithPid<T>(0)
        {
            _003C_003E4__this = this,
            request = request,
            onComplete = onComplete
        };
    }

    public static DateTime GetDateTimeCompEnd(long seconds)
    {
        return DateTime.Now.AddSeconds((double)seconds);
    }

    protected RequestsBackendStorage storage;

    protected string storageFilename = "backendStorage.bytes";

    private string memoryCachedPlayerId = "";

    public delegate void OnComplete(GGServerRequestsBackend.ServerRequest request);

    public class UrlBuilder
    {
        public static string GetTimestampAttributeName()
        {
            return "timestamp";
        }

        public UrlBuilder(string hostName)
        {
            this.urlBase = hostName;
            this.addParams(GGServerRequestsBackend.UrlBuilder.GetTimestampAttributeName(), DateTime.UtcNow.ToString());
            this.addParams("run_platform", "android");
        }

        public GGServerRequestsBackend.UrlBuilder addPath(string path)
        {
            this.urlBase += path;
            return this;
        }

        public GGServerRequestsBackend.UrlBuilder addData(string newData)
        {
            this.data = newData;
            return this;
        }

        public GGServerRequestsBackend.UrlBuilder addParams(string paramName, string paramVal)
        {
            this.paramPairs.Add(paramName, paramVal);
            return this;
        }

        public GGServerRequestsBackend.UrlBuilder addParams(string paramName, int paramVal)
        {
            this.paramPairs.Add(paramName, paramVal.ToString());
            return this;
        }

        public GGServerRequestsBackend.UrlBuilder addParams(string paramName, double paramVal)
        {
            this.paramPairs.Add(paramName, paramVal.ToString());
            return this;
        }

        public GGServerRequestsBackend.UrlBuilder addParams(string paramName, List<string> paramVal)
        {
            this.paramListPairs.Add(paramName, paramVal);
            return this;
        }

        public string SignAndToString(string publicKey, string privateKey)
        {
            string text = this.urlBase;
            this.paramPairs.Add("ap", publicKey);
            foreach (KeyValuePair<string, string> keyValuePair in this.paramPairs)
            {
                try
                {
                    text = string.Concat(new string[]
                    {
                        text,
                        (text == this.urlBase) ? "?" : "&",
                        keyValuePair.Key,
                        "=",
                        Uri.EscapeDataString(keyValuePair.Value)
                    });
                }
                catch
                {
                    UnityEngine.Debug.Log(string.Concat(new string[]
                    {
                        "Problem with key = ",
                        keyValuePair.Key,
                        " value: \"",
                        keyValuePair.Value,
                        "\""
                    }));
                }
            }
            foreach (KeyValuePair<string, List<string>> keyValuePair2 in this.paramListPairs)
            {
                foreach (string stringToEscape in keyValuePair2.Value)
                {
                    text = string.Concat(new string[]
                    {
                        text,
                        (text == this.urlBase) ? "?" : "&",
                        keyValuePair2.Key,
                        "=",
                        Uri.EscapeDataString(stringToEscape)
                    });
                }
            }
            string hashSha = Hash.getHashSha256(Regex.Replace(text, "http(s)?://[^/]+", "") + this.data + privateKey);
            text = text + "&sig=" + Uri.EscapeDataString(hashSha);
            return text;
        }

        public override string ToString()
        {
            string text = this.urlBase;
            foreach (KeyValuePair<string, string> keyValuePair in this.paramPairs)
            {
                if (keyValuePair.Value != null)
                {
                    text = string.Concat(new string[]
                    {
                        text,
                        (text == this.urlBase) ? "?" : "&",
                        keyValuePair.Key,
                        "=",
                        Uri.EscapeDataString(keyValuePair.Value)
                    });
                }
            }
            foreach (KeyValuePair<string, List<string>> keyValuePair2 in this.paramListPairs)
            {
                foreach (string text2 in keyValuePair2.Value)
                {
                    if (text2 != null)
                    {
                        text = string.Concat(new string[]
                        {
                            text,
                            (text == this.urlBase) ? "?" : "&",
                            keyValuePair2.Key,
                            "=",
                            Uri.EscapeDataString(text2)
                        });
                    }
                }
            }
            return text;
        }

        private string urlBase;

        private Dictionary<string, string> paramPairs = new Dictionary<string, string>();

        private Dictionary<string, List<string>> paramListPairs = new Dictionary<string, List<string>>();

        private string data = "";
    }

    public class ServerRequest
    {
        public string errorMessage
        {
            get
            {
                return this._003CerrorMessage_003Ek__BackingField;
            }
            protected set
            {
                this._003CerrorMessage_003Ek__BackingField = value;
            }
        }

        public T GetResponse<T>() where T : class
        {
            return this.responseObj as T;
        }

        protected virtual string GetUrl()
        {
            return "";
        }

        protected virtual WWW CreateQuery()
        {
            return null;
        }

        protected virtual void ParseQueryResponse(WWW query)
        {
            this.ParseResponse(query.bytes);
        }

        protected virtual void ParseResponse(byte[] bytes)
        {
            this.responseObj = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public virtual string GetCacheKey(string url)
        {
            return this.RemoveChangingParams(url);
        }

        private string StripParamFromUrl(string url, string paramName)
        {
            int num = url.IndexOf(paramName);
            if (num == -1)
            {
                return url;
            }
            int num2 = num + paramName.Length;
            int num3 = url.IndexOf("&", num2);
            if (num3 == -1)
            {
                return url.Remove(num2);
            }
            return url.Remove(num2, num3 - num2);
        }

        private string RemoveChangingParams(string url)
        {
            url = this.StripParamFromUrl(url, GGServerRequestsBackend.UrlBuilder.GetTimestampAttributeName());
            url = this.StripParamFromUrl(url, "sig");
            return url;
        }

        protected virtual bool TryGetFromMemoryCache(string url)
        {
            byte[] array = Singleton<GGRequestCache>.Instance.Get<byte[]>(this.GetCacheKey(url));
            if (array != null)
            {
                this.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                this.ParseResponse(array);
            }
            return array != null;
        }

        protected virtual bool TryGetFromFileCache(string url)
        {
            byte[] array = SingletonInit<GGRequestFileCache>.Instance.Get<byte[]>(this.GetCacheKey(url));
            if (array != null)
            {
                this.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                this.ParseResponse(array);
            }
            return array != null;
        }

        protected virtual void CacheResults(WWW query)
        {
            Singleton<GGRequestCache>.Instance.Put(this.GetCacheKey(query.url), query.bytes, this.cacheTimeToLive);
        }

        protected virtual void CacheToFile(WWW query)
        {
            SingletonInit<GGRequestFileCache>.Instance.Put(this.GetCacheKey(query.url), query.bytes, this.cacheTimeToLive);
        }

        public virtual bool TryGetFromCache()
        {
            string url = this.GetUrl();
            if (this.cache == CacheStategy.GetFromCache)
            {
                if (this.TryGetFromMemoryCache(url))
                {
                    this.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                    return true;
                }
            }
            else if (this.cache == CacheStategy.CacheToFile && this.TryGetFromFileCache(url))
            {
                this.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                return true;
            }
            return false;
        }

        public virtual IEnumerator RequestCoroutine()
        {
            return new GGServerRequestsBackend.ServerRequest._003CRequestCoroutine_003Ed__30(0)
            {
                _003C_003E4__this = this
            };
        }

        public GGServerRequestsBackend.ServerRequest.OnComplete onComplete;

        public int groupId;

        public float progress;

        public float timeoutSec = 15f;

        public CacheGetStrategy cacheGetStrategy;

        public CacheStategy cache;

        public TimeSpan cacheTimeToLive = TimeSpan.FromHours(6.0);

        public GGServerRequestsBackend.ServerRequest.RequestStatus status = GGServerRequestsBackend.ServerRequest.RequestStatus.NotSent;

        private string _003CerrorMessage_003Ek__BackingField;

        protected object responseObj;

        public enum RequestStatus
        {
            Success,
            Error,
            NotSent
        }

        public delegate void OnComplete(GGServerRequestsBackend.ServerRequest request);

        private sealed class _003CRequestCoroutine_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CRequestCoroutine_003Ed__30(int _003C_003E1__state)
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
                GGServerRequestsBackend.ServerRequest serverRequest = this._003C_003E4__this;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        return false;
                    }
                    this._003C_003E1__state = -1;
                    serverRequest.progress = this._003Cquery_003E5__2.progress;
                    this._003Ctime_003E5__3 += RealTime.deltaTime;
                    if (this._003Ctime_003E5__3 >= serverRequest.timeoutSec)
                    {
                        UnityEngine.Debug.Log("TIMEOUT1");
                        this._003Ctimeout_003E5__4 = true;
                        this._003Cquery_003E5__2.Dispose();
                        goto IL_10D;
                    }
                }
                else
                {
                    this._003C_003E1__state = -1;
                    serverRequest.progress = 0f;
                    if (!GGSupportMenu.instance.isNetworkConnected())
                    {
                        if (serverRequest.TryGetFromCache())
                        {
                            return false;
                        }
                        serverRequest.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Error;
                        serverRequest.errorMessage = "Not Connected to internet! Please connect and try again";
                        return false;
                    }
                    else
                    {
                        if (serverRequest.cacheGetStrategy != CacheGetStrategy.GetFromCacheOnlyIfRequestFails && serverRequest.TryGetFromCache())
                        {
                            return false;
                        }
                        this._003Cquery_003E5__2 = serverRequest.CreateQuery();
                        serverRequest.progress = this._003Cquery_003E5__2.progress;
                        this._003Ctime_003E5__3 = 0f;
                        this._003Ctimeout_003E5__4 = false;
                    }
                }
                if (!this._003Cquery_003E5__2.isDone)
                {
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 1;
                    return true;
                }
                IL_10D:
                if (!this._003Ctimeout_003E5__4 && string.IsNullOrEmpty(this._003Cquery_003E5__2.error) && this._003Cquery_003E5__2.bytes != null)
                {
                    serverRequest.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                    serverRequest.ParseQueryResponse(this._003Cquery_003E5__2);
                    if (serverRequest.cache == CacheStategy.GetFromCache)
                    {
                        serverRequest.CacheResults(this._003Cquery_003E5__2);
                    }
                    else if (serverRequest.cache == CacheStategy.CacheToFile)
                    {
                        serverRequest.CacheToFile(this._003Cquery_003E5__2);
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Request Failed");
                    if (serverRequest.cacheGetStrategy == CacheGetStrategy.GetFromCacheOnlyIfRequestFails && serverRequest.TryGetFromCache())
                    {
                        return false;
                    }
                    if (!this._003Ctimeout_003E5__4)
                    {
                        serverRequest.errorMessage = this._003Cquery_003E5__2.text;
                        if (this._003Cquery_003E5__2 != null && Application.isEditor)
                        {
                            UnityEngine.Debug.Log("URL: " + this._003Cquery_003E5__2.url);
                        }
                        UnityEngine.Debug.Log("ERROR: " + serverRequest.errorMessage);
                    }
                    else
                    {
                        UnityEngine.Debug.Log("TIMEOUT");
                    }
                    serverRequest.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Error;
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

            public GGServerRequestsBackend.ServerRequest _003C_003E4__this;

            private WWW _003Cquery_003E5__2;

            private float _003Ctime_003E5__3;

            private bool _003Ctimeout_003E5__4;
        }
    }

    public class ProtoRequest<T> : GGServerRequestsBackend.ServerRequest where T : class
    {
        public T response
        {
            get
            {
                return base.GetResponse<T>();
            }
            set
            {
                this.responseObj = value;
            }
        }

        protected override WWW CreateQuery()
        {
            return null;
        }

        protected override void ParseResponse(byte[] bytes)
        {
            T t = default(T);
            if (ProtoIO.LoadFromByteStream<T>(bytes, out t))
            {
                this.responseObj = t;
                return;
            }
            UnityEngine.Debug.Log("failed to load");
            this.responseObj = null;
        }
    }

    private interface NonceSetRequest
    {
        void SetNonce(string nonce);
    }

    private interface PidSetRequest
    {
        void SetPid(string pid);
    }

    public class ProtoRequestPid<T> : GGServerRequestsBackend.ProtoRequest<T>, GGServerRequestsBackend.PidSetRequest where T : class
    {
        public virtual void SetPid(string pid)
        {
        }
    }

    public class UploadLeadDataRequest : GGServerRequestsBackend.ProtoRequestPid<StatusMessage>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.uploadLeadDataPath).addParams("gameName", this.app).addParams("eventId", this.eventId).addParams("playerId", this.pid).addParams("DataId", this.dataId).addParams("nonce", this.nonce).addData(this.data).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["Content-Type"] = "text/plain";
            return new WWW(this.GetUrl(), Encoding.UTF8.GetBytes(this.data), dictionary);
        }

        private string app;

        private string pid;

        private string data;

        public string eventId;

        public string dataId;

        public string nonce;
    }

    public class EventLeadRequest : GGServerRequestsBackend.ProtoRequestPid<EventLeads>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getOnlineEventsLeaderboardsPath).addParams("app", this.appName).addParams("res", "proto").addParams("PlayerId", this.pid).addParams("Eventid", this.eventId).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        public string appName;

        public string eventId;

        public string pid;
    }

    public class EventScoreUpdateRequest : GGServerRequestsBackend.ProtoRequestPid<UpdateLeadResponse>
    {
        public int score1
        {
            get
            {
                return this.s;
            }
        }

        public double score2
        {
            get
            {
                return this.s2;
            }
        }

        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.updateOnlineEventsPath).addParams("app", this.app).addParams("res", "proto").addParams("PlayerId", this.pid).addParams("Eventid", this.eventId).addParams("Score1", this.score1).addParams("Score2", this.score2).addParams("player_name", this.name).addParams("player_rank", this.rank).addParams("player_flag", this.countryFlag).addParams("imageUrl", this.imageUrl).addParams("nonce", this.nonce).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string pid;

        private string app;

        private int rank;

        private string name;

        private string countryFlag;

        private int s;

        private double s2;

        private string imageUrl;

        public string nonce;

        public string eventId;
    }

    public class NonceRequest : GGServerRequestsBackend.ServerRequest
    {
        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.nonceUrlPath).ToString();
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }
    }

    public class LeaderboardsRequest : GGServerRequestsBackend.ProtoRequestPid<Lead>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            GGServerRequestsBackend.UrlBuilder urlBuilder = new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.leadUrlPath).addParams("app", this.app).addParams("pid", this.pid).addParams("sR", this.sR.ToString()).addParams("country", this.country).addParams("res", "protobuf").addParams("distAroundPlayer", this.distAroundPlayer).addParams("topEntries", this.topEntries.ToString()).addParams("leadTotalEntries", this.leadTotalEntries).addParams("lv", this.version.ToString());
            if (this.comp >= 0)
            {
                urlBuilder.addParams("comp", this.comp.ToString());
            }
            if (this.e >= 0)
            {
                urlBuilder.addParams("e", this.e.ToString());
            }
            return urlBuilder.SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private int comp;

        private int e;

        private int sR;

        private string country;

        private string distAroundPlayer;

        private int topEntries;

        private string leadTotalEntries;

        private int version;
    }

    public class UpdateAppMessageRead : GGServerRequestsBackend.ProtoRequestPid<StatusMessage>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.updateAppMessagesPath).addParams("message_id", this.messageId).addParams("pid", this.pid).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string messageId;

        private string pid;
    }

    public class AppMessagesRequest : GGServerRequestsBackend.ProtoRequestPid<MessageList>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getAppMessagesPath).addParams("app", this.app).addParams("playerID", this.pid).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;
    }

    public class SegmentedLeaderboardsRequest : GGServerRequestsBackend.ProtoRequestPid<CombinationLeads>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            GGServerRequestsBackend.UrlBuilder urlBuilder = new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getSegmentedLeaderboards).addParams("app", this.app).addParams("pid", this.pid).addParams("sR", this.sR.ToString()).addParams("country", this.country).addParams("res", "protobuf").addParams("distAroundPlayer", this.distAroundPlayer).addParams("topEntries", this.topEntries.ToString()).addParams("maxEntriesPerLead", GGServerConstants.instance.maxEntriesPerLead.ToString()).addParams("leadTotalEntries", this.leadTotalEntries).addParams("lv", this.version.ToString());
            if (this.comp >= 0)
            {
                urlBuilder.addParams("comp", this.comp.ToString());
            }
            if (this.e >= 0)
            {
                urlBuilder.addParams("e", this.e.ToString());
            }
            return urlBuilder.SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private int comp;

        private int e;

        private int sR;

        private string country;

        private string distAroundPlayer;

        private int topEntries;

        private string leadTotalEntries;

        private int version;
    }

    public class ActiveCompetitionRequest : GGServerRequestsBackend.ProtoRequest<CompetitionMessage>
    {
        public TimeSpan timeSpanTillEndOfCompetition
        {
            get
            {
                return this.endTime - DateTime.Now;
            }
        }

        protected override bool TryGetFromMemoryCache(string url)
        {
            GGServerRequestsBackend.ActiveCompetitionRequest activeCompetitionRequest = Singleton<GGRequestCache>.Instance.Get<GGServerRequestsBackend.ActiveCompetitionRequest>(this.GetCacheKey(url));
            if (activeCompetitionRequest != null)
            {
                this.endTime = activeCompetitionRequest.endTime;
                base.response = activeCompetitionRequest.response;
            }
            return activeCompetitionRequest != null;
        }

        protected override void CacheResults(WWW query)
        {
            Singleton<GGRequestCache>.Instance.Put(this.GetCacheKey(query.url), this, this.timeSpanTillEndOfCompetition);
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getActiveCompetitionUrlPath).addParams("app", GGServerConstants.instance.appName).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override void ParseResponse(byte[] bytes)
        {
            base.ParseResponse(bytes);
            this.endTime = GGServerRequestsBackend.GetDateTimeCompEnd(base.response.compEndTimestamp);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private DateTime endTime;
    }

    public class UpdateRequest : GGServerRequestsBackend.ProtoRequestPid<StatusMessage>
    {
        public UpdateRequest(int sR, string n, string c)
        {
            this.app = GGServerConstants.instance.appName;
            this.pid = "";
            this.sR = sR;
            this.n = n;
            this.c = c;
            this.s = 0;
            this.imageUrl = "";
            this.version = GGServerConstants.instance.leadVersion;
        }

        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        public void SetScore(int s)
        {
            this.s = s;
        }

        protected override string GetUrl()
        {
            GGServerRequestsBackend.UrlBuilder urlBuilder = new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.updateUrlPath).addParams("app", this.app).addParams("pid", this.pid).addParams("sR", this.sR.ToString()).addParams("c", this.c).addParams("nonce", this.nonce).addParams("rank2", this.rank2).addParams("lv", this.version.ToString());
            if (this.n != "")
            {
                urlBuilder.addParams("n", this.n);
            }
            if (this.s >= 0)
            {
                urlBuilder.addParams("s", this.s.ToString());
            }
            if (this.imageUrl != "")
            {
                urlBuilder.addParams("image", this.imageUrl);
            }
            if (!string.IsNullOrEmpty(this.room))
            {
                urlBuilder.addParams("room", this.room);
            }
            return urlBuilder.SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string pid;

        private string app;

        private int sR;

        private string n;

        private string c;

        private int s;

        private string imageUrl;

        private int version;

        public string nonce;

        public int rank2;

        public string room;
    }

    public class IdRequest : GGServerRequestsBackend.ProtoRequest<Pid>
    {
        public IdRequest()
        {
            this.installId = GGUID.InstallId();
            this.fbId = "";
            this.gId = "";
            this.fbIdForApp = "";
            GGPlayerSettings instance = GGPlayerSettings.instance;
            string facebookPlayerId = instance.Model.facebookPlayerId;
            string applePlayerId = instance.Model.applePlayerId;
            bool flag = GGUtil.HasText(applePlayerId);
            if (GGUtil.HasText(facebookPlayerId))
            {
                this.fbIdForApp = facebookPlayerId + ConfigBase.instance.facebookAppPlayerSuffix;
            }
            else if (flag)
            {
                this.fbIdForApp = applePlayerId + "-apl-" + ConfigBase.instance.facebookAppPlayerSuffix;
            }
            this.app = GGServerConstants.instance.appName;
        }

        protected override string GetUrl()
        {
            GGServerRequestsBackend.UrlBuilder urlBuilder = new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getIdUrlPath).addParams("installId", this.installId).addParams("app", this.app);
            if (this.fbId != "")
            {
                urlBuilder.addParams("fbId", this.fbId);
            }
            if (this.fbIdForApp != "")
            {
                urlBuilder.addParams("fbIdForApp", this.fbIdForApp);
            }
            if (this.gId != "")
            {
                urlBuilder.addParams("gId", this.gId);
            }
            return urlBuilder.SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string fbId;

        private string gId;

        private string installId;

        private string app;

        private string fbIdForApp;
    }

    public class GetPrizesRequest : GGServerRequestsBackend.ProtoRequestPid<Lead>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getPrizesUrlPath).addParams("app", this.app).addParams("pid", this.pid).addParams("sR", this.sR.ToString()).addParams("country", this.country).addParams("distAroundPlayer", this.distAroundPlayer).addParams("topEntries", this.topEntries.ToString()).addParams("leadTotalEntries", this.leadTotalEntries).addParams("leadType", this.leadType).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private int sR;

        private string country;

        private string distAroundPlayer;

        private int topEntries;

        private string leadTotalEntries;

        private string leadType;
    }

    public class GetPrizesRequestCombinationLead : GGServerRequestsBackend.ProtoRequestPid<CombinationLeads>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getPrizesUrlPath).addParams("app", this.app).addParams("pid", this.pid).addParams("sR", this.sR.ToString()).addParams("country", this.country).addParams("distAroundPlayer", this.distAroundPlayer).addParams("topEntries", this.topEntries.ToString()).addParams("leadTotalEntries", this.leadTotalEntries).addParams("maxEntriesPerLead", GGServerConstants.instance.maxEntriesPerLead.ToString()).addParams("leadType", this.leadType).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private int sR;

        private string country;

        private string distAroundPlayer;

        private int topEntries;

        private string leadTotalEntries;

        private string leadType;
    }

    public class AckPrizesRequest : GGServerRequestsBackend.ProtoRequestPid<StatusMessage>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.ackPrizesUrlPath).addParams("app", this.app).addParams("pid", this.pid).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;
    }

    public class CloudSyncRequest : GGServerRequestsBackend.ProtoRequestPid<CloudSyncData>
    {
        public int snapshotId
        {
            get
            {
                return this._003CsnapshotId_003Ek__BackingField;
            }
            protected set
            {
                this._003CsnapshotId_003Ek__BackingField = value;
            }
        }

        public string snapshotGUID
        {
            get
            {
                return this._003CsnapshotGUID_003Ek__BackingField;
            }
            protected set
            {
                this._003CsnapshotGUID_003Ek__BackingField = value;
            }
        }

        public override void SetPid(string pid)
        {
        }

        public void SetVersionInfo(int snapshotId, string snapshotGUID)
        {
            this.snapshotId = snapshotId;
            this.snapshotGUID = snapshotGUID;
        }

        public virtual CloudSyncData GetRequestData()
        {
            return null;
        }

        public string nonce;

        private int _003CsnapshotId_003Ek__BackingField;

        private string _003CsnapshotGUID_003Ek__BackingField;
    }

    public class GetCloudSyncDataRequest : GGServerRequestsBackend.CloudSyncRequest
    {
        public GetCloudSyncDataRequest()
        {
            this.app = GGServerConstants.instance.appName;
            this.pid = "";
            base.snapshotId = -1;
            base.snapshotGUID = "";
        }

        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        public override CloudSyncData GetRequestData()
        {
            return null;
        }

        protected override string GetUrl()
        {
            GGServerRequestsBackend.UrlBuilder urlBuilder = new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getCloudSyncUrlPath).addParams("app", this.app).addParams("pid", this.pid).addParams("nonce", this.nonce);
            if (ConfigBase.instance.cloudSyncType == ConfigBase.GGFileIOCloudSyncTypes.GGSnapshotCloudSync)
            {
                urlBuilder = urlBuilder.addParams("addSnapshotInfo", "true");
            }
            return urlBuilder.SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;
    }

    public class UpdateCloudSyncDataRequest : GGServerRequestsBackend.CloudSyncRequest
    {
        public UpdateCloudSyncDataRequest()
        {
            this.app = GGServerConstants.instance.appName;
            this.pid = "";
            base.snapshotId = -1;
            base.snapshotGUID = "";
        }

        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        public void AddData(CloudSyncData syncData)
        {
            this.data = ProtoIO.SerializeToByte64<CloudSyncData>(syncData);
        }

        public override CloudSyncData GetRequestData()
        {
            CloudSyncData result = null;
            if (!ProtoIO.LoadFromBase64String<CloudSyncData>(this.data, out result))
            {
                result = new CloudSyncData();
            }
            return result;
        }

        protected override string GetUrl()
        {
            GGServerRequestsBackend.UrlBuilder urlBuilder = new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.updateCloudSyncUrlPath).addParams("app", this.app).addParams("pid", this.pid).addParams("nonce", this.nonce).addData(this.data);
            if (base.snapshotId >= 0)
            {
                urlBuilder = urlBuilder.addParams("snapshotId", base.snapshotId.ToString()).addParams("snapshotGUID", base.snapshotGUID);
            }
            return urlBuilder.SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["Content-Type"] = "text/plain";
            return new WWW(this.GetUrl(), Encoding.UTF8.GetBytes(this.data), dictionary);
        }

        private string app;

        private string pid;

        private string data;
    }

    public class FacebookLoginRequest : GGServerRequestsBackend.ProtoRequest<FBLogin>
    {
    }

    public class FacebookInviteFriends : GGServerRequestsBackend.ProtoRequestPid<InvitableFriends>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.FBInvitableFriendsPath).addParams("app", this.app).addParams("pid", this.pid).addParams("pages", this.pagesToFetch.ToString()).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private int pagesToFetch;
    }

    public class FacebookPlayingFriends : GGServerRequestsBackend.ProtoRequestPid<InvitableFriends>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.FBPlayingFriendsPath).addParams("app", this.app).addParams("pid", this.pid).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;
    }

    public class GGServerPlayerMessages : GGServerRequestsBackend.ProtoRequestPid<ServerMessages>
    {
        public GGServerPlayerMessages()
        {
            this.app = GGServerConstants.instance.appName;
            this.pid = "";
        }

        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.PlayerGetMessagesPath).addParams("app", this.app).addParams("pid", this.pid).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;
    }

    public class GGServerMarkMessagesAsRead : GGServerRequestsBackend.ProtoRequestPid<StatusMessage>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.MarkMessageReadPath).addParams("app", this.app).addParams("pid", this.pid).addParams("requestIds", this.requestIds).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private string requestIds;
    }

    public class GGServerGetFriendProfiles : GGServerRequestsBackend.ProtoRequestPid<FriendsProfiles>
    {
        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getFriendProfilesPath).addParams("app", this.app).addParams("pid", this.pid).addParams("filename", this.files).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            return new WWW(this.GetUrl());
        }

        private string app;

        private string pid;

        private List<string> files;
    }

    public class GetPlayerPositionsRequest : GGServerRequestsBackend.ProtoRequestPid<PlayerPositions>
    {
        public GetPlayerPositionsRequest()
        {
            this.app = GGServerConstants.instance.appName;
            this.pid = "";
        }

        public override void SetPid(string pid)
        {
            this.pid = pid;
        }

        public void AddData(PlayerPositions players)
        {
            this.data = ProtoIO.SerializeToByte64<PlayerPositions>(players);
        }

        protected override string GetUrl()
        {
            return new GGServerRequestsBackend.UrlBuilder(GGServerConstants.instance.urlBase).addPath(GGServerConstants.instance.getPlayerPositionList).addParams("app", this.app).addParams("pid", this.pid).addParams("res", "protobuf").addData(this.data).SignAndToString(GGServerConstants.instance.publicKey, GGServerConstants.instance.privateKey);
        }

        protected override WWW CreateQuery()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["Content-Type"] = "text/plain";
            return new WWW(this.GetUrl(), Encoding.UTF8.GetBytes(this.data), dictionary);
        }

        private string app;

        private string pid;

        private string data;
    }

    private sealed class _003CDoCallWhenRequestComplete_003Ed__61 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoCallWhenRequestComplete_003Ed__61(int _003C_003E1__state)
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
                if (this.request == null)
                {
                    if (this.onComplete != null)
                    {
                        this.onComplete(this.request);
                    }
                    return false;
                }
            }
            if (this.request.status == GGServerRequestsBackend.ServerRequest.RequestStatus.NotSent)
            {
                if (this.onComplete != null)
                {
                    this.onComplete(this.request);
                }
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

        public GGServerRequestsBackend.ServerRequest request;

        public Action<GGServerRequestsBackend.ServerRequest> onComplete;
    }

    private sealed class _003C_003Ec__DisplayClass64_0
    {
        internal void _003CDoUpdateEventsLeaderboards_003Eb__0(GGServerRequestsBackend.ServerRequest pid)
        {
            this.playerId = pid;
        }

        public GGServerRequestsBackend.ServerRequest playerId;
    }

    private sealed class _003CDoUpdateEventsLeaderboards_003Ed__64 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoUpdateEventsLeaderboards_003Ed__64(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new GGServerRequestsBackend._003C_003Ec__DisplayClass64_0();
                    this._003C_003E8__1.playerId = null;
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(ggserverRequestsBackend.GetPlayerId(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoUpdateEventsLeaderboards_003Eb__0)));
                    this._003C_003E1__state = 1;
                    return true;
                case 1:
                    this._003C_003E1__state = -1;
                    this._003CnonceReq_003E5__2 = new GGServerRequestsBackend.NonceRequest();
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CnonceReq_003E5__2.RequestCoroutine());
                    this._003C_003E1__state = 2;
                    return true;
                case 2:
                    this._003C_003E1__state = -1;
                    if (this._003C_003E8__1.playerId.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success && this._003CnonceReq_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this.updateRequest.SetPid(this._003C_003E8__1.playerId.GetResponse<Pid>().pid);
                        this.updateRequest.nonce = this._003CnonceReq_003E5__2.GetResponse<string>();
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.updateRequest.RequestCoroutine());
                        this._003C_003E1__state = 3;
                        return true;
                    }
                    break;
                case 3:
                    this._003C_003E1__state = -1;
                    break;
                default:
                    return false;
            }
            if (this.onComplete != null)
            {
                this.onComplete(this.updateRequest);
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

        public GGServerRequestsBackend _003C_003E4__this;

        private GGServerRequestsBackend._003C_003Ec__DisplayClass64_0 _003C_003E8__1;

        public GGServerRequestsBackend.EventScoreUpdateRequest updateRequest;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__2;
    }

    private sealed class _003C_003Ec__DisplayClass69_0
    {
        internal void _003CDoUpdateUser_003Eb__0(GGServerRequestsBackend.ServerRequest pid)
        {
            this.playerId = pid;
        }

        public GGServerRequestsBackend.ServerRequest playerId;
    }

    private sealed class _003CDoUpdateUser_003Ed__69 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoUpdateUser_003Ed__69(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new GGServerRequestsBackend._003C_003Ec__DisplayClass69_0();
                    this._003C_003E8__1.playerId = null;
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(ggserverRequestsBackend.GetPlayerId(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoUpdateUser_003Eb__0)));
                    this._003C_003E1__state = 1;
                    return true;
                case 1:
                    this._003C_003E1__state = -1;
                    this._003CnonceReq_003E5__2 = new GGServerRequestsBackend.NonceRequest();
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CnonceReq_003E5__2.RequestCoroutine());
                    this._003C_003E1__state = 2;
                    return true;
                case 2:
                    this._003C_003E1__state = -1;
                    if (this._003C_003E8__1.playerId.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success && this._003CnonceReq_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this.update.SetPid(this._003C_003E8__1.playerId.GetResponse<Pid>().pid);
                        this.update.nonce = this._003CnonceReq_003E5__2.GetResponse<string>();
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.update.RequestCoroutine());
                        this._003C_003E1__state = 3;
                        return true;
                    }
                    break;
                case 3:
                    this._003C_003E1__state = -1;
                    break;
                default:
                    return false;
            }
            UnityEngine.Debug.Log("Update success " + this.update.status);
            if (this.onComplete != null)
            {
                this.onComplete(this.update);
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

        public GGServerRequestsBackend _003C_003E4__this;

        private GGServerRequestsBackend._003C_003Ec__DisplayClass69_0 _003C_003E8__1;

        public GGServerRequestsBackend.UpdateRequest update;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__2;
    }

    private sealed class _003CGetPlayerId_003Ed__71 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CGetPlayerId_003Ed__71(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            if (num != 0)
            {
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
                if (this._003CidRequest_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                {
                    if (string.IsNullOrEmpty(ggserverRequestsBackend.memoryCachedPlayerId))
                    {
                        ggserverRequestsBackend.cachedPlayerId = this._003CidRequest_003E5__2.GetResponse<Pid>().pid;
                    }
                }
                else if (!string.IsNullOrEmpty(ggserverRequestsBackend.storage.lastKnownPid))
                {
                    this._003CidRequest_003E5__2.response = new Pid();
                    this._003CidRequest_003E5__2.response.pid = ggserverRequestsBackend.cachedPlayerId;
                    this._003CidRequest_003E5__2.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                    this.onComplete(this._003CidRequest_003E5__2);
                    return false;
                }
                if (this.onComplete != null)
                {
                    this.onComplete(this._003CidRequest_003E5__2);
                }
                return false;
            }
            else
            {
                this._003C_003E1__state = -1;
                this._003CidRequest_003E5__2 = new GGServerRequestsBackend.IdRequest();
                if (ConfigBase.instance.isFakePlayerIdOn)
                {
                    UnityEngine.Debug.Log("USING FAKE ID:" + ConfigBase.instance.playerId);
                    ggserverRequestsBackend.memoryCachedPlayerId = ConfigBase.instance.playerId;
                }
                if (!string.IsNullOrEmpty(ggserverRequestsBackend.memoryCachedPlayerId))
                {
                    this._003CidRequest_003E5__2.response = new Pid();
                    this._003CidRequest_003E5__2.response.pid = ggserverRequestsBackend.memoryCachedPlayerId;
                    this._003CidRequest_003E5__2.status = GGServerRequestsBackend.ServerRequest.RequestStatus.Success;
                    this.onComplete(this._003CidRequest_003E5__2);
                    return false;
                }
                this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CidRequest_003E5__2.RequestCoroutine());
                this._003C_003E1__state = 1;
                return true;
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

        public GGServerRequestsBackend _003C_003E4__this;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.IdRequest _003CidRequest_003E5__2;
    }

    private sealed class _003CGetFacebookLogin_003Ed__72 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CGetFacebookLogin_003Ed__72(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            if (num == 0)
            {
                this._003C_003E1__state = -1;
                UnityEngine.Debug.Log("GetFacebookLogin");
                this._003CloginRequest_003E5__2 = new GGServerRequestsBackend.FacebookLoginRequest();
                this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CloginRequest_003E5__2.RequestCoroutine());
                this._003C_003E1__state = 1;
                return true;
            }
            if (num != 1)
            {
                return false;
            }
            this._003C_003E1__state = -1;
            if (this._003CloginRequest_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
            {
                UnityEngine.Debug.Log("Caching player id");
                ggserverRequestsBackend.cachedPlayerId = this._003CloginRequest_003E5__2.GetResponse<FBLogin>().pid;
            }
            if (this.onComplete != null)
            {
                this.onComplete(this._003CloginRequest_003E5__2);
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

        public GGServerRequestsBackend _003C_003E4__this;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.FacebookLoginRequest _003CloginRequest_003E5__2;
    }

    private sealed class _003CDoRequest_003Ed__74 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoRequest_003Ed__74(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            if (num == 0)
            {
                this._003C_003E1__state = -1;
                this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.req.RequestCoroutine());
                this._003C_003E1__state = 1;
                return true;
            }
            if (num != 1)
            {
                return false;
            }
            this._003C_003E1__state = -1;
            if (this.onComplete != null)
            {
                this.onComplete(this.req);
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

        public GGServerRequestsBackend _003C_003E4__this;

        public GGServerRequestsBackend.ActiveCompetitionRequest req;

        public GGServerRequestsBackend.OnComplete onComplete;
    }

    private sealed class _003C_003Ec__DisplayClass80_0
    {
        internal void _003CDoSyncCsData_003Eb__0(GGServerRequestsBackend.ServerRequest pid)
        {
            this.playerId = pid;
        }

        public GGServerRequestsBackend.ServerRequest playerId;
    }

    private sealed class _003CDoSyncCsData_003Ed__80 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoSyncCsData_003Ed__80(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new GGServerRequestsBackend._003C_003Ec__DisplayClass80_0();
                    this._003C_003E8__1.playerId = null;
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(ggserverRequestsBackend.GetPlayerId(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoSyncCsData_003Eb__0)));
                    this._003C_003E1__state = 1;
                    return true;
                case 1:
                    this._003C_003E1__state = -1;
                    this._003CnonceReq_003E5__2 = new GGServerRequestsBackend.NonceRequest();
                    UnityEngine.Debug.Log("nonce cache policy: " + this._003CnonceReq_003E5__2.cache);
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CnonceReq_003E5__2.RequestCoroutine());
                    this._003C_003E1__state = 2;
                    return true;
                case 2:
                    this._003C_003E1__state = -1;
                    if (this._003C_003E8__1.playerId.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success && this._003CnonceReq_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this.dataRequest.SetPid(this._003C_003E8__1.playerId.GetResponse<Pid>().pid);
                        this.dataRequest.nonce = this._003CnonceReq_003E5__2.GetResponse<string>();
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.dataRequest.RequestCoroutine());
                        this._003C_003E1__state = 3;
                        return true;
                    }
                    break;
                case 3:
                    this._003C_003E1__state = -1;
                    break;
                default:
                    return false;
            }
            if (this.onComplete != null)
            {
                this.onComplete(this.dataRequest);
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

        public GGServerRequestsBackend _003C_003E4__this;

        private GGServerRequestsBackend._003C_003Ec__DisplayClass80_0 _003C_003E8__1;

        public GGServerRequestsBackend.CloudSyncRequest dataRequest;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__2;
    }

    private sealed class _003C_003Ec__DisplayClass82_0
    {
        internal void _003CDoUploadLeadData_003Eb__0(GGServerRequestsBackend.ServerRequest pid)
        {
            this.playerId = pid;
        }

        public GGServerRequestsBackend.ServerRequest playerId;
    }

    private sealed class _003CDoUploadLeadData_003Ed__82 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoUploadLeadData_003Ed__82(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new GGServerRequestsBackend._003C_003Ec__DisplayClass82_0();
                    this._003C_003E8__1.playerId = null;
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(ggserverRequestsBackend.GetPlayerId(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoUploadLeadData_003Eb__0)));
                    this._003C_003E1__state = 1;
                    return true;
                case 1:
                    this._003C_003E1__state = -1;
                    this._003CnonceReq_003E5__2 = new GGServerRequestsBackend.NonceRequest();
                    UnityEngine.Debug.Log("nonce cache policy: " + this._003CnonceReq_003E5__2.cache);
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CnonceReq_003E5__2.RequestCoroutine());
                    this._003C_003E1__state = 2;
                    return true;
                case 2:
                    this._003C_003E1__state = -1;
                    if (this._003C_003E8__1.playerId.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success && this._003CnonceReq_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this.dataRequest.SetPid(this._003C_003E8__1.playerId.GetResponse<Pid>().pid);
                        this.dataRequest.nonce = this._003CnonceReq_003E5__2.GetResponse<string>();
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.dataRequest.RequestCoroutine());
                        this._003C_003E1__state = 3;
                        return true;
                    }
                    break;
                case 3:
                    this._003C_003E1__state = -1;
                    break;
                default:
                    return false;
            }
            if (this.onComplete != null)
            {
                this.onComplete(this.dataRequest);
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

        public GGServerRequestsBackend _003C_003E4__this;

        private GGServerRequestsBackend._003C_003Ec__DisplayClass82_0 _003C_003E8__1;

        public GGServerRequestsBackend.UploadLeadDataRequest dataRequest;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__2;
    }

    private sealed class _003C_003Ec__DisplayClass93_0
    {
        internal void _003CDoExecuteAllInterfacesRequest_003Eb__0(GGServerRequestsBackend.ServerRequest pid)
        {
            if (pid.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
            {
                this.setPid.SetPid(pid.GetResponse<Pid>().pid);
            }
        }

        public GGServerRequestsBackend.PidSetRequest setPid;
    }

    private sealed class _003CDoExecuteAllInterfacesRequest_003Ed__93 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoExecuteAllInterfacesRequest_003Ed__93(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    {
                        this._003C_003E1__state = -1;
                        GGServerRequestsBackend._003C_003Ec__DisplayClass93_0 _003C_003Ec__DisplayClass93_ = new GGServerRequestsBackend._003C_003Ec__DisplayClass93_0();
                        _003C_003Ec__DisplayClass93_.setPid = (this.request as GGServerRequestsBackend.PidSetRequest);
                        if (_003C_003Ec__DisplayClass93_.setPid != null)
                        {
                            this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(ggserverRequestsBackend.GetPlayerId(new GGServerRequestsBackend.OnComplete(_003C_003Ec__DisplayClass93_._003CDoExecuteAllInterfacesRequest_003Eb__0)));
                            this._003C_003E1__state = 1;
                            return true;
                        }
                        break;
                    }
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    if (this._003CnonceReq_003E5__3.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this._003CsetNonce_003E5__2.SetNonce(this._003CnonceReq_003E5__3.GetResponse<string>());
                    }
                    this._003CnonceReq_003E5__3 = null;
                    goto IL_EF;
                case 3:
                    this._003C_003E1__state = -1;
                    if (this.onComplete != null)
                    {
                        this.onComplete(this.request);
                    }
                    return false;
                default:
                    return false;
            }
            this._003CsetNonce_003E5__2 = (this.request as GGServerRequestsBackend.NonceSetRequest);
            if (this._003CsetNonce_003E5__2 != null)
            {
                this._003CnonceReq_003E5__3 = new GGServerRequestsBackend.NonceRequest();
                this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CnonceReq_003E5__3.RequestCoroutine());
                this._003C_003E1__state = 2;
                return true;
            }
            IL_EF:
            this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.request.RequestCoroutine());
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

        public GGServerRequestsBackend.ServerRequest request;

        public GGServerRequestsBackend _003C_003E4__this;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.NonceSetRequest _003CsetNonce_003E5__2;

        private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__3;
    }

    private sealed class _003CDoExecuteNonceRequest_003Ed__94 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoExecuteNonceRequest_003Ed__94(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003CsetNonce_003E5__2 = (this.request as GGServerRequestsBackend.NonceSetRequest);
                    if (this._003CsetNonce_003E5__2 != null)
                    {
                        this._003CnonceReq_003E5__3 = new GGServerRequestsBackend.NonceRequest();
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003CnonceReq_003E5__3.RequestCoroutine());
                        this._003C_003E1__state = 1;
                        return true;
                    }
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.request.RequestCoroutine());
                    this._003C_003E1__state = 3;
                    return true;
                case 1:
                    this._003C_003E1__state = -1;
                    if (this._003CnonceReq_003E5__3.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this._003CsetNonce_003E5__2.SetNonce(this._003CnonceReq_003E5__3.GetResponse<string>());
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.request.RequestCoroutine());
                        this._003C_003E1__state = 2;
                        return true;
                    }
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    break;
                case 3:
                    this._003C_003E1__state = -1;
                    goto IL_F5;
                default:
                    return false;
            }
            this._003CnonceReq_003E5__3 = null;
            IL_F5:
            if (this.onComplete != null)
            {
                this.onComplete(this.request);
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

        public GGServerRequestsBackend.ServerRequest request;

        public GGServerRequestsBackend _003C_003E4__this;

        public GGServerRequestsBackend.OnComplete onComplete;

        private GGServerRequestsBackend.NonceSetRequest _003CsetNonce_003E5__2;

        private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__3;
    }

    private sealed class _003CDoExecuteRequest_003Ed__95 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoExecuteRequest_003Ed__95(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            if (num == 0)
            {
                this._003C_003E1__state = -1;
                this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this.request.RequestCoroutine());
                this._003C_003E1__state = 1;
                return true;
            }
            if (num != 1)
            {
                return false;
            }
            this._003C_003E1__state = -1;
            if (this.onComplete != null)
            {
                this.onComplete(this.request);
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

        public GGServerRequestsBackend _003C_003E4__this;

        public GGServerRequestsBackend.ServerRequest request;

        public GGServerRequestsBackend.OnComplete onComplete;
    }

    private sealed class _003C_003Ec__DisplayClass96_0<T> where T : class
    {
        internal void _003CDoExecuteRequestWithPid_003Eb__0(GGServerRequestsBackend.ServerRequest pid)
        {
            this.playerId = pid;
            if (pid.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
            {
                this.request.SetPid(pid.GetResponse<Pid>().pid);
            }
        }

        public GGServerRequestsBackend.ServerRequest playerId;

        public GGServerRequestsBackend.ProtoRequestPid<T> request;
    }

    private sealed class _DoExecuteRequestWithPid<T> : IEnumerator<object>, IEnumerator, IDisposable where T : class
    {
        [DebuggerHidden]
        public _DoExecuteRequestWithPid(int _003C_003E1__state)
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
            GGServerRequestsBackend ggserverRequestsBackend = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003C_003E8__1 = new GGServerRequestsBackend._003C_003Ec__DisplayClass96_0<T>();
                    this._003C_003E8__1.request = this.request;
                    this._003C_003E8__1.playerId = null;
                    this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(ggserverRequestsBackend.GetPlayerId(new GGServerRequestsBackend.OnComplete(this._003C_003E8__1._003CDoExecuteRequestWithPid_003Eb__0)));
                    this._003C_003E1__state = 1;
                    return true;
                case 1:
                    this._003C_003E1__state = -1;
                    if (this._003C_003E8__1.playerId.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
                    {
                        this._003C_003E2__current = ggserverRequestsBackend.StartCoroutine(this._003C_003E8__1.request.RequestCoroutine());
                        this._003C_003E1__state = 2;
                        return true;
                    }
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    break;
                default:
                    return false;
            }
            if (this.onComplete != null)
            {
                this.onComplete(this._003C_003E8__1.request);
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

        public GGServerRequestsBackend.ProtoRequestPid<T> request;

        public GGServerRequestsBackend _003C_003E4__this;

        private GGServerRequestsBackend._003C_003Ec__DisplayClass96_0<T> _003C_003E8__1;

        public GGServerRequestsBackend.OnComplete onComplete;
    }
}
