using System;
using System.Collections.Generic;
using ProtoModels;
using UnityEngine;

namespace GGMatch3
{
    public class Match3StagesDB : ScriptableObject
    {
        public void OnDestroy()
        {
            Match3StagesDB.applicationIsQuitting = true;
        }

        public static Match3StagesDB instance
        {
            get
            {
                string stagesDBName = GGTest.stagesDBName;
                if (stagesDBName != Match3StagesDB.loadedDBName || Match3StagesDB.instance_ == null)
                {
                    if (Match3StagesDB.applicationIsQuitting)
                    {
                        return null;
                    }
                    Match3StagesDB.loadedDBName = stagesDBName;
                    UnityEngine.Debug.Log("Loading singleton from " + stagesDBName);
                    Match3StagesDB.instance_ = Resources.Load<Match3StagesDB>(stagesDBName);
                    if (Match3StagesDB.instance_ == null)
                    {
                        Match3StagesDB.instance_ = Resources.Load<Match3StagesDB>(typeof(Match3StagesDB).ToString());
                    }
                    if (Match3StagesDB.instance_ != null)
                    {
                        Match3StagesDB.instance_.UpdateData();
                    }
                    SingletonInit<FileIOChanges>.instance.OnChange(new FileIOChanges.OnDataChangedDelegate(Match3StagesDB.instance_.ReloadModel));
                }
                return Match3StagesDB.instance_;
            }
        }

        public Match3StagesDB.Stage GetStageForLevelName(string levelDBName, string levelName)
        {
            for (int i = 0; i < this.stages.Count; i++)
            {
                Match3StagesDB.Stage stage = this.stages[i];
                if (stage.levelReference.levelDBName == levelDBName && stage.levelReference.levelName == levelName)
                {
                    return stage;
                }
                for (int j = 0; j < stage.multiLevelReference.Count; j++)
                {
                    Match3StagesDB.LevelReference levelReference = stage.multiLevelReference[j];
                    if (levelReference.levelDBName == levelDBName && levelReference.levelName == levelName)
                    {
                        return stage;
                    }
                }
            }
            return null;
        }

        public Match3StagesDB.Stage currentStage
        {
            get
            {
                return this.stages[Mathf.Clamp(this.model.passedStages, 0, this.stages.Count - 1)];
            }
        }

        public int PassedStagesInRow(int maxStage)
        {
            int num = Mathf.Clamp(this.model.passedStages, 0, this.stages.Count - 1);
            if (this.stages[num].timesPlayed > 0)
            {
                return 0;
            }
            int num2 = 0;
            maxStage = Mathf.Max(0, maxStage);
            for (int i = num - 1; i >= maxStage; i--)
            {
                Match3StagesDB.Stage stage = this.stages[i];
                num2++;
                if (stage.timesPlayed > 1 || num2 > 3)
                {
                    break;
                }
            }
            return num2;
        }

        public int passedStages
        {
            get
            {
                return this.model.passedStages;
            }
        }

        public Match3Stages.Stage GetModelForStage(Match3StagesDB.Stage stage)
        {
            if (this.model.stages == null)
            {
                this.model.stages = new List<Match3Stages.Stage>();
            }
            for (int i = 0; i < this.model.stages.Count; i++)
            {
                Match3Stages.Stage stage2 = this.model.stages[i];
                if (stage2.stageIndex == stage.index)
                {
                    return stage2;
                }
            }
            Match3Stages.Stage stage3 = new Match3Stages.Stage();
            stage3.stageIndex = stage.index;
            stage3.stageName = stage.levelReference.levelName;
            stage3.forbittenBoosters = new List<ProtoModels.BoosterType>();
            for (int j = 0; j < stage.forbittenBoosters.Count; j++)
            {
                GGMatch3.BoosterType booster = stage.forbittenBoosters[j];
                stage3.forbittenBoosters.Add(BoosterConfig.BoosterToProtoType(booster));
            }
            this.model.stages.Add(stage3);
            this.SaveModel();
            return stage3;
        }

        protected void UpdateData()
        {
            for (int i = 0; i < this.stages.Count; i++)
            {
                this.stages[i].Init(this, i);
            }
            this.ReloadModel();
        }

        public void ResetAll()
        {
            this.model = new Match3Stages();
            this.SaveModel();
        }

        private void ReloadModel()
        {
            if (!ProtoIO.LoadFromFileLocal<Match3Stages>("st.bytes", out this.model))
            {
                this.model = new Match3Stages();
            }
        }

        public Match3Stages ModelFromData(CloudSyncData fileSystemData)
        {
            Match3Stages result = ProtoIO.Clone<Match3Stages>(this.model);
            if (fileSystemData == null)
            {
                return result;
            }
            CloudSyncData.CloudSyncFile file = ProtoModelExtensions.GetFile(fileSystemData, "st.bytes");
            if (file == null)
            {
                return result;
            }
            Match3Stages match3Stages = null;
            if (!ProtoIO.LoadFromBase64String<Match3Stages>(file.data, out match3Stages))
            {
                return result;
            }
            if (match3Stages == null)
            {
                return result;
            }
            return match3Stages;
        }

        public void SaveModel()
        {
            ProtoIO.SaveToFileCS<Match3Stages>("st.bytes", this.model);
        }

        public int stagesPassed
        {
            get
            {
                return this.model.passedStages;
            }
            set
            {
                this.model.passedStages = value;
                this.SaveModel();
            }
        }

        private static bool applicationIsQuitting;

        protected static string loadedDBName;

        protected static Match3StagesDB instance_;

        private const string Filename = "st.bytes";

        public List<BoosterConfig> defaultBoosters = new List<BoosterConfig>();

        [SerializeField]
        public int limit;

        [SerializeField]
        public List<Match3StagesDB.Stage> stages = new List<Match3StagesDB.Stage>();

        private Match3Stages model;

        [Serializable]
        public class LevelReference
        {
            public LevelDB levelDB
            {
                get
                {
                    if (string.IsNullOrEmpty(this.levelDBName))
                    {
                        return ScriptableObjectSingleton<LevelDB>.instance;
                    }
                    return LevelDB.NamedInstance(this.levelDBName);
                }
            }

            public LevelDefinition level
            {
                get
                {
                    return this.levelDB.Get(this.levelName);
                }
            }

            public string levelDBName;

            public string levelName;
        }

        [Serializable]
        public class Stage : Match3GameListener
        {
            public Match3StagesDB.Stage.Difficulty difficulty
            {
                get
                {
                    return this.difficulty_;
                }
            }

            private Match3Stages.Stage model
            {
                get
                {
                    return this.stages.GetModelForStage(this);
                }
            }

            public List<Match3StagesDB.LevelReference> allLevelReferences
            {
                get
                {
                    if (this.allLevelReferences_ == null)
                    {
                        this.allLevelReferences_ = new List<Match3StagesDB.LevelReference>();
                    }
                    this.allLevelReferences_.Clear();
                    if (this.multiLevelReference.Count > 0)
                    {
                        this.allLevelReferences_.AddRange(this.multiLevelReference);
                    }
                    else
                    {
                        this.allLevelReferences_.Add(this.levelReference);
                    }
                    return this.allLevelReferences_;
                }
            }

            public bool shouldUseStarDialog
            {
                get
                {
                    if (this.showStarDialog)
                    {
                        return true;
                    }
                    LevelDefinition level = this.levelReference.level;
                    return level != null && level.tutorialMatches.Count > 0;
                }
            }

            public int timesPlayed
            {
                get
                {
                    return this.model.timesPlayed;
                }
            }

            public void Init(Match3StagesDB stages, int index)
            {
                this.stages = stages;
                this.index = index;
            }

            public bool isIntroMessageShown
            {
                get
                {
                    return this.model.isIntroMessageShown;
                }
                set
                {
                    this.model.isIntroMessageShown = value;
                    this.stages.SaveModel();
                }
            }

            public bool isPassed
            {
                get
                {
                    return this.model.isPassed;
                }
            }

            public void OnGameComplete(GameCompleteParams completeParams)
            {
                Match3Game game = completeParams.game;
                if (completeParams.isWin)
                {
                    this.model.isPassed = true;
                    if (this.stages.model.passedStages == this.index)
                    {
                        this.stages.model.passedStages++;
                    }
                    this.stages.SaveModel();
                    new Analytics.StageCompletedEvent
                    {
                        stageState = completeParams.stageState
                    }.Send();
                    return;
                }
                if (game != null && !game.hasPlayedAnyMoves)
                {
                    this.model.timesPlayed = Mathf.Max(this.model.timesPlayed - 1, 0);
                    this.stages.SaveModel();
                }
                GameScreen.StageState stageState = completeParams.stageState;
                if (stageState.userMovesCount > 0)
                {
                    new Analytics.StageFailedEvent
                    {
                        stageState = stageState
                    }.Send();
                }
            }

            public void OnGameStarted(GameStartedParams startedParams)
            {
                Match3Stages.Stage model = this.model;
                int timesPlayed = model.timesPlayed;
                model.timesPlayed = timesPlayed + 1;
                this.stages.SaveModel();
                new Analytics.StageStartedEvent
                {
                    stageState = startedParams.stageState
                }.Send();
            }

            [SerializeField]
            private Match3StagesDB.Stage.Difficulty difficulty_;

            [NonSerialized]
            public int index;

            [NonSerialized]
            private Match3StagesDB stages;

            [SerializeField]
            public Match3StagesDB.LevelReference levelReference = new Match3StagesDB.LevelReference();

            private List<Match3StagesDB.LevelReference> levelReferencesToPublish = new List<Match3StagesDB.LevelReference>();

            [SerializeField]
            public List<Match3StagesDB.LevelReference> multiLevelReference = new List<Match3StagesDB.LevelReference>();

            private List<Match3StagesDB.LevelReference> allLevelReferences_;

            [SerializeField]
            public int coinsCount;

            [SerializeField]
            public List<BoosterType> forbittenBoosters = new List<BoosterType>();

            [SerializeField]
            public bool hideUIElements;

            [SerializeField]
            private bool showStarDialog;

            [SerializeField]
            public List<string> startMessages = new List<string>();

            [SerializeField]
            public List<Match3StagesDB.Stage.Alternative> alternatives = new List<Match3StagesDB.Stage.Alternative>();

            private bool isIntroMessageShown_;

            public enum Difficulty
            {
                Normal,
                Hard,
                Nightmare
            }

            [Serializable]
            public class Alternative
            {
                [SerializeField]
                public Match3StagesDB.LevelReference levelReference = new Match3StagesDB.LevelReference();
            }
        }
    }
}
