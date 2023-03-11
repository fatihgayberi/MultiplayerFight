using System;
using Photon.Pun;
using UnityEngine;
using WonnasmithTools;

namespace Wonnasmith
{
    public class TourController : Singleton<TourController>
    {
        public delegate void TourControllerMyTurnChange(bool isMyTurn);
        public static event TourControllerMyTurnChange MyTurnChange;

        [SerializeField] private PhotonView tourControllerPhotonView;

        private bool _isMyTurn;

        public bool IsMyTurn { get => _isMyTurn; }

        private const string functionName_PunRPC_TurnChange = "PunRPC_TurnChange";


        private void OnEnable()
        {
            GameManager.TourPrepare += OnTourPrepare;
            GameManager.TourStart += OnTourStart;
        }
        private void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;
            GameManager.TourStart -= OnTourStart;
        }


        private void OnTourPrepare()
        {
            if (tourControllerPhotonView == null)
            {
                return;
            }

            tourControllerPhotonView.RPC(functionName_PunRPC_TurnChange, RpcTarget.All, false);
        }


        private void OnTourStart()
        {
            if (tourControllerPhotonView == null)
            {
                return;
            }

            tourControllerPhotonView.RPC(functionName_PunRPC_TurnChange, RpcTarget.MasterClient, true);
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
        public void PunRPC_TurnChange(bool isTurn)
        {
            _isMyTurn = isTurn;

            MyTurnChange?.Invoke(_isMyTurn);
        }


        [PunRPC]
        public void PunRPC_TurnChange()
        {
            _isMyTurn = !_isMyTurn;

            MyTurnChange?.Invoke(_isMyTurn);
        }
    }
}