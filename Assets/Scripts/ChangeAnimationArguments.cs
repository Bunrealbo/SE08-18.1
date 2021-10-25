using System;

public struct ChangeAnimationArguments
{
	public bool isNoAnimation
	{
		get
		{
			return !this.isAnimationAvailable;
		}
	}

	public bool isAnimationAvailable
	{
		get
		{
			return this.animation != null;
		}
	}

	public static ChangeAnimationArguments NoAnimation
	{
		get
		{
			return default(ChangeAnimationArguments);
		}
	}

	public static ChangeAnimationArguments Create(string roomName, string sceneObjectName)
	{
		DecoratingSceneConfig.AnimationSequenceGroup animationSequenceGroup = ScriptableObjectSingleton<DecoratingSceneConfig>.instance.GetAnimationSequenceGroup(roomName);
		ChangeAnimationArguments result = default(ChangeAnimationArguments);
		if (animationSequenceGroup != null)
		{
			result.animation = animationSequenceGroup.SequencForOpenAnimation(sceneObjectName);
		}
		return result;
	}

	public DecoratingSceneConfig.AnimationSequence animation;

	public Action onComplete;

	public bool showWideBars;
}
