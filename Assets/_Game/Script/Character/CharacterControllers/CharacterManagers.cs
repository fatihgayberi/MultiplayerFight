using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class CharacterManagers : MonoBehaviourPunCallbacks
    {
        public static event MatchController.MatchControllerCharacterCountChange CharacterCountChange;

        public delegate void CharacterManagersCharacterGenerated(int playerPhotonViewID);

        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] public PhotonView characterManagerPhotonView;

        private CharacterBase _characterBase;

        private List<int> _allCharacterViewIDList = new List<int>();


        public override void OnEnable()
        {
            base.OnEnable();

            RoomController.RoomFulled += OnRoomFulled;

            CharacterBase.CharacterGenerated += OnCharacterGenerated;

            MatchController.MatchFinish += OnMatchFinish;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            RoomController.RoomFulled -= OnRoomFulled;

            CharacterBase.CharacterGenerated -= OnCharacterGenerated;

            MatchController.MatchFinish -= OnMatchFinish;
        }


        private void OnRoomFulled()
        {
            if (characterManagerPhotonView == null)
            {
                return;
            }

            characterManagerPhotonView.RPC("PunRPC_CharacterGenerate", RpcTarget.All);
        }


        private void OnCharacterGenerated(int playerPhotonViewID)
        {
            if (characterManagerPhotonView == null)
            {
                return;
            }

            characterManagerPhotonView.RPC("PunRPC_CharacterGenerated", RpcTarget.All, playerPhotonViewID);
        }


        private void OnMatchFinish()
        {
            GenerateAI();
        }


        private void GenerateAI()
        {
            if (aiPrefab == null)
            {
                return;
            }

            GameObject generatedAI = Instantiate(aiPrefab);

            AIController aiController = generatedAI.GetComponent<AIController>();

            // aiController.Initialize();
        }


        [PunRPC]
        private void PunRPC_CharacterGenerate()
        {
            if (_characterBase != null)
            {
                return;
            }

            if (characterPrefab == null)
            {
                return;
            }

            GameObject createdCharacter = PhotonNetwork.Instantiate(characterPrefab.name, Vector3.zero, Quaternion.identity);

            if (createdCharacter == null)
            {
                return;
            }

            _characterBase = createdCharacter.GetComponent<CharacterBase>();

            if (_characterBase != null)
            {
                // _characterBase.CharacterSpriteRendererEnable(false);

                _characterBase.CharacterInitialize();
            }
        }


        [PunRPC]
        private void PunRPC_CharacterGenerated(int playerPhotonViewID)
        {
            if (_allCharacterViewIDList == null)
            {
                _allCharacterViewIDList = new List<int>();
            }


            if (!_allCharacterViewIDList.Contains(playerPhotonViewID))
            {
                _allCharacterViewIDList.Add(playerPhotonViewID);
                CharacterCountChange?.Invoke(_allCharacterViewIDList.Count);
            }
        }
    }
}