using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class UIPowerUpButtonController : MonoBehaviour
    {
        public delegate void UIPowerUpButtonControllerBallButtonSelect(BallManager.BallType ballType, bool isMasterClientButton);
        public static event /*UIPowerUpButtonController.*/UIPowerUpButtonControllerBallButtonSelect BallButtonSelect;

        [SerializeField] private BallManager.BallType ballType;
        [SerializeField] private bool isMasterClientButton;

        private bool _isButtonUsing;


        private void OnEnable()
        {
            TourController.TurnChanged += OnTurnChanged;
            PlayerController.CharacterThrowed += OnCharacterThrowed;
            AIController.CharacterThrowed += OnCharacterThrowed;
            UIPowerUpButtonController.BallButtonSelect += OnBallButtonSelect;
        }
        private void OnDisable()
        {
            TourController.TurnChanged -= OnTurnChanged;
            PlayerController.CharacterThrowed -= OnCharacterThrowed;
            AIController.CharacterThrowed -= OnCharacterThrowed;
            UIPowerUpButtonController.BallButtonSelect -= OnBallButtonSelect;
        }

        private void Start()
        {
            if (isMasterClientButton != PhotonNetwork.IsMasterClient)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnBallButtonSelect(BallManager.BallType ballType, bool isMasterClientButton)
        {
            if (this.isMasterClientButton == isMasterClientButton)
            {
                _isButtonUsing = false;
            }
        }

        private void OnTurnChanged(bool isTurnOfMasterClient)
        {
            _isButtonUsing = true;
        }


        private void OnCharacterThrowed()
        {
            _isButtonUsing = false;
        }


        public void _BUTTON_SelectBall()
        {
            BallButtonSelect?.Invoke(ballType, isMasterClientButton);
            gameObject.SetActive(false);
        }
    }
}