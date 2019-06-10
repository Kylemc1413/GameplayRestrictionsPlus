using IllusionInjector;
using IllusionPlugin;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace GameplayRestrictionsPlus
{
    public class Plugin : IPlugin
    {
        public string Name => "GameplayRestrictionsPlus";
        public string Version => "1.2.2";

        public static readonly Config Config = new Config(Path.Combine(Environment.CurrentDirectory, "UserData\\GamePlayRestrictionsPlus.ini"));

        private BeatmapObjectSpawnController _spawnController;
        public static BS_Utils.Gameplay.LevelData LevelData { get; private set; }
        private static StandardLevelFailedController _levelFailedController;
        private static StandardLevelRestartController _levelRestartController;
        private static GameEnergyCounter _energyCounter;
        private static ScoreController _scoreController;
        public static bool activateDuringIsolated = false;
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            if (PluginManager.Plugins.Any(x => x.Name == "BeatSaberChallenges"))
                ChallengeIntegration.AddListeners();

        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            Config.Save();
            if (newScene.name == "MenuCore")
                activateDuringIsolated = false;

            if (newScene.name == "GameCore")
            {
                if (BS_Utils.Gameplay.Gamemode.IsIsolatedLevel && !activateDuringIsolated) return;
                GetObjects();
            }
        }

        private void _spawnController_noteWasMissedEvent(BeatmapObjectSpawnController arg1, NoteController noteController)
        {
            if (noteController.noteData.noteType != NoteType.Bomb && Config.failOnMiss)
                Fail();
        }




        
        private void _spawnController_noteWasCutEvent(BeatmapObjectSpawnController arg1, NoteController noteController, NoteCutInfo cutInfo)
        {
         
            if (noteController.noteData.noteType != NoteType.Bomb && (Config.failOnBadCut && !cutInfo.allIsOK))
                Fail();
            if (noteController.noteData.noteType == NoteType.Bomb && Config.failOnBomb)
                Fail();


            //Try to get score of the cut (Nevermind this isn't accurate)
            if (noteController.noteData.noteType != NoteType.Bomb && (Config.failOnImperfectCut && cutInfo.allIsOK))
            {
                
                ScoreBuffer buffer = new ScoreBuffer(cutInfo, cutInfo.afterCutSwingRatingCounter);
                buffer.didFinishEvent += BufferDidFinishEvent;

            }



        }
        private void BufferDidFinishEvent(ScoreBuffer buffer)
        {
            if (buffer.ReturnScore() < Config.imperfectCutThreshold)
                Fail();
        }
        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "MenuCore")
            {
                UI.CreateUI();
                Config.Save();
            }




        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }


        public static bool CheckRestrictions()
        {
            if (LevelData.GameplayCoreSceneSetupData.gameplayModifiers.noFail && !Config.stricterAngles) return false;
            if (Config.failOnBadCut || Config.failOnMiss || Config.failOnBomb || Config.failOnSaberClash ||  Config.failOnImperfectCut || Config.restartOnFail || Config.crashOnFail || Config.stricterAngles)
                return true;
            else
                return false;

        }


        private void Fail()
        {
            if (Config.restartOnFail)
                _levelRestartController.RestartLevel();
            else if (Config.crashOnFail)
                Application.Quit();
            else
                _levelFailedController.StartLevelFailed();

        }


        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        private void GetObjects()
        {
            if (_spawnController == null)
                _spawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
            if (_spawnController == null) return;
            _spawnController.noteWasCutEvent -= _spawnController_noteWasCutEvent;
            _spawnController.noteWasMissedEvent -= _spawnController_noteWasMissedEvent;
            if (LevelData == null)
                LevelData = BS_Utils.Plugin.LevelData;
            if (LevelData == null) return;

            if (_levelFailedController == null)
                _levelFailedController = Resources.FindObjectsOfTypeAll<StandardLevelFailedController>().FirstOrDefault();

            if (_levelRestartController == null)
                _levelRestartController = Resources.FindObjectsOfTypeAll<StandardLevelRestartController>().FirstOrDefault();

            if (_energyCounter == null)
                _energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault();


            if (_scoreController == null)
                _scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault();

            if (!CheckRestrictions()) return;

            _spawnController.noteWasCutEvent += _spawnController_noteWasCutEvent;
            _spawnController.noteWasMissedEvent += _spawnController_noteWasMissedEvent;
            _energyCounter.gameEnergyDidReach0Event += _energyCounter_gameEnergyDidReach0Event;
            if (Config.failOnSaberClash)
                LevelData.GameplayCoreSceneSetupData.gameplayModifiers.failOnSaberClash = true;
            if(Config.stricterAngles)
            {
                Log("Disabling Score Submission for Stricter Angles");
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("Gameplay Restrictions Plus");
                LevelData.GameplayCoreSceneSetupData.gameplayModifiers.strictAngles = true;

            }
        }

        private void _energyCounter_gameEnergyDidReach0Event()
        {
            Fail();
        }

        public void OnFixedUpdate()
        {
        }

        public static void Log(string message)
        {
            Console.WriteLine("[{0}] {1}", "GameplayRestrictionsPlus", message);
        }

    }
}
