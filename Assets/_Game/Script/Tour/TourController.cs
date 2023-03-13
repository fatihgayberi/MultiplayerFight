using System;
using Photon.Pun;
using UnityEngine;
using WonnasmithTools;

namespace Wonnasmith
{
    public class TourController : Singleton<TourController>
    {
        public delegate void TourControllerTourCountDownChange(float currentCountDownSecond, float targetCountDownSecond);
        public static event TourControllerTourCountDownChange TourCountDownChange;
        public static event TourControllerTourCountDownChange TourCountDownFinish;
        public static event TourControllerTourCountDownChange TourCountDownStart;

        public delegate void TourControllerTurnChanged(bool isTurnOfMasterClient);
        public static event TourControllerTurnChanged TurnChanged;

        [SerializeField] private WonnaTimeDatas countDownTime;
        [SerializeField] private PhotonView tourControllerPhotonView;

        private bool _isTurnOfMasterClient;

        public bool IsTurnOfMasterClient { get => _isTurnOfMasterClient; }

        private const string functionName_PunRPC_TurnChange = "PunRPC_TurnChange";

        private float _currentCountDownTime;
        private float _targetTime;

        private bool _isCountDown;

        private void Start()
        {
            _targetTime = countDownTime.WonnaTimeDatas2TotalSecond();
        }

        private void OnEnable()
        {
            GameManager.TourStart += OnTourStart;

            PlayerController.CharacterThrowed += OnCharacterThrowed;
            AIController.CharacterThrowed += OnCharacterThrowed;
        }
        private void OnDisable()
        {
            GameManager.TourStart -= OnTourStart;

            PlayerController.CharacterThrowed -= OnCharacterThrowed;
            AIController.CharacterThrowed -= OnCharacterThrowed;
        }


        private void FixedUpdate()
        {
            if (!GameManager.Instance.GameIsPlaying())
            {
                return;
            }

            TurnCountDownControl();
        }


        private void OnTourStart()
        {
            if (tourControllerPhotonView == null)
            {
                return;
            }

            tourControllerPhotonView.RPC(functionName_PunRPC_TurnChange, RpcTarget.All, true);
        }


        private void OnCharacterThrowed()
        {
            _isCountDown = false;
        }


        private void TurnCountDownControl()
        {
            if (!_isCountDown)
            {
                return;
            }

            _currentCountDownTime += Time.fixedDeltaTime;

            TourCountDownChange?.Invoke(Mathf.Clamp(_targetTime - _currentCountDownTime, 0, _targetTime), _targetTime);

            if (_currentCountDownTime >= _targetTime)
            {
                _isCountDown = false;

                if (PhotonNetwork.IsMasterClient)
                {
                    TourCountDownFinish?.Invoke(_targetTime, _targetTime);
                }
            }
        }


        private void CountDownReset()
        {
            _currentCountDownTime = 0;
            _isCountDown = true;
            TourCountDownStart?.Invoke(_targetTime, _targetTime);
        }


        public void TurnChange()
        {
            if (tourControllerPhotonView == null)
            {
                return;
            }

            tourControllerPhotonView.RPC(functionName_PunRPC_TurnChange, RpcTarget.All);
        }


        [PunRPC]
        public void PunRPC_TurnChange(bool isTurnOfMasterClient)
        {
            _isTurnOfMasterClient = isTurnOfMasterClient;

            TurnChanged?.Invoke(_isTurnOfMasterClient);

            CountDownReset();
        }


        [PunRPC]
        public void PunRPC_TurnChange()
        {
            _isTurnOfMasterClient = !_isTurnOfMasterClient;

            TurnChanged?.Invoke(_isTurnOfMasterClient);

            CountDownReset();
        }
    }
}