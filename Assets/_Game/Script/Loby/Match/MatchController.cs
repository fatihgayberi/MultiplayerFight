using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class MatchController : MonoBehaviour
    {
        public delegate void MatchControllerCharacterCountChange(int newCharacterCount);

        public delegate void MatchControllerTimeDownChange(float currentTimeDown);
        public static event MatchControllerTimeDownChange TimeDownChange;

        public delegate void MatchControllerMatchFinih();
        public static event MatchControllerMatchFinih MatchFinish;

        [SerializeField] private WonnaTimeDatas matchWaitTimeDatas;

        private float _currentTime;
        private float _targetTime;

        private bool _isWaiting;


        private void OnEnable()
        {
            GameManager.MatchWaiting += OnMatchWaiting;
            GameManager.MatchFound += OnMatchFound;

            CharacterManagers.CharacterCountChange += OnCharacterCountChange;
        }
        private void OnDisable()
        {
            GameManager.MatchWaiting -= OnMatchWaiting;
            GameManager.MatchFound -= OnMatchFound;

            CharacterManagers.CharacterCountChange -= OnCharacterCountChange;
        }


        private void FixedUpdate()
        {
            TimeWait();
        }


        private void OnMatchWaiting()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            _isWaiting = true;

            _targetTime = matchWaitTimeDatas.WonnaTimeDatas2TotalSecond();
        }


        private void OnCharacterCountChange(int newCharacterCount)
        {
            if (newCharacterCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                GameManager.Instance.SetState(GameState.Game_MATCH_FOUND);
            }
        }


        private void OnMatchFound()
        {
            _isWaiting = false;
            
            GameManager.Instance.SetState(GameState.Game_TOUR_START);
        }


        private void TimeWait()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (!_isWaiting)
            {
                return;
            }

            _currentTime += Time.fixedDeltaTime;

            TimeDownChange?.Invoke(Mathf.Clamp(_targetTime - _currentTime, 0, _targetTime));

            if (_currentTime >= _targetTime)
            {
                _isWaiting = false;

                MatchFinish?.Invoke();
                GameManager.Instance.SetState(GameState.Game_TOUR_START);
            }
        }
    }
}