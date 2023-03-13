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
        GAME_PHOTON_CONNECTED,
        GAME_LOBBY_CONNECTED,
        GAME_IN_LOBBY,
        GAME_MATCH_WAIT,
        GAME_MATCH_FOUND,
        GAME_TOUR_START,
        GAME_TOUR_PLAY,
        GAME_TOUR_WIN,
        GAME_TOUR_LOSE,
        GAME_TOUR_FINISHED,
        GAME_QUIT
    }

    public class GameManager : Singleton<GameManager>
    {
        public delegate void GameManagerStateChange();


        /// <summary> Dataların yüklenmesini sağlar </summary>
        public static event /*GameManager.*/GameManagerStateChange DataLoad;

        /// <summary> Oyunun başladığını bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange GameStart;

        /// <summary> Turun win olduğu bildirilir </summary>
        public static event /*GameManager.*/GameManagerStateChange TourWin;

        /// <summary> Turun lose olduğu bildirilir </summary>
        public static event /*GameManager.*/GameManagerStateChange TourLose;

        /// <summary> Turun bittiği bildirilir </summary>
        public static event /*GameManager.*/GameManagerStateChange TourFinish;

        /// <summary> Oyunda eşleşme aranmaya başladığını bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange MatchWaiting;

        /// <summary> Oyunda eşleşme bulunduğunu bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange MatchFound;

        /// <summary> Yeni tur başlaması için hazırlıklar yapılır </summary>
        public static event /*GameManager.*/GameManagerStateChange TourPrepare;

        /// <summary> Turun başladığını bildirir </summary>
        public static event /*GameManager.*/GameManagerStateChange TourStart;

        private GameState _currentGameState = GameState.NONE;

        private void Start()
        {
            SetState(GameState.GAME_START);
        }

        public void SetState(GameState newGameState)
        {
            Debug.Log("<color=green>:::" + newGameState + "::</color>", gameObject);

            _currentGameState = newGameState;

            if (newGameState.Equals(GameState.GAME_START))
            {
                GameStart?.Invoke();
                DataLoad?.Invoke();
            }
            if (newGameState.Equals(GameState.GAME_LOBBY_CONNECTED))
            {
                SetState(GameState.GAME_IN_LOBBY);
            }
            else if (newGameState.Equals(GameState.GAME_MATCH_WAIT))
            {
                MatchWaiting?.Invoke();
            }
            else if (newGameState.Equals(GameState.GAME_MATCH_FOUND))
            {
                MatchFound?.Invoke();
            }
            else if (newGameState.Equals(GameState.GAME_TOUR_START))
            {
                TourPrepare?.Invoke();
                TourStart?.Invoke();
                SetState(GameState.GAME_TOUR_PLAY);
            }
            else if (newGameState.Equals(GameState.GAME_TOUR_WIN))
            {
                TourWin?.Invoke();
                SetState(GameState.GAME_TOUR_FINISHED);
            }
            else if (newGameState.Equals(GameState.GAME_TOUR_LOSE))
            {
                TourLose?.Invoke();
                SetState(GameState.GAME_TOUR_FINISHED);
            }
            else if (newGameState.Equals(GameState.GAME_TOUR_FINISHED))
            {
                TourFinish?.Invoke();
            }
        }


        public bool GameIsPlaying()
        {
            if (_currentGameState.Equals(GameState.GAME_TOUR_PLAY)) { return true; }
            else { return false; }
        }


        public bool GameInLobby()
        {
            if (_currentGameState.Equals(GameState.GAME_IN_LOBBY)) { return true; }
            else { return false; }
        }
    }
}
