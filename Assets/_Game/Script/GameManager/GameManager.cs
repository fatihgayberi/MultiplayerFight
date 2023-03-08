using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WonnasmithTools;

namespace Wonnasmith
{
    public enum GameState
    {
        NONE,

        GAME_SCENE_LOAD,
        GAME_LEVEL_LOAD,
        GAME_START,
        GAME_IS_PLAY,
        GAME_IS_WIN,
        GAME_IS_LOSE,
        GAME_PAUSED,
        GAME_RESUME,
        GAME_FINISHED,
        GAME_DATA_SAVE,
        GAME_QUIT
    }

    [DefaultExecutionOrder(-1)]
    public class GameManager : Singleton<GameManager>
    {
        public delegate void GameManagerLevelClear();
        public delegate void GameManagerDataLoad();
        public delegate void GameManagerLevelLoad();
        public delegate void GameManagerLevelPrepare();
        public delegate void GameManagerPreparePrepareNewSceneLoad();
        public delegate void GameManagerLevelWin();
        public delegate void GameManagerLevelLose();
        public delegate void GameManagerLevelFinish(bool isWin);
        public delegate void GameManagerGameDataSave();
        public delegate void GameManagerGamePause();
        public delegate void GameManagerGameResume();

        /// <summary> Önceki levelden kalma ne varsa temizlenir </summary>
        public static event /*GameManager.*/GameManagerLevelClear LevelClear;

        /// <summary> Dataların yüklenmesini sağlar </summary>
        public static event /*GameManager.*/GameManagerDataLoad DataLoad;

        /// <summary> leveli hazırlar </summary>
        public static event /*GameManager.*/GameManagerLevelLoad LevelLoad;

        /// <summary> Yeni levele hazırlık yapılır </summary>
        public static event /*GameManager.*/GameManagerLevelPrepare LevelPrepare;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerPreparePrepareNewSceneLoad PrepareNewSceneLoad;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerLevelWin LevelWin;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerLevelLose LevelLose;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerLevelFinish LevelFinish;

        /// <summary> Oyunun datalarının kaydedildiğini bildirir </summary>
        public static event /*GameManager.*/GameManagerGameDataSave GameDataSave;

        /// <summary> Oyunun pause geçtiğini haber salar </summary>
        public static event /*GameManager.*/GameManagerGamePause GamePause;

        /// <summary> Oyunun pause geçtiğini haber salar </summary>
        public static event /*GameManager.*/GameManagerGameResume GameResume;

        private GameState _currentGameState = GameState.NONE;
        private GameState _prevGameState = GameState.NONE;

        private void Start()
        {
            SetState(GameState.GAME_SCENE_LOAD);
        }

        private void OnEnable()
        {
            LevelClear += OnLevelClear;
            DataLoad += OnDataLoad;
            LevelLoad += OnLevelLoad;
            LevelPrepare += OnLevelPrepare;
            PrepareNewSceneLoad += OnPrepareNewSceneLoad;
        }

        private void OnDisable()
        {
            LevelClear -= OnLevelClear;
            DataLoad -= OnDataLoad;
            LevelLoad -= OnLevelLoad;
            LevelPrepare -= OnLevelPrepare;
            PrepareNewSceneLoad -= OnPrepareNewSceneLoad;
        }



        public void SetState(GameState newGameState)
        {
            Debug.Log("G-A-M-E_S-T-A-T-E: ==> " + newGameState);

            _prevGameState = _currentGameState;
            _currentGameState = newGameState;

            if (newGameState.Equals(GameState.GAME_LEVEL_LOAD))
            {
                SetState(/*GameManager.*/GameState.GAME_START);
                LevelClear?.Invoke();
                DataLoad?.Invoke();
                LevelLoad?.Invoke();
                LevelPrepare?.Invoke();
            }
            else if (newGameState.Equals(GameState.GAME_SCENE_LOAD))
            {
                PrepareNewSceneLoad?.Invoke();
            }
            else if (newGameState.Equals(GameState.GAME_IS_WIN))
            {
                LevelWin?.Invoke();
                LevelFinish?.Invoke(true);
                SetState(/*GameManager.*/GameState.GAME_FINISHED);
            }
            else if (newGameState.Equals(GameState.GAME_IS_LOSE))
            {
                LevelLose?.Invoke();
                LevelFinish?.Invoke(false);
                SetState(/*GameManager.*/GameState.GAME_FINISHED);
            }
            else if (newGameState.Equals(GameState.GAME_DATA_SAVE))
            {
                GameDataSave?.Invoke();
            }
            else if (newGameState.Equals(GameState.GAME_PAUSED))
            {
                Time.timeScale = 0;
                GamePause?.Invoke();
            }
            else if (newGameState.Equals(GameState.GAME_RESUME))
            {
                Time.timeScale = 1;
                GameResume?.Invoke();

                SetState(/*GameManager.*/GameState.GAME_IS_PLAY);
            }
        }


        public bool GameIsStart()
        {
            if (_currentGameState.Equals(GameState.GAME_START)) { return true; }
            else { return false; }
        }



        public bool GameIsPlaying()
        {
            if (_currentGameState.Equals(GameState.GAME_IS_PLAY)) { return true; }
            else { return false; }
        }



        public bool GameIsPause()
        {
            if (_currentGameState.Equals(GameState.GAME_PAUSED)) { return true; }
            else { return false; }
        }



        public bool GameIsResume()
        {
            if (_currentGameState.Equals(GameState.GAME_RESUME)) { return true; }
            else { return false; }
        }



        public bool GameIsSceneLoad()
        {
            if (_currentGameState.Equals(GameState.GAME_SCENE_LOAD)) { return true; }
            else { return false; }
        }



        public bool GameIsFinish()
        {
            if (_currentGameState.Equals(GameState.GAME_FINISHED)) { return true; }
            else { return false; }
        }



        private void OnDataLoad()
        {
            Debug.Log("<color=grey>:::DataLoad:::</color>");
        }



        private void OnLevelClear()
        {
            Debug.Log("<color=red>:::LevelClear:::</color>");
        }



        private void OnLevelLoad()
        {
            Debug.Log("<color=yellow>:::OnLevelLoad:::</color>");
        }



        private void OnLevelPrepare()
        {
            Debug.Log("<color=green>:::OnLevelPrepare:::</color>");

            /*GameManager.Instance.*/
            SetState(/*GameManager.*/GameState.GAME_IS_PLAY);
        }



        private void OnPrepareNewSceneLoad()
        {
            Debug.Log("<color=cyan>:::OnPrepareNewSceneLoad:::</color>");
            SetState(/*GameManager.*/GameState.GAME_LEVEL_LOAD);
        }



        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("<color=orange>:::OnSceneLoaded:::</color>" + scene.name + "<color=orange>:::</color>");
            /*GameManager.Instance.*/
            SetState(/*GameManager.*/GameState.GAME_SCENE_LOAD);
        }
    }
}
