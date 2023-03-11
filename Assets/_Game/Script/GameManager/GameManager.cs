using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using WonnasmithTools;

namespace Wonnasmith
{
    public enum GameState
    {
        NONE,

        GAME_START,
        Game_MATCH_WAIT,
        Game_MATCH_FOUND,
        Game_TOUR_START,
        GAME_PLAY,
        GAME_WIN,
        GAME_LOSE,
        GAME_FINISHED,
        GAME_QUIT
    }

    [DefaultExecutionOrder(-1)]
    public class GameManager : Singleton<GameManager>
    {
        public delegate void GameManagerStateChange();


        /// <summary> Dataların yüklenmesini sağlar </summary>
        public static event /*GameManager.*/GameManagerStateChange DataLoad;

        /// <summary> leveli hazırlar </summary>
        public static event /*GameManager.*/GameManagerStateChange LevelLoad;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerStateChange GameStart;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerStateChange LevelWin;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerStateChange LevelLose;

        /// <summary> Yeni sahnenin yüklenmeye başladığını haber salar </summary>
        public static event /*GameManager.*/GameManagerStateChange LevelFinish;

        /// <summary> Oyunda eşleşme aranmaya başladığını bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange MatchWaiting;

        /// <summary> Oyunda eşleşme bulunduğunu bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange MatchFound;

        /// <summary> Yeni tur başlaması için hazırlıklar yapılır </summary>
        public static event /*GameManager.*/GameManagerStateChange TourPrepare;

        /// <summary> Oyunda tur başladığını bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange TourStart;

        private GameState _currentGameState = GameState.NONE;
        private GameState _prevGameState = GameState.NONE;

        private void Start()
        {
            SetState(GameState.GAME_START);
        }

        private void OnEnable()
        {
            DataLoad += OnDataLoad;
            LevelLoad += OnLevelLoad;
            TourPrepare += OnTourPrepare;
            TourStart += OnTourStart;
        }

        private void OnDisable()
        {
            DataLoad -= OnDataLoad;
            LevelLoad -= OnLevelLoad;
            TourPrepare -= OnTourPrepare;
            TourStart -= OnTourStart;
        }


        public void SetState(GameState newGameState)
        {
            Debug.Log("G-A-M-E_S-T-A-T-E: ==> " + newGameState, gameObject);

            _prevGameState = _currentGameState;
            _currentGameState = newGameState;

            if (newGameState.Equals(GameState.GAME_START))
            {
                GameStart?.Invoke();
                DataLoad?.Invoke();
            }
            else if (newGameState.Equals(GameState.Game_MATCH_WAIT))
            {
                MatchWaiting?.Invoke();
            }
            else if (newGameState.Equals(GameState.Game_MATCH_FOUND))
            {
                MatchFound?.Invoke();
            }
            else if (newGameState.Equals(GameState.Game_TOUR_START))
            {
                TourPrepare?.Invoke();
                TourStart?.Invoke();
                SetState(GameState.GAME_PLAY);
            }
            else if (newGameState.Equals(GameState.GAME_WIN))
            {
                LevelWin?.Invoke();
                SetState(GameState.GAME_FINISHED);
            }
            else if (newGameState.Equals(GameState.GAME_LOSE))
            {
                LevelLose?.Invoke();
                SetState(GameState.GAME_FINISHED);
            }
            else if (newGameState.Equals(GameState.GAME_FINISHED))
            {
                LevelFinish?.Invoke();
            }
        }


        public bool GameIsPlaying()
        {
            if (_currentGameState.Equals(GameState.GAME_PLAY)) { return true; }
            else { return false; }
        }

        
        private void OnDataLoad()
        {
            Debug.Log("<color=grey>:::DataLoad:::</color>");
        }


        private void OnLevelLoad()
        {
            Debug.Log("<color=yellow>:::OnLevelLoad:::</color>");
        }


        private void OnTourPrepare()
        {
            Debug.Log("<color=green>:::OnTourPrepare:::</color>");
        }


        private void OnTourStart()
        {
            Debug.Log("<color=green>:::OnTourStart:::</color>");
        }
    }
}
