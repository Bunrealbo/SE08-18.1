using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class DecoratingScene : MonoBehaviour
{
    public DecoratingScene.GroupDefinition GetGroupForIndex(int index)
    {
        for (int i = 0; i < this.groupDefinitions.Count; i++)
        {
            DecoratingScene.GroupDefinition groupDefinition = this.groupDefinitions[i];
            if (groupDefinition.groupIndex == index)
            {
                return groupDefinition;
            }
        }
        return null;
    }

    public DecoratingScene.GroupDefinition CurrentGroup()
    {
        for (int i = 0; i < this.groupDefinitions.Count; i++)
        {
            DecoratingScene.GroupDefinition groupDefinition = this.groupDefinitions[i];
            if (!this.IsAllElementsPickedUpInGroup(groupDefinition.groupIndex))
            {
                return groupDefinition;
            }
        }
        return null;
    }

    public bool IsAllElementsPickedUpInGroup(int index)
    {
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = this.visualObjectBehaviours[i];
            if (visualObjectBehaviour.visualObject.sceneObjectInfo.groupIndex == index && !visualObjectBehaviour.visualObject.isOwned)
            {
                return false;
            }
        }
        return true;
    }

    public int ownedItemsCount
    {
        get
        {
            int num = 0;
            for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
            {
                if (this.visualObjectBehaviours[i].visualObject.isOwned)
                {
                    num++;
                }
            }
            return num;
        }
    }

    public DecoratingSceneConfig.AnimationSequence runningAnimation
    {
        get
        {
            if (this.animationPlayer == null)
            {
                return null;
            }
            return this.animationPlayer.lastAnimation;
        }
    }

    public bool isCharacterAvailable
    {
        get
        {
            return this.animationPlayer != null;
        }
    }

    public Vector3 CharacterBubblePosition(CharacterAvatar avatar)
    {
        if (avatar == null)
        {
            return Vector3.zero;
        }
        Vector3 bubblePosition = avatar.bubblePosition;
        Vector3 vector = this.characterScene.WorldToScreenPoint(bubblePosition);
        return this.PSDToWorldPoint(new Vector2(vector.x, vector.y - (float)this.config.height));
    }

    public List<DecoratingSceneConfig.AnimationSequence> GetAvailableSequences()
    {
        if (this.animationPlayer == null)
        {
            return null;
        }
        DecoratingSceneConfig.AnimationSequenceGroup animationSequenceGroup = ScriptableObjectSingleton<DecoratingSceneConfig>.instance.GetAnimationSequenceGroup(base.name);
        if (animationSequenceGroup == null)
        {
            UnityEngine.Debug.Log("NO GROUP " + base.name);
            return null;
        }
        return animationSequenceGroup.AvailableSequences(this);
    }

    public void StopCharacterAnimation()
    {
        if (this.animationPlayer == null)
        {
            return;
        }
        this.animationPlayer.Stop();
        this.animationPlayer.HideAvatars();
    }

    public ChangeAnimationArguments AnimationForVisualBehaviour(VisualObjectBehaviour behaviour)
    {
        if (this.animationPlayer == null || behaviour == null)
        {
            return ChangeAnimationArguments.NoAnimation;
        }
        return ChangeAnimationArguments.Create(base.name, behaviour.visualObject.name);
    }

    public void PlayCharacterAnimation(ChangeAnimationArguments arguments)
    {
        if (this.animationPlayer == null)
        {
            if (arguments.onComplete != null)
            {
                arguments.onComplete();
            }
            return;
        }
        this.animationPlayer.Play(arguments);
    }

    public void SetCharacterAlpha(float alpha)
    {
        if (this.roomSceneRender == null)
        {
            return;
        }
        this.roomScreen.SetSpeachBubbleAlpha(alpha);
        this.roomSceneRender.SetAlpha(alpha);
    }

    public void SetCharacterAnimationAlpha(float alpha)
    {
        if (this.roomSceneRender == null)
        {
            return;
        }
        this.roomSceneRender.animationAlpha = alpha;
        this.roomScreen.SetSpeachBubbleAlpha(alpha);
    }

    public void AnimateCharacterAlphaTo(float alpha)
    {
        if (this.roomSceneRender == null)
        {
            return;
        }
        this.roomSceneRender.AnimateAlphaTo(alpha, 0.25f, this.roomScreen);
    }

    public Vector3 psdTransformationScale
    {
        get
        {
            return this.psdTransformationTransform.localScale;
        }
    }

    public Vector3 rootTransformScale
    {
        get
        {
            return this.rootTransform.localScale;
        }
        set
        {
            this.rootTransform.localScale = value;
        }
    }

    public Vector3 rootTransformOffset
    {
        get
        {
            return this.rootTransformOffset_;
        }
        set
        {
            this.rootTransformOffset_ = value;
            this.SetRootTransform();
        }
    }

    public Vector3 rootTransformOffsetAcceleration
    {
        get
        {
            return this.rootTransformOffsetAcceleration_;
        }
        set
        {
            this.rootTransformOffsetAcceleration_ = value;
            this.SetRootTransform();
        }
    }

    private void SetRootTransform()
    {
        this.rootTransform.localPosition = this.rootTransformOffsetAcceleration_ + this.rootTransformOffset_;
    }

    public GraphicsSceneConfig CreateConfig()
    {
        GraphicsSceneConfig graphicsSceneConfig = new GraphicsSceneConfig();
        graphicsSceneConfig.width = this.config.width;
        graphicsSceneConfig.height = this.config.height;
        graphicsSceneConfig.objects.Clear();
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = this.visualObjectBehaviours[i];
            if (!(visualObjectBehaviour == null))
            {
                GraphicsSceneConfig.VisualObject item = JsonUtility.FromJson<GraphicsSceneConfig.VisualObject>(JsonUtility.ToJson(visualObjectBehaviour.visualObject));
                graphicsSceneConfig.objects.Add(item);
            }
        }
        return graphicsSceneConfig;
    }

    public void ShowAll()
    {
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = this.visualObjectBehaviours[i];
            if (visualObjectBehaviour.isPlayerControlledObject)
            {
                visualObjectBehaviour.ShowVariationBehaviour(UnityEngine.Random.RandomRange(0, 3));
            }
        }
    }

    public void ShowGroup(int groupIndex)
    {
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = this.visualObjectBehaviours[i];
            if (visualObjectBehaviour.isPlayerControlledObject)
            {
                if (visualObjectBehaviour.visualObject.sceneObjectInfo.groupIndex == groupIndex)
                {
                    visualObjectBehaviour.ShowVariationBehaviour(0);
                }
                else
                {
                    visualObjectBehaviour.Hide();
                }
            }
        }
    }

    public void DestroyCharacterScene()
    {
        if (this.roomSceneRender != null)
        {
            UnityEngine.Object.DestroyImmediate(this.roomSceneRender.gameObject, true);
            this.roomSceneRender = null;
        }
        if (this.characterScene == null)
        {
            return;
        }
        GameObject gameObject = this.characterScene.gameObject;
        if (Application.isPlaying)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        else
        {
            UnityEngine.Object.DestroyImmediate(gameObject);
        }
        this.characterScene = null;
    }

    private void DestroyAllObjectBehaviours()
    {
        List<VisualObjectBehaviour> list = new List<VisualObjectBehaviour>();
        list.AddRange(this.visualObjectBehaviours);
        foreach (object obj in this.offsetTransform)
        {
            VisualObjectBehaviour component = ((Transform)obj).GetComponent<VisualObjectBehaviour>();
            if (component == null)
            {
                return;
            }
            if (!list.Contains(component))
            {
                list.Add(component);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = list[i];
            if (!(visualObjectBehaviour == null))
            {
                visualObjectBehaviour.DestroySelf();
            }
        }
        this.visualObjectBehaviours.Clear();
    }

    private VisualObjectBehaviour CreateVisualObjectBehaviour(GraphicsSceneConfig.VisualObject vo)
    {
        VisualObjectBehaviour visualObjectBehaviour = new GameObject
        {
            transform =
            {
                parent = this.offsetTransform,
                localPosition = Vector3.zero
            },
            name = vo.name,

        }.AddComponent<VisualObjectBehaviour>();
        visualObjectBehaviour.Init(vo);
        return visualObjectBehaviour;
    }

    public void CreateSceneBehaviours(GraphicsSceneConfig config)
    {
        this.DestroyAllObjectBehaviours();
        this.config = config;
        if (this.rootTransform == null)
        {
            this.rootTransform = new GameObject("root").transform;
            this.rootTransform.parent = base.transform;
            this.rootTransform.localPosition = Vector3.zero;
            this.rootTransform.localScale = Vector3.one;
            this.rootTransform.localRotation = Quaternion.identity;
        }
        if (this.offsetTransform == null)
        {
            this.offsetTransform = new GameObject("offset").transform;
            this.offsetTransform.parent = this.rootTransform;
            this.offsetTransform.localPosition = Vector3.zero;
            this.offsetTransform.localScale = Vector3.one;
            this.offsetTransform.localRotation = Quaternion.identity;
        }
        for (int i = 0; i < config.objects.Count; i++)
        {
            GraphicsSceneConfig.VisualObject vo = config.objects[i];
            VisualObjectBehaviour visualObjectBehaviour = this.CreateVisualObjectBehaviour(vo);
            if (visualObjectBehaviour.isPlayerControlledObject)
            {
                this.visualObjectBehaviours.Add(visualObjectBehaviour);
            }
        }
    }

    private Transform psdTransformationTransform
    {
        get
        {
            return this.offsetTransform;
        }
    }

    public DecoratingScene.RoomProgressState GetRoomProgressState()
    {
        int num = 0;
        int num2 = 0;
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = this.visualObjectBehaviours[i];
            if (visualObjectBehaviour.isPlayerControlledObject)
            {
                num2++;
                if (visualObjectBehaviour.visualObject.isOwned)
                {
                    num++;
                }
            }
        }
        return new DecoratingScene.RoomProgressState
        {
            completed = num,
            total = num2
        };
    }

    public VisualObjectBehaviour GetBehaviour(string name)
    {
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = this.visualObjectBehaviours[i];
            if (visualObjectBehaviour.name.ToLower() == name)
            {
                return visualObjectBehaviour;
            }
        }
        return null;
    }

    public Vector3 PSDToWorldPoint(Vector2 point)
    {
        return this.psdTransformationTransform.TransformPoint(new Vector3(point.x, point.y, 0f));
    }

    public Vector3 WorldToPSDPoint(Vector2 point)
    {
        return this.psdTransformationTransform.InverseTransformPoint(new Vector3(point.x, point.y, 0f));
    }

    public void ResetZoomIn()
    {
        this.rootTransform.localPosition = Vector3.zero;
        this.rootTransform.localScale = Vector3.one;
    }

    public void InitRuntimeData()
    {
        string name = base.name;
        DecoratingSceneConfig.RoomConfig roomConfig = ScriptableObjectSingleton<DecoratingSceneConfig>.instance.GetRoomConfig(name);
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            this.visualObjectBehaviours[i].InitRuntimeData(roomConfig);
        }
    }

    public string roomName
    {
        get
        {
            return base.name;
        }
    }

    public RoomsBackend.RoomAccessor roomBackend
    {
        get
        {
            return SingletonInit<RoomsBackend>.instance.GetRoom(this.roomName);
        }
    }

    public void Init(Camera mainCamera, float additionalMargin, DecorateRoomScreen roomScreen)
    {
        this.roomScreen = roomScreen;
        if (this.animationPlayer != null)
        {
            this.animationPlayer.Init(this);
        }
        this.rootTransform.localPosition = Vector3.zero;
        this.rootTransform.localScale = Vector3.one;
        this.ScaleToFitInCamera(mainCamera, additionalMargin);
        Color color = this.backgroundColor;
        color.a = 1f;
        mainCamera.backgroundColor = color;
        RoomsBackend.RoomAccessor room = SingletonInit<RoomsBackend>.instance.GetRoom(base.name);
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            this.visualObjectBehaviours[i].Init(room);
        }
        this.InitRuntimeData();
    }

    public void ZoomIn(VisualObjectBehaviour visualObjectBehaviour)
    {
        this.animationEnum = this.DoZoomInAnimation(visualObjectBehaviour);
    }

    public void ZoomOut()
    {
        this.animationEnum = null;
        if (this.rootTransformScale == Vector3.one && this.rootTransformOffset == Vector3.zero)
        {
            return;
        }
        this.animationEnum = this.DoZoomOutAnimation();
    }

    private IEnumerator DoZoomOutAnimation()
    {
        return new DecoratingScene._003CDoZoomOutAnimation_003Ed__78(0)
        {
            _003C_003E4__this = this
        };
    }

    private IEnumerator DoZoomInAnimation(VisualObjectBehaviour visualObjectBehaviour)
    {
        return new DecoratingScene._003CDoZoomInAnimation_003Ed__79(0)
        {
            _003C_003E4__this = this,
            visualObjectBehaviour = visualObjectBehaviour
        };
    }

    private void ScaleToFitInCamera(Camera mainCamera, float additionalMargin)
    {
        Vector2 visibleScenePercent = SceneObjectsDB.instance.maxMargins.visibleScenePercent;
        Vector2 marginsOffset = SceneObjectsDB.instance.maxMargins.marginsOffset;
        int width = this.config.width;
        int height = this.config.height;
        Vector3 vector = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 vector2 = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        float num = vector2.x - vector.x;
        float num2 = vector2.y - vector.y;
        Mathf.Min(visibleScenePercent.x, visibleScenePercent.y);
        float num3 = Mathf.Max(num / ((float)width * visibleScenePercent.x - additionalMargin * 2f), num2 / ((float)height * visibleScenePercent.y - additionalMargin * 2f));
        Transform psdTransformationTransform = this.psdTransformationTransform;
        psdTransformationTransform.localScale = new Vector3(num3, num3, 1f);
        psdTransformationTransform.position = new Vector3((float)(-(float)width) + marginsOffset.x, (float)height - marginsOffset.y) * num3 * 0.5f;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (this.characterAnimationEnum != null && !this.characterAnimationEnum.MoveNext())
        {
            this.characterAnimationEnum = null;
        }
        if (this.animationEnum == null)
        {
            return;
        }
        if (!this.animationEnum.MoveNext())
        {
            this.animationEnum = null;
        }
    }

    [SerializeField]
    private List<DecoratingScene.GroupDefinition> groupDefinitions = new List<DecoratingScene.GroupDefinition>();

    [SerializeField]
    public List<VisualObjectBehaviour> visualObjectBehaviours = new List<VisualObjectBehaviour>();

    private List<VisualObjectBehaviour> activeBehaviours = new List<VisualObjectBehaviour>();

    public string sceneFolder;

    public string hierarchyJSONFile;

    public string hitboxJSONFile;

    public string metadataJSONFile;

    public string handlesJSONFile;

    public string dashLineJSONFile;

    [SerializeField]
    public bool shouldCreateCharacterScene;

    [SerializeField]
    public Transform rootTransform;

    [SerializeField]
    public RoomCharacterScene characterScene;

    [SerializeField]
    public CharacterAnimationPlayer animationPlayer;

    [SerializeField]
    public RoomSceneRenderObject roomSceneRender;

    [NonSerialized]
    public DecorateRoomScreen roomScreen;

    private Vector3 rootTransformOffset_ = Vector3.zero;

    private Vector3 rootTransformOffsetAcceleration_ = Vector3.zero;

    [SerializeField]
    public Transform offsetTransform;

    [SerializeField]
    public GraphicsSceneConfig config;

    public DecoratingScene.ImagesFolderName imagesFolder;

    public Color backgroundColor = Color.black;

    public bool showCollisions;

    private IEnumerator animationEnum;

    private IEnumerator characterAnimationEnum;

    [Serializable]
    public class GroupDefinition
    {
        public int groupIndex;

        public string title;

        public List<string> toSayAfterGroupCompletes = new List<string>();

        public DecoratingScene.GroupDefinition.AnimationDef playAfterFinish = new DecoratingScene.GroupDefinition.AnimationDef();

        [Serializable]
        public class AnimationDef
        {
            public string groupName;

            public string animationName;
        }
    }

    public enum ImagesFolderName
    {
        Folder_2048,
        Folder_1024
    }

    public struct RoomProgressState
    {
        public bool isPassed
        {
            get
            {
                return this.completed >= this.total;
            }
        }

        public float Progress(int removeCompleted)
        {
            return Mathf.InverseLerp(0f, (float)this.total, (float)(this.completed - removeCompleted));
        }

        public float progress
        {
            get
            {
                return Mathf.InverseLerp(0f, (float)this.total, (float)this.completed);
            }
        }

        public int completed;

        public int total;
    }

    private sealed class _003CDoZoomOutAnimation_003Ed__78 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoZoomOutAnimation_003Ed__78(int _003C_003E1__state)
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
            DecoratingScene decoratingScene = this._003C_003E4__this;
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
                this._003Csettings_003E5__2 = Match3Settings.instance.confirmPurchasePanelSettings;
                this._003Ctime_003E5__3 = 0f;
                this._003CstartScale_003E5__4 = decoratingScene.rootTransformScale;
                this._003CstartPos_003E5__5 = decoratingScene.rootTransformOffset;
                this._003CendPos_003E5__6 = Vector3.zero;
                this._003CendScale_003E5__7 = Vector3.one;
            }
            if (this._003Ctime_003E5__3 > this._003Csettings_003E5__2.zoomOutDuration)
            {
                return false;
            }
            this._003Ctime_003E5__3 += Time.deltaTime;
            float num2 = Mathf.InverseLerp(0f, this._003Csettings_003E5__2.zoomOutDuration, this._003Ctime_003E5__3);
            if (this._003Csettings_003E5__2.outCurve != null)
            {
                num2 = this._003Csettings_003E5__2.outCurve.Evaluate(num2);
            }
            Vector3 rootTransformOffset = Vector3.LerpUnclamped(this._003CstartPos_003E5__5, this._003CendPos_003E5__6, num2);
            Vector3 rootTransformScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__4, this._003CendScale_003E5__7, num2);
            decoratingScene.rootTransformScale = rootTransformScale;
            decoratingScene.rootTransformOffset = rootTransformOffset;
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

        public DecoratingScene _003C_003E4__this;

        private ConfirmPurchasePanel.Settings _003Csettings_003E5__2;

        private float _003Ctime_003E5__3;

        private Vector3 _003CstartScale_003E5__4;

        private Vector3 _003CstartPos_003E5__5;

        private Vector3 _003CendPos_003E5__6;

        private Vector3 _003CendScale_003E5__7;
    }

    private sealed class _003CDoZoomInAnimation_003Ed__79 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoZoomInAnimation_003Ed__79(int _003C_003E1__state)
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
            DecoratingScene decoratingScene = this._003C_003E4__this;
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
                Vector3 iconHandlePosition = this.visualObjectBehaviour.iconHandlePosition;
                this._003Csettings_003E5__2 = Match3Settings.instance.confirmPurchasePanelSettings;
                Vector3 a = decoratingScene.PSDToWorldPoint(iconHandlePosition);
                this._003Ctime_003E5__3 = 0f;
                this._003CstartScale_003E5__4 = decoratingScene.rootTransformScale;
                this._003CstartPos_003E5__5 = decoratingScene.rootTransformOffset;
                this._003CendPos_003E5__6 = -a;
                this._003CendPos_003E5__6 = Vector3.Lerp(Vector3.zero, this._003CendPos_003E5__6, this._003Csettings_003E5__2.moveTowardsFactor);
                this._003CendScale_003E5__7 = new Vector3(this._003Csettings_003E5__2.zoomInFactor, this._003Csettings_003E5__2.zoomInFactor, 1f);
                this._003CendScale_003E5__7 = Vector3.Lerp(Vector3.one, this._003CendScale_003E5__7, this._003Csettings_003E5__2.moveTowardsFactor);
            }
            if (this._003Ctime_003E5__3 > this._003Csettings_003E5__2.zoomInDuration)
            {
                return false;
            }
            this._003Ctime_003E5__3 += Time.deltaTime;
            float num2 = Mathf.InverseLerp(0f, this._003Csettings_003E5__2.zoomInDuration, this._003Ctime_003E5__3);
            if (this._003Csettings_003E5__2.curve != null)
            {
                num2 = this._003Csettings_003E5__2.curve.Evaluate(num2);
            }
            Vector3 rootTransformOffset = Vector3.LerpUnclamped(this._003CstartPos_003E5__5, this._003CendPos_003E5__6, num2);
            Vector3 rootTransformScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__4, this._003CendScale_003E5__7, num2);
            decoratingScene.rootTransformScale = rootTransformScale;
            decoratingScene.rootTransformOffset = rootTransformOffset;
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

        public VisualObjectBehaviour visualObjectBehaviour;

        public DecoratingScene _003C_003E4__this;

        private ConfirmPurchasePanel.Settings _003Csettings_003E5__2;

        private float _003Ctime_003E5__3;

        private Vector3 _003CstartScale_003E5__4;

        private Vector3 _003CstartPos_003E5__5;

        private Vector3 _003CendPos_003E5__6;

        private Vector3 _003CendScale_003E5__7;
    }
}
