using System;
using System.Collections.Generic;
using Expressive;
using Expressive.Expressions;
using Expressive.Functions;
using UnityEngine;

public class DecoratingSceneConfig : ScriptableObjectSingleton<DecoratingSceneConfig>
{
	public AnimationBeats.Sequence GetBeatsSequence(string groupName, string animationName)
	{
		for (int i = 0; i < this.beatsSequences.Count; i++)
		{
			AnimationBeats.Sequence sequence = this.beatsSequences[i];
			if (sequence.groupName == groupName && sequence.animationName == animationName)
			{
				return sequence;
			}
		}
		return null;
	}

	public DecoratingSceneConfig.AnimationSequenceGroup GetAnimationSequenceGroup(string name)
	{
		if (!this.animationsEnabled)
		{
			return null;
		}
		for (int i = 0; i < this.animations.Count; i++)
		{
			DecoratingSceneConfig.AnimationSequenceGroup animationSequenceGroup = this.animations[i];
			if (animationSequenceGroup.groupName == name)
			{
				return animationSequenceGroup;
			}
		}
		return null;
	}

	public DecoratingSceneConfig.RoomAnimationList GetRoomAnimationList(string roomName)
	{
		for (int i = 0; i < this.roomCharacterAnimations.Count; i++)
		{
			DecoratingSceneConfig.RoomAnimationList roomAnimationList = this.roomCharacterAnimations[i];
			if (roomAnimationList.roomName == roomName)
			{
				return roomAnimationList;
			}
		}
		return null;
	}

	public DecoratingSceneConfig.AnimationsList GetAnimationsForSceneObject(string roomName, string sceneObjectName)
	{
		DecoratingSceneConfig.RoomAnimationList roomAnimationList = this.GetRoomAnimationList(roomName);
		if (roomAnimationList == null)
		{
			return null;
		}
		return roomAnimationList.GetAnimationsForSceneObject(sceneObjectName);
	}

	public DecoratingSceneConfig.RoomConfig GetRoomConfig(string roomName)
	{
		for (int i = 0; i < this.roomConfigs.Count; i++)
		{
			DecoratingSceneConfig.RoomConfig roomConfig = this.roomConfigs[i];
			if (roomConfig.roomName == roomName)
			{
				return roomConfig;
			}
		}
		return null;
	}

	public DecoratingSceneConfig.ScaleAnimationSettings GetScaleAnimationSettingsOrDefault(string name)
	{
		for (int i = 0; i < this.scaleAnimationSettingsList.Count; i++)
		{
			DecoratingSceneConfig.ScaleAnimationSettings scaleAnimationSettings = this.scaleAnimationSettingsList[i];
			if (scaleAnimationSettings.name == name)
			{
				return scaleAnimationSettings;
			}
		}
		return this.scaleAnimationSettings;
	}

	protected override void UpdateData()
	{
		base.UpdateData();
		for (int i = 0; i < this.roomCharacterAnimations.Count; i++)
		{
			this.roomCharacterAnimations[i].Init();
		}
	}

	[SerializeField]
	private bool animationsEnabled;

	public float additionalTimeForRandomAnimation = 7f;

	public float additionalTimeForNewObjectAnimation = 7f;

	public float additionalInitialTime = 2f;

	public AnimationCurve headAnimationCurve;

	public Vector3 speachBubbleHeadOffset;

	public List<AnimationBeats.Sequence> beatsSequences = new List<AnimationBeats.Sequence>();

	[SerializeField]
	public List<DecoratingSceneConfig.AnimationSequenceGroup> animations = new List<DecoratingSceneConfig.AnimationSequenceGroup>();

	[SerializeField]
	public List<DecoratingSceneConfig.RoomAnimationList> roomCharacterAnimations = new List<DecoratingSceneConfig.RoomAnimationList>();

	public DecoratingSceneConfig.ScaleAnimationSettings scaleAnimationSettings = new DecoratingSceneConfig.ScaleAnimationSettings();

	public List<DecoratingSceneConfig.ScaleAnimationSettings> scaleAnimationSettingsList = new List<DecoratingSceneConfig.ScaleAnimationSettings>();

	[SerializeField]
	public List<DecoratingSceneConfig.RoomConfig> roomConfigs = new List<DecoratingSceneConfig.RoomConfig>();

	[Serializable]
	public class CharacterAnimationLine
	{
		public DecoratingSceneConfig.CharacterAnimation GetCharacterAnimation(DecoratingSceneConfig config)
		{
			DecoratingSceneConfig.RoomAnimationList roomAnimationList = config.GetRoomAnimationList(this.namedAnimationRoom);
			if (roomAnimationList == null)
			{
				return null;
			}
			DecoratingSceneConfig.AnimationsList animationsForSceneObject = roomAnimationList.GetAnimationsForSceneObject(this.namedAnimationName);
			if (animationsForSceneObject == null || this.namedAnimationIndex >= animationsForSceneObject.animations.Count)
			{
				return null;
			}
			return animationsForSceneObject.animations[this.namedAnimationIndex];
		}

		[SerializeField]
		public DecoratingSceneConfig.CharacterAnimationLine.LineType lineType;

		[SerializeField]
		public bool isCharacterVisible;

		[SerializeField]
		public string namedAnimationRoom;

		[SerializeField]
		public string namedAnimationName;

		[SerializeField]
		public int namedAnimationIndex;

		[SerializeField]
		public float pauseDuration;

		public enum LineType
		{
			NamedAnimation,
			Pause
		}
	}

	[Serializable]
	public class CharacterAnimationSequence
	{
		[SerializeField]
		public string characterName;

		[SerializeField]
		public List<DecoratingSceneConfig.CharacterAnimationLine> animationLines = new List<DecoratingSceneConfig.CharacterAnimationLine>();
	}

	[Serializable]
	public class AnimationSequence
	{
		public bool IsForOpenAnimation(string objectName)
		{
			return this.openAnimationFor == objectName;
		}

		private Expression availableExpression
		{
			get
			{
				if (string.IsNullOrEmpty(this.expressionString))
				{
					return null;
				}
				Expression expression = new Expression(this.expressionString);
				DecoratingSceneConfig.AnimationSequence.expressionFunctions.RegisterFunctions(expression);
				return expression;
			}
		}

		public bool isAvailable(DecoratingScene scene)
		{
			Expression availableExpression = this.availableExpression;
			if (availableExpression == null)
			{
				return false;
			}
			DecoratingSceneConfig.AnimationSequence.expressionFunctions.Start(scene);
			bool result = false;
			try
			{
				result = Convert.ToBoolean(availableExpression.Evaluate());
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Problem with expression " + this.expressionString + "\n" + ex.ToString());
				result = false;
			}
			DecoratingSceneConfig.AnimationSequence.expressionFunctions.End();
			return result;
		}

		private static DecoratingSceneConfig.AnimationSequence.ExpressionFunctions expressionFunctions = new DecoratingSceneConfig.AnimationSequence.ExpressionFunctions();

		[SerializeField]
		public string name;

		[SerializeField]
		public string expressionString;

		[SerializeField]
		public string openAnimationFor;

		[SerializeField]
		public bool leaveAfterInit;

		[SerializeField]
		public AnimationBeats.TestSetup testSetup = new AnimationBeats.TestSetup();

		[SerializeField]
		public List<DecoratingSceneConfig.CharacterAnimationSequence> characters = new List<DecoratingSceneConfig.CharacterAnimationSequence>();

		private class ExpressionFunctions
		{
			public void Start(DecoratingScene scene)
			{
				this.scene = scene;
			}

			public void End()
			{
				this.scene = null;
			}

			public void RegisterFunctions(Expression expression)
			{
				this.unlockedFunction.functions = this;
				this.ownedFunction.functions = this;
				expression.RegisterFunction(this.unlockedFunction);
				expression.RegisterFunction(this.ownedFunction);
			}

			public bool IsUnlocked(string name)
			{
				if (this.scene == null)
				{
					return false;
				}
				VisualObjectBehaviour behaviour = this.scene.GetBehaviour(name);
				return !(behaviour == null) && behaviour.visualObject.IsUnlocked(this.scene) && !behaviour.visualObject.isOwned;
			}

			public bool IsOwned(string name)
			{
				if (this.scene == null)
				{
					return false;
				}
				VisualObjectBehaviour behaviour = this.scene.GetBehaviour(name);
				return !(behaviour == null) && behaviour.visualObject.isOwned;
			}

			private DecoratingSceneConfig.AnimationSequence.ExpressionFunctions.UnlockedFunction unlockedFunction = new DecoratingSceneConfig.AnimationSequence.ExpressionFunctions.UnlockedFunction();

			private DecoratingSceneConfig.AnimationSequence.ExpressionFunctions.OwnedFunction ownedFunction = new DecoratingSceneConfig.AnimationSequence.ExpressionFunctions.OwnedFunction();

			private DecoratingScene scene;

			internal class UnlockedFunction : IFunction
			{
				public IDictionary<string, object> Variables
				{
					get
					{
						return this._003CVariables_003Ek__BackingField;
					}
					set
					{
						this._003CVariables_003Ek__BackingField = value;
					}
				}

				public string Name
				{
					get
					{
						return "u";
					}
				}

				public object Evaluate(IExpression[] parameters, ExpressiveOptions options)
				{
					return this.functions.IsUnlocked(parameters[0].Evaluate(this.Variables) as string);
				}

				public DecoratingSceneConfig.AnimationSequence.ExpressionFunctions functions;

				private IDictionary<string, object> _003CVariables_003Ek__BackingField;
			}

			internal class OwnedFunction : IFunction
			{
				public IDictionary<string, object> Variables
				{
					get
					{
						return this._003CVariables_003Ek__BackingField;
					}
					set
					{
						this._003CVariables_003Ek__BackingField = value;
					}
				}

				public string Name
				{
					get
					{
						return "o";
					}
				}

				public object Evaluate(IExpression[] parameters, ExpressiveOptions options)
				{
					return this.functions.IsOwned(parameters[0].Evaluate(this.Variables) as string);
				}

				public DecoratingSceneConfig.AnimationSequence.ExpressionFunctions functions;

				private IDictionary<string, object> _003CVariables_003Ek__BackingField;
			}
		}
	}

	[Serializable]
	public class AnimationSequenceGroup
	{
		public List<DecoratingSceneConfig.AnimationSequence> AvailableSequences(DecoratingScene scene)
		{
			this.availableSequences.Clear();
			for (int i = 0; i < this.animations.Count; i++)
			{
				DecoratingSceneConfig.AnimationSequence animationSequence = this.animations[i];
				if (animationSequence.isAvailable(scene))
				{
					this.availableSequences.Add(animationSequence);
				}
			}
			return this.availableSequences;
		}

		public DecoratingSceneConfig.AnimationSequence SequencForOpenAnimation(string objectName)
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				DecoratingSceneConfig.AnimationSequence animationSequence = this.animations[i];
				if (animationSequence.IsForOpenAnimation(objectName))
				{
					return animationSequence;
				}
			}
			return null;
		}

		[SerializeField]
		public string groupName;

		[SerializeField]
		public List<DecoratingSceneConfig.AnimationSequence> animations = new List<DecoratingSceneConfig.AnimationSequence>();

		private List<DecoratingSceneConfig.AnimationSequence> availableSequences = new List<DecoratingSceneConfig.AnimationSequence>();
	}

	[Serializable]
	public class ScaleAnimationSettings
	{
		public string name;

		public Vector3 scaleFrom;

		public AnimationCurve scaleCurve;

		public float duration;

		public Vector3 localPositionFrom;

		public AnimationCurve localPositionCurve;
	}

	public interface IAnimationPart
	{
		bool isAnimationActive { get; }

		void ResetAnimationState();

		void StartAnimation(CharacterAvatar avatar);

		float Update(float deltaTime, CharacterAvatar avatar);
	}

	public interface IAnimationPartPlayer
	{
		bool isAnimationActive { get; }

		void StartAnimation(CharacterAvatar avatar);

		void UpdateAnimation(float deltaTime, CharacterAvatar avatar);

		void Init(List<DecoratingSceneConfig.CharacterAnimation.AnimationPart> parts);

		void Init(List<DecoratingSceneConfig.CharacterAnimation.TextAnimationPart> parts);

		void Init(List<DecoratingSceneConfig.CharacterAnimation.LookAnimationPart> parts);
	}

	public class AnimationPartPlayer : DecoratingSceneConfig.IAnimationPartPlayer
	{
		public bool isAnimationActive
		{
			get
			{
				return this.animationState.isActive;
			}
		}

		public void Init(List<DecoratingSceneConfig.CharacterAnimation.AnimationPart> parts)
		{
			this.animationParts.Clear();
			for (int i = 0; i < parts.Count; i++)
			{
				DecoratingSceneConfig.CharacterAnimation.AnimationPart item = parts[i];
				this.animationParts.Add(item);
			}
		}

		public void Init(List<DecoratingSceneConfig.CharacterAnimation.TextAnimationPart> parts)
		{
			this.animationParts.Clear();
			for (int i = 0; i < parts.Count; i++)
			{
				DecoratingSceneConfig.CharacterAnimation.TextAnimationPart item = parts[i];
				this.animationParts.Add(item);
			}
		}

		public void Init(List<DecoratingSceneConfig.CharacterAnimation.LookAnimationPart> parts)
		{
			this.animationParts.Clear();
			for (int i = 0; i < parts.Count; i++)
			{
				DecoratingSceneConfig.CharacterAnimation.LookAnimationPart item = parts[i];
				this.animationParts.Add(item);
			}
		}

		public void StartAnimation(CharacterAvatar avatar)
		{
			this.animationState = default(DecoratingSceneConfig.AnimationPartPlayer.AnimationState);
			this.animationState.isActive = true;
			if (this.animationParts.Count == 0)
			{
				this.animationState.isActive = false;
				return;
			}
			for (int i = 0; i < this.animationParts.Count; i++)
			{
				this.animationParts[i].ResetAnimationState();
			}
			this.animationParts[0].StartAnimation(avatar);
		}

		public void UpdateAnimation(float deltaTime, CharacterAvatar avatar)
		{
			if (!this.animationState.isActive)
			{
				return;
			}
			while (this.animationState.currentPartIndex < this.animationParts.Count)
			{
				DecoratingSceneConfig.IAnimationPart animationPart = this.animationParts[this.animationState.currentPartIndex];
				if (!animationPart.isAnimationActive)
				{
					animationPart.StartAnimation(avatar);
				}
				deltaTime = animationPart.Update(deltaTime, avatar);
				if (!animationPart.isAnimationActive)
				{
					this.animationState.currentPartIndex = this.animationState.currentPartIndex + 1;
				}
				if (deltaTime <= 0f)
				{
					return;
				}
			}
			this.animationState.isActive = false;
		}

		private List<DecoratingSceneConfig.IAnimationPart> animationParts = new List<DecoratingSceneConfig.IAnimationPart>();

		private DecoratingSceneConfig.AnimationPartPlayer.AnimationState animationState;

		private struct AnimationState
		{
			public bool isActive;

			public int currentPartIndex;
		}
	}

	[Serializable]
	public class CharacterAnimation
	{
		public void Init(DecoratingSceneConfig.AnimationsList animationList)
		{
			this.animationList = animationList;
		}

		public bool isAnimationActive
		{
			get
			{
				for (int i = 0; i < this.players.Count; i++)
				{
					if (this.players[i].isAnimationActive)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void StartAnimation(CharacterAvatar avatar)
		{
			this.partPlayer.Init(this.animationParts);
			this.textPartPlayer.Init(this.textAnimations);
			this.lookPartPlayer.Init(this.lookAnimations);
			this.players.Clear();
			this.players.Add(this.partPlayer);
			this.players.Add(this.textPartPlayer);
			this.players.Add(this.lookPartPlayer);
			for (int i = 0; i < this.players.Count; i++)
			{
				this.players[i].StartAnimation(avatar);
			}
		}

		public void UpdateAnimation(float deltaTime, CharacterAvatar avatar)
		{
			if (!this.isAnimationActive)
			{
				return;
			}
			for (int i = 0; i < this.players.Count; i++)
			{
				this.players[i].UpdateAnimation(deltaTime, avatar);
			}
		}

		[SerializeField]
		public bool playAtInit;

		[SerializeField]
		public bool leaveAfterInit;

		[SerializeField]
		public List<DecoratingSceneConfig.CharacterAnimation.AnimationPart> animationParts = new List<DecoratingSceneConfig.CharacterAnimation.AnimationPart>();

		[SerializeField]
		public List<DecoratingSceneConfig.CharacterAnimation.TextAnimationPart> textAnimations = new List<DecoratingSceneConfig.CharacterAnimation.TextAnimationPart>();

		[SerializeField]
		public List<DecoratingSceneConfig.CharacterAnimation.LookAnimationPart> lookAnimations = new List<DecoratingSceneConfig.CharacterAnimation.LookAnimationPart>();

		[SerializeField]
		private List<string> unlockedThatEnable = new List<string>();

		[SerializeField]
		private List<string> ownedThatDisable = new List<string>();

		[SerializeField]
		private List<string> ownedThaEnable = new List<string>();

		private DecoratingSceneConfig.IAnimationPartPlayer partPlayer = new DecoratingSceneConfig.AnimationPartPlayer();

		private DecoratingSceneConfig.IAnimationPartPlayer textPartPlayer = new DecoratingSceneConfig.AnimationPartPlayer();

		private DecoratingSceneConfig.IAnimationPartPlayer lookPartPlayer = new DecoratingSceneConfig.AnimationPartPlayer();

		private List<DecoratingSceneConfig.IAnimationPartPlayer> players = new List<DecoratingSceneConfig.IAnimationPartPlayer>();

		[NonSerialized]
		public DecoratingSceneConfig.AnimationsList animationList;

		[Serializable]
		public class TextAnimationPart : DecoratingSceneConfig.IAnimationPart
		{
			public bool isAnimationActive
			{
				get
				{
					return this.animationState.isAnimating;
				}
			}

			public void ResetAnimationState()
			{
				this.animationState = default(DecoratingSceneConfig.CharacterAnimation.TextAnimationPart.AnimationState);
			}

			public void StartAnimation(CharacterAvatar avatar)
			{
				this.animationState = default(DecoratingSceneConfig.CharacterAnimation.TextAnimationPart.AnimationState);
				this.animationState.isAnimating = true;
				if (string.IsNullOrEmpty(this.text))
				{
					avatar.HideSpeachBubble();
					return;
				}
				avatar.ShowSpeachBubble(this.text);
			}

			public float Update(float deltaTime, CharacterAvatar avatar)
			{
				if (!this.animationState.isAnimating)
				{
					return deltaTime;
				}
				float num = this.duration - this.animationState.time;
				this.animationState.time = this.animationState.time + deltaTime;
				if (this.animationState.time >= this.duration)
				{
					avatar.HideSpeachBubble();
					this.animationState.isAnimating = false;
				}
				return Mathf.Max(0f, deltaTime - num);
			}

			public string text;

			public float duration;

			[NonSerialized]
			private DecoratingSceneConfig.CharacterAnimation.TextAnimationPart.AnimationState animationState;

			private struct AnimationState
			{
				public bool isAnimating;

				public float time;
			}
		}

		[Serializable]
		public class LookAnimationPart : DecoratingSceneConfig.IAnimationPart
		{
			public bool isAnimationActive
			{
				get
				{
					return this.animationState.isAnimating;
				}
			}

			public void ResetAnimationState()
			{
				this.animationState = default(DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.AnimationState);
			}

			public void StartAnimation(CharacterAvatar avatar)
			{
				this.animationState = default(DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.AnimationState);
				this.animationState.isAnimating = true;
				if (this.lookAtType == DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.ChangeWeight)
				{
					avatar.ChangeLookAtWeight(this.weight, this.transitionDuration);
					return;
				}
				if (this.lookAtType == DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.Pause)
				{
					return;
				}
				Vector3 position = this.LookAtPosition(avatar);
				avatar.LookAt(position, this.weight, this.transitionDuration);
			}

			public float Update(float deltaTime, CharacterAvatar avatar)
			{
				if (!this.animationState.isAnimating)
				{
					return deltaTime;
				}
				float num = this.duration - this.animationState.time;
				this.animationState.time = this.animationState.time + deltaTime;
				if (this.animationState.time >= this.duration)
				{
					this.animationState.isAnimating = false;
				}
				return Mathf.Max(0f, deltaTime - num);
			}

			private Vector3 LookAtPosition(CharacterAvatar avatar)
			{
				if (this.lookAtType == DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.Position)
				{
					return this.offset;
				}
				if (this.lookAtType == DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.LocalOffset)
				{
					return avatar.transform.forward * this.offset.z + avatar.transform.right * this.offset.x + avatar.transform.up * this.offset.y;
				}
				if (this.lookAtType == DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.Character)
				{
					if (avatar.decoratingScene == null)
					{
						return this.offset;
					}
					CharacterAvatar avatar2 = avatar.decoratingScene.animationPlayer.GetAvatar(this.lookAtName);
					if (avatar2 == null)
					{
						return this.offset;
					}
					return avatar2.headPosition + this.offset;
				}
				else if (this.lookAtType == DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.Marker)
				{
					Transform marker = avatar.decoratingScene.animationPlayer.GetMarker(this.lookAtName);
					if (marker == null)
					{
						return this.offset;
					}
					return marker.position + this.offset;
				}
				else
				{
					if (this.lookAtType != DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt.ObjectInRoom)
					{
						return this.offset;
					}
					VisualObjectBehaviour behaviour = avatar.decoratingScene.GetBehaviour(this.lookAtName);
					if (behaviour == null)
					{
						return this.offset;
					}
					return behaviour.characterBehaviour.sceneItem.lookAtPosition + this.offset;
				}
			}

			public DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.LookAt lookAtType;

			public string lookAtName;

			public float weight;

			public Vector3 offset;

			public float transitionDuration = 0.5f;

			public float duration;

			[NonSerialized]
			private DecoratingSceneConfig.CharacterAnimation.LookAnimationPart.AnimationState animationState;

			public enum LookAt
			{
				Position,
				LocalOffset,
				Character,
				ObjectInRoom,
				Pause,
				ChangeWeight,
				Marker
			}

			private struct AnimationState
			{
				public bool isAnimating;

				public float time;
			}
		}

		[Serializable]
		public class AnimationPart : DecoratingSceneConfig.IAnimationPart
		{
			public bool isAnimationActive
			{
				get
				{
					return this.animationState.isAnimating;
				}
			}

			public void ResetAnimationState()
			{
				this.animationState = default(DecoratingSceneConfig.CharacterAnimation.AnimationPart.AnimationState);
			}

			public void StartAnimation(CharacterAvatar avatar)
			{
				this.animationState = default(DecoratingSceneConfig.CharacterAnimation.AnimationPart.AnimationState);
				this.animationState.isAnimating = true;
				if (this.setTransform)
				{
					Vector3 vector = this.position;
					Vector3 rotation = this.rotationEuler;
					avatar.SetPosition(vector);
					avatar.SetRotation(rotation);
				}
				avatar.SetProperty(this.property, true);
			}

			public float Update(float deltaTime, CharacterAvatar avatar)
			{
				if (!this.animationState.isAnimating)
				{
					return deltaTime;
				}
				float num = this.duration - this.animationState.time;
				this.animationState.time = this.animationState.time + deltaTime;
				if (this.animationState.time >= this.duration)
				{
					avatar.SetProperty(this.property, false);
					this.animationState.isAnimating = false;
				}
				return Mathf.Max(0f, deltaTime - num);
			}

			public bool setTransform;

			public Vector3 position;

			public Vector3 rotationEuler;

			public float duration;

			public string property;

			[NonSerialized]
			private DecoratingSceneConfig.CharacterAnimation.AnimationPart.AnimationState animationState;

			private struct AnimationState
			{
				public bool isAnimating;

				public float time;
			}
		}
	}

	[Serializable]
	public class RoomAnimationList
	{
		public void Init()
		{
			for (int i = 0; i < this.sceneObjectAnimations.Count; i++)
			{
				this.sceneObjectAnimations[i].Init();
			}
		}

		public DecoratingSceneConfig.AnimationsList GetAnimationsForSceneObject(string sceneObjectName)
		{
			for (int i = 0; i < this.sceneObjectAnimations.Count; i++)
			{
				DecoratingSceneConfig.AnimationsList animationsList = this.sceneObjectAnimations[i];
				if (animationsList.sceneObjectName == sceneObjectName)
				{
					return animationsList;
				}
			}
			return null;
		}

		[SerializeField]
		public string roomName;

		[SerializeField]
		public List<DecoratingSceneConfig.AnimationsList> sceneObjectAnimations = new List<DecoratingSceneConfig.AnimationsList>();
	}

	[Serializable]
	public class AnimationsList
	{
		public void Init()
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				this.animations[i].Init(this);
			}
		}

		public string sceneObjectName;

		public bool isDefaultAnimation;

		public List<DecoratingSceneConfig.CharacterAnimation> animations = new List<DecoratingSceneConfig.CharacterAnimation>();

		private List<DecoratingSceneConfig.CharacterAnimation> availableAnimations_ = new List<DecoratingSceneConfig.CharacterAnimation>();
	}

	[Serializable]
	public class VisualObjectOverride
	{
		public bool isSettingSaved;

		public string visualObjectName;

		public Vector3 iconHandlePositionOffset;

		public Vector3 iconHandlePositionScale = Vector3.one;

		public Vector3 iconHandleRotation = Vector3.zero;
	}

	[Serializable]
	public class RoomConfig
	{
		public DecoratingSceneConfig.VisualObjectOverride GetObjectOverride(string objectName)
		{
			for (int i = 0; i < this.objectOverrides.Count; i++)
			{
				DecoratingSceneConfig.VisualObjectOverride visualObjectOverride = this.objectOverrides[i];
				if (visualObjectOverride.visualObjectName == objectName)
				{
					return visualObjectOverride;
				}
			}
			return null;
		}

		public string roomName;

		[SerializeField]
		private List<DecoratingSceneConfig.VisualObjectOverride> objectOverrides = new List<DecoratingSceneConfig.VisualObjectOverride>();
	}
}
