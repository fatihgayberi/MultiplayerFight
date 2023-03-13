using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    [Serializable]
    public class UITurnPanel : MonoBehaviour
    {
        [Serializable]
        class TurnImageDatas
        {
            public Transform turnImageTR;
            public TMPro.TMP_Text turnText;
            public bool isRight;
        }

        [SerializeField] private GameObject countDownSlider;
        [SerializeField] private TurnImageDatas masterClientTurnImageDatas;
        [SerializeField] private TurnImageDatas otherTurnImageDatas;

        private const string youTurnMessage = "Your Turn";
        private const string otherTurnMessage = "Other's Turn";

        private void OnEnable()
        {
            GameManager.TourPrepare += OnTourPrepare;
            TourController.TurnChanged += OnTurnChanged;

            PlayerController.CharacterThrowed += OnCharacterThrowed;
            AIController.CharacterThrowed += OnCharacterThrowed;

            TourController.TourCountDownStart += OnTourCountDownStart;
        }
        private void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;
            TourController.TurnChanged -= OnTurnChanged;

            PlayerController.CharacterThrowed -= OnCharacterThrowed;
            AIController.CharacterThrowed -= OnCharacterThrowed;

            TourController.TourCountDownStart -= OnTourCountDownStart;
        }

        private void OnTourCountDownStart(float currentCountDownSecond, float targetCountDownSecond)
        {
            countDownSlider.SetActiveNullCheck(true);
        }

        private void OnTourPrepare()
        {
            TextEdit();
        }


        private void OnTurnChanged(bool isTurnOfMasterClient)
        {
            if (isTurnOfMasterClient)
            {
                PanelActivator(masterClientTurnImageDatas, true);
                PanelActivator(otherTurnImageDatas, false);
            }
            else
            {
                PanelActivator(masterClientTurnImageDatas, false);
                PanelActivator(otherTurnImageDatas, true);
            }
        }


        private void OnCharacterThrowed()
        {
            countDownSlider.SetActiveNullCheck(false);
        }

        private void TextEdit()
        {
            if (masterClientTurnImageDatas == null)
            {
                return;
            }
            if (masterClientTurnImageDatas.turnText == null)
            {
                return;
            }

            if (otherTurnImageDatas == null)
            {
                return;
            }
            if (otherTurnImageDatas.turnText == null)
            {
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                masterClientTurnImageDatas.turnText.text = youTurnMessage;
                otherTurnImageDatas.turnText.text = otherTurnMessage;
            }
            else
            {
                masterClientTurnImageDatas.turnText.text = otherTurnMessage;
                otherTurnImageDatas.turnText.text = youTurnMessage;
            }
        }

        private void PanelActivator(TurnImageDatas turnImageData, bool isPanelActive)
        {
            if (turnImageData == null)
            {
                return;
            }

            if (turnImageData.turnImageTR == null)
            {
                return;
            }

            turnImageData.turnImageTR.SetActiveNullCheck(isPanelActive);
        }
    }
}