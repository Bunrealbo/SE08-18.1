using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterAvatar : MonoBehaviour
{
    public void InitWithDecoratingScene(DecoratingScene decoratingScene)
    {
        this.decoratingScene = decoratingScene;
    }

    private AnimatorControllerParameter[] allParameters
    {
        get
        {
            if (this.allParameters_ == null)
            {
                this.allParameters_ = this.animator.parameters;
            }
            return this.allParameters_;
        }
    }

    public void ShowSpeachBubble(string text)
    {
        CharacterSpeachBubble speachBubble = this.speachBubble;
        if (speachBubble == null)
        {
            return;
        }
        speachBubble.SetActive(true);
        speachBubble.SetText(text);
    }

    public void HideSpeachBubble()
    {
        if (this.speachBubble == null)
        {
            return;
        }
        this.speachBubble.SetActive(false);
    }

    public bool isRunningDefaultAnimation
    {
        get
        {
            return this.lastAnimation != null && this.lastAnimation.animationList != null && this.lastAnimation.animationList.isDefaultAnimation;
        }
    }

    public bool isAnimating
    {
        get
        {
            return this.animationState.isActive;
        }
    }

    public void StopLookAt()
    {
        this.lookAtWeight = 0f;
    }

    public Vector3 headPosition
    {
        get
        {
            return this.animator.GetBoneTransform(HumanBodyBones.Head).position;
        }
    }

    public void UpdateLookAtInAnimation(Vector3 position)
    {
        Vector3 acceptableHeadLookAtPosition = this.GetAcceptableHeadLookAtPosition(position);
        this.lookAtState.lookAtPosition = acceptableHeadLookAtPosition;
    }

    public void ChangeLookAtWeight(float weight, float animateDuration)
    {
        if (this.headLookAt == null)
        {
            return;
        }
        this.lookAtState = default(CharacterAvatar.LookAtState);
        Vector3 position = this.headLookAt.position;
        this.headLookAt.position = position;
        this.lookAtState.startLookAtPosition = position;
        this.lookAtState.duration = animateDuration;
        this.lookAtState.weightAtStart = this.lookAtWeight;
        this.lookAtState.weight = weight;
        this.lookAtState.lookAtPosition = position;
        this.lookAtState.isActive = true;
    }

    public void LookAt(Vector3 position, float weight, float animateDuration)
    {
        if (this.headLookAt == null)
        {
            return;
        }
        this.lookAtState = default(CharacterAvatar.LookAtState);
        Vector3 acceptableHeadLookAtPosition = this.GetAcceptableHeadLookAtPosition(position);
        Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.Head);
        float d = Vector3.Distance(acceptableHeadLookAtPosition, boneTransform.position);
        boneTransform.position = boneTransform.position + boneTransform.forward * d;
        Vector3 vector = acceptableHeadLookAtPosition;
        Vector3 startLookAtPosition = Vector3.Lerp(vector, this.headLookAt.position, this.lookAtWeight);
        startLookAtPosition = vector;
        this.lookAtState.startLookAtPosition = startLookAtPosition;
        if (this.lookAtWeight <= 0f)
        {
            this.lookAtState.startLookAtPosition = acceptableHeadLookAtPosition;
            this.headLookAt.position = acceptableHeadLookAtPosition;
        }
        else
        {
            this.lookAtState.startLookAtPosition = this.headLookAt.position;
        }
        this.lookAtState.duration = animateDuration;
        this.lookAtState.weightAtStart = this.lookAtWeight;
        this.lookAtState.weight = weight;
        this.lookAtState.lookAtPosition = acceptableHeadLookAtPosition;
        this.lookAtState.isActive = true;
        if (animateDuration <= 0f)
        {
            this.lookAtWeight = this.lookAtState.weight;
            this.headLookAt.position = this.lookAtState.lookAtPosition;
            this.lookAtState.isActive = false;
        }
    }

    public void StopAnimation()
    {
        this.lastAnimation = null;
        this.animationState = default(CharacterAvatar.AnimationState);
        this.HideSpeachBubble();
        foreach (AnimatorControllerParameter animatorControllerParameter in this.allParameters)
        {
            this.animator.SetBool(animatorControllerParameter.name, false);
        }
        this.animator.Play(this.idleState, 0);
    }

    public void RunAnimation(CharacterAvatar.ChangeAnimationArguments animationArguments)
    {
        DecoratingSceneConfig.CharacterAnimation animation = animationArguments.animation;
        this.timeNotAnimating = 0f;
        this.StopAnimation();
        if (animation == null)
        {
            if (animationArguments.onComplete != null)
            {
                animationArguments.onComplete();
            }
            return;
        }
        this.lastAnimation = animation;
        this.animationState.isActive = true;
        this.animationState.additionalTime = animationArguments.additionalTime;
        this.animationState.characterAnimation = animation;
        this.animationState.changeAnimationArguments = animationArguments;
        this.animationState.characterAnimation.StartAnimation(this);
    }

    public void RunAnimation(string sceneObjectName, float additionalTime)
    {
        CharacterAvatar.ChangeAnimationArguments animationArguments = CharacterAvatar.ChangeAnimationArguments.Create(this.roomName, sceneObjectName, 0);
        animationArguments.additionalTime = additionalTime;
        this.RunAnimation(animationArguments);
    }

    public void SetProperty(string name, bool value)
    {
        this.animator.SetBool(name, value);
    }

    public Vector3 bubblePosition
    {
        get
        {
            return this.animator.GetBoneTransform(HumanBodyBones.Head).position + ScriptableObjectSingleton<DecoratingSceneConfig>.instance.speachBubbleHeadOffset;
        }
    }

    public void SetPosition(Vector3 position)
    {
        base.transform.position = position;
    }

    public void SetRotation(Vector3 eulerRotation)
    {
        base.transform.rotation = Quaternion.Euler(eulerRotation);
    }

    public void PopIn()
    {
        this.animationEnum = this.DoPopIn();
    }

    private CharacterSpeachBubble speachBubble
    {
        get
        {
            if (this.decoratingScene == null)
            {
                return null;
            }
            return this.decoratingScene.roomScreen.GetSpeachBubble(this);
        }
    }

    private IEnumerator DoPopIn()
    {
        return new CharacterAvatar._003CDoPopIn_003Ed__52(0)
        {
            _003C_003E4__this = this
        };
    }

    private void LateUpdate()
    {
        if (this.shadowTransform != null)
        {
            Vector3 position = this.shadowTransform.position;
            position.y = this.shadowy;
            this.shadowTransform.position = position;
        }
    }

    private void Update()
    {
        if (this.animationEnum != null && !this.animationEnum.MoveNext())
        {
            this.animationEnum = null;
        }
        if (this.lookAtState.isActive)
        {
            this.lookAtState.time = this.lookAtState.time + Time.deltaTime;
            float num = Mathf.InverseLerp(0f, this.lookAtState.duration, this.lookAtState.time);
            AnimationCurve headAnimationCurve = ScriptableObjectSingleton<DecoratingSceneConfig>.instance.headAnimationCurve;
            if (headAnimationCurve != null)
            {
                num = headAnimationCurve.Evaluate(num);
            }
            Vector3 position = Vector3.Lerp(this.lookAtState.startLookAtPosition, this.lookAtState.lookAtPosition, num);
            float num2 = Mathf.Lerp(this.lookAtState.weightAtStart, this.lookAtState.weight, num);
            this.lookAtWeight = num2;
            this.headLookAt.position = position;
            if (this.lookAtState.time >= this.lookAtState.duration)
            {
                this.lookAtState.isActive = false;
            }
        }
        if (!this.animationState.isActive)
        {
            this.timeNotAnimating += Time.deltaTime;
            return;
        }
        DecoratingSceneConfig.CharacterAnimation characterAnimation = this.animationState.characterAnimation;
        if (!characterAnimation.isAnimationActive)
        {
            this.animationState.additionalTime = this.animationState.additionalTime - Time.deltaTime;
            if (this.animationState.additionalTime <= 0f)
            {
                this.animationState.Complete();
            }
        }
        characterAnimation.UpdateAnimation(Time.deltaTime, this);
        if (!characterAnimation.isAnimationActive && this.animationState.additionalTime <= 0f)
        {
            this.animationState.Complete();
        }
    }

    private Vector3 GetAcceptableHeadLookAtPosition(Vector3 initialPosition)
    {
        Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.Head);
        if (boneTransform == null)
        {
            return initialPosition;
        }
        Vector3 vector = initialPosition;
        Vector3 vector2 = vector - boneTransform.position;
        Vector3 normalized = Vector3Ex.OnGround(base.transform.forward, 0f).normalized;
        Vector3 vector3 = Vector3.Cross(normalized, Vector3.up);
        float num = Mathf.Abs(Vector3.Dot(normalized, vector2));
        float f = Vector3.Dot(vector3, vector2);
        float num2 = Mathf.Tan(0.0174532924f * this.maxHorizontalHeadAngleDeg) * num;
        if (Mathf.Abs(f) > num2)
        {
            Vector3 vector4 = boneTransform.position + num * normalized + Mathf.Sign(f) * num2 * vector3;
            vector4.y = vector.y;
            vector = vector4;
        }
        vector2 = vector - boneTransform.position;
        float magnitude = Vector3Ex.OnGround(vector2, 0f).magnitude;
        float y = vector2.y;
        float num3 = Mathf.Tan(0.0174532924f * this.maxHeadAngleDeg) * magnitude;
        if (Mathf.Abs(y) > num3)
        {
            vector.y = boneTransform.position.y + Mathf.Sign(y) * num3;
        }
        return vector;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (this.animator == null || this.headLookAt == null)
        {
            return;
        }
        this.animator.SetLookAtWeight(this.lookAtWeight);
        if (this.lookAtWeight <= 0f)
        {
            return;
        }
        Vector3 position = this.headLookAt.position;
        this.animator.SetLookAtPosition(position);
    }

    [SerializeField]
    private float popInDuration;

    [SerializeField]
    private AnimationCurve popInCurve;

    [SerializeField]
    private Vector3 startScale;

    [SerializeField]
    private Vector3 endScale;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform shadowTransform;

    [SerializeField]
    private float shadowy = 0.001f;

    [SerializeField]
    private string idleState;

    [SerializeField]
    public string roomName;

    [SerializeField]
    public string sceneObjectName;

    [SerializeField]
    public int animationIndex;

    [SerializeField]
    private float lookAtChangeSpeed = 1f;

    [SerializeField]
    public Transform headLookAt;

    [SerializeField]
    private float maxHeadAngleDeg = 30f;

    [SerializeField]
    private float maxHorizontalHeadAngleDeg = 30f;

    [SerializeField]
    public float lookAtWeight;

    [NonSerialized]
    public DecoratingScene decoratingScene;

    private AnimatorControllerParameter[] allParameters_;

    [NonSerialized]
    public float timeNotAnimating = 10000f;

    private CharacterAvatar.LookAtState lookAtState;

    private CharacterAvatar.AnimationState animationState;

    private IEnumerator animationEnum;

    [NonSerialized]
    public DecoratingSceneConfig.CharacterAnimation lastAnimation;

    public struct ChangeAnimationArguments
    {
        public static CharacterAvatar.ChangeAnimationArguments Create(string roomName, string sceneObjectName, int animationVariantIndex = 0)
        {
            DecoratingSceneConfig.AnimationsList animationsForSceneObject = ScriptableObjectSingleton<DecoratingSceneConfig>.instance.GetAnimationsForSceneObject(roomName, sceneObjectName);
            CharacterAvatar.ChangeAnimationArguments result = default(CharacterAvatar.ChangeAnimationArguments);
            if (animationsForSceneObject != null && animationsForSceneObject.animations.Count > animationVariantIndex)
            {
                result.animation = animationsForSceneObject.animations[animationVariantIndex];
            }
            return result;
        }

        public DecoratingSceneConfig.CharacterAnimation animation;

        public bool useFadeInOut;

        public float initialDelay;

        public RoomSceneRenderObject roomRenderer;

        public float additionalTime;

        public Action onComplete;

        public VisualObjectBehaviour lookAt;
    }

    private struct LookAtState
    {
        public bool isActive;

        public Vector3 lookAtPosition;

        public float duration;

        public Vector3 startLookAtPosition;

        public float weightAtStart;

        public float weight;

        public float time;
    }

    private struct AnimationState
    {
        public void Complete()
        {
            this.isActive = false;
            if (this.changeAnimationArguments.onComplete != null)
            {
                this.changeAnimationArguments.onComplete();
            }
        }

        public bool isActive;

        public DecoratingSceneConfig.CharacterAnimation characterAnimation;

        public float additionalTime;

        public CharacterAvatar.ChangeAnimationArguments changeAnimationArguments;
    }

    private sealed class _003CDoPopIn_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoPopIn_003Ed__52(int _003C_003E1__state)
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
            CharacterAvatar characterAvatar = this._003C_003E4__this;
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
            }
            if (this._003Ctime_003E5__2 > characterAvatar.popInDuration)
            {
                return false;
            }
            this._003Ctime_003E5__2 += Time.deltaTime;
            float num2 = Mathf.InverseLerp(0f, characterAvatar.popInDuration, this._003Ctime_003E5__2);
            if (characterAvatar.popInCurve != null)
            {
                num2 = characterAvatar.popInCurve.Evaluate(num2);
            }
            Vector3 localScale = Vector3.LerpUnclamped(characterAvatar.startScale, characterAvatar.endScale, num2);
            characterAvatar.transform.localScale = localScale;
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

        public CharacterAvatar _003C_003E4__this;

        private float _003Ctime_003E5__2;
    }
}
