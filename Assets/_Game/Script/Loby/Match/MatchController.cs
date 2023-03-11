using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class MatchController : MonoBehaviour
    {
        [SerializeField] private WonnaTimeDatas matchWaitTimeDatas;

        private float _currentTime;
        private float _targetTime;

        private bool _isWaiting;


        private void OnEnable()
        {
            GameManager.MatchWaiting += OnMatchWaiting;
            GameManager.MatchFound += OnMatchFound;
        }
        private void OnDisable()
        {
            GameManager.MatchWaiting -= OnMatchWaiting;
            GameManager.MatchFound -= OnMatchFound;
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


        private void OnMatchFound()
        {
            GameManager.Instance.SetState(GameState.Game_TOUR_START);
        }


        private void FixedUpdate()
        {
            TimeWait();
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

            if (_currentTime >= _targetTime)
            {
                _isWaiting = false;
            }
        }
    }
}