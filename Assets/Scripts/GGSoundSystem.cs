using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class GGSoundSystem : MonoBehaviour
{
	public void PlayMusic(GGSoundSystem.MusicType musicType)
	{
		for (int i = 0; i < this.musics.Count; i++)
		{
			GGSoundSystem.MusicSource musicSource = this.musics[i];
			GGUtil.SetActive(musicSource.source.gameObject, musicSource.musicType == musicType);
		}
	}

	public static void Play(GGSoundSystem.MusicType musicType)
	{
		GGSoundSystem instance = GGSoundSystem.instance;
		if (instance == null)
		{
			return;
		}
		instance.PlayMusic(musicType);
	}

	public static GGSoundSystem instance
	{
		get
		{
			if (GGSoundSystem.soundSystem_ == null)
			{
				GGSoundSystem.soundSystem_ = NavigationManager.instance.GetObject<GGSoundSystem>();
			}
			return GGSoundSystem.soundSystem_;
		}
	}

	public static void Play(GGSoundSystem.PlayParameters playParameters)
	{
		GGSoundSystem instance = GGSoundSystem.instance;
		if (instance == null)
		{
			return;
		}
		instance.PlayFx(playParameters);
	}

	public static void Play(GGSoundSystem.SFXType soundType)
	{
		GGSoundSystem.Play(new GGSoundSystem.PlayParameters
		{
			soundType = soundType,
			variationIndex = 0
		});
	}

	public static void ReportMatch()
	{
		GGSoundSystem instance = GGSoundSystem.instance;
		if (instance == null)
		{
			return;
		}
		instance.DoReportMatch();
	}

	public void PlayFx(GGSoundSystem.PlayParameters p)
	{
		GGSoundSystem.SoundFxClip clip = this.GetClip(p.soundType);
		this.Play(clip, p);
	}

	private void DoReportMatch()
	{
		this.matchesCounter.count = this.matchesCounter.count + 1;
		this.matchesCounter.leftTime = 1f;
	}

	private void Start()
	{
		ConfigBase.instance.SetAudioMixerValues(GGPlayerSettings.instance);
		GGSoundSystem.Play(this.defaultMusic);
	}

	private void Update()
	{
		this.matchesCounter.leftTime = this.matchesCounter.leftTime - Time.deltaTime;
		if (this.matchesCounter.leftTime <= 0f)
		{
			this.matchesCounter = default(GGSoundSystem.TimedCounter);
		}
	}

	private GGSoundSystem.SoundFxClip GetClip(GGSoundSystem.SFXType soundType)
	{
		for (int i = 0; i < this.soundFxList.Count; i++)
		{
			GGSoundSystem.SoundFxClip soundFxClip = this.soundFxList[i];
			if (soundFxClip.clipType == soundType)
			{
				return soundFxClip;
			}
		}
		return null;
	}

	private long LastPlayingFrameNumber(GGSoundSystem.SFXType soundType)
	{
		long num = -1L;
		for (int i = 0; i < this.audioClipsSources.Count; i++)
		{
			GGSoundSystem.AudioSourceData audioSourceData = this.audioClipsSources[i];
			if (audioSourceData.playedSound == soundType && audioSourceData.audioSource.isPlaying && audioSourceData.frameIndexWhenStart > num)
			{
				num = audioSourceData.frameIndexWhenStart;
			}
		}
		return num;
	}

	private GGSoundSystem.AudioSourceData NextAudioSource()
	{
		for (int i = 0; i < this.audioClipsSources.Count; i++)
		{
			GGSoundSystem.AudioSourceData audioSourceData = this.audioClipsSources[i];
			if (!audioSourceData.audioSource.isPlaying)
			{
				return audioSourceData;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.audioClipSourcePrefab, base.transform);
		GGSoundSystem.AudioSourceData audioSourceData2 = new GGSoundSystem.AudioSourceData(gameObject.GetComponent<AudioSource>());
		GGUtil.SetActive(gameObject, true);
		this.audioClipsSources.Add(audioSourceData2);
		return audioSourceData2;
	}

	private void Play(GGSoundSystem.SoundFxClip clip, GGSoundSystem.PlayParameters p)
	{
		if (clip == null)
		{
			return;
		}
		GGSoundSystem.SoundFxClip.VariationParameters variation = clip.GetVariation(p.variationIndex);
		AudioClip clip2 = variation.clip;
		if (clip2 == null)
		{
			return;
		}
		if (variation.volume <= 0f)
		{
			return;
		}
		long num = this.LastPlayingFrameNumber(p.soundType);
		if (p.frameNumber == 0L)
		{
			p.frameNumber = (long)Time.frameCount;
		}
		if (num == p.frameNumber)
		{
			return;
		}
		GGSoundSystem.AudioSourceData audioSourceData = this.NextAudioSource();
		AudioSource audioSource = audioSourceData.audioSource;
		audioSource.clip = clip2;
		audioSource.loop = false;
		audioSource.pitch = variation.pitch;
		audioSource.volume = variation.volume;
		audioSource.Play();
		audioSourceData.frameIndexWhenStart = p.frameNumber;
		audioSourceData.playedSound = p.soundType;
	}

	[SerializeField]
	private bool isDebug;

	private GGSoundSystem.TimedCounter matchesCounter;

	[SerializeField]
	private GGSoundSystem.MusicType defaultMusic;

	[SerializeField]
	private GameObject audioClipSourcePrefab;

	[SerializeField]
	private List<GGSoundSystem.MusicSource> musics = new List<GGSoundSystem.MusicSource>();

	[SerializeField]
	private List<GGSoundSystem.SoundFxClip> soundFxList = new List<GGSoundSystem.SoundFxClip>();

	private List<GGSoundSystem.AudioSourceData> audioClipsSources = new List<GGSoundSystem.AudioSourceData>();

	private static GGSoundSystem soundSystem_;

	public enum MusicType
	{
		NoMusic,
		MainMenuMusic,
		GameMusic
	}

	public enum SFXType
	{
		ButtonPress,
		CancelPress,
		None,
		ButtonFail,
		FlyIn,
		ButtonConfirm,
		Flip,
		PurchaseSuccess,
		GiftPresented,
		GiftOpen,
		ChipSwap,
		ChipTap,
		CollectGoal,
		GoalsComplete,
		FlyRocket,
		CollectGoalStart,
		CreatePowerup,
		BombExplode,
		PlainMatch,
		SeekingMissleTakeOff,
		DiscoBallElectricity,
		DiscoBallExplode,
		CoinCollect,
		CoinCollectStart,
		BreakColorSlate,
		BreakBox,
		FlyCrossRocketAction,
		BreakChain,
		BreakIce,
		GingerbreadManRescue,
		SeekingMissleLand,
		HammerHit,
		PowerHammerHit,
		HammerStart,
		GrowingElementFinish,
		GrowingElementGrowFlower,
		CollectMoreMoves,
		CollectMoreMovesStart,
		RockBreak,
		ChocolateBreak,
		SnowDestroy,
		SnowCreate,
		BunnyOutOfHat,
		WinScreenStart,
		WinScreenReceieveAnimationStart,
		RecieveStar,
		RecieveCoin,
		YouWinAnimation
	}

	private struct TimedCounter
	{
		public float leftTime;

		public int count;
	}

	[Serializable]
	public class MusicSource
	{
		public GGSoundSystem.MusicType musicType;

		public AudioSource source;
	}

	[Serializable]
	public class SoundFxClip
	{
		public GGSoundSystem.SoundFxClip.VariationParameters GetVariation(int index)
		{
			GGSoundSystem.SoundFxClip.VariationParameters result = default(GGSoundSystem.SoundFxClip.VariationParameters);
			result.clip = this.clip;
			result.pitch = 1f;
			result.volume = 1f;
			if (this.variationList.Count == 0)
			{
				return result;
			}
			GGSoundSystem.SoundFxClip.ClipVariation clipVariation = this.variationList[Mathf.Clamp(index, 0, this.variationList.Count - 1)];
			if (clipVariation.clip != null)
			{
				result.clip = clipVariation.clip;
			}
			result.pitch = clipVariation.pitch;
			result.volume = clipVariation.volume;
			return result;
		}

		public GGSoundSystem.SFXType clipType;

		public AudioClip clip;

		public List<GGSoundSystem.SoundFxClip.ClipVariation> variationList = new List<GGSoundSystem.SoundFxClip.ClipVariation>();

		[Serializable]
		public class ClipVariation
		{
			public AudioClip clip;

			public float pitch = 1f;

			public float volume = 1f;
		}

		public struct VariationParameters
		{
			public float pitch;

			public AudioClip clip;

			public float volume;
		}
	}

	private class AudioSourceData
	{
		public AudioSourceData(AudioSource audioSource)
		{
			this.audioSource = audioSource;
		}

		public AudioSource audioSource;

		public GGSoundSystem.SFXType playedSound;

		public long frameIndexWhenStart;
	}

	public struct PlayParameters
	{
		public GGSoundSystem.SFXType soundType;

		public int variationIndex;

		public long frameNumber;
	}
}
