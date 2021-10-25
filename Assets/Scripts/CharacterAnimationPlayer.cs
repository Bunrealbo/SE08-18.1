using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterAnimationPlayer : MonoBehaviour
{
    public DecoratingSceneConfig.AnimationSequence lastAnimation
    {
        get
        {
            return this.animationState.animationSequence;
        }
    }

    public void Init(DecoratingScene scene)
    {
        this.scene = scene;
        for (int i = 0; i < this.avatars.Count; i++)
        {
            this.avatars[i].InitWithDecoratingScene(scene);
        }
    }

    public Transform GetMarker(string name)
    {
        for (int i = 0; i < this.markers.Count; i++)
        {
            Transform transform = this.markers[i];
            if (transform.name == name)
            {
                return transform;
            }
        }
        return null;
    }

    public void Stop()
    {
        this.animationState = default(CharacterAnimationPlayer.AnimationState);
        this.StopAvatarAnimations();
    }

    public void PlayWithSetup(ChangeAnimationArguments arguments)
    {
        DecoratingSceneConfig.AnimationSequence animation = arguments.animation;
        List<VisualObjectBehaviour> visualObjectBehaviours = this.scene.visualObjectBehaviours;
        for (int i = 0; i < visualObjectBehaviours.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = visualObjectBehaviours[i];
            visualObjectBehaviour.visualObject.isOwned = animation.testSetup.ShouldBeOwned(visualObjectBehaviour.name);
        }
        this.scene.roomScreen.Init();
        this.scene.SetCharacterAlpha(1f);
        this.Play(arguments);
    }

    public void HideAvatars()
    {
        for (int i = 0; i < this.avatars.Count; i++)
        {
            GGUtil.SetActive(this.avatars[i], false);
        }
    }

    public void Play(ChangeAnimationArguments arguments)
    {
        if (arguments.isNoAnimation)
        {
            this.Stop();
            return;
        }
        this.animationState = default(CharacterAnimationPlayer.AnimationState);
        this.animationState.arguments = arguments;
        this.animationState.animationSequence = arguments.animation;
        this.animationState.animationEnumerator = this.DoPlay(this.animationState.arguments);
        this.animationState.animationEnumerator.MoveNext();
    }

    public CharacterAvatar GetAvatar(string characterName)
    {
        for (int i = 0; i < this.avatars.Count; i++)
        {
            CharacterAvatar characterAvatar = this.avatars[i];
            if (characterAvatar.name == characterName)
            {
                return characterAvatar;
            }
        }
        return null;
    }

    private float deltaTime
    {
        get
        {
            return Time.deltaTime;
        }
    }

    private void StopAvatarAnimations()
    {
        for (int i = 0; i < this.avatars.Count; i++)
        {
            CharacterAvatar characterAvatar = this.avatars[i];
            characterAvatar.StopAnimation();
            characterAvatar.StopLookAt();
        }
    }

    private IEnumerator DoPlay(ChangeAnimationArguments arguments)
    {
        return new CharacterAnimationPlayer._003CDoPlay_003Ed__20(0)
        {
            _003C_003E4__this = this,
            arguments = arguments
        };
    }

    private IEnumerator DoPlaySequence(CharacterAvatar avatar, DecoratingSceneConfig.CharacterAnimationSequence sequence)
    {
        return new CharacterAnimationPlayer._003CDoPlaySequence_003Ed__21(0)
        {
            _003C_003E4__this = this,
            avatar = avatar,
            sequence = sequence
        };
    }

    public void Update()
    {
        IEnumerator animationEnumerator = this.animationState.animationEnumerator;
        if (animationEnumerator != null && !animationEnumerator.MoveNext())
        {
            this.animationState.animationEnumerator = null;
        }
    }

    [SerializeField]
    public string beatToPlay;

    [SerializeField]
    private List<CharacterAvatar> avatars = new List<CharacterAvatar>();

    [SerializeField]
    private List<Transform> markers = new List<Transform>();

    [NonSerialized]
    public BeatMarkers beatMarkers = new BeatMarkers();

    [NonSerialized]
    public DecoratingScene scene;

    private EnumeratorsList enumList = new EnumeratorsList();

    private CharacterAnimationPlayer.AnimationState animationState;

    private struct AnimationState
    {
        public IEnumerator animationEnumerator;

        public DecoratingSceneConfig.AnimationSequence animationSequence;

        public ChangeAnimationArguments arguments;
    }

    private sealed class _003CDoPlay_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoPlay_003Ed__20(int _003C_003E1__state)
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
            CharacterAnimationPlayer characterAnimationPlayer = this._003C_003E4__this;
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
                DecoratingSceneConfig.AnimationSequence animation = this.arguments.animation;
                characterAnimationPlayer.StopAvatarAnimations();
                characterAnimationPlayer.HideAvatars();
                List<DecoratingSceneConfig.CharacterAnimationSequence> characters = animation.characters;
                characterAnimationPlayer.enumList.Clear();
                for (int i = 0; i < characters.Count; i++)
                {
                    DecoratingSceneConfig.CharacterAnimationSequence characterAnimationSequence = characters[i];
                    CharacterAvatar avatar = characterAnimationPlayer.GetAvatar(characterAnimationSequence.characterName);
                    if (!(avatar == null))
                    {
                        characterAnimationPlayer.enumList.Add(characterAnimationPlayer.DoPlaySequence(avatar, characterAnimationSequence), 0f, null, null, false);
                    }
                }
            }
            if (!characterAnimationPlayer.enumList.Update())
            {
                if (this.arguments.onComplete != null)
                {
                    this.arguments.onComplete();
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

        public ChangeAnimationArguments arguments;

        public CharacterAnimationPlayer _003C_003E4__this;
    }

    private sealed class _003C_003Ec__DisplayClass21_0
    {
        internal void _003CDoPlaySequence_003Eb__0()
        {
            this.isAnimationRunning = false;
        }

        public bool isAnimationRunning;
    }

    private sealed class _003CDoPlaySequence_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
    {
        [DebuggerHidden]
        public _003CDoPlaySequence_003Ed__21(int _003C_003E1__state)
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
            CharacterAnimationPlayer characterAnimationPlayer = this._003C_003E4__this;
            switch (num)
            {
                case 0:
                    this._003C_003E1__state = -1;
                    this._003Clines_003E5__2 = this.sequence.animationLines;
                    this._003Ci_003E5__3 = 0;
                    goto IL_162;
                case 1:
                    this._003C_003E1__state = -1;
                    break;
                case 2:
                    this._003C_003E1__state = -1;
                    goto IL_135;
                default:
                    return false;
            }
            IL_B5:
            if (this._003Ctime_003E5__5 <= this._003Citem_003E5__4.pauseDuration)
            {
                this._003Ctime_003E5__5 += characterAnimationPlayer.deltaTime;
                this._003C_003E2__current = null;
                this._003C_003E1__state = 1;
                return true;
            }
            DecoratingSceneConfig.CharacterAnimation characterAnimation = this._003Citem_003E5__4.GetCharacterAnimation(ScriptableObjectSingleton<DecoratingSceneConfig>.instance);
            if (characterAnimation == null)
            {
                goto IL_150;
            }
            CharacterAvatar.ChangeAnimationArguments animationArguments = default(CharacterAvatar.ChangeAnimationArguments);
            animationArguments.animation = characterAnimation;
            this._003C_003E8__1.isAnimationRunning = true;
            animationArguments.onComplete = new Action(this._003C_003E8__1._003CDoPlaySequence_003Eb__0);
            this.avatar.RunAnimation(animationArguments);
            IL_135:
            if (this._003C_003E8__1.isAnimationRunning)
            {
                this._003C_003E2__current = null;
                this._003C_003E1__state = 2;
                return true;
            }
            this._003C_003E8__1 = null;
            this._003Citem_003E5__4 = null;
            IL_150:
            int num2 = this._003Ci_003E5__3;
            this._003Ci_003E5__3 = num2 + 1;
            IL_162:
            if (this._003Ci_003E5__3 >= this._003Clines_003E5__2.Count)
            {
                return false;
            }
            this._003C_003E8__1 = new CharacterAnimationPlayer._003C_003Ec__DisplayClass21_0();
            this._003Citem_003E5__4 = this._003Clines_003E5__2[this._003Ci_003E5__3];
            GGUtil.SetActive(this.avatar, this._003Citem_003E5__4.isCharacterVisible);
            this._003Ctime_003E5__5 = 0f;
            goto IL_B5;
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

        public DecoratingSceneConfig.CharacterAnimationSequence sequence;

        public CharacterAvatar avatar;

        public CharacterAnimationPlayer _003C_003E4__this;

        private CharacterAnimationPlayer._003C_003Ec__DisplayClass21_0 _003C_003E8__1;

        private List<DecoratingSceneConfig.CharacterAnimationLine> _003Clines_003E5__2;

        private int _003Ci_003E5__3;

        private DecoratingSceneConfig.CharacterAnimationLine _003Citem_003E5__4;

        private float _003Ctime_003E5__5;
    }
}
